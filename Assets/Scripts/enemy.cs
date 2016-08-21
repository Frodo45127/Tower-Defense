using UnityEngine;
using System.Collections;

//-----------------------------------------------------------------------
// enemy.cs
// 
// Este script es del que heredan los scripts de los enemigos.
// Todas las variables, comportamientos comunes y estrategias van aquí.
//
//-----------------------------------------------------------------------

public class enemy : MonoBehaviour {

	// cacheo
	protected Transform myTransform;

	// velocidad del enemigo
	protected float enemySpeed;

	// vida del enemigo
	protected int healthpoints;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
