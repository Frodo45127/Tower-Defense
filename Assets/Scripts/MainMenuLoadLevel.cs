// Copyright © Ismael Gutiérrez González <frodo_gv@hotmail.com>, 2016
//
// This work is free.  You can redistribute it and/or modify it under the
// terms of the Do What The Fuck You Want To But It's Not My Fault Public
// License, Version 1, as published by Ben McGinnes.  See the
// LICENSE.txt file for more details.

using UnityEngine;
using System.Collections;
// hay que añadir el siguiente namespace para que el SceneManager tire
using UnityEngine.SceneManagement;
// hay que añadir el siguiente namespace para poder manipular la UI
using UnityEngine.UI;
// necesario para usar las listas
using System.Collections.Generic;

//-----------------------------------------------------------------------
// MainMenuLoadLevel.cs
//
// Este script es el que controla la pantalla de carga de niveles.
// Tambien controla el boton de regreso al menu principal.
//
//-----------------------------------------------------------------------

public class MainMenuLoadLevel : MainMenuCommon {

	// lista de botones de cargar nivel, en orden
	public List<Button> LevelList;

	// desbloquea los niveles en función de los que haya completado el jugador
	public void UnlockLevel(int maxLevelCompleted) {
		// por cada botón en la lista de niveles
		foreach (Button button in LevelList) {
			// comprobamos si es igual o menor al mayor nivel completado
			if (LevelList.IndexOf(button) <= maxLevelCompleted) {
				// si lo es, desbloquealo
				button.interactable = true;
			}
			// si no lo es
			else {
				// bloquealo
				button.interactable = false;
			}
		}
	}

	// carga el nivel que le entre como argumento
	public void LoadLevel(int level) {
		// setea el nivel actual
		GameManager.Instance.CurrentLevel = level;
		// carga el nivel
		SceneManager.LoadScene (level);
	}
}
