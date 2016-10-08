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

public class MainMenu : MonoBehaviour {

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

	// muestra la pantalla de nueva partida
	public void ShowNewGameScreen() {

		// activa el panel de nueva partida
		newGamePanel.SetActive(true);
	}

	// carga el nivel que le entre como argumento
	public void LoadFirstLevel(int level) {
		SceneManager.LoadScene (level);
	}

	// sale del juego
	public void ExitFromMainMenu(){

		// sale al escritorio
		GameManager.Instance.ExitToDesktop ();
	}
		
	//----------------------------------------
	// Cosas de panel de nueva partida
	//----------------------------------------

	// inicia una nueva partida
	public void StartNewGame(){

		// si tenemos un nombre de jugador
		if (!string.IsNullOrEmpty (GameManager.Instance.PlayerName)) {

			// carga el nivel 1
			SceneManager.LoadScene (1);
		}
	}

	//----------------------------------------
	// Cosas de la cámara
	//----------------------------------------

	// muestra la tabla de puntuaciones
	public void ShowHighScores () {
		// hace que la cámara gire
		GameObject.FindGameObjectWithTag ("MainCamera").SendMessage ("MoveCameraToHighScores");

		// FIXME: esto no hay que descomentarlo hasta que hagamos la tabla de puntuaciones
		// actualiza la lista de puntuación
		//GameObject.FindGameObjectWithTag ("HighScoreList").SendMessage ("UpdateList");
	}

	// muestra los créditos 
	public void ShowCredits (){
		GameObject.FindGameObjectWithTag ("MainCamera").SendMessage ("MoveCameraToCredits");
	}

	//----------------------------------------
	// Cosas de prueba
	//----------------------------------------

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

	// consigue el nombre del jugador
	public void GetPlayerName(string playerName) {

		// guarda el nombre del jugador actual en el GameManager
		GameManager.Instance.PlayerName = playerName;

		// crea el perfil del jugador
		GameManager.Instance.Save (playerName);

		// actualiza la lista de jugadores
		RefreshPlayerList ();
	}

	// cambia al jugador actual
	public void ChangeCurrentPlayer(int index) {

		// sustituye el jugador actual por el nuevo
		GameManager.Instance.PlayerName = playerList[index];

		// y cambiamos al seleccionado del dropdown
		playerListDropdown.value = index;
	}

	// recarga la lista de jugadores
	void RefreshPlayerList() {

		// saca la lista de jugadores
		playerList = GameManager.Instance.GetPlayerList ();

		// limpiamos el dropdown
		playerListDropdown.options.Clear ();

		// añade los jugadores al dropdown
		playerListDropdown.AddOptions (playerList);
	}
}
