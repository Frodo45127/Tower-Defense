// Copyright © Ismael Gutiérrez González <frodo_gv@hotmail.com>, 2016
//
// This work is free.  You can redistribute it and/or modify it under the
// terms of the Do What The Fuck You Want To But It's Not My Fault Public
// License, Version 1, as published by Ben McGinnes.  See the
// LICENSE.txt file for more details.

using UnityEngine;
using System.Collections;
// necesario para usar listas
using System.Collections.Generic;
// necesario para la clase String
using System;
// hay que añadir el siguiente namespace para poder manipular la UI
using UnityEngine.UI;


//-----------------------------------------------------------------------
// MainMenuCredits.cs
//
// Este script es el que controla la pantalla de creditos.
// Tambien controla el boton de regreso al menu principal.
//
//-----------------------------------------------------------------------


public class MainMenuCredits : MainMenuCommon {

	// archivo con los créditos
	public TextAsset creditsFile;

	// créditos importados del archivo, en una linea
	private String creditsImported;

	// creditos importador, separados por líneas
	public List<String> creditsProcesed;

	// gameobject donde mostrar los créditos
	public GameObject creditsList;

	void Start() {
		// saca los créditos y muestralos
		GiveMeCredits ();
	}

	// función para sacar los créditos
	void GiveMeCredits() {
		// saca el texto del archivo en una línea
		creditsImported = creditsFile.text;
		// inicializa la lista
		creditsProcesed = new List<String>();
		// añade todo el texto separado por intros a la lista
		creditsProcesed.AddRange (creditsImported.Split ("\n" [0]));
		// seteamos el tamaño inicial del campo de créditos
		int creditsVerticalSize = 20;
		// por cada línea que haya en los créditos
		for (int i = 0; i < creditsProcesed.Count; i++) {
			creditsVerticalSize += 20;
		}
		// setea el nuevo tamaño del texto
		creditsList.GetComponent<RectTransform> ().sizeDelta = new Vector2 (creditsList.GetComponent<RectTransform> ().rect.width, creditsVerticalSize);
		// muestra los creditos
		creditsList.GetComponent<Text> ().text = creditsImported;
	}
}
