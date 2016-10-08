// Copyright © Ismael Gutiérrez González <frodo_gv@hotmail.com>, 2016
//
// This work is free.  You can redistribute it and/or modify it under the
// terms of the Do What The Fuck You Want To But It's Not My Fault Public
// License, Version 1, as published by Ben McGinnes.  See the
// LICENSE.txt file for more details.

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

		// arrancamos el gamemanager si no esta arrancado y seteamos los ajustes del nivel
		LevelManager.Instance.SetLevelVariables (800, 0, 5, 2);

		// tiempo entre oleadas
		timerWave = 1f;
		timerWaveReset = 2f;

		// tiempo entre spawns en una misma oleada
		timerSpawn = 0.8f;
		timerSpawnReset = 0.8f;

		// contador de oleadas
		waveCounter = 0;
		maxWaveCounter = 2;

		// número de unidades spawneadas por oleada y máximo
		unitsSpawnedPerWave = -1;
		maxUnitsSpawnedPerWave = 5;

		// check para ver si el boss ha spawneado
		bossSpawned = false;
	}
}