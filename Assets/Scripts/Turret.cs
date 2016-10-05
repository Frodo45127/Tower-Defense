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

public class Turret : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

	// cacheo del transform
	protected Transform myTransform;

	// cacheo de la lista de enemigos spawneados y del spawner
	public List<GameObject> enemyList;
	protected GameObject spawner;

	// enemigo mas cercano de la torreta
	protected GameObject targetEnemy;

	// la torreta en la que pinchamos
	private GameObject clickedTurret;

	// variable para saber si la torreta es antiaérea
	protected bool isAATurret;
	protected bool isHibridTurret;

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

	// booleana para saber si tenemos el ratón sobre la torreta
	private bool isOver9000;

	// variable para hacer el círculo del rango
	private LineRenderer rangeBorder;

	// variables necesarias para cada torreta (se setean en el unity, no en el script)
	[SerializeField]
	protected int damage;
	[SerializeField]
	protected int range;
	[SerializeField]
	protected float timerShotReset;
	protected float timerShot;
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
		spawner = GameObject.Find("Spawner");
		enemyList = spawner.GetComponent<Spawner> ().SpawnedEnemyList;
		rangeBorder = gameObject.GetComponent<LineRenderer>();
	}

	// y empezamos la fiesta
	void Update(){

		// si no somos un fantasma
		if (!isPhantom) {
			
			// si no tenemos enemigo
			if (targetEnemy == null) {

				// y nuestra torreta es sólo AA
				if (isAATurret) {
				
					// buscamos un enemigo aéreo
					targetEnemy = FindNearestEnemy (true, false);	
				}

				// y nuestra torreta es hibrida
				else if (isHibridTurret) {
				
					// buscamos a cualquier enemigo
					targetEnemy = FindNearestEnemy (false, false);
				}

				// y nuestra torreta solo ataca a tierra
				else {

					// buscamos un enemigo terrestre
					targetEnemy = FindNearestEnemy (false, true);
				}
			}

			// y si tenemos un enemigo
			else {

				// calculamos la distancia a la que esta
				float enemyDist = Vector3.Distance (myTransform.position, targetEnemy.transform.position);

				// si ya no esta en rango, olvidalo
				if (enemyDist >= range) {
					targetEnemy = null;
				}
			}
		}

		// si es un fantasma
		else {

			// muestra todo el rato el rango
			isOver9000 = true;
		}

		// si colocamos el ratón sobre la torreta
		if (isOver9000) {

			// si el componente LineRenderer está desactivado
			if (!rangeBorder.enabled){

				// lo activamos
				rangeBorder.enabled = true;
			}

			// mostramos el rango
			ShowRange (range);
		}
		// si quitamos el ratón de la torreta
		else {

			// ocultamos el rango
			rangeBorder.enabled = false;
		}
	}

	// usamos la interfaz IPointerClickHandler para que el click no atraviese
	// al boton, haciendo que el menu desaparezca
	#region IPointerClickHandler implementation

	// Cuando clickemos en un punto de la pantalla Y SOLTEMOS EL CLICK
	public void OnPointerClick (PointerEventData eventData){

		// pillamos la torreta en la que hemos pinchado
		clickedTurret = this.gameObject;

		// si estamos destruyendo torretas
		if (GameManager.Instance.SelectedTurret == -1) {
			
			// se la mandamos a la función de destrucción de torretas
			GameObject.Find("Ground").GetComponent<TurretBuilder>().SendMessage ("DestroyTurret", clickedTurret);
		}

		// si no estamos destruyendo torretas
		else {
			Debug.Log ("Hacemos algo, no se el qué.");
		}
	}

	#endregion

	// usamos la interfaz IPointerEnterHandler para detectar cuando ponemos el raton
	// sobre la torreta, y que empieze a mostrar el alcance
	#region IPointerEnterHandler implementation

	// Cuando pongamos el raton sobre la torreta
	public void OnPointerEnter (PointerEventData eventData){

		// estamos sobre la torreta
		isOver9000 = true;
	}

	#endregion

	// usamos la interfaz IPointerExitHandler para detectar cuando quitamos el raton
	// de la torreta, y que empieze a ocultar el alcance
	#region IPointerExitHandler implementation

	// Cuando quitemos el raton de la torreta
	public void OnPointerExit (PointerEventData eventData){

		// estamos fuera la torreta
		isOver9000 = false;
	}

	#endregion

	// funcion para encontrar al enemigo mas cercano
	GameObject FindNearestEnemy(bool searchFlyingEnemiesOnly, bool searchLandEnemiesOnly){
		
		// creamos las variables para almacenar las distancias y los enemigos
		float enemyDist;
		GameObject nearestEnemy = null;
		float nearestEnemyDist = 0f;

		// comprobamos la distancia a la que está cada enemigo de la lista de enemigos spawneados
		foreach (GameObject e in enemyList.ToArray()) {

			// si solo queremos buscar a los voladores
			if (searchFlyingEnemiesOnly == true) {

				// comprueba si es volador
				if (!e.GetComponent<Enemy>().isFlying) {

					// si no es volador, saltatelo
					continue;
				}
			}

			// si solo queremos buscar a los terrestres
			else if (searchLandEnemiesOnly == true) {

				// comprueba si es volador
				if (e.GetComponent<Enemy>().isFlying) {

					// si es volador, saltatelo
					continue;
				}
			}

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

	// función para calcular el rango de la torreta y mostrar un círculo de dicho rango
	void ShowRange(int _range) {

		// variables que necesitamos:
		// cantidad de líneas que formarán el círculo
		int lines = 25;

		// posición de cada nuevo punto del círculo
		float x;
		float y;

		// ángulo entre cada punto y alcance de la torreta escalado
		float angle = (360f / lines);
		int turretRange = _range / (int)myTransform.localScale.x;

		// creamos un array de vértices, con tantos vértices como líneas + 1
		// (para cerrar el círculo) tengamos
		Vector3[] vertexList = new Vector3[lines + 1];

		// le decimos la cantidad de vértices + 1
		// (para cerrar el círculo) que tiene que tener
		rangeBorder.SetVertexCount (lines + 1);

		// ahora creamos el círculo
		for (int i = 0; i <= lines; i++) {

			// si no estamos en el último vértice
			if (i < lines) {
				
				//  sacamos el nuevo vértice escalado
				x = Mathf.Sin (Mathf.Deg2Rad * angle) * (turretRange);
				y = Mathf.Cos (Mathf.Deg2Rad * angle) * (turretRange);

				// añadimos el nuevo vértice a la lista
				vertexList [i] = new Vector3 (x, y, -1f);
					
			}
			// si estamos en el último vértice
			else {

				// le ponemos en el mismo sitio que el primero
				vertexList [i] = vertexList [0];
			}

			// y aumentamos el ángulo
			angle += (360f / lines);
		}

		// y para terminar, creamos líneas entre los distintos vértices
		rangeBorder.SetPositions (vertexList);
	}
}