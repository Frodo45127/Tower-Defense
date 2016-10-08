// Copyright © Ismael Gutiérrez González <frodo_gv@hotmail.com>, 2016
//
// This work is free.  You can redistribute it and/or modify it under the
// terms of the Do What The Fuck You Want To But It's Not My Fault Public
// License, Version 1, as published by Ben McGinnes.  See the
// LICENSE.txt file for more details.

using UnityEngine;
using System.Collections;

//-----------------------------------------------------------------------
// MainMenuCredits.cs
//
// Este script es el que controla la pantalla de creditos.
// Tambien controla el boton de regreso al menu principal.
//
//-----------------------------------------------------------------------

public class MainMenuCredits : MonoBehaviour {

	// función para decirle a la cámara que vuelva al menú principal
	public void ReturnToMainMenu() {
		GameObject.FindGameObjectWithTag ("MainCamera").SendMessage ("MoveCameraToMainMenu");
	}
}
