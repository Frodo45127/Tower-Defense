using UnityEngine;
using System.Collections;
// necesario para poder clickar en las barricadas
using UnityEngine.EventSystems;

//-----------------------------------------------------------------------
// Barricade.cs
// 
// Este script controla el comportamiento de las barricadas.
//
//-----------------------------------------------------------------------

public class Barricade : MonoBehaviour, IPointerClickHandler {


	// variable para saber si es un fantasma o no
	[SerializeField]
	protected bool isPhantom;
	public bool IsPhantom {
		get {
			return isPhantom;
		}
		set {
			isPhantom = value;
		}
	}

	// usamos la interfaz IPointerClickHandler para que el click no atraviese
	// las cosas
	#region IPointerClickHandler implementation

	// Cuando clickemos en una barricada Y SOLTEMOS EL CLICK
	public void OnPointerClick (PointerEventData eventData){

		// si no es fantasma
		if (!isPhantom) {

			// si la partida no ha empezado
			if (!LevelManager.Instance.isPlayerReady) {
				
				// saca el nodo de la barricada
				Node barricadeNode = GameObject.Find ("Ground").GetComponent<Grid> ().GetNodeFromWorldPosition (this.transform.position);

				// borra la barricada
				Destroy (this.gameObject);

				// restaura el nodo
				barricadeNode.isWalkable = true;
				barricadeNode.isBuildableAndHasABarricade = false;

				// suma la barricada borrada a las barricadas restantes
				LevelManager.Instance.Barricades++;	
			}
		}
	}
	#endregion
}
