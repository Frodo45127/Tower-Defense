using UnityEngine;
using System.Collections;
// necesario para usar las listas
using System.Collections.Generic;
// necesario para el fix de clickar a traves de los botones
using UnityEngine.EventSystems;

//-----------------------------------------------------------------------
// TurretBuilder.cs
// 
// Este script es el que se encarga de la construccion, destruccion y
// mejora de las torretas.
//
//-----------------------------------------------------------------------

public class TurretBuilder : MonoBehaviour, IPointerClickHandler {

	// torretas a construir
	public GameObject turret1, turret2, turret3;

	// y el array con las barricadas a usar
	// ATENCION: el orden de las barricadas es el siguiente:
	// - 0: horizontal
	// - 1: vertical
	// - 2: inclinada de arriba a abajo
	// - 3: inclinada de abajo a arriba
	public GameObject[] barricadeList;

	// variables para el cacheo del grid y del pathfinder
	private Grid grid;
	private Pathfinder pathfinder;

	// variable para el nodo clickeado
	private Node clickedNode;

	// variable para el nodo en el que estamos
	private Node hoveredNode;

	// variable para saber si hemos seteado a null la torreta fantasma al empezar
	private bool selectedTurretPhantomHasBeenCleared;

	// variables para el fantasma de la torreta seleccionada
	private GameObject selectedTurretPhantom;
	private GameObject selectedTurretPhantomInActualNode;


	// en el awake cacheamos lo necesario
	void Awake(){
		grid = GetComponent<Grid>();
		pathfinder = GetComponent<Pathfinder> ();
	}
		
	void Update() {

		// si la partida ha empezado
		if (GameManager.Instance.isPlayerReady) {

			// TODO: cambiar esto por un bloqueo de los botones hasta que le damos a empezar
			// si por alguna razón la torreta fantasma no es nula al empezar
			if (!selectedTurretPhantomHasBeenCleared) {

				// la ponemos nula
				GameManager.Instance.SelectedTurret = 0;

				// decimos que la hemos apañado
				selectedTurretPhantomHasBeenCleared = true;
			}
			
			// si tenemos torreta seleccionada
			if (GameManager.Instance.SelectedTurret > 0) {

				// si no tenemos nodo todavia
				if (hoveredNode == null) {

					// sacamos el nodo en el que estamos
					hoveredNode = GetNodeFromMousePosition ();
				}
				
				// si no tenemos torreta fantasma (por lo que sea) o acabamos de cambiar de torreta
				if (selectedTurretPhantom == null || GameManager.Instance.hasSelectedTurretChanged) {

					// pilla la torreta nueva
					selectedTurretPhantom = GetSelectedTurret ();

					// si hemos pillado una nueva torreta porque ha cambiado la seleccionada
					if (GameManager.Instance.hasSelectedTurretChanged) {
					
						// destruye la torreta actual
						Destroy (selectedTurretPhantomInActualNode);

						// y dile al GameManager que ya la hemos cambiado
						GameManager.Instance.hasSelectedTurretChanged = false;
					}
				}

				// si ya tenemos torreta
				if (selectedTurretPhantom != null) {

					// si no tenemos ningún fantasma creado en ningún nodo
					if (selectedTurretPhantomInActualNode == null) {

						// es el primer nodo sobre el que nos ponemos, luego ponemos la torreta
						selectedTurretPhantomInActualNode = (GameObject)Instantiate (selectedTurretPhantom, hoveredNode.worldPosition, Quaternion.identity);

						// le quitamos el script y el box collider de la torreta, y el box collider
						// de sus hijos, para que no haga cosas raras y se comporte como un fantasma
						Destroy (selectedTurretPhantomInActualNode.GetComponent<Turret> ());
						Destroy (selectedTurretPhantomInActualNode.GetComponent<BoxCollider2D> ());
						Destroy (selectedTurretPhantomInActualNode.GetComponentInChildren<BoxCollider> ());

						// TODO: hacer que el fantasma sea transparente.

						// TODO: hacer que el fantasma muestre el alcance.
					}

				// si tenemos una torreta fantasma
				else {

						// sacamos el nuevo nodo del ratón
						Node newHoveredNode = GetNodeFromMousePosition ();

						// si el nodo nuevo es distinto al viejo y no es nulo
						if (newHoveredNode != null && newHoveredNode != hoveredNode) {

							// actualiza el nodo
							hoveredNode = newHoveredNode;

							// mueve el fantasma a la nueva posición
							selectedTurretPhantomInActualNode.transform.position = hoveredNode.worldPosition;
						}
					}
				}
			}

			// si no tenemos torreta seleccionada
			else {

				// si tenemos un fantasma, nos lo ventilamos
				Destroy (selectedTurretPhantomInActualNode);
			}
		}
	}
	// usamos la interfaz IPointerClickHandler para que el click no atraviese
	// al boton, haciendo que el menu desaparezca
	#region IPointerClickHandler implementation

	// Cuando clickemos en un punto de la pantalla Y SOLTEMOS EL CLICK
	public void OnPointerClick (PointerEventData eventData){

		// sacamos el nodo sobre el que hemos clickado
		clickedNode = GetNodeFromMousePosition ();

		// si el nodo existe y la partida ha empezado
		if (clickedNode != null && GameManager.Instance.isPlayerReady) {

			// y es construible, no es camino y no tiene torretas ni barricadas y 
			// tenemos una torreta seleccionada
			if (clickedNode.isBuildable &&
				!clickedNode.isWalkable &&
				!clickedNode.isBuildableAndHasATurret &&
				!clickedNode.isBuildableAndHasABarricade &&
				GameManager.Instance.SelectedTurret != 0) {

				// si no esta activada la opción de destruir torretas
				if (GameManager.Instance.SelectedTurret != -1) {

					// intentamos construir la torreta
					BuildTurret ();	
				}

				// si está activada la opción de destruir torretas
				else {
					Debug.Log ("Modo destroyer, no hay nada que destruir.");
				}
			}

			// y es construible, pero ya tiene una torreta
			else if (clickedNode.isBuildable && clickedNode.isBuildableAndHasATurret) {
				Debug.Log("Ya hay una torreta, asi que dale a la torreta.");
			}

			// y no tenemos torreta seleccionada
			else if (GameManager.Instance.SelectedTurret == 0) {
				Debug.Log("No hay torreta seleccionada.");
			}
			// y no es construible
			else {
				Debug.Log("Aqui no se puede construir.");
			}
		}

		// si el nodo existe y la partida no ha empezado
		else if (clickedNode != null && !GameManager.Instance.isPlayerReady) {

			// y el nodo es caminable, construible y no tiene una barricada construida
			if (clickedNode.isWalkable &&
				clickedNode.isBuildable &&
				!clickedNode.isBuildableAndHasABarricade) {

				// sacamos todos sus vecinos y los guardamos en una lista
				List<Node> neighboursList = grid.GetNeighbours(clickedNode);

				// ponemos una variable para ver el numero de conexiones caminables
				int walkableConnection = 0;

				// y con un foreach miramos cuantos vecinos son caminables
				foreach (Node n in neighboursList){
					if (n.isWalkable){
						walkableConnection++;
					}	
				}

				// si hay menos de 3 caminables
				if (walkableConnection < 3){
					
					// y quedan barricadas
					if (GameManager.Instance.Barricades > 0) {

						// calculamos si, tras construir la barricada habria un camino libre:
						// seteamos el nodo como no caminable
						clickedNode.isWalkable = false;

						// buscamos el camino mas corto
						pathfinder.FindShortestPathFromAllSources ();

						// devolvemos el nodo a su estado original
						clickedNode.isWalkable = true;

						// si existe al menos un camino de principio a fin
						if (grid.pathList.Count > 0) {

							// construye la barricada
							BuildBarricade();
						}

						// si no existe al menos un camino
						else {

							// no hagas nada
							Debug.Log ("Esto bloquea los caminos, asi que no se puede construir aquí.");
						}
					}
					// y no quedan barricadas, no hagas nada
					else {
						Debug.Log ("No quedan barricadas.");
					}
				}

				// si hay 3 o mas conexiones caminables
				else {
					
					// es un cruce, así que no se pueden construir barricadas
					Debug.Log ("No se pueden hacer barricadas en un cruce.");
				}
			}

			// si tiene una barricada (en principio con comprobar esto vale, pues que
			// tenga una barricada implica que es caminable y construible)
			else if (clickedNode.isBuildableAndHasABarricade) {
				Debug.Log ("Ya hay una barricada, asi que dale a la barricada.");
			}
		}

		// si no se cumple nada de lo anterior
		else {
			// do nothing, succesfully
			Debug.Log ("Nu se, no has na por si acaso.");
		}
	}

	#endregion

	// Función para construir la torreta.
	void BuildTurret(){

		// sacamos la torreta a construir y su coste
		GameObject turretToBuild = GetSelectedTurret ();
		int costNewTurret = turretToBuild.GetComponent<Turret>().Cost;

		// si tenemos torreta seleccionada y existe
		if (turretToBuild != null) {
			
			// si tenemos suficiente dinero para construirla
			if (costNewTurret <= GameManager.Instance.Money) {

				// instanciamos la nueva torreta
				Instantiate (turretToBuild, clickedNode.worldPosition, Quaternion.identity);

				// seteamos el nodo como ocupado
				clickedNode.isBuildableAndHasATurret = true;

				// reducimos el dinero que tenemos porque nos lo hemos gastado
				GameManager.Instance.Money -= costNewTurret;	
			} 

			// si no tenemos dinero, suelta un error
			else {
				Debug.Log ("nos falta dinero");
			}
		}
	}

	// Funcion para construir barricadas
	void BuildBarricade() {

		// preparamos el nodo para tener la barricada
		clickedNode.isWalkable = false;
		clickedNode.isBuildableAndHasABarricade = true;

		// comprobamos los nodos adyacentes para ver cómo debemos orientar la barricada
		// para ello, primero sacamos los nodos adyacentes
		List<Node> neighboursList = grid.GetNeighbours (clickedNode);

		// creamos la variable en la que guardaremos la barricada a spawnear
		GameObject barricadeToInstantiate = null;

		// despues comprobamos las posibles orientaciones (max: 2 caminos)
		// ATENCION: el orden de los nodos es el siguiente:
		// - 0: izquierda
		// - 1: abajo
		// - 2: arriba
		// - 3: derecha
		// Y el de las barricadas el siguiente:
		// - 0: horizontal
		// - 1: vertical
		// - 2: inclinada de arriba a abajo
		// - 3: inclinada de abajo a arriba
		// si no tiene conexiones con nada caminable o solo una
		// si el de la izquierda es caminable
		if (neighboursList[0].isWalkable) {

			// y el de la derecha tambien
			if (neighboursList [3].isWalkable){
				// entonces es una barricada horizontal
				barricadeToInstantiate = barricadeList [0];
			}
			// y el de abajo tambien
			else if (neighboursList [1].isWalkable){
				// entonces es una barricada en diagonal de arriba a abajo
				barricadeToInstantiate = barricadeList [2];
			}
			// y el de arriba tambien
			else if (neighboursList [2].isWalkable){
				// entonces es una barricada en diagonal de abajo a arriba
				barricadeToInstantiate = barricadeList [3];
			}
		}
		// si el de la derecha es caminable
		else if (neighboursList [3].isWalkable){

			// y el de arriba tambien
			if (neighboursList [2].isWalkable){
				// entonces es una barricada en diagonal de arriba a abajo
				barricadeToInstantiate = barricadeList [2];
			}
			// y el de abajo tambien
			if (neighboursList [1].isWalkable){
				// entonces es una barricada en diagonal de abajo a arriba
				barricadeToInstantiate = barricadeList [3];
			}
		}
		// si el de arriba y el de abajo son caminables
		else if (neighboursList [1].isWalkable && neighboursList [2].isWalkable){
			// entonces es una vertical
			barricadeToInstantiate = barricadeList [1];
		}
		// y si no, error
		else {
			Debug.Log("Error, orientación desconocida.");
			barricadeToInstantiate = null;
		}

		// si todo ha salido bien y tenemos una barricada lista
		if (barricadeToInstantiate != null) {

			// colocamos la barricada
			Instantiate (barricadeToInstantiate, new Vector2(clickedNode.worldPosition.x, clickedNode.worldPosition.y), Quaternion.identity);
		}

		// restamos la barricada de las que tenemos
		GameManager.Instance.Barricades--;
	}

	// Función para destruir la torreta.
	public void DestroyTurret(GameObject _clickedTurret){

		// variable temporal para almacenar la torreta
		GameObject clickedTurret = _clickedTurret;

		// nos aseguramos de que exista la torreta a destruir
		if (clickedTurret != null) {

			// sacamos el nodo sobre el que esta la torreta
			Node clickedTurretNode = grid.GetNodeFromWorldPosition (clickedTurret.transform.position);

			// recuperamos algo del dinero que costo construirla
			GameManager.Instance.Money += clickedTurret.GetComponent<Turret> ().DestructionCost;

			// destruimos la torreta
			Destroy (clickedTurret);

			// seteamos el nodo como construible
			clickedTurretNode.isBuildableAndHasATurret = false;
		}

		// si no hay torreta, no hacemos nada
		else {
			Debug.Log("No hay torreta");
		}
	}

	// Función para guardar la torreta que hemos elegido
	GameObject GetSelectedTurret () {

		// depende del numero que pongamos en el boton, instancia una torreta u otra
		switch (GameManager.Instance.SelectedTurret) {
		case 1:
			return turret1;
		case 2:
			return turret2;
		case 3:
			return turret3;
		default:
			Debug.Log ("La torreta no existe.");
			return null;
		}
	}

	// Función para sacar el nodo sobre el que está el ratón
	Node GetNodeFromMousePosition() {
		
		// guardamos la posicion del raton
		Vector2 clickedMousePosition = Input.mousePosition;

		// pasamos la posicion del raton a coordenadas del mundo
		// NOTA: la coordenada Z es la distancia desde la camara al suelo
		Vector3 clickedMousePositionInWorldSpace = Camera.main.ScreenToWorldPoint(new Vector3(clickedMousePosition.x, clickedMousePosition.y, 9f));

		// si las coordenadas estan fuera del grid, no hagas nada
		if ((clickedMousePositionInWorldSpace.x > grid.gridWorldSize.x / 2) ||
			(clickedMousePositionInWorldSpace.x < -grid.gridWorldSize.x / 2) ||
			(clickedMousePositionInWorldSpace.y > grid.gridWorldSize.y / 2) ||
			(clickedMousePositionInWorldSpace.y < -grid.gridWorldSize.y / 2)) {
			Debug.Log ("El nodo esta fuera del grid, luego no hagas nada");
			return null;
		}

		// si el nodo esta dentro del array, buscamos que nodo es
		else {

			// comprobamos en que nodo hemos soltado el raton
			return grid.GetNodeFromWorldPosition (clickedMousePositionInWorldSpace);
		}
	}
}
