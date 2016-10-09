// Copyright © Ismael Gutiérrez González <frodo_gv@hotmail.com>, 2016
//
// This work is free.  You can redistribute it and/or modify it under the
// terms of the Do What The Fuck You Want To But It's Not My Fault Public
// License, Version 1, as published by Ben McGinnes.  See the
// LICENSE.txt file for more details.

using UnityEngine;
// hay que añadir el siguiente namespace para que el SceneManager tire
using UnityEngine.SceneManagement;
using System.Collections;
// hay que añadir el siguiente namespace para poder manipular la UI
using UnityEngine.UI;
// necesario para usar las listas
using System;
using System.Collections.Generic;

//-----------------------------------------------------------------------
// MainMenu.cs
//
// Este script es el que controla lo que hace cada boton del menu principal.
// Para cosas con la camara, envia llamadas a mainMenuCamera.cs.
//
//-----------------------------------------------------------------------

public class MainMenu : MainMenuCommon {

	//----------------------------------------
	// Cosas del menú principal
	//----------------------------------------

	// variable para pillar el panel de nueva partida
	public GameObject newGamePanel, changePlayerPanel;

	// dropdown de la lista de jugadores
	private Dropdown playerListDropdown;

	// lista de jugadores
	private List<String> playerList;

	// cacheamos
	void Start() {
		// mostramos el panel y cacheamos el dropdown
		changePlayerPanel.SetActive (true);
		playerListDropdown = GameObject.Find ("PlayerList").GetComponent<Dropdown> ();

		// actualiza la lista de jugadores
		RefreshPlayerList ();
	}

	//TODO: chapuza de proporciones épicas, arreglar
	void Update() {
		// si no hay ningun jugador guardado
		if (playerList.Count == 0) {
			// no muestres el panel de jugador
			changePlayerPanel.SetActive (false);
		}
		// si hay algún perfil de jugador guardado
		else {
			// y por alguna razón el panel no se ha activado, lo activamos
			if (!changePlayerPanel.activeSelf) {
				changePlayerPanel.SetActive (true);
			}
			// y tenemos un jugador seleccionado
			if (GameManager.Instance.PlayerName != null) {
				// variable para guardar el índice del jugador
				int playerIndex = -1;
				// compara el último jugador con los de la lista
				foreach (String playerName in playerList.ToArray()) {
					// si el jugador coincide con el de la lista
					if (GameManager.Instance.PlayerName == playerName) {
						// saca el índice del jugador
						playerIndex = playerList.IndexOf (playerName);
						// y cambiamos al jugador usado
						ChangeCurrentPlayer (playerIndex);
					}
				}
				// si tras terminar el bucle, no se ha encontrado el índice del jugador
				if (playerIndex == -1) {
					// el jugador ha sido borrado pero sigue como último, luego seteamos jugador nulo
					GameManager.Instance.PlayerName = null;
				}
			}
			// y no tenemos jugador seleccionado
			else {
				// seteamos al primero que haya
				ChangeCurrentPlayer (0);
			}
		}
	}

	// sale del juego
	public void ExitFromMainMenu(){

		// sale al escritorio
		GameManager.Instance.ExitToDesktop ();
	}

	//----------------------------------------
	// Cosas de la cámara
	//----------------------------------------

	// muestra la pantalla de nueva partida 
	public void ShowNewGameScreen (){
		GameObject.FindGameObjectWithTag ("MainCamera").SendMessage ("MoveCamera", 1);
	}

	// muestra la pantalla de carga de niveles
	public void ShowLoadLevelScreen (){
		GameObject.FindGameObjectWithTag ("MainCamera").SendMessage ("MoveCamera", 2);
	}

	// muestra la tabla de puntuaciones
	public void ShowHighScoresScreen () {
		// hace que la cámara gire
		GameObject.FindGameObjectWithTag ("MainCamera").SendMessage ("MoveCamera", 3);

		// FIXME: esto no hay que descomentarlo hasta que hagamos la tabla de puntuaciones
		// actualiza la lista de puntuación
		//GameObject.FindGameObjectWithTag ("HighScoreList").SendMessage ("UpdateList");
	}

	// muestra los créditos 
	public void ShowCreditsScreen (){
		GameObject.FindGameObjectWithTag ("MainCamera").SendMessage ("MoveCamera", 4);
	}

	//----------------------------------------
	// Cosas de prueba
	//----------------------------------------

	// recarga la lista de jugadores
	public void RefreshPlayerList() {
		// saca la lista de jugadores
		playerList = GameManager.Instance.GetPlayerList ();
		// limpiamos el dropdown
		playerListDropdown.options.Clear ();
		// añade los jugadores al dropdown
		playerListDropdown.AddOptions (playerList);
	}

	// cambia al jugador actual
	public void ChangeCurrentPlayer(int index) {
		// sustituye el jugador actual por el nuevo
		GameManager.Instance.PlayerName = playerList[index];
		// cargamos los datos del nuevo jugador
		GameManager.Instance.Load (GameManager.Instance.PlayerName);
		// actualiza la lista de niveles desbloqueados del jugador
		GameObject.Find ("CanvasLoadLevel").GetComponent<MainMenuLoadLevel> ().UnlockLevel (GameManager.Instance.HighestLevelCompleted);
		// y cambiamos al seleccionado del dropdown
		playerListDropdown.value = index;
	}

	// borra al jugador seleccionado
	public void DeletePlayer() {
		// borra al jugador seleccionado
		GameManager.Instance.Delete (GameManager.Instance.PlayerName);
		// actualiza la lista de jugadores
		RefreshPlayerList ();
		// si sigue habiendo jugadores en la lista
		if (playerList.Count > 0) {
			//seteamos al jugador 0 como nuevo jugador
			ChangeCurrentPlayer (0);
		}
	}
}
