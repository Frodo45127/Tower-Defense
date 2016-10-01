using UnityEngine;
using System.Collections;

//-----------------------------------------------------------------------
// AATurretTest.cs
// 
// Este script es el que se encarga del comportamiento de la torreta
// AA de pruebas.
//
// NOTA: las variables que se heredan de Turret (coste, rango, daño,...)
// se ponen en el inspector (unity), no aquí.
//
//-----------------------------------------------------------------------

public class AATurretTest : Turret {

	// la bala a disparar
	public GameObject bullet;

	// inicializamos los tiempos entre disparos y demás
	void Start() {
		isAATurret = true;
		isHibridTurret = false;
		timerShot = 0.2f;
		timerShotReset = 0.4f;
	}

	void FixedUpdate() {

		// si tenemos enemigo
		if (targetEnemy != null) {

			// y es volador
			if (targetEnemy.GetComponent<Enemy>().isFlying) {
				
				// haz un temporizador
				timerShot -= Time.deltaTime;
				if (timerShot <= 0) {

					// y dispara la bala con la rotacion que tenga la torreta desde el cañon
					GameObject shotedBullet = (GameObject)Instantiate (bullet, new Vector2 (turretTop.position.x, turretTop.position.y + 0.6f), turretTop.rotation);

					// y le decimos a la bala el daño que debe tener
					shotedBullet.GetComponent<Bullet> ().damage = damage;

					// y le decimos a la bala la posición a la que debe dirigirse
					shotedBullet.GetComponent<Bullet> ().targetTransform = targetEnemy.transform;

					// y reseteamos el temporizador
					timerShot = timerShotReset;
				}	
			}
		}
	}
}