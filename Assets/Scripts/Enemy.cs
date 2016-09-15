﻿using UnityEngine;
using System.Collections;
// necesario para manipular la lista del camino a seguir
using System.Collections.Generic;

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
	protected InGameMenu menus;

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

	// velocidad del enemigo
	[SerializeField]
	protected float enemySpeed;

	// vida del enemigo
	[SerializeField]
	protected int healthPoints;

	// variable para saber si es un boss o no
	protected bool isBoss;
		
	// cacheamos
	void Awake() {
		spawner = GameObject.Find ("Spawner").GetComponent<Spawner> ();
		menus = GameObject.Find ("Menus").GetComponent<InGameMenu> ();
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

					// hacemos daño a la base y nos destruimos
					GameManager.Instance.EnemyAtTheGates(damage);
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
		healthPoints -= damage;
		// si no nos queda vida
		if (healthPoints <= 0) {

			// el jugador gana puntos y dinero
			GameManager.Instance.Score += score;
			GameManager.Instance.Money += money;

			// si somos el boss
			if (isBoss == true) {

				// el jugador ha ganado
				menus.LevelCompleted();
			}

			// eliminamos al bicho de la lista de enemigos spawneados
			spawner.SpawnedEnemyList.Remove(this.gameObject);

			// y nos destruimos
			Destroy (this.gameObject);
		}
	}
}
