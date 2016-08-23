using UnityEngine;
using System.Collections;

//-----------------------------------------------------------------------
// MainMenuCredits.cs
//
// Este script es el que controla la pantalla de creditos.
// Tambien controla el boton de regreso al menu principal.
//
//-----------------------------------------------------------------------

public class MainMenuCredits : MonoBehaviour {

	// función para decirle a la cámara que vuelva al menú principal
	public void ReturnToMainMenu() {
		GameObject.FindGameObjectWithTag ("MainCamera").SendMessage ("MoveCameraToMainMenu");
	}
}
