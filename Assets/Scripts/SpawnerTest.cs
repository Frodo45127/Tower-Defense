using UnityEngine;
using System.Collections;
// necesario para manipular la lista del camino a seguir
using System.Collections.Generic;

//-----------------------------------------------------------------------
// SpawnerTest.cs
//
// Este script es el que se encarga del spawn del nivel de pruebas.
//
//-----------------------------------------------------------------------

public class SpawnerTest : Spawner {

	// Use this for initialization
	void Start () {

		// tiempo entre oleadas
		timerWave = 1f;
		timerWaveReset = 10f;

		// tiempo entre spawns en una misma oleada
		timerSpawn = 0.4f;
		timerSpawnReset = 5f;

		// contador de oleadas
		waveCounter = 0;
		maxWaveCounter = 10;

		// número de unidades spawneadas por oleada y máximo
		unitsSpawnedPerWave = -1;
		maxUnitsSpawnedPerWave = 5;

		// check para ver si el boss ha spawneado
		bossSpawned = false;
	}

	// Update is called once per frame
	void Update () {

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
				Instantiate (enemySpawnList [0], spawnPosition, Quaternion.identity);
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
			Instantiate (boss, new Vector3 (27.2f, 0, 0), Quaternion.identity);
			bossSpawned = true;
		}
	}
}