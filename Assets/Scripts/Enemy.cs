﻿// Copyright © Ismael Gutiérrez González <frodo_gv@hotmail.com>, 2016
//
// This work is free.  You can redistribute it and/or modify it under the
// terms of the Do What The Fuck You Want To But It's Not My Fault Public
// License, Version 1, as published by Ben McGinnes.  See the
// LICENSE.txt file for more details.

using UnityEngine;
using System.Collections;
// necesario para manipular la lista del camino a seguir
using System.Collections.Generic;
// necesario para usar las barras de vida
using UnityEngine.UI;

//-----------------------------------------------------------------------
// Enemy.cs
// 
// Este script es del que heredan los scripts de los enemigos.
// Todas las variables, comportamientos comunes y estrategias van aquí.
//
//-----------------------------------------------------------------------

public class Enemy : MonoBehaviour {

	// cacheo
	protected Transform myTransform;
	protected Spawner spawner;
	protected Canvas enemyUI;
	public Slider healthBar;

	// lista de nodos que forman el camino a seguir y nodo actual
	protected List<Node> pathToFollow;
	protected Node currentNode;
	protected int currentNodeInTheList;

	// puntos que da el enemigo
	[SerializeField]
	protected int score;

	// dinero que da el enemigo
	[SerializeField]
	protected int money;

	// daño que nos hara si llega a la base
	[SerializeField]
	protected int damage;

	// velocidad del enemigo y modificador
	[SerializeField]
	protected float enemySpeed;
	protected float enemySpeedModifier;

	// vida del enemigo
	[SerializeField]
	protected int healthPoints;

	// variable para saber si es volador o terrestre
	public bool isFlying;
		
	// cacheamos
	void Awake() {
		spawner = GameObject.Find ("Spawner").GetComponent<Spawner> ();
		enemyUI = gameObject.GetComponentInChildren<Canvas> ();
	}
	// aqui movemos al enemigo por el camino
	void FixedUpdate() {

		// si el nodo existe, esta en la lista de nodos a seguir, y no hemos jodio nada
		if (currentNode != null && currentNodeInTheList <= pathToFollow.Count){

			// cacheamos nuestra posicion
			Vector2 currentPosition = new Vector2 (transform.position.x, transform.position.y);

			// calcula la distancia a moverse en cada ejecucion
			float distanceToMove = enemySpeed * Time.deltaTime;

			// muevete hacia el siguiente nodo en cada ejecucion
			myTransform.position = Vector2.MoveTowards (currentPosition, currentNode.worldPosition, distanceToMove);

			// si hemos llegado al nodo que queremos
			if (myTransform.position == currentNode.worldPosition) {

				// suma uno al selector del nodo que queremos como objetivo de la lista
				currentNodeInTheList++;

				// si el nodo es menor que el maximo de nodos de la lista (luego esta en la lista)
				if (currentNodeInTheList < pathToFollow.Count) {

					// asignalo como nuevo nodo objetivo
					currentNode = pathToFollow [currentNodeInTheList];
				}

				// si el nodo es igual o mayor, es que hemos acabado de recorrer el camino
				else {
					Debug.Log ("Se acabo el camino, te destruyes y le quitas un puntito al jugador.");

					// hacemos daño a la base
					LevelManager.Instance.EnemyAtTheGates(damage);

					// eliminamos al bicho de la lista de enemigos spawneados
					spawner.SpawnedEnemyList.Remove(this.gameObject);

					// y destruimos al bicho
					Destroy (this.gameObject);
				}
			}
		}

		// si no, es que algo se ha jodido
		else {
			Debug.Log ("El nodo no existe");
		}
	}

	//-------------------------------------------//
	// Funciones comunes para todos los enemigos //
	//-------------------------------------------//

	// recibe daño
	void ReceiveDamage(int damage) {

		// si el canvas de la barra de vida es invisible
		if (!enemyUI.enabled) {
			
			// le hacemos visible
			enemyUI.enabled = true;
		}
		// le baja la vida y la barra de vida
		healthPoints -= damage;
		healthBar.value = healthPoints;

		// si no nos queda vida
		if (healthPoints <= 0) {

			// el jugador gana puntos y dinero
			LevelManager.Instance.Score += score;
			LevelManager.Instance.Money += money;

			// eliminamos al bicho de la lista de enemigos spawneados
			spawner.SpawnedEnemyList.Remove(this.gameObject);

			// y nos destruimos
			Destroy (this.gameObject);
		}
	}
}
