using UnityEngine;
using System.Collections;

//-----------------------------------------------------------------------
// EnemyTestFly.cs
// 
// Este script es el que define el comportamiento del bicho volador de pruebas.
// Por ahora el comportamiento es comun para todos, aqui solo va la
// funcion Start().
//
//-----------------------------------------------------------------------

public class EnemyTestFly : Enemy {

	// Use this for initialization
	void Start () {

		// cacheo del transform
		myTransform = transform;

		// vida y velocidad del bicho
		healthPoints = 3;
		enemySpeed = 100f;
		score = 40;
		money = 20;
		damage = 1;
		isFlying = true;

		// seteamos el máximo de vida y la vida inicial
		healthBar.maxValue = healthPoints;
		healthBar.value = healthPoints;

		// hacemos invisible el canvas de la barra de vida mientras la vida esté a tope
		enemyUI.enabled = false;

		// cogemos la lista de nodos (camino) que haya en el spawner
		pathToFollow = spawner.Path;

		// ponemos como actual el primer nodo de la lista
		currentNodeInTheList = 0;
		currentNode = pathToFollow [currentNodeInTheList];
	}
}
