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

//FIXME: arreglar esto cuando montemos el nivel de pruebas
public class InGameMenu : MonoBehaviour {

	// paneles hijos del canvas de los menus
	public GameObject pauseMenu, levelCompletedMenu, gameOverMenu;

	// variable interna para la puntuacion
	private string score;

	// texto en el que van las barricadas restantes
	private Text barricadeCounter;

	void Awake() {
		barricadeCounter = GameObject.Find ("BarricadesLeft").GetComponent<Text> ();
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
		if (!GameManager.Instance.isPlayerReady) {

			// manten actualizado el contador de barricadas
			barricadeCounter.text = "Te quedan " + GameManager.Instance.Barricades + " barricadas.";
		}

		// si pulsamos escape, pausa el juego
		if (Input.GetKeyDown(KeyCode.Escape)){
			PauseGame ();
		}
	}

	// función para dar comienzo a la partida, tras colocar las barricadas
	public void StartGame() {

		// dile al gamemanager que estamos listos para empezar
		GameManager.Instance.isPlayerReady = true;

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
			score = GameManager.Instance.Score.ToString();
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
		score = GameManager.Instance.Score.ToString();
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
		score = GameManager.Instance.Score.ToString();
		// busca su text y le cuela los datos guardados
		GameObject.Find("LevelCompletedMenuScore").GetComponent<Text>().text = "Puntos: " + score;
	}

	// funcion para salir al menu
	public void ExitToMenu () {
		GameManager.Instance.ExitToMainMenu();
	}
}
