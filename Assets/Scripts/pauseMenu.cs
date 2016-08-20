using UnityEngine;
// hay que añadir el siguiente namespace para poder manipular el SceneManager
using UnityEngine.SceneManagement;
// hay que añadir el siguiente namespace para poder manipular la UI
using UnityEngine.UI;
using System.Collections;

//-----------------------------------------------------------------------
// pauseMenu.cs
//
// Este script es el que controla el menu de pausa.
//
//-----------------------------------------------------------------------

//FIXME: arreglar esto cuando montemos el nivel de pruebas
public class pauseMenu : MonoBehaviour {

	// panel hijo del menu pausa, no el menu pausa en sí ke si no no va
	public GameObject PauseMenu;

	// variable interna para la puntuacion
	private string score;

	// Use this for initialization
	void Start () {

		// esconde por defecto el menu del juego
		PauseMenu.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		// si pulsamos escape, pausa el juego
		if (Input.GetKeyDown(KeyCode.Escape)){
			PauseGame ();
		}
	}

	// función que controla el pausado del juego
	public void PauseGame(){
		// si el juego funciona, paralo
		if (Time.timeScale == 1) {
			Time.timeScale = 0;
			// haz el menu de pausa visible
			PauseMenu.SetActive (true);
			// pilla los datos de la puntuación
			score = gameManager.Instance.Score.ToString();
			// busca su text y le cuela los datos guardados
			GameObject.FindGameObjectWithTag("CurrentScorePauseMenu").GetComponent<Text>().text = "Puntos: " + score;
		}
		// si el juego está pausado, despausalo
		else if (Time.timeScale == 0) {
			Time.timeScale = 1;
			// esconde el menu del juego
			PauseMenu.SetActive (false);
		}
	}
	// funcion para salir al menu
	public void ExitToMenu () {
		gameManager.Instance.ExitToMainMenu();
	}
}
