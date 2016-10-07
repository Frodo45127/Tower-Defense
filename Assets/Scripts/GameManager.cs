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

	// nombre del jugador
	public string PlayerName { get; set;}

	// inicializamos la variable para el logo con el constructor
	private GameManager(){
		IsFirstStart = true;
	}
		
	// funcion para salir al menu principal
	public void ExitToMainMenu() {
		// Se asegura de que el tiempo funciona, pues esto se llama normalmente con el tiempo parado
		Time.timeScale = 1;
		// Si hay un nombre escrito en el campo de jugador
		if (!string.IsNullOrEmpty (GameManager.Instance.PlayerName)) {
			// guarda su puntuación
			AddToHighScoreListSorted (PlayerName, LevelManager.Instance.Score);
		}
		// Guardamos el progreso del jugador actual
		Save (PlayerName);
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

			// y cerramos el archivo
			file.Close();
		}

		// si el archivo no existe
		else {
			Debug.Log ("El usuario " + playerName + " no existe.");
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
	//public int maxLevelUnlocked;
	//public int maxPoints;

	// con un constructor pillamos los datos a guardar automáticamente
	public PlayerData() {
		playerName = GameManager.Instance.PlayerName;
	}
}