﻿using UnityEngine;
using System.Collections;
// necesario para usar las listas
using System.Collections.Generic;

//-----------------------------------------------------------------------
// TurretMenu.cs
// 
// Este script es el que se encarga de manejar el menu que sale al clickar
// en un nodo o en una torreta
//
//-----------------------------------------------------------------------

public class TurretMenu : MonoBehaviour {

	// añadimos las torretas a instanciar
	public GameObject turret1, turret2, turret3;

	// y el array con las barricadas a usar
	// ATENCION: el orden de las barricadas es el siguiente:
	// - 0: horizontal
	// - 1: vertical
	// - 2: inclinada de arriba a abajo
	// - 3: inclinada de abajo a arriba
	public GameObject[] barricadeList;

	// menu de seleccion de torretas y menu de manejo de torretas
	public GameObject turretSelectionMenu;
	public GameObject turretManagementMenu;

	// el grid, para poder sacar el nodo donde estan las torretas
	private Grid grid;

	// nodo en el que hemos clickado
	private Node clickedNode;

	// torreta en la que hemos clickado
	private GameObject clickedTurret;

	// cacheamos el grid y el dinero
	void Awake(){
		grid = GameObject.Find ("Ground").GetComponent<Grid> ();
	}

	// recibimos el nodo donde colocar la torreta al abrir el menu y abrimos el menu
	void OpenTurretSelectionMenu(Node _clickedNode){

		// guardamos el nodo
		clickedNode = _clickedNode;

		// movemos el menu sobre el nodo y hacemos que aparezca
		turretSelectionMenu.transform.position = new Vector3(clickedNode.worldPosition.x, clickedNode.worldPosition.y, -8f);

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
		turretManagementMenu.transform.position = new Vector3(clickedTurret.transform.position.x, clickedTurret.transform.position.y, -8f);

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

		// variables temporales para guardar la torreta a construir, la construida, y su coste
		GameObject turretToBuild;
		GameObject newTurret;
		int costNewTurret;

		// depende del numero que pongamos en el boton, instancia una torreta u otra
		if (turret == 0) {
			turretToBuild = turret1;
			costNewTurret = turret1.GetComponent<TurretTest>().Cost;
		} 
		else if (turret == 1) {
			turretToBuild = turret2;
			costNewTurret = turret2.GetComponent<TurretTest2>().Cost;
		}
		else if (turret == 2) {
			turretToBuild = turret3;
			costNewTurret = turret3.GetComponent<AATurretTest>().Cost;
		}
		else {
			Debug.Log ("La torreta no existe.");
			turretToBuild = null;
			costNewTurret = 0;
		}

		// si tenemos suficiente dinero para construirla
		if (costNewTurret <= GameManager.Instance.Money){
			
			// instanciamos la nueva torreta y la guardamos en gameObject
			newTurret = (GameObject)Instantiate (turretToBuild, clickedNode.worldPosition, Quaternion.identity);

			// buscamos la instancia existente del menu de las torretas y se la añadimos
			newTurret.GetComponent<Turret>().turretMenu = GameObject.FindGameObjectWithTag ("TurretMenu");

			// seteamos el nodo como ocupado
			clickedNode.isBuildableAndHasATurret = true;

			// reducimos el dinero que tenemos porque nos lo hemos gastado
			GameManager.Instance.Money -= costNewTurret;

			// despues de instanciar la torreta, oculta el menu
			turretSelectionMenu.SetActive (false);			
		}
		else {
			Debug.Log("nos falta dinero");
		}

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

		// despues de destruir la torreta, oculta el menu
		turretManagementMenu.SetActive (false);
	}

	// Funcion para construir barricadas
	public void BuildBarricade(Node _clickedNode) {

		// guardamos el nodo
		clickedNode = _clickedNode;

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
}
