// Copyright © Ismael Gutiérrez González <frodo_gv@hotmail.com>, 2016
//
// This work is free.  You can redistribute it and/or modify it under the
// terms of the Do What The Fuck You Want To But It's Not My Fault Public
// License, Version 1, as published by Ben McGinnes.  See the
// LICENSE.txt file for more details.

using UnityEngine;
using System.Collections;
// hay que añadir el siguiente namespace para poder manipular la UI
using UnityEngine.UI;

//-----------------------------------------------------------------------
// MainMenuScoreList.cs
//
// Este script es el que controla la actualizacion de la tabla de puntuaciones.
// Tambien controla el boton de regreso al menu principal.
//
//-----------------------------------------------------------------------

public class MainMenuScoreList : MainMenuCommon {

	// variables para almacenar los datos del jugador y su puntuación
	private string playerName;
	private string playerScore;

	// función para recargar la lista. Se ejecuta al pulsar el botón en el mainMenu
	void UpdateList () {

		// actualiza todos los campos de la lista con un bucle
		for (int i = 1; i < 11; i++){

			// si el puesto ya está creado, coge los datos del jugador
			if (PlayerPrefs.HasKey(i+"Player")){
				// pilla los datos guardados
				playerName = PlayerPrefs.GetString (i + "Player");
				playerScore = PlayerPrefs.GetInt (i + "Score").ToString();
				// busca su text y le cuela los datos guardados
				GameObject.Find(i+"PlayerName").GetComponent<Text>().text = playerName;
				GameObject.Find(i+"PlayerScore").GetComponent<Text>().text = playerScore;
				continue;
			}
			// si no, pon guiones
			else {
				playerName = "----";
				playerScore = "----";
				// busca su text y le cuela los datos guardados
				GameObject.Find(i+"PlayerName").GetComponent<Text>().text = playerName;
				GameObject.Find(i+"PlayerScore").GetComponent<Text>().text = playerScore;
				continue;
			}
		}
	}
}
