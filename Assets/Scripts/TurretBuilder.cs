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

	// variable para el cacheo del grid
	public GameObject pathfinder;

	// menu de seleccion de torretas
	public GameObject turretSelectionMenu;

	// usamos la interfaz IPointerClickHandler para que el click no atraviese
	// al boton, haciendo que el menu desaparezca
	#region IPointerClickHandler implementation

	// Cuando clickemos en un punto de la pantalla Y SOLTEMOS EL CLICK
	public void OnPointerClick (PointerEventData eventData){
		
		// si no hay un menu de construccion de torretas, lo sacamos
		if (!turretSelectionMenu.activeSelf) {

			// guardamos la posicion del raton al dejar de clickar
			Vector2 clickedMousePosition = Input.mousePosition;

			// pasamos la posicion del raton a coordenadas del mundo
			// NOTA: la coordenada Z es la distancia desde la camara al suelo
			Vector3 clickedMousePositionInWorldSpace = Camera.main.ScreenToWorldPoint(new Vector3(clickedMousePosition.x, clickedMousePosition.y, 9f));

			// comprobamos en que nodo hemos soltado el raton
			Node clickedNode = pathfinder.GetComponent<Grid> ().GetNodeFromWorldPosition (clickedMousePositionInWorldSpace);

			// si el nodo existe
			if (clickedNode != null) {

				// colocamos el menu de las torretas sobre el raton y lo activamos
				turretSelectionMenu.transform.position = clickedMousePositionInWorldSpace;
				turretSelectionMenu.SetActive (true);
				turretSelectionMenu.SendMessage ("OpenTurretSelectionMenu", clickedNode);
			}
		}

		// si ya hay un menu sacado, lo ocultamos
		else {
			turretSelectionMenu.SetActive (false);
		}
	}

	#endregion
}
