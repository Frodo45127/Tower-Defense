// Copyright © Ismael Gutiérrez González <frodo_gv@hotmail.com>, 2016
//
// This work is free.  You can redistribute it and/or modify it under the
// terms of the Do What The Fuck You Want To But It's Not My Fault Public
// License, Version 1, as published by Ben McGinnes.  See the
// LICENSE.txt file for more details.

using UnityEngine;
using System.Collections;

//-----------------------------------------------------------------------
// BulletAA.cs
// 
// Este script es el que se encarga del comportamiento de las balas AA.
//
//-----------------------------------------------------------------------

public class BulletAA : Bullet {

	// que hacer cuando impacta contra un enemigo o un boss
	void OnTriggerEnter2D (Collider2D collider) {
		if ((collider.gameObject.CompareTag ("Enemy") || collider.gameObject.CompareTag ("Boss")) && collider.GetComponent<Enemy>().isFlying) {
			collider.SendMessage ("ReceiveDamage", damage);
			Destroy (this.gameObject);
		}
	}
}
