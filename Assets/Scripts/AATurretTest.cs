﻿// Copyright © Ismael Gutiérrez González <frodo_gv@hotmail.com>, 2016
//
// This work is free.  You can redistribute it and/or modify it under the
// terms of the Do What The Fuck You Want To But It's Not My Fault Public
// License, Version 1, as published by Ben McGinnes.  See the
// LICENSE.txt file for more details.

using UnityEngine;
using System.Collections;

//-----------------------------------------------------------------------
// AATurretTest.cs
// 
// Este script es el que se encarga del comportamiento de la torreta
// AA de pruebas.
//
// NOTA: las variables que se heredan de Turret (coste, rango, daño,...)
// se ponen en el inspector (unity), no aquí.
//
//-----------------------------------------------------------------------

public class AATurretTest : Turret {

	// la bala a disparar
	public GameObject bullet;

	// inicializamos los tiempos entre disparos y demás
	void Start() {
		isAATurret = true;
		isHibridTurret = false;
		timerShot = 0.2f;
		timerShotReset = 0.4f;
	}

	void FixedUpdate() {

		// si no somos un fantasma
		if (!isPhantom) {
			
			// si tenemos enemigo
			if (targetEnemy != null) {

				// y es volador
				if (targetEnemy.GetComponent<Enemy> ().isFlying) {
				
					// haz un temporizador
					timerShot -= Time.deltaTime;
					if (timerShot <= 0) {

						// y dispara la bala con la rotacion que tenga la torreta desde el cañon
						GameObject shotedBullet = (GameObject)Instantiate (bullet, new Vector2 (myTransform.position.x, myTransform.position.y), Quaternion.identity);

						// y le decimos a la bala el daño que debe tener
						shotedBullet.GetComponent<Bullet> ().damage = damage;

						// y le decimos a la bala la posición a la que debe dirigirse
						shotedBullet.GetComponent<Bullet> ().targetTransform = targetEnemy.transform;

						// y reseteamos el temporizador
						timerShot = timerShotReset;
					}	
				}
			}
		}
	}
}