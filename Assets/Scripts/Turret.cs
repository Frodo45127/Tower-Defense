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
	protected GameObject nearestEnemy;
	protected float nearestEnemyDist;

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

	// seteamos los valores por defecto
	void Start(){
		nearestEnemy = null;
		nearestEnemyDist = 0f;
	}

	// y empezamos la fiesta
	void Update(){

		// si no tenemos enemigo
		if (nearestEnemy == null) {
			
			// creamos la variable para almacenar las distancias
			float enemyDist;

			// comporbamos la distancia a cada enemigo de la lista de enemigos spawneados
			foreach (GameObject e in enemyList) {

				// FIXME: esto funciona mal, no calcula el mas cercano ni comprueba si esta en rango
				// si el enemigo existe
				if (e != null) {
					
					// sacamos la distancia en las X y en las Y en absolutos
					float distanceX = Mathf.Abs (myTransform.position.x - e.transform.position.x);
					float distanceY = Mathf.Abs (myTransform.position.y - e.transform.position.y);

					// dependiendo de cual es mayor, devolvemos una ecuacion u otra
					if (distanceX < distanceY){
						enemyDist = distanceY - distanceX;
					}
					else {
						enemyDist = distanceX - distanceY;
					}

					// si no tenemos un enemigo fijado (primer ciclo o tras muerte) 
					// o el nuevo enemigo esta mas cerca que el que tenemos
					if (nearestEnemy == null || enemyDist < nearestEnemyDist){

						// seteamos el nuevo enemigo y su distancia
						nearestEnemy = e;
						nearestEnemyDist = enemyDist;
					}
				}
			}
		}

		// y si tenemos enemigo
		else {

			// si tenemos un enemigo y esta en rango, enfocamos la torreta
			if (nearestEnemyDist <= range && nearestEnemy != null) {
				turretTop.up = nearestEnemy.transform.position - myTransform.position;
			}

			// si el enemigo ha salido del rango o ha muerto
			else {

				// reseteamos el enemigo
				nearestEnemy = null;
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
}