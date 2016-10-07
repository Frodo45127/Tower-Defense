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
