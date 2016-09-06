using UnityEngine;
using System.Collections;
// necesario para el fix de clickar a traves de los botones
using UnityEngine.EventSystems;

//-----------------------------------------------------------------------
// Turret.cs
// 
// Este script es del que heredan todos los scripts que controlan las
// torretas. Tambien es el que se encarga de controlar que ocurre si
// clikas sobre una torreta.
//
//-----------------------------------------------------------------------

public class Turret : MonoBehaviour, IPointerClickHandler {

	// menu de mantenimiento de las torretas
	public GameObject turretMenu;

	// la torreta en la que pinchamos
	private GameObject clickedTurret;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// usamos la interfaz IPointerClickHandler para que el click no atraviese
	// al boton, haciendo que el menu desaparezca
	#region IPointerClickHandler implementation

	// Cuando clickemos en un punto de la pantalla Y SOLTEMOS EL CLICK
	public void OnPointerClick (PointerEventData eventData){

		// pillamos la torreta en la que hemos pinchado
		clickedTurret = this.gameObject;

		// y activamos el menu de mantenimiento y se la pasamos
		turretMenu.SendMessage ("OpenTurretManagementMenu", clickedTurret);
	}

	#endregion
}
