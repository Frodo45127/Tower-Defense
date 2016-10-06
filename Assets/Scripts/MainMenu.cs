using UnityEngine;
// hay que añadir el siguiente namespace para que el SceneManager tire
using UnityEngine.SceneManagement;
using System.Collections;

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
