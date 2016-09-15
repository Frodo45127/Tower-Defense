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
	}
}