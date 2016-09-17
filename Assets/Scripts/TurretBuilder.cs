using UnityEngine;
using System.Collections;
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

	// menu de seleccion de torretas
	public GameObject turretSelectionMenu;

	// menu de mantenimiento de torretas
	public GameObject turretManagementMenu;

	// canvas donde estan los menuses
	public GameObject turretMenu;

	// variables para el cacheo del grid y del pathfinder
	private Grid grid;
	private Pathfinder pathfinder;

	// variable para el nodo clickeado
	private Node clickedNode;

	// en el awake cacheamos lo necesario
	void Awake(){
		grid = GetComponent<Grid>();
		pathfinder = GetComponent<Pathfinder> ();
	}

	// usamos la interfaz IPointerClickHandler para que el click no atraviese
	// al boton, haciendo que el menu desaparezca
	#region IPointerClickHandler implementation

	// Cuando clickemos en un punto de la pantalla Y SOLTEMOS EL CLICK
	public void OnPointerClick (PointerEventData eventData){

		// guardamos la posicion del raton al dejar de clickar
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
			return;
		}

		// si el nodo esta dentro del array, buscamos que nodo es
		else {
			
			// comprobamos en que nodo hemos soltado el raton
			clickedNode = grid.GetNodeFromWorldPosition (clickedMousePositionInWorldSpace);
		}

		// si el nodo existe y la partida ha empezado
		if (clickedNode != null && GameManager.Instance.isPlayerReady) {

			// y es construible, no es camino y no tiene torretas
			if (clickedNode.isBuildable && !clickedNode.isWalkable && !clickedNode.isBuildableAndHasATurret) {

				// le decimos que intente abrirlo
				turretMenu.SendMessage ("OpenTurretSelectionMenu", clickedNode);
			}

			// y es construible, pero ya tiene una torreta
			else if (clickedNode.isBuildable && clickedNode.isBuildableAndHasATurret) {
				Debug.Log("Ya hay una torreta, asi que dale a la torreta.");
			}

			// y no es construible
			else {
				Debug.Log("Aqui no se puede construir.");
			}
		}

		// si el nodo existe y la partida no ha empezado
		else if (clickedNode != null && !GameManager.Instance.isPlayerReady) {

			// y el nodo es caminable, construible y no tiene una barricada construida
			if (clickedNode.isWalkable && clickedNode.isBuildable && !clickedNode.isBuildableAndHasABarricade) {

				// si quedan barricadas
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
						turretMenu.SendMessage ("BuildBarricade", clickedNode);
					}

					// si no existe al menos un camino
					else {

						// no hagas nada
						Debug.Log ("Esto bloquea los caminos, asi que no se puede construir aquí.");
					}
				}
				// si no quedan barricadas, no hagas nada
				else {
					Debug.Log ("No quedan barricadas.");
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
}
