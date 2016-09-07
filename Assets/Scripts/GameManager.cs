using UnityEngine;
// hay que añadir el siguiente namespace para que el SceneManager tire
using UnityEngine.SceneManagement;
using System.Collections;

//-----------------------------------------------------------------------
// GameManager.cs
//
// Este script es el que controla todo lo que tiene que ocurrir de manera
// coherente entre escenas (conservar puntos, guardar puntuaciones, etc)
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
	// variable necesaria para el arranque
	public bool IsFirstStart { get; set;}

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
	public string PlayerName { get; set;}


	// inicializamos la variable para el logo con el constructor
	public GameManager(){
		IsFirstStart = true;
	}

	//inicializamos las variables únicas de la partida
	void Start(){
		ResetLevel ();
	}

	// funcion para resetear las variables de la partida (dinero, puntos,...)
	public void ResetLevel() {
		// Reinicializamos todas las variables del jugador para la siguiente partida
		Money = 800;
		Score = 0;
		PlayerName = "";
	}

	// funcion para salir al menu principal
	public void ExitToMainMenu() {
		// Se asegura de que el tiempo funciona, pues esto se llama normalmente con el tiempo parado
		Time.timeScale = 1;
		// Si hay un nombre escrito en el campo de jugador, guarda su puntuación
		if (string.IsNullOrEmpty(GameManager.Instance.PlayerName)) {
			// do nothing
		}	
		else {
			AddToHighScoreListSorted (PlayerName, Score);
		}
		// Al salir reinicializamos todas las variables del jugador para la siguiente partida
		ResetLevel ();
		// Volvemos al menú, después de dejar todo listo para otra partida.
		SceneManager.LoadScene (0);
	}
		
	// funcion para añadir una puntuación a la lista de puntuaciones, ordenadas de mayor a menor.
	void AddToHighScoreListSorted (string playerName, int score){
		// haz un bucle con todos los jugadores guardados (max i = 10)
		for (int i = 1; i < 11; i++){
			// si el puesto del jugador existe (porque hay suficientes jugadores en la lista)
			if (PlayerPrefs.HasKey (i+"Player")) {
				// y tiene menos puntos que tu
				if (PlayerPrefs.GetInt(i+"Score") < score) {
					// mueve toda la lista uno para abajo
					for (int p = 11; p > i; p--){
						// si el jugador existe, claro.
						if(PlayerPrefs.HasKey (p-1+"Player")) {
							string newPlayer = PlayerPrefs.GetString (p - 1 + "Player");
							int newScore = PlayerPrefs.GetInt (p - 1 + "Score");
							PlayerPrefs.SetString (p + "Player", newPlayer);
							PlayerPrefs.SetInt (p + "Score", newScore);
						}
						// si el jugador no existe, prueba con el siguiente puesto.
						else {
							continue;
						}
					}
					// y te metes tu en ese puesto y ROMPES EL PUTO BUCLE
					PlayerPrefs.SetString (i + "Player", playerName);
					PlayerPrefs.SetInt (i + "Score", score);
					break;
				}
				// si no tienes más puntos que el del puesto que toca en el bucle, prueba con el siguiente puesto
				else {
					continue;
				}
			}
			// si no existe
			else {
				//crealo y sal
				PlayerPrefs.SetString (i+"Player", playerName);
				PlayerPrefs.SetInt (i+"Score", score);
				break;
			}
		}
		// al final, guarda los cambios de puntuación
		PlayerPrefs.Save ();
	}
}