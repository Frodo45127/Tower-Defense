using UnityEngine;
using System.Collections;

//-----------------------------------------------------------------------
// Bullet.cs
// 
// Este script es el que se encarga del comportamiento de las balas de pruebas.
//
//-----------------------------------------------------------------------

public class Bullet : MonoBehaviour {

	// cacheo
	private Transform myTransform;

	// velocidad
	public float speed;

	// daño de la bala
	public int damage;

	// Use this for initialization
	void Start () {
		myTransform = transform;
		speed = 5f;
	}
	
	// Update is called once per frame
	void Update () {

		// mueve la bala
		myTransform.Translate (Vector3.up * speed);
	}

	// que hacer cuando impacta contra un enemigo o un boss
	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.gameObject.CompareTag ("Enemy") || collider.gameObject.CompareTag ("Boss") ) {
			collider.SendMessage ("ReceiveDamage", damage);
			Destroy (this.gameObject);
		}
	}
}
