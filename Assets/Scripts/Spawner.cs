using UnityEngine;
using System.Collections;
// necesario para manipular la lista del camino a seguir
using System.Collections.Generic;

//-----------------------------------------------------------------------
// Spawner.cs
//
// Este script es del que heredan los spawners de cada nivel.
// Tiene las variables que pueden usar todos los spawners.
//
// NOTA: Ta echo asi por si nos da por hacer cosas especiales por niveles.
//
//-----------------------------------------------------------------------

public class Spawner : MonoBehaviour {

	// define el objeto que tiene el pathfinder y el grid
	public GameObject ground;
	protected Pathfinder pathfinder;
	protected Grid grid;
	protected InGameMenu InGameUI;

	// TODO: hacer que esto funcione con multiples caminos
	// define el camino a seguir
	protected List<Node> path;
	public List<Node> Path {
		get {
			return path;
		}
	}

	// define el lugar de donde salen los monstruos
	public GameObject startPosition;
	protected Vector2 spawnPosition;

	// array de enemigos spawneables
	public GameObject[] enemySpawnList = new GameObject[1];

	// define al boss
	public GameObject boss;

	// lista de todos los enemigos spawneados en pantalla
	[SerializeField]
	protected List<GameObject> spawnedEnemyList;
	public List<GameObject> SpawnedEnemyList {
		get {
			return spawnedEnemyList;
		}
		set {
			spawnedEnemyList = value;
		}
	}

	// tiempo entre oleadas
	public float timerWave { get; protected set;}
	protected float timerWaveReset;

	// tiempo entre spawns en una misma oleada
	public float timerSpawn { get; protected set;}
	protected float timerSpawnReset;

	// contador de oleadas
	protected int waveCounter;
	public int maxWaveCounter { get; protected set;}

	// número de unidades spawneadas por oleada y máximo
	protected int unitsSpawnedPerWave;
	public int maxUnitsSpawnedPerWave { get; protected set;}

	// check para ver si el boss ha spawneado
	protected bool bossSpawned;
	public bool BossSpawned {
		get {
			return bossSpawned;
		} 
	}

	// abreviaturas
	void Awake () {
		spawnPosition = startPosition.transform.position;
		pathfinder = ground.GetComponent<Pathfinder> ();
		grid = ground.GetComponent<Grid> ();
		InGameUI = GameObject.Find ("InGameUI").GetComponent<InGameMenu> ();
	}

	// Update is called once per frame
	void Update () {

		// si hemos dado a iniciar partida
		if (LevelManager.Instance.isPlayerReady) {
			
			// actualiza los timers para las oleadas y los spawns
			timerWave -= Time.deltaTime;
			timerSpawn -= Time.deltaTime;

			// si el timer de oleada llega a cero y han pasado menos del tope de oleadas
			if (timerWave <= 0 && waveCounter < maxWaveCounter){

				// si no ha salido ninguna oleada, saca el camino a seguir y guardalo
				if (unitsSpawnedPerWave == -1) {

					// sacamos el camino a seguir
					pathfinder.FindShortestPathFromAllSources ();

					// si todo ha salido bien y tenemos un camino en la lista de caminos
					if (grid.pathList != null) {

						// guardamos en una lista el camino a seguir
						path = ground.GetComponent<Grid>().pathList [0];
					}
					// si no, tenemos un problema
					else {
						Debug.Log ("Algo ha salido mal, no hay camino");
					}

					// y sumamos uno para que empiece la ronda de spawn normal
					unitsSpawnedPerWave++;
				}

				// si el timer de los spawns llega a cero y han spawneado menos del máximo de unidades
				else if (timerSpawn <= 0 && unitsSpawnedPerWave < maxUnitsSpawnedPerWave){

					// spawnea una unidad en el punto de inicio
					GameObject enemy = (GameObject)Instantiate (enemySpawnList [0], spawnPosition, Quaternion.identity);

					// le añadimos a la lista de enemigos spawneados
					spawnedEnemyList.Add (enemy);

					// y sumamos uno a la cantidad de enemigos spawneados por oleada
					unitsSpawnedPerWave++;

					// resetea el timer del spawn de unidades
					timerSpawn = timerSpawnReset;
				}

				// si ha terminado de spawnear las unidades de la oleada
				else if (unitsSpawnedPerWave >= maxUnitsSpawnedPerWave) {

					// resetea el timer de oleada y el contador de unidades, y aumenta en uno el contador de oleadas
					timerWave = timerWaveReset;
					unitsSpawnedPerWave = 0;
					waveCounter++;
				}
			}

			// spawnea al boss si ha terminado de spawnear a los demás
			if (waveCounter == maxWaveCounter && bossSpawned == false && timerWave <= 0.0f) {
				GameObject enemy = (GameObject)Instantiate (boss, spawnPosition, Quaternion.identity);
				spawnedEnemyList.Add (enemy);
				bossSpawned = true;
			}

			// si el boss ha sido spawneado y ya no quedan enemigos vivos spawneados
			if (bossSpawned && SpawnedEnemyList.Count == 0) {

				// es que hemos matado a to cristo y hemos ganao
				InGameUI.LevelCompleted ();
			}
		}
	}
}