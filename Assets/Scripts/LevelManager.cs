// Copyright © Ismael Gutiérrez González <frodo_gv@hotmail.com>, 2016
//
// This work is free.  You can redistribute it and/or modify it under the
// terms of the Do What The Fuck You Want To But It's Not My Fault Public
// License, Version 1, as published by Ben McGinnes.  See the
// LICENSE.txt file for more details.

using UnityEngine;
using System.Collections;

//-----------------------------------------------------------------------
// LevelManager.cs
//
// Este script es el que controla todo lo que tiene que ocurrir de manera
// coherente en un nivel (coordina el nivel, por asi decirlo).
//
//-----------------------------------------------------------------------

public class LevelManager : MonoBehaviour {

	// con esto ponemos en marcha el singleton
	// solo sirve para cosas con una instancia (puntos, vidas spawner,...)
	public static LevelManager _instance;

	public static LevelManager Instance {
		get {
			if (_instance == null) {
				GameObject go = new GameObject ("LevelManager");
				go.AddComponent<LevelManager> ();
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
	}
		
	// aquí empezamos a definir las variables y sus propiedades

	//---------------------------
	// Variables para la partida
	//---------------------------

	// variables necesarias para la partida
	[SerializeField]
	private int money;
	public int Money { 
		get {
			return money;
		} 
		set {
			money = value;
		}
	}
	[SerializeField]
	private int score;
	public int Score {
		get {
			return score;
		} 
		set {
			score = value;
		}
	}
	// estos son los puntos de vida de tu base
	[SerializeField]
	private int baseHealthPoints;
	public int BaseHealthPoints {
		get {
			return baseHealthPoints;
		} 
		set {
			baseHealthPoints = value;
		}
	}
	// estas son las barricadas que te quedan
	[SerializeField]
	private int barricades;
	public int Barricades {
		get {
			return barricades;
		} 
		set {
			barricades = value;
		}
	}

	// variable para saber si estamos en fase de preparación o de juego
	public bool isPlayerReady;

	// propiedad para saber que torreta tenemos clickeada y si ha cambiado
	[SerializeField]
	public bool hasSelectedTurretChanged;
	private int selectedTurret;
	public int SelectedTurret {
		get { 
			return selectedTurret;
		}
		set {
			if (selectedTurret != value) {
				selectedTurret = value;
				hasSelectedTurretChanged = true;
			}
		}
	}

	// funcion para reducir la vida de la base y perder la partida
	public void EnemyAtTheGates(int damage) {

		// si llega un enemigo a la base, perdemos vida
		BaseHealthPoints -= damage;

		// si nos quedamos sin puntos, hemos perdio
		if (BaseHealthPoints <= 0) {
			Debug.Log ("Hemos perdio.");

			// ejecuta un gameover de manual
			GameObject.Find ("InGameUI").GetComponent<InGameMenu> ().GameOver ();
		}
	}

	// funcion para resetear las variables de la partida (dinero, puntos,...)
	public void SetLevelVariables(int _money, int _score, int _baseHealthPoints, int _barricades) {
		// Reinicializamos todas las variables del jugador para la siguiente partida
		isPlayerReady = false;
		SelectedTurret = 0;
		Money = _money;
		Score = _score;
		BaseHealthPoints = _baseHealthPoints;
		Barricades = _barricades;
	}
}
