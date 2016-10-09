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
// necesario para usar la clase String
using System;

//-----------------------------------------------------------------------
// MainMenuScoreList.cs
//
// Este script es el que controla la actualizacion de la tabla de puntuaciones.
// Tambien controla el boton de regreso al menu principal.
//
//-----------------------------------------------------------------------

public class MainMenuScoreList : MainMenuCommon {

	// función para actualizar la lista. Se ejecuta al pulsar el botón en el mainMenu
	void UpdateList () {
		// si tenemos jugador
		if (GameManager.Instance.PlayerName != null) {
			// por cada nivel que tengamos
			foreach (int maxScore in GameManager.Instance.MaxScorePerLevel) {
				// si es la posición 0 nos la saltamos
				if (GameManager.Instance.MaxScorePerLevel.IndexOf (maxScore) == 0) {
					continue;
				}
				// busca el texto de la puntuación de dicho nivel
				GameObject go = GameObject.Find ("MaxScoreLevel" + GameManager.Instance.MaxScorePerLevel.IndexOf (maxScore));
				// y pon la máxima puntuación
				go.GetComponent<Text> ().text = maxScore.ToString();
			}
		}
		// si el jugador es nulo (no tenemos)
		else {
			// busca el texto de la puntuación de nivel
			GameObject[] goArray = GameObject.FindGameObjectsWithTag ("HighscoreTableScoreField");
			// coge cada uno de los niveles
			foreach (GameObject go in goArray) {
				// y pon su puntuacion como ----
				go.GetComponent<Text> ().text = "----";
			}
		}
	}
}
