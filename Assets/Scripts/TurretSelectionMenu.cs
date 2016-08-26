using UnityEngine;
using System.Collections;

//-----------------------------------------------------------------------
// TurretSelectionMenu.cs
// 
// Este script es el que se encarga de manejar el menu que sale al clickar
// en un nodo o en una torreta
//
//-----------------------------------------------------------------------

public class TurretSelectionMenu : MonoBehaviour {

	// añadimos las torretas a instanciar
	public GameObject turret1, turret2;

	// menu de seleccion de torretas
	public GameObject turretSelectionMenu;

	// variable para el cacheo del grid
	public GameObject grid;

	// nodo en el que hemos clickado
	private Node clickedNode;

	// recibimos el nodo donde colocar la torreta al abrir el menu
	void OpenTurretSelectionMenu(Node _clickedNode){
		clickedNode = _clickedNode;
	}
	
	// Función para construir la torreta.
	public void BuildTurret(int turret){

		// si el nodo es construible
		if (clickedNode.isBuildable) {
			
			// depende del numero que pongamos en el boton, instancia una torreta u otra
			if (turret == 0) {
				Instantiate (turret1, clickedNode.worldPosition, Quaternion.identity);
			} else if (turret == 1) {
				Instantiate (turret2, clickedNode.worldPosition, Quaternion.identity);
			}

			clickedNode.isBuildable = false;

			// despues de instanciar la torreta, oculta el menu
			turretSelectionMenu.SetActive (false);
		}

		// si el nodo ya esta ocupado
		else {
			Debug.Log ("Nodo ocupado, usa otro;");
			// despues de instanciar la torreta, oculta el menu
			turretSelectionMenu.SetActive (false);
		}
	}
}
