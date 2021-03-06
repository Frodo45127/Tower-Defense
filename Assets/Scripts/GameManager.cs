﻿// Copyright © Ismael Gutiérrez González <frodo_gv@hotmail.com>, 2016
//
// This work is free.  You can redistribute it and/or modify it under the
// terms of the Do What The Fuck You Want To But It's Not My Fault Public
// License, Version 1, as published by Ben McGinnes.  See the
// LICENSE.txt file for more details.

using UnityEngine;
// hay que añadir el siguiente namespace para que el SceneManager tire
using UnityEngine.SceneManagement;
using System.Collections;
// necesitamos estos namespaces para el guardado y el cargado de partidas
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
// necesario para usar las listas
using System.Collections.Generic;

//-----------------------------------------------------------------------
// GameManager.cs
//
// Este script es el que controla todo lo que tiene que ocurrir de manera
// coherente entre escenas (coordina el juego al completo).
//
//-----------------------------------------------------------------------

public class GameManager : MonoBehaviour {

	// con esto ponemos en marcha el singleton
	// solo sirve para cosas con una instancia (puntos, vidas spawner,...)
	public static GameManager _instance;

	public static GameManager Instance {
		get {
			if (_instance == null) {
				GameObject go = new GameObject ("GameManager");
				go.AddComponent<GameManager> ();
			}
			return _instance;
		}
	}

	// con esto despierta el singleton, y si existe otro distinto se lo ventila (en teoria)
	// la alternativa es _instance = this;
	void Awake(){
		if (_instance == null) {
			_instance = this;            
		} 
		else if (_instance != this) {
			Destroy (gameObject);
		}
		// le decimos que no desaparezca al cambiar de escena
		DontDestroyOnLoad (gameObject);
	}

	// NOTA: El objeto GameManager se crea siempre que algo llame a algo de este script
	// aquí empezamos a definir las variables y sus propiedades

	//---------------------------
	// Variables para los menus
	//---------------------------

	// variable necesaria para el arranque
	public bool IsFirstStart { get; set;}

	//---------------------------
	// Info del jugador
	//---------------------------

	// nombre del jugador
	public string PlayerName { get; set;}

	// máximo nivel completado
	public int HighestLevelCompleted { get; set;}

	// nivel actual
	public int CurrentLevel { get; set;}

	// maxima puntuación por nivel
	public List<int> MaxScorePerLevel { get; set;}

	//---------------------------
	// Funciones del GameManager
	//---------------------------

	// inicializamos con el constructor al arrancar el juego
	private GameManager(){
		// variable para que el logo no salga mas de una vez al volver al menu
		IsFirstStart = true;
	}
		
	void Start() {
		// saca el nombre del ultimo jugador si existe
		if (PlayerPrefs.HasKey("LastPlayer")) {
			PlayerName = PlayerPrefs.GetString ("LastPlayer");
		}
		// si no hay último jugador
		else {
			Debug.Log ("No hay ultimo jugador");
		}
	}

	// funcion para salir al menu principal
	public void ExitToMainMenu() {
		// Se asegura de que el tiempo funciona, pues esto se llama normalmente con el tiempo parado
		Time.timeScale = 1;
		// Guardamos el progreso del jugador actual
		Save (PlayerName);
		// Reseteamos el nivel actual a 0
		CurrentLevel = 0;
		// Volvemos al menú principal
		SceneManager.LoadScene (0);
	}

	// funcion para salir del juego al escritorio
	public void ExitToDesktop() {
		// si tenemos un jugador lo guardamos como último jugador
		if (PlayerName != null) {
			PlayerPrefs.SetString ("LastPlayer", PlayerName);
		}
		PlayerPrefs.Save ();
		// y salimos del juego
		Application.Quit ();
	}

	// función para guardar la partida
	public void Save(string playerName) {
		// si no existe la carpeta de perfiles de jugador
		if (!Directory.Exists (Application.persistentDataPath + "/PlayerProfiles/")) {
			// la creamos
			Directory.CreateDirectory (Application.persistentDataPath + "/PlayerProfiles/");
		}
		// sacamos un binaryFormatter
		BinaryFormatter bf = new BinaryFormatter();
		// creamos el archivo en el que vamos a guardar
		FileStream file = File.Create (Application.persistentDataPath + "/PlayerProfiles/" + playerName + ".dat");
		// creamos una instancia de la clase que vamos a meter en el guardado
		PlayerData playerData = new PlayerData ();
		// despues de que la clase haya cogido todo lo necesario para el guardado, serializamos
		bf.Serialize (file, playerData);
		// y cerramos el archivo
		file.Close();
	}

	// función para cargar la partida guardada
	public void Load(string playerName) {
		// si no existe la carpeta de perfiles de jugador
		if (!Directory.Exists (Application.persistentDataPath + "/PlayerProfiles/")) {
			// la creamos
			Directory.CreateDirectory (Application.persistentDataPath + "/PlayerProfiles/");
		}
		// si el archivo a cargar existe
		if (File.Exists(Application.persistentDataPath + "/PlayerProfiles/" + playerName + ".dat")) {
			// sacamos un binaryFormatter
			BinaryFormatter bf = new BinaryFormatter();
			// abrimos el archivo
			FileStream file = File.Open (Application.persistentDataPath + "/PlayerProfiles/" + playerName + ".dat", FileMode.Open);
			// deserializamos los datos del jugador
			PlayerData playerData = (PlayerData)bf.Deserialize (file);
			// pillamos los datos del archivo guardado
			PlayerName = playerData.playerName;
			HighestLevelCompleted = playerData.highestLevelBeated;
			MaxScorePerLevel = playerData.maxScorePerLevel;
			// y cerramos el archivo
			file.Close();
		}
		// si el archivo no existe
		else {
			Debug.Log ("El usuario " + playerName + " no existe.");
		}
	}

	// función para borrar la partida
	public void Delete(string playerName) {
		// seteamos el jugador a nulo
		PlayerName = null;
		// si existe la carpeta de perfiles de jugador
		if (Directory.Exists (Application.persistentDataPath + "/PlayerProfiles/")) {
			// borramos el archivo de perfil del jugador
			File.Delete (Application.persistentDataPath + "/PlayerProfiles/" + playerName + ".dat");
		}
		// si no existe
		else {
			Debug.Log ("Error, carpeta de perfiles no encontrada.");
		}
	}

	// función para mostrar una lista con todos los perfiles de jugador guardados
	public List<String> GetPlayerList () {
		// si existe la carpeta de perfiles de jugador
		if (Directory.Exists (Application.persistentDataPath + "/PlayerProfiles/")) {
			// hacemos la lista de nombres
			List<String> playerList = new List<String>();
			// por cada archivo de guardado que haya
			foreach (String fileName in Directory.GetFiles(Application.persistentDataPath + "/PlayerProfiles/")) {
				// sacamos el nombre del jugador
				String playerName = Path.GetFileNameWithoutExtension (Application.persistentDataPath + "/PlayerProfiles/" + fileName);
				// si el jugador tiene un nombre vacío
				if (playerName.Length == 0) {
					// te lo saltas
					continue;
				}
				// y añadimos ese jugador a la lista
				playerList.Add (playerName);
			}
			// si la lista esta vacía
			if (playerList == null) {
				// no hay jugadores guardados todavia
				Debug.Log ("No hay perfiles creados.");
				return null;
			} 
			// si la lista no está vacía
			else {
				// al terminar, devuelve la lista de jugadores
				return playerList;
			}
		}
		// si no existe la carpeta de perfiles
		else {
			Debug.Log ("No hay perfiles creados.");
			return null;
		}
	}
}

// clase privada serializable para guardar y cargar los datos
[Serializable]
class PlayerData {

	// los datos que queremos guardar de cada partida
	public string playerName;
	public int highestLevelBeated;
	public List<int> maxScorePerLevel;

	// con un constructor pillamos los datos a guardar automáticamente
	public PlayerData() {
		playerName = GameManager.Instance.PlayerName;
		highestLevelBeated = GameManager.Instance.HighestLevelCompleted;
		maxScorePerLevel = GameManager.Instance.MaxScorePerLevel;
	}
}