﻿using UnityEngine;
using System.Collections;

//-----------------------------------------------------------------------
// TurretTest2.cs
// 
// Este script es el que se encarga del comportamiento de la segunda
// torreta de pruebas.
//
// NOTA: las variables que se heredan de Turret (coste, rango, daño,...)
// se ponen en el inspector (unity), no aquí.
//
//-----------------------------------------------------------------------

public class TurretTest2 : Turret {

	// la bala a disparar
	public GameObject bullet;

	// tiempo entre disparos
	private float timerShot = 1f;
	private float timerShotReset = 1f;

	void FixedUpdate() {

		// si tenemos enemigo
		if (nearestEnemy != null) {

			// haz un temporizador
			timerShot -= Time.deltaTime;
			if (timerShot <= 0) {

				// y dispara la bala con la rotacion que tenga la torreta desde el cañon
				GameObject shotedBullet = (GameObject)Instantiate (bullet, new Vector2 (turretTop.position.x, turretTop.position.y + 0.6f), turretTop.rotation);

				// y le decimos a la bala el daño que debe tener
				shotedBullet.GetComponent<Bullet> ().damage = damage;
				timerShot = timerShotReset;
			}
		}
	}
}