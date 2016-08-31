using UnityEngine;
using System.Collections;

//-----------------------------------------------------------------------
// TurretSelectionMenu.cs
// 
// Este script es el que se encarga de manejar el menu que sale al clickar
// en un nodo o en una torreta
//
//-----------------------------------------------------------------------

public class TurretMenu : MonoBehaviour {

	// añadimos las torretas a instanciar
	public GameObject turret1, turret2;

	// menu de seleccion de torretas y menu de manejo de torretas
	public GameObject turretSelectionMenu;
	public GameObject turretManagementMenu;

	// nodo en el que hemos clickado
	private Node clickedNode;

	// recibimos el nodo donde colocar la torreta al abrir el menu y abrimos el menu
	void OpenMenu(Node _clickedNode){
		clickedNode = _clickedNode;
	}
	
	// Función para construir la torreta.
	public void BuildTurret(int turret){

		// depende del numero que pongamos en el boton, instancia una torreta u otra
		if (turret == 0) {
			Instantiate (turret1, clickedNode.worldPosition, Quaternion.identity);
		} else if (turret == 1) {
			Instantiate (turret2, clickedNode.worldPosition, Quaternion.identity);
		}

		clickedNode.isBuildableAndHasATurret = true;

		// despues de instanciar la torreta, oculta el menu
		turretSelectionMenu.SetActive (false);
	}

	// Función para mejorar la torreta.
	public void ImproveTurret(){
		Debug.Log ("no hace nada por ahora");

		// despues de mejorar la torreta, oculta el menu
		turretManagementMenu.SetActive (false);
	}

	// Función para destruir la torreta.
	public void DestroyTurret(){

		// nos aseguramos de que exista la torreta a destruir
		if (clickedNode.isBuildableAndHasATurret) {
			Debug.Log ("no hace nada por ahora");
		}

		// si no hay torreta, no hacemos nada
		else {
			Debug.Log("No hay torreta");
		}

		// despues de destruir la torreta, oculta el menu
		turretManagementMenu.SetActive (false);
	}
}
