using UnityEngine;
using System.Collections;

//-----------------------------------------------------------------------
// BossTest.cs
// 
// Este script es el que define el comportamiento del boss de pruebas.
// Por ahora el comportamiento es comun para todos, aqui solo va la
// funcion Start().
//
//-----------------------------------------------------------------------

public class BossTest : Enemy {

	// Use this for initialization
	void Start () {

		// cacheo del transform
		myTransform = transform;

		// vida y velocidad del bicho
		healthPoints = 5;
		enemySpeed = 50f;
		score = 50;
		money = 20;
		damage = 1;

		// cogemos la lista de nodos (camino) que haya en el spawner
		pathToFollow = spawner.Path;

		// ponemos como actual el primer nodo de la lista
		currentNodeInTheList = 0;
		currentNode = pathToFollow [currentNodeInTheList];
	}
}
