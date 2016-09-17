using UnityEngine;
using System.Collections;
// necesario para la lista de enemigos
using System.Collections.Generic;
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

	// cacheo del transform
	protected Transform myTransform;
	protected Transform turretTop;

	// cacheo de la lista de enemigos spawneados y del spawner
	public List<GameObject> enemyList;
	protected GameObject spawner;

	// enemigo mas cercano de la torreta
	protected GameObject targetEnemy;

	// la torreta en la que pinchamos
	private GameObject clickedTurret;

	// variables necesarias para cada torreta (se setean en el unity, no en el script)
	[SerializeField]
	protected int damage;
	[SerializeField]
	protected int range;
	[SerializeField]
	protected int cost;
	public int Cost {
		get {
			return cost;
		}
	}

	public int DestructionCost {
		get {
			return cost / 2;
		}
	}

	// cacheamos
	void Awake(){
		myTransform = transform;
		turretTop = myTransform.Find("TurretTop");
		spawner = GameObject.Find("Spawner");
		enemyList = spawner.GetComponent<Spawner> ().SpawnedEnemyList;
	}

	// y empezamos la fiesta
	void Update(){

		// si no tenemos enemigo
		if (targetEnemy == null) {

			// buscamos uno
			targetEnemy = FindNearestEnemy ();
		}

		// y si tenemos un enemigo
		else {

			// calculamos la distancia a la que esta
			float enemyDist = Vector3.Distance(myTransform.position, targetEnemy.transform.position);

			// y si esta en rango, enfocamos la torreta
			if (enemyDist <= range) {
				turretTop.up = targetEnemy.transform.position - myTransform.position;
			}
		}
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

	// funcion para encontrar al enemigo mas cercano
	GameObject FindNearestEnemy(){
		
		// creamos las variable para almacenar las distancias y los enemigos
		float enemyDist;
		GameObject nearestEnemy = null;
		float nearestEnemyDist = 0f;

		// comporbamos la distancia a cada enemigo de la lista de enemigos spawneados
		foreach (GameObject e in enemyList) {

			// si el enemigo existe
			if (e != null) {

				// sacamos la distancia entre la torreta y el enemigo
				float distance = Vector3.Distance (myTransform.position, e.transform.position);

				// si esta dentro del rango
				if (distance <= range) {

					// guardamos su distancia
					enemyDist = distance;

					// si no tenemos un objetivo previo o esta mas cerca que los demas enemigos que 
					// ya hemos comprobado
					if (nearestEnemy == null || enemyDist < nearestEnemyDist){

						// seteamos el nuevo enemigo y su distancia
						nearestEnemy = e;
						nearestEnemyDist = enemyDist;
					}
				}
			}
		}

		// devolvemos el enemigo mas cercano
		return nearestEnemy;
	}
}