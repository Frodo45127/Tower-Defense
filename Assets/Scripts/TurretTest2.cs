using UnityEngine;
using System.Collections;

//-----------------------------------------------------------------------
// TurretTest2.cs
// 
// Este script es el que se encarga del comportamiento de la segunda
// torreta de pruebas.
//
// NOTA: las variables que se heredan de Turret (coste, rango, daño,...)
// se ponen en el inspector (unity), no aquí.
//
//-----------------------------------------------------------------------

public class TurretTest2 : Turret {

	// la bala a disparar
	public GameObject bullet;

	// inicializamos los tiempos entre disparos y demás
	void Start() {
		isAATurret = false;
		isHibridTurret = false;
		timerShot = 1f;
		timerShotReset = 1f;
	}

	void FixedUpdate() {

		// si no somos un fantasma
		if (!isPhantom) {
			
			// si tenemos enemigo
			if (targetEnemy != null) {

				// y no es volador
				if (!targetEnemy.GetComponent<Enemy> ().isFlying) {
				
					// haz un temporizador
					timerShot -= Time.deltaTime;
					if (timerShot <= 0) {

						// y dispara la bala con la rotacion que tenga la torreta desde el cañon
						GameObject shotedBullet = (GameObject)Instantiate (bullet, new Vector2 (myTransform.position.x, myTransform.position.y), Quaternion.identity);

						// y le decimos a la bala el daño que debe tener
						shotedBullet.GetComponent<CannonBall> ().damage = damage;

						// y le decimos a la bala la posición a la que debe dirigirse
						shotedBullet.GetComponent<CannonBall> ().targetTransform = targetEnemy.transform;

						// y reseteamos el temporizador
						timerShot = timerShotReset;
					}	
				}
			}
		}
	}
}