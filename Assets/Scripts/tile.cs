using UnityEngine;
using System.Collections;

//-----------------------------------------------------------------------
// tile.cs
//
// Este script es el que controla todo lo relacionado con cada celda del grid.
// Basicamente, el mapa es una grilla de 20x20 de celdas de 20x20 (arreglar eto)
// Cada celda es un plano clickable con unas variables que le especifican
// esa celda. Dependiendo de esas variables el plano usa un sprite u otro
// y permite construir o no.
//
// Al menos ese es el plan.
//-----------------------------------------------------------------------

public class tile : MonoBehaviour {

	// variables para determinar el tipo de terreno
	public bool isRoad;
	public bool isField;
	public bool isMountain;
	public bool isWater;
	private bool isBuildable;
	private bool isBlocked;

	// variables para determinar el comienzo y el fin del camino
	public bool isStartingPoint;
	public bool isEndingPoint;

	// variables para determinar si esta dentro de los limites
	private bool isPlayableTerrain;

	// cacheos
	private Transform tileTransform;


	// Use this for initialization
	void Start () {

		// cacheos
		tileTransform = transform;

		// variable para identificar caminos bloqueados
		isBlocked = false;

		// Salvaguarda para que no se pueda setear un comienzo o final en una casilla que no sea camino.
		if (isStartingPoint || isEndingPoint) {
			isRoad = true;
		}

		// Determinamos que tipo de celda es por prioridad (temporal)
		if (isRoad) {
			isField = false;
			isMountain = false;
			isWater = false;
		}
		else if (isWater) {
			isField = false;
			isMountain = false;
		}
		else if (isMountain) {
			isField = false;
		}
		else {
			isField = true;
		}

		// TODO: apañar esto para poder elegir limites mejor
		// Comprobamos que la tile esta dentro de los limites
		if ((tileTransform.position.x > 250f) || (tileTransform.position.x < -250f) || (tileTransform.position.y > 250f) || (tileTransform.position.y < -250f)) {
			isPlayableTerrain = false;
		}
		// Si no esta fuera de los limites es terreno jugable
		else {
			isPlayableTerrain = true;
		}
			
		// Primero, solo debe ser construible si es camino o campo
		// y si esta dentro de los limites
		if ((!isRoad && !isField) || (!isPlayableTerrain) || (isWater)){
			isBuildable = false;
		}
		// Si no, solo puede ser montaña o estar fuera del mapa
		else {
			isBuildable = true;
		}
	}
	
	// Update is called once per frame
	void Update () {

		// codigo provisional para comprobar el funcionamiento de las variables
		if (isPlayableTerrain) {
			if (isRoad){
				this.GetComponent<Renderer> ().material.color = Color.grey;
			}
			else if (isField) {
				this.GetComponent<Renderer> ().material.color = Color.green;
			}
			else if (isMountain) {
				this.GetComponent<Renderer> ().material.color = Color.white;
			}
			else if (isWater) {
				this.GetComponent<Renderer> ().material.color = Color.blue;
			}
		}
		else {
			this.GetComponent<Renderer> ().material.color = Color.black;
		}
	}
	// TODO: Montar el sistema de construccion con tooltips

	// TODO: hacer que el menu de pausa funcione (script completo, hay que hacer la UI)
}
