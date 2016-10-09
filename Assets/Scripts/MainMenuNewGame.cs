// Copyright © Ismael Gutiérrez González <frodo_gv@hotmail.com>, 2016
//
// This work is free.  You can redistribute it and/or modify it under the
// terms of the Do What The Fuck You Want To But It's Not My Fault Public
// License, Version 1, as published by Ben McGinnes.  See the
// LICENSE.txt file for more details.

using UnityEngine;
using System.Collections;
// necesario para manipular la lista de máximas puntuaciones por nivel
using System.Collections.Generic;
// hay que añadir el siguiente namespace para que el SceneManager tire
using UnityEngine.SceneManagement;

//-----------------------------------------------------------------------
// MainMenuNewGame.cs
//
// Este script es el que controla la pantalla de nueva partida.
// Tambien controla el boton de regreso al menu principal.
//
//-----------------------------------------------------------------------

public class MainMenuNewGame : MainMenuCommon {

	// consigue el nombre del jugador
	public void GetPlayerName(string playerName) {
		// guarda el nombre del jugador actual en el GameManager
		GameManager.Instance.PlayerName = playerName;
		// setea el mayor nivel completado de ese jugador a 0
		GameManager.Instance.HighestLevelCompleted = 0;
		// crea su lista de máximas puntuaciones por nivel (cantidad de niveles + 1)
		GameManager.Instance.MaxScorePerLevel = new List<int> (new int[11]);
		// crea el perfil del jugador
		GameManager.Instance.Save (playerName);
		// actualiza la lista de jugadores
		GameObject.Find("CanvasMainMenu").GetComponent<MainMenu>().RefreshPlayerList();
	}

	// inicia una nueva partida
	public void StartNewGame(){
		// si tenemos un nombre de jugador
		if (!string.IsNullOrEmpty (GameManager.Instance.PlayerName)) {
			// setea el nivel actual a 1
			GameManager.Instance.CurrentLevel = 1;
			// carga el nivel 1
			SceneManager.LoadScene (1);
		}
	}
}
