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

	// variable para pillar el panel de nueva partida
	public GameObject newGamePanel;

	// TODO: esto en principio es para pruebas, kitarlo al terminar
	void Update() {
		GameObject.Find("CurrentPlayer").GetComponent<Text>().text = GameManager.Instance.PlayerName;
	}

	// muestra la pantalla de nueva partida
	public void ShowNewGameScreen() {

		// activa el panel de nueva partida
		newGamePanel.SetActive(true);
	}

	// consigue el nombre del jugador
	public void GetPlayerName(string playerName) {
	
		// guarda el nombre del jugador actual en el GameManager
		GameManager.Instance.PlayerName = playerName;
	}
	// recarga la lista de jugadores
	public void RefreshPlayerList() {

		// saca la lista de jugadores
		List<String> playerList = GameManager.Instance.GetPlayerList ();

		// si la lista de jugadores no es nula
		if (playerList != null) {
		
			// saca el dropdown de la lista de jugadores
			Dropdown dropdown = GameObject.Find ("PlayerList").GetComponent<Dropdown>();

			// borramos lo que haya en el dropdown
			dropdown.options.Clear ();

			// por cada miembro en la lista de jugadores
			foreach (String playerName in playerList) {

				// añade una entrada con su nombre a la lista de jugadores
				dropdown.options.Add (new Dropdown.OptionData () { text = playerName });
			}
		}

		// si la lista de jugadores es nula
		else {
			Debug.Log ("No hay jugadores creados, aún.");
		}
	}
	// consigue el nombre del jugador
	public void Save() {

		// guarda el nombre del jugador actual en el GameManager
		//GameManager.Instance.PlayerName = playerName;
		GameManager.Instance.Save (GameManager.Instance.PlayerName);
	}

	// inicia una nueva partida
	public void StartNewGame(){

		// si tenemos un nombre de jugador
		if (!string.IsNullOrEmpty (GameManager.Instance.PlayerName)) {

			// carga el nivel 1
			SceneManager.LoadScene (1);
		}
	}

	// carga el nivel que le entre como argumento
	public void LoadFirstLevel(int level) {
		SceneManager.LoadScene (level);
	}

	// sale del juego
	public void ExitFromMainMenu(){
		Application.Quit ();
	}
		
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
}
