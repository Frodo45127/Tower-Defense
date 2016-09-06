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

	// variable para el cacheo del grid
	private Grid grid;

	// variable para el nodo clickeado
	private Node clickedNode;

	// en el awake cacheamos lo necesario
	void Awake(){
		grid = GetComponent<Grid>();
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

		// si el nodo existe
		if (clickedNode != null) {

			// y es construible y no tiene torretas
			if (clickedNode.isBuildable && !clickedNode.isBuildableAndHasATurret) {

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
	}

	#endregion
}
