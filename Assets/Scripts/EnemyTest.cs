﻿// Copyright © Ismael Gutiérrez González <frodo_gv@hotmail.com>, 2016
//
// This work is free.  You can redistribute it and/or modify it under the
// terms of the Do What The Fuck You Want To But It's Not My Fault Public
// License, Version 1, as published by Ben McGinnes.  See the
// LICENSE.txt file for more details.

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
		enemySpeed = 50f + enemySpeedModifier;
		score = 50;
		money = 20;
		damage = 1;
		isFlying = false;

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
