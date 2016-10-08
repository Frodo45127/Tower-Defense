// Copyright © Ismael Gutiérrez González <frodo_gv@hotmail.com>, 2016
//
// This work is free.  You can redistribute it and/or modify it under the
// terms of the Do What The Fuck You Want To But It's Not My Fault Public
// License, Version 1, as published by Ben McGinnes.  See the
// LICENSE.txt file for more details.

using UnityEngine;
using System.Collections;

//-----------------------------------------------------------------------
// TurretTest.cs
// 
// Este script es el que se encarga del comportamiento de la torreta de pruebas.
//
// NOTA: las variables que se heredan de Turret (coste, rango, daño,...)
// se ponen en el inspector (unity), no aquí.
//
//-----------------------------------------------------------------------

public class TurretTest : Turret {

	// la bala a disparar
	public GameObject bullet;

	// inicializamos los tiempos entre disparos y demás
	void Start() {
		isAATurret = false;
		isHibridTurret = true;
		timerShot = 1f;
		timerShotReset = 1f;
	}

	void FixedUpdate() {

		// si no somos un fantasma
		if (!isPhantom) {
			
			// si tenemos enemigo
			if (targetEnemy != null) {

				// haz un temporizador
				timerShot -= Time.deltaTime;
				if (timerShot <= 0) {

					// y dispara la bala con la rotacion que tenga la torreta desde el cañon
					GameObject shotedBullet = (GameObject)Instantiate (bullet, new Vector2 (myTransform.position.x, myTransform.position.y), Quaternion.identity);

					// le decimos a la bala el daño que debe tener
					shotedBullet.GetComponent<Bullet> ().damage = damage;

					// y el enemigo al que debe perseguir
					shotedBullet.GetComponent<Bullet> ().targetTransform = targetEnemy.transform;

					// y reseteamos el temporizador
					timerShot = timerShotReset;
				}
			}
		}
	}
}
