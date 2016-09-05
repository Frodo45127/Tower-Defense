using UnityEngine;
using System.Collections;

//-----------------------------------------------------------------------
// EnemyTest.cs
// 
// Este script es el que define el comportamiento del bicho de pruebas.
// Por ahora el comportamiento es comun para todos, aqui solo va la
// funcion Start().
//
//-----------------------------------------------------------------------

public class EnemyTest : Enemy {

	// Use this for initialization
	void Start () {
		
		// cacheo del transform
		myTransform = transform;

		// vida y velocidad del bicho
		healthPoints = 5;
		enemySpeed = 50f;

		// TODO: esto es una chapuza. Hay que arreglarlo.
		// cogemos la lista de nodos (camino) que haya en el spawner
		pathToFollow = SpawnerTest.path;

		// ponemos como actual el primer nodo de la lista
		currentNodeInTheList = 0;
		currentNode = pathToFollow [currentNodeInTheList];
	}
}
