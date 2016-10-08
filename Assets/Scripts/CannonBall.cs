// Copyright © Ismael Gutiérrez González <frodo_gv@hotmail.com>, 2016
//
// This work is free.  You can redistribute it and/or modify it under the
// terms of the Do What The Fuck You Want To But It's Not My Fault Public
// License, Version 1, as published by Ben McGinnes.  See the
// LICENSE.txt file for more details.

using UnityEngine;
using System.Collections;
// necesario para la lista de enemigos
using System.Collections.Generic;

//-----------------------------------------------------------------------
// CannonBall.cs
// 
// Este script es el que se encarga del comportamiento de las balas de cañón
// (daño de área). Para la parábola, la ecuacion a usar es: "-y = x²"
//
//-----------------------------------------------------------------------

// TODO: ver si puede heredar de un script comun a todos los proyectiles.

public class CannonBall : MonoBehaviour {

	// cacheo
	private Transform myTransform;
	public Transform targetTransform;
	private Vector3 targetPosition;

	// cacheo de la lista de enemigos spawneados y del spawner
	public List<GameObject> enemyList;

	// velocidad de la bala de cañón
	private float speed;

	// daño de la bala de cañón
	public int damage;
	private int damageArea;

	// variables necesarias para el movimiento parabólico
	private float yOffset;
	private float distance;

	// cacheamos
	void Awake() {
		myTransform = transform;
	}

	// seteamos variables
	void Start() {
		damageArea = 20;
		speed = 100f;
		targetPosition = targetTransform.position;
		distance = Vector3.Distance (myTransform.position, targetPosition);
		enemyList = GameObject.Find("Spawner").GetComponent<Spawner> ().SpawnedEnemyList;
	}

	// TODO: descubrir por que cojones al aterrizar los tiros "resbalan".
	// movimiento de la bala de cañón
	void Update() {

		// distancia a moverse por cada update
		float distanceToMove = speed * Time.deltaTime;

		// si no ha llegado a la mitad del trayecto
		if (Vector3.Distance (myTransform.position, targetPosition) >= (distance / 2)) {

			// aumentamos el offset
			yOffset = Vector3.Distance (myTransform.position, targetPosition) / (distance / 2);
		}
		// si ha pasado la mitad del trayecto
		else {

			// reducimos el offset
			yOffset = Vector3.Distance (myTransform.position, targetPosition) / 100;
		}

		// y nos movemos
		myTransform.position = Vector3.MoveTowards (myTransform.position, new Vector3(targetPosition.x, targetPosition.y + yOffset * 20, targetPosition.z), distanceToMove);

		// si el tiro ha llegado a su destino
		if (myTransform.position == targetPosition) {

			// TODO: explota

			// daña a los que haya cerca
			HitAllEnemiesInRange ();

			// y destruyete
			Destroy (this.gameObject);
		}
	}

	// funcion para encontrar y herir a los enemigos en la zona de impacto
	void HitAllEnemiesInRange(){

		// comprobamos la distancia a la que está cada enemigo de la lista de enemigos spawneados
		foreach (GameObject e in enemyList.ToArray()) {

			// si el enemigo existe
			if (e != null) {

				// y no es volador
				if (!e.GetComponent<Enemy>().isFlying) {

					// sacamos la distancia entre el impacto y el enemigo
					float distance = Vector3.Distance (myTransform.position, e.transform.position);

					// si esta dentro del área de impacto
					if (distance <= damageArea) {

						// le quitamos vida
						e.SendMessage ("ReceiveDamage", damage);
					}	
				}
			}
		}
	}
}
