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
	public Transform targetTransform;

	// velocidad
	private float speed;

	// daño de la bala
	public int damage;

	// Use this for initialization
	void Start () {
		myTransform = transform;
		speed = 200f;
	}
	
	// Update is called once per frame
	void Update () {

		// si el objetivo sigue vivo
		if (targetTransform) {
			
			// seteamos la distancia a moverse en cada update
			float distanceToMove = speed * Time.deltaTime;

			// mueve la bala hacia el enemigo
			myTransform.position = Vector2.MoveTowards(myTransform.position, targetTransform.position, distanceToMove);	
		}
		// si el objetivo ha muerto
		else {

			// destruye la bala
			Destroy (this.gameObject);
		}

	}

	// que hacer cuando impacta contra un enemigo o un boss
	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.gameObject.CompareTag ("Enemy") || collider.gameObject.CompareTag ("Boss") ) {
			collider.SendMessage ("ReceiveDamage", damage);
			Destroy (this.gameObject);
		}
	}
}
