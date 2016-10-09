// Copyright © Ismael Gutiérrez González <frodo_gv@hotmail.com>, 2016
//
// This work is free.  You can redistribute it and/or modify it under the
// terms of the Do What The Fuck You Want To But It's Not My Fault Public
// License, Version 1, as published by Ben McGinnes.  See the
// LICENSE.txt file for more details.

using UnityEngine;
// hay que añadir el siguiente namespace para poder manipular el SceneManager
using UnityEngine.SceneManagement;
// hay que añadir el siguiente namespace para poder manipular la UI
using UnityEngine.UI;
using System.Collections;

//-----------------------------------------------------------------------
// InGameMenu.cs
//
// Este script es el que controla todos los menus que hay en un nivel
// (el menu de pausa, el de game over, el de nivel completado,...).
//
//-----------------------------------------------------------------------

public class InGameMenu : MonoBehaviour {

	//TODO: hacer que esto lo encuentre automaticamente
	// paneles hijos del canvas de los menus
	public GameObject pauseMenu, levelCompletedMenu, gameOverMenu;

	// variable interna para la puntuacion
	private int score;

	// texto en el que van las barricadas restantes
	private Text barricadeCounter;
	private Text scoreCounter;
	private Text moneyCounter;

	void Awake() {
		barricadeCounter = GameObject.Find ("BarricadesLeft").GetComponent<Text> ();
		scoreCounter = GameObject.Find ("InGameUIScore").GetComponent<Text> ();
		moneyCounter = GameObject.Find ("InGameUIMoney").GetComponent<Text> ();
	}

	// Use this for initialization
	void Start () {

		// esconde por defecto todos los menus del juego
		pauseMenu.SetActive (false);
		levelCompletedMenu.SetActive (false);
		gameOverMenu.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		// si todavia no hemos empezado
		if (!LevelManager.Instance.isPlayerReady) {

			// manten actualizado el contador de barricadas
			barricadeCounter.text = "Te quedan " + LevelManager.Instance.Barricades + " barricadas.";
		}

		// actualizamos todo el rato el contador de puntos y dinero
		scoreCounter.text = LevelManager.Instance.Score.ToString();
		moneyCounter.text = LevelManager.Instance.Money.ToString();

		// si pulsamos escape, pausa el juego
		if (Input.GetKeyDown(KeyCode.Escape)){
			PauseGame ();
		}
	}

	// función para seleccionar una torreta
	public void SelectTurret(int selectedTurret) {

		// dependiendo del botón al que le demos, cojemos una torreta u otra
		switch (selectedTurret) {

		// caso -1, destruye la torreta en la que hagas click
		case -1:
			LevelManager.Instance.SelectedTurret = -1;
			break;
		// caso 0, limpieza de la torreta seleccionada
		case 0:
			LevelManager.Instance.SelectedTurret = 0;
			break;
		case 1:
			LevelManager.Instance.SelectedTurret = 1;
			break;
		case 2:
			LevelManager.Instance.SelectedTurret = 2;
			break;
		case 3:
			LevelManager.Instance.SelectedTurret = 3;
			break;
		default:
			LevelManager.Instance.SelectedTurret = 0;
			Debug.Log ("Error, torreta seleccionada no valida");
			break;
		}
	}

	// función para dar comienzo a la partida, tras colocar las barricadas
	public void StartGame() {

		// dile al gamemanager que estamos listos para empezar
		LevelManager.Instance.isPlayerReady = true;

		// y oculta la UI que permite iniciar el juego (el boton y el contador de barricadas)
		GameObject.Find ("StartGameMenu").SetActive(false);
	}

	// función que controla el pausado del juego
	public void PauseGame(){
		// si el juego funciona, paralo
		if (Time.timeScale == 1) {
			Time.timeScale = 0;
			// haz el menu de pausa visible
			pauseMenu.SetActive (true);
			// pilla los datos de la puntuación
			score = LevelManager.Instance.Score;
			// busca su text y le cuela los datos guardados
			GameObject.Find("PauseMenuCurrentScore").GetComponent<Text>().text = "Puntos: " + score;
		}
		// si el juego está pausado, despausalo
		else if (Time.timeScale == 0) {
			Time.timeScale = 1;
			// esconde el menu del juego
			pauseMenu.SetActive (false);
		}
	}

	// funcion que controla que pasa con los menus al perder
	public void GameOver(){
		// si el juego funciona, paralo
		if (Time.timeScale == 1) {
			Time.timeScale = 0;
		}
		// haz el menu de game over visible
		gameOverMenu.SetActive (true);
		// pilla los datos de la puntuación
		score = LevelManager.Instance.Score;
		// busca su text y le cuela los datos guardados
		GameObject.Find("GameOverMenuScore").GetComponent<Text>().text = "Puntos: " + score;
	}

	// funcion que controla que pasa con los menus al ganar
	public void LevelCompleted(){
		// si el juego funciona, paralo
		if (Time.timeScale == 1) {
			Time.timeScale = 0;
		}
		// haz el menu de nivel completado visible
		levelCompletedMenu.SetActive (true);
		// pilla los datos de la puntuación
		score = LevelManager.Instance.Score;
		// si la puntuación conseguida es mayor que la conseguida hasta la fecha en este nivel
		if (score > GameManager.Instance.MaxScorePerLevel[GameManager.Instance.CurrentLevel]) {;
			// pon la nueva puntuación conseguida como la mayor
			GameManager.Instance.MaxScorePerLevel [GameManager.Instance.CurrentLevel] = score;
		}
		// si el nivel completado es mayor que el mayor nivel completado hasta la fecha
		if (GameManager.Instance.CurrentLevel > GameManager.Instance.HighestLevelCompleted) {
			// este es el nuevo mayor nivel completado
			GameManager.Instance.HighestLevelCompleted = GameManager.Instance.CurrentLevel;
		}
		// busca su text y le cuela los datos guardados
		GameObject.Find("LevelCompletedMenuScore").GetComponent<Text>().text = "Puntos: " + score;
	}

	// funcion para salir al menu
	public void ExitToMenu () {
		GameManager.Instance.ExitToMainMenu();
	}
}
