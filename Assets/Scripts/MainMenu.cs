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
