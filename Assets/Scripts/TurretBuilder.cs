// Copyright © Ismael Gutiérrez González <frodo_gv@hotmail.com>, 2016
//
// This work is free.  You can redistribute it and/or modify it under the
// terms of the Do What The Fuck You Want To But It's Not My Fault Public
// License, Version 1, as published by Ben McGinnes.  See the
// LICENSE.txt file for more details.

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
	private GameObject selectedTurretPhantomInCurrentNode;
	private GameObject selectedBarricadePhantom;
	private GameObject selectedBarricadePhantomInCurrentNode;


	// en el awake cacheamos lo necesario
	void Awake(){
		grid = GetComponent<Grid>();
		pathfinder = GetComponent<Pathfinder> ();
	}
		
	void Update() {

		// si la partida no ha empezado
		if (!LevelManager.Instance.isPlayerReady) {

			// si tenemos un nodo válido
			if (hoveredNode != null) {
			
				// si tenemos barricadas
				if (LevelManager.Instance.Barricades > 0) {

					// comprobamos si el nodo en el que estamos es válido
					bool CanIPutABarricadeHere = CheckBarricadeRequeriments (hoveredNode);

					// si el nodo el que estamos es válido
					if (CanIPutABarricadeHere) {

						// si ya tenemos la barricada elegida
						if (selectedBarricadePhantom != null) {

							// si no tenemos una barricada fantasma creada
							if (selectedBarricadePhantomInCurrentNode == null) {

								// es el primer nodo sobre el que nos ponemos, luego ponemos la barricada
								selectedBarricadePhantomInCurrentNode = (GameObject)Instantiate (selectedBarricadePhantom, hoveredNode.worldPosition, Quaternion.identity);

								// la seteamos como fantasma y la quitamos el BoxCollider
								selectedBarricadePhantomInCurrentNode.GetComponent<Barricade> ().IsPhantom = true;
								Destroy (selectedBarricadePhantomInCurrentNode.GetComponent<BoxCollider> ());

								// ANALOGIA DE PLSQL
								// CREA UN CURSOR CON LOS DATOS DE COLOR DEL FANTASMA (r,g,b,a)
								Color newalpha = selectedBarricadePhantomInCurrentNode.GetComponent<SpriteRenderer>().material.color;
								// A LA (a) LE APLICA EL NUEVO VALOR
								newalpha.a = 0.5f;
								// ESCRIBE EN EL COLOR DEL FANTASMA LOS DATOS ALMACENADOS EN EL CURSOR
								selectedBarricadePhantomInCurrentNode.GetComponent<SpriteRenderer>().material.color = newalpha;
								// FIN DE LA ANALOGÍA
							}

							// si ya tenemos una barricada fantasma creada
							else {

								// sacamos el nuevo nodo del ratón
								Node newHoveredNode = GetNodeFromMousePosition ();

								// si el nodo nuevo es distinto al viejo y no es nulo (hemos movido el ratón)
								if (newHoveredNode != null && newHoveredNode != hoveredNode) {

									// cambia el nodo
									hoveredNode = newHoveredNode;

									// borra la barricada seleccionada (pues hay que buscar una nueva para el nuevo nodo)
									selectedBarricadePhantom = null;

									// y destruye el fantasma
									Destroy (selectedBarricadePhantomInCurrentNode);
								}
							}
						}

						// si no tenemos barricada elegida
						else {
							
							// sacamos una barricada
							selectedBarricadePhantom = SelectOrientedBarricade (hoveredNode);
						}
					}

					// si el nodo en el que estamos no es válido para poner barricadas
					else {

						// lo seteamos a null
						hoveredNode = null;
					}
				}

				// si no nos quedan barricadas
				else {

					// si no hemos destruído la barricada fantasma
					if (selectedBarricadePhantomInCurrentNode != null) {

						// la destruímos
						Destroy (selectedBarricadePhantomInCurrentNode);
					}
				}
			}

			// si no tenemos nodo válido
			else {

				// sacamos el nodo en el que estamos
				hoveredNode = GetNodeFromMousePosition ();
			}
		}

		// si la partida ha empezado
		else {

			// TODO: cambiar esto por un bloqueo de los botones hasta que le damos a empezar
			// si por alguna razón la torreta fantasma no es nula al empezar
			if (!selectedTurretPhantomHasBeenCleared) {

				// la ponemos nula
				LevelManager.Instance.SelectedTurret = 0;

				// decimos que la hemos apañado
				selectedTurretPhantomHasBeenCleared = true;
			}
			
			// si tenemos torreta seleccionada
			if (LevelManager.Instance.SelectedTurret > 0) {

				// si tenemos un nodo válido
				if (hoveredNode != null) {

					// si no tenemos torreta fantasma (por lo que sea) o acabamos de cambiar de torreta
					if (selectedTurretPhantom == null || LevelManager.Instance.hasSelectedTurretChanged) {

						// pilla la torreta nueva
						selectedTurretPhantom = GetSelectedTurret ();

						// si hemos pillado una nueva torreta porque ha cambiado la seleccionada
						if (LevelManager.Instance.hasSelectedTurretChanged) {

							// destruye la torreta actual
							Destroy (selectedTurretPhantomInCurrentNode);

							// y dile al GameManager que ya la hemos cambiado
							LevelManager.Instance.hasSelectedTurretChanged = false;
						}
					}

					// si ya tenemos torreta
					if (selectedTurretPhantom != null) {

						// si no tenemos ningún fantasma creado en ningún nodo
						if (selectedTurretPhantomInCurrentNode == null) {

							// comprueba si sobre la posición actual se puede poner una torreta
							bool CanIPutATurretHere = CheckTurretRequeriments (hoveredNode);

							// si puedo poner una torreta
							if (CanIPutATurretHere) {

								// es el primer nodo sobre el que nos ponemos, luego ponemos la torreta
								selectedTurretPhantomInCurrentNode = (GameObject)Instantiate (selectedTurretPhantom, hoveredNode.worldPosition, Quaternion.identity);

								// le quitamos el boxCollider y la seteamos como fantasma, para que no haga cosas raras
								selectedTurretPhantomInCurrentNode.GetComponent<Turret> ().IsPhantom = true;
								Destroy (selectedTurretPhantomInCurrentNode.GetComponent<BoxCollider> ());

								// ANALOGIA DE PLSQL
								// CREA UN CURSOR CON LOS DATOS DE COLOR DEL FANTASMA (r,g,b,a)
								Color newalpha = selectedTurretPhantomInCurrentNode.GetComponent<SpriteRenderer>().material.color;
								// A LA (a) LE APLICA EL NUEVO VALOR
								newalpha.a = 0.5f;
								// ESCRIBE EN EL COLOR DEL FANTASMA LOS DATOS ALMACENADOS EN EL CURSOR
								selectedTurretPhantomInCurrentNode.GetComponent<SpriteRenderer>().material.color = newalpha;
								// FIN DE LA ANALOGÍA
							}

							// si no podemos poner la torreta
							else {

								// seteamos el nodo como nulo
								hoveredNode = null;
							}
						}

						// si tenemos una torreta fantasma
						else {

							// sacamos el nuevo nodo del ratón
							Node newHoveredNode = GetNodeFromMousePosition ();

							// si el nodo nuevo es distinto al viejo y no es nulo
							if (newHoveredNode != null && newHoveredNode != hoveredNode) {

								// comprueba si sobre la nueva posición se puede poner una torreta
								bool CanIMoveThePhantomTurretHere = CheckTurretRequeriments (newHoveredNode);

								// si se pueden poner torretas en el nodo
								if (CanIMoveThePhantomTurretHere) {

									// actualiza el nodo
									hoveredNode = newHoveredNode;

									// mueve el fantasma a la nueva posición
									selectedTurretPhantomInCurrentNode.transform.position = hoveredNode.worldPosition;
								}
							}
						}
					}
				}

				// si no tenemos nodo válido
				else {
					
					// sacamos el nodo en el que estamos
					hoveredNode = GetNodeFromMousePosition ();
				}
			}

			// si no tenemos torreta seleccionada
			else {

				// y por la razón que sea el fantasma sigue existiendo
				if (selectedTurretPhantomInCurrentNode != null) {
					
					// si tenemos un fantasma, nos lo ventilamos
					Destroy (selectedTurretPhantomInCurrentNode);	
				}
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

		// comprobamos el estado del juego
		switch (LevelManager.Instance.isPlayerReady){

		// si el juego ha empezado
		case true:

			// comprueba si puedo poner la torreta en el nodo
			bool CanIPutThisTurretHere = CheckTurretRequeriments (clickedNode);

			// si puedo ponerla
			if (CanIPutThisTurretHere) {

				// la construímos
				BuildTurret (clickedNode);
			}

			break;

		// si el juego no ha emnpezado
		case false:
			
			// comprueba si puedo poner la barricada en el nodo
			bool CanIPutThisBarricadeHere = CheckBarricadeRequeriments (clickedNode);

			// si puedo ponerla
			if (CanIPutThisBarricadeHere) {

				// la construímos
				BuildBarricade (clickedNode);
			}
			break;

		// si no se cumple nada de lo anterior
		default:

			// do nothing, succesfully
			Debug.Log ("Nu se, no has na por si acaso.");
			break;
		}
	}

	#endregion

	// Función para construir la torreta.
	void BuildTurret(Node currentNode){

		// sacamos la torreta a construir y su coste
		GameObject turretToBuild = GetSelectedTurret ();
		int costNewTurret = turretToBuild.GetComponent<Turret>().Cost;

		// si tenemos torreta seleccionada y existe
		if (turretToBuild != null) {
			
			// si tenemos suficiente dinero para construirla
			if (costNewTurret <= LevelManager.Instance.Money) {

				// instanciamos la nueva torreta
				Instantiate (turretToBuild, currentNode.worldPosition, Quaternion.identity);

				// seteamos el nodo como ocupado
				currentNode.isBuildableAndHasATurret = true;

				// reducimos el dinero que tenemos porque nos lo hemos gastado
				LevelManager.Instance.Money -= costNewTurret;	
			} 

			// si no tenemos dinero, suelta un error
			else {
				Debug.Log ("nos falta dinero");
			}
		}
	}

	// Función para sacar la barricada a spawnear (orientada)
	GameObject SelectOrientedBarricade(Node currentNode) {
		
		// comprobamos los nodos adyacentes para ver cómo debemos orientar la barricada
		// para ello, primero sacamos los nodos adyacentes
		List<Node> neighboursList = grid.GetNeighbours (currentNode);

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
				// entonces es una barricada vertical
				barricadeToInstantiate = barricadeList [1];
			}
			// y el de abajo tambien
			else if (neighboursList [1].isWalkable){
				// entonces es una barricada en diagonal de abajo a arriba
				barricadeToInstantiate = barricadeList [3];
			}
			// y el de arriba tambien
			else if (neighboursList [2].isWalkable){
				// entonces es una barricada en diagonal de arriba a abajo
				barricadeToInstantiate = barricadeList [2];
			}
			// y ninguno más lo es
			else {
				// entonces es una esquina, luego barricada vertical
				barricadeToInstantiate = barricadeList [1];
			}
		}
		// si el de la derecha es caminable
		else if (neighboursList [3].isWalkable){

			// y el de arriba tambien
			if (neighboursList [2].isWalkable){
				// entonces es una barricada en diagonal de abajo a arriba
				barricadeToInstantiate = barricadeList [3];
			}
			// y el de abajo tambien
			if (neighboursList [1].isWalkable) {
				// entonces es una barricada en diagonal de arriba a abajo
				barricadeToInstantiate = barricadeList [2];
			}
			// y ninguno más lo es
			else {
				// entonces es una esquina, luego barricada vertical
				barricadeToInstantiate = barricadeList [1];
			}
		}
		// si el de arriba y el de abajo son caminables
		else if (neighboursList [1].isWalkable && neighboursList [2].isWalkable){
			// entonces es una horizontal
			barricadeToInstantiate = barricadeList [0];
		}
		// si el de arriba es caminable y el de abajo no
		else if (!neighboursList [1].isWalkable && neighboursList [2].isWalkable) {
			// entonces es una esquina, luego barricada horizontal
			barricadeToInstantiate = barricadeList [0];
		}
		// si el de arriba no es caminable y el de abajo si
		else if (neighboursList [1].isWalkable && !neighboursList [2].isWalkable) {
			// entonces es una esquina, luego barricada horizontal
			barricadeToInstantiate = barricadeList [0];
		}

		// y si no, error
		else {
			Debug.Log("Error, orientación desconocida.");
			barricadeToInstantiate = null;
		}

		// si todo ha salido bien y tenemos una barricada lista
		if (barricadeToInstantiate != null) {

			// devolvemos la barricada orientada
			return barricadeToInstantiate;
		}

		// si por lo que sea no hemos podido sacar una barricada
		else {
			return null;
		}
	}

	// Funcion para construir barricadas
	void BuildBarricade(Node currentNode) {

		// sacamos la barricada correcta dependiendo de su orientación
		GameObject barricade = SelectOrientedBarricade (currentNode);

		// si todo ha salido bien y tenemos una barricada lista
		if (barricade != null) {

			// preparamos el nodo para tener la barricada
			currentNode.isWalkable = false;
			currentNode.isBuildableAndHasABarricade = true;

			// colocamos la barricada
			Instantiate (barricade, new Vector2(currentNode.worldPosition.x, currentNode.worldPosition.y), Quaternion.identity);
		}

		// restamos la barricada de las que tenemos
		LevelManager.Instance.Barricades--;
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
			LevelManager.Instance.Money += clickedTurret.GetComponent<Turret> ().DestructionCost;

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
		switch (LevelManager.Instance.SelectedTurret) {
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

	// Función para saber si un nodo cumple los requisitos para poner una torreta
	bool CheckTurretRequeriments(Node currentNode) {

		// si el nodo existe
		if (currentNode != null) {
			
			// si la partida ha empezado, es construible, no es camino,
			// no tiene torretas ni barricadas, tenemos una torreta seleccionada y no estamos en modo destroyer
			if (LevelManager.Instance.isPlayerReady &&
				currentNode.isBuildable &&
				!currentNode.isWalkable &&
				!currentNode.isBuildableAndHasATurret &&
				!currentNode.isBuildableAndHasABarricade &&
				LevelManager.Instance.SelectedTurret != 0 &&
				LevelManager.Instance.SelectedTurret != -1) {

				// el nodo es válido
				return true;
			}

			// si está activada la opción de destruir torretas
			else if (LevelManager.Instance.SelectedTurret == -1) {
				Debug.Log ("Modo destroyer, no hay nada que destruir.");
				return false;
			}

			// y es construible, pero ya tiene una torreta
			else if (currentNode.isBuildable && currentNode.isBuildableAndHasATurret) {
				Debug.Log ("Ya hay una torreta, asi que dale a la torreta.");
				return false;
			}

			// y no tenemos torreta seleccionada
			else if (LevelManager.Instance.SelectedTurret == 0) {
				Debug.Log ("No hay torreta seleccionada.");
				return false;
			}
			// y no es construible
			else {
				Debug.Log ("Aqui no se puede construir.");
				return false;
			}
		}

		// nodo nulo, no se puede construir
		else {
			Debug.Log ("Nodo nulo.");
			return false;
		}

	}

	// Función para saber si un nodo cumple los requisitos para poner una barricada
	bool CheckBarricadeRequeriments(Node currentNode) {

		// si el nodo existe
		if(currentNode != null) {
			
			// si la partida no ha empezado
			if (!LevelManager.Instance.isPlayerReady) {

				// y el nodo es caminable, construible y no tiene una barricada construida
				if (currentNode.isWalkable &&
					currentNode.isBuildable &&
					!currentNode.isBuildableAndHasABarricade) {

					// sacamos todos sus vecinos y los guardamos en una lista
					List<Node> neighboursList = grid.GetNeighbours(currentNode);

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
						if (LevelManager.Instance.Barricades > 0) {

							// calculamos si, tras construir la barricada habria un camino libre:
							// seteamos el nodo como no caminable
							currentNode.isWalkable = false;

							// buscamos el camino mas corto
							pathfinder.FindShortestPathFromAllSources ();

							// devolvemos el nodo a su estado original
							currentNode.isWalkable = true;

							// si existe al menos un camino de principio a fin
							if (grid.pathList.Count > 0) {

								// se puede construir la barricada
								return true;
							}

							// si no existe al menos un camino
							else {

								// no hagas nada
								Debug.Log ("Esto bloquea los caminos, asi que no se puede construir aquí.");
								return false;
							}
						}
						// y no quedan barricadas, no hagas nada
						else {
							Debug.Log ("No quedan barricadas.");
							return false;
						}
					}

					// si hay 3 o mas conexiones caminables
					else {

						// es un cruce, así que no se pueden construir barricadas
						Debug.Log ("No se pueden hacer barricadas en un cruce.");
						return false;
					}
				}

				// si tiene una barricada (en principio con comprobar esto vale, pues que
				// tenga una barricada implica que es caminable y construible)
				else if (currentNode.isBuildableAndHasABarricade) {
					Debug.Log ("Ya hay una barricada, asi que dale a la barricada.");
					return false;
				}

				// si no se cumple nada de lo anterior (¿fallo?) no se puede construir
				else {
					return false;
				}
			}
			// si el nodo no es válido o hemos empezado la partida
			else {
				Debug.Log ("Nodo no existe o partida empezada.");
				return false;
			}
		}

		// nodo nulo, no se puede construir
		else {
			Debug.Log ("Nodo nulo.");
			return false;
		}
	}
}
