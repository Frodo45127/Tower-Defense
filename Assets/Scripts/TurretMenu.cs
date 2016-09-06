﻿using UnityEngine;
using System.Collections;

//-----------------------------------------------------------------------
// TurretMenu.cs
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

	// el grid, para poder sacar el nodo donde estan las torretas
	private Grid grid;

	// nodo en el que hemos clickado
	private Node clickedNode;

	// torreta en la que hemos clickado
	private GameObject clickedTurret;

	// cacheamos el grid
	void Awake(){
		grid = GameObject.Find ("Ground").GetComponent<Grid> ();
	}

	// recibimos el nodo donde colocar la torreta al abrir el menu y abrimos el menu
	void OpenTurretSelectionMenu(Node _clickedNode){

		// guardamos el nodo
		clickedNode = _clickedNode;

		// movemos el menu sobre el nodo y hacemos que aparezca
		turretSelectionMenu.transform.position = clickedNode.worldPosition;

		// y ningún menu de torretas esta activo
		if (!turretSelectionMenu.activeSelf && !turretManagementMenu.activeSelf) {

			// activamos el menu
			turretSelectionMenu.SetActive (true);
		}

		// si ya habia un menu activo (cualquiera), lo desactivamos y no hacemos nada mas
		else {
			turretSelectionMenu.SetActive (false);
			turretManagementMenu.SetActive (false);
		}

	}

	// recibimos la torreta en la que hemos clickado al abrir el menu y abrimos el menu
	void OpenTurretManagementMenu(GameObject _clickedTurret){

		// guardamos la torreta
		clickedTurret = _clickedTurret;

		// movemos el menu sobre la torreta y hacemos que aparezca
		turretManagementMenu.transform.position = clickedTurret.transform.position;

		// y ningún menu de torretas esta activo
		if (!turretSelectionMenu.activeSelf && !turretManagementMenu.activeSelf) {

			// activamos el menu
			turretManagementMenu.SetActive (true);
		}

		// si ya habia un menu activo (cualquiera), lo desactivamos y no hacemos nada mas
		else {
			turretSelectionMenu.SetActive (false);
			turretManagementMenu.SetActive (false);
		}
	}

	// Función para construir la torreta.
	public void BuildTurret(int turret){

		// variables temporales para guardar la torreta a construir y la construida
		GameObject turretToBuild;
		GameObject newTurret;

		// depende del numero que pongamos en el boton, instancia una torreta u otra
		if (turret == 0) {
			turretToBuild = turret1;
		} 
		else if (turret == 1) {
			turretToBuild = turret2;
		}
		else {
			Debug.Log ("La torreta no existe.");
			turretToBuild = null;
		}

		// instanciamos la nueva torreta y la guardamos en gameObject
		newTurret = (GameObject)Instantiate (turretToBuild, clickedNode.worldPosition, Quaternion.identity);

		// buscamos la instancia existente del menu de las torretas y se la añadimos
		newTurret.GetComponent<Turret>().turretMenu = GameObject.FindGameObjectWithTag ("TurretMenu");

		// seteamos el nodo como ocupado
		clickedNode.isBuildableAndHasATurret = true;

		// despues de instanciar la torreta, oculta el menu
		turretSelectionMenu.SetActive (false);
	}

	// Función para mejorar la torreta.
	public void ImproveTurret(){
		
		// nos aseguramos de que exista la torreta a mejorar
		if (clickedTurret != null) {
			Debug.Log ("no hace nada por ahora");
		}

		// despues de mejorar la torreta, oculta el menu
		turretManagementMenu.SetActive (false);
	}

	// Función para destruir la torreta.
	public void DestroyTurret(){

		// nos aseguramos de que exista la torreta a destruir
		if (clickedTurret != null) {

			// sacamos el nodo sobre el que esta la torreta
			Node clickedTurretNode = grid.GetNodeFromWorldPosition (clickedTurret.transform.position);

			// destruimos la torreta
			Destroy (clickedTurret);

			// seteamos el nodo como construible
			clickedTurretNode.isBuildableAndHasATurret = false;
		}

		// si no hay torreta, no hacemos nada
		else {
			Debug.Log("No hay torreta");
		}

		// despues de destruir la torreta, oculta el menu
		turretManagementMenu.SetActive (false);
	}
}
