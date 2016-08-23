﻿using UnityEngine;
using System.Collections;

//-----------------------------------------------------------------------
// Grid.cs
// 
// Este script es el que se encarga de crear el grid de nodos para el pathfinding.
//
// Nota:
// - grid...: posicion dentro del grid en enteros.
// - gridWorld...: posicion del grid en coordenadas del mundo.
//
//-----------------------------------------------------------------------

public class Grid : MonoBehaviour {

	// variables que necesitamos
	// transform del enemigo, para probar el grid
	public Transform enemyTest;

	// - La capa en la que debe estar el terreno caminable
	public LayerMask walkableTerrain;

	// - El tamaño del grid
	public Vector2 gridWorldSize;

	// - El radio de cada nodo del grid
	public float nodeRadius;

	// - Una lista de dos dimensiones con todos los nodos del grid
	Node[,] grid;

	// - Diámetro de los nodos, necesarios para ciertas operaciones
	private float nodeDiameter;

	// - Tamaño de la X y de la Y del Grid en funcion del numero de nodos 
	// que podemos meter en integers, para que no quede medio nodo dentro 
	// del grid en los bordes y cosas asi
	private int gridSizeX, gridSizeY;

	// - Offset de los porcentajes para la deteccion del nodo en el que se
	// encuentra un enemigo
	private float percentXoffset, percentYoffset;

	// Inicializamos los valores necesarios
	void Start(){
		nodeDiameter = nodeRadius * 2;

		// sacamos el tamaño horizontal y vertical del grid en funcion del
		// numero de nodos que podemos meter, en integers
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

		// calculamos el offset de la deteccion del enemigo dependiendo del tamaño del grid
		// esto evita que el error por redondeo se vuelva enorme al alejarnos del centro
		percentXoffset = (1f / gridSizeX) / 2f;
		percentYoffset = (1f / gridSizeY) / 2f;

		// creamos el grid
		CreateGrid();
	}

	// Funcion para crear el grid
	void CreateGrid(){

		// hacemos que la lista de nodos tenga el tamaño justo para que entren
		// nodos enteros, sin medias partes
		grid = new Node[gridSizeX, gridSizeY];

		// le decimos cual es la posicion de abajo a la izquierda, pues es
		// por donde vamos a empezar a crear el grid (en coordenadas del mundo)
		Vector3 gridWorldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

		// por cada columna hasta llegar a la del nodo del final del grid
		for (int x = 0; x < gridSizeX; x++){
			// cogemos una celda
			for (int y = 0; y < gridSizeY; y++){
				// sacamos su centro en coordenadas del mundo
				Vector3 gridWorldNodeCenter = gridWorldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);

				// sacamos si el nodo esta tocando terreno caminable
				bool isWalkable = Physics.CheckSphere (gridWorldNodeCenter, nodeRadius, walkableTerrain);
				
				// y añadimos el nodo a la lista de nodos en el grid 
				grid [x, y] = new Node (isWalkable, gridWorldNodeCenter);
			}
		}
	}

	// Funcion para saber sobre que nodo esta un enemigo sabiendo sus coordenadas
	// del mundo, la cual nos devuelve un nodo del grid
	public Node GetNodeFromWorldPosition(Vector3 worldPosition){

		// sacamos la posicion en la que estamos en el grid en porcentaje (entre 0 y 1)
		// NOTA: esto asume que el centro del grid es 0,0,0 y que el grid es cuadrado.
		// Es posible que se rompa si el grid no esta en esa posicion.
		float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
		float percentY = (worldPosition.y + gridWorldSize.x / 2) / gridWorldSize.y;

		// limitamos entre 0 y 1 el porcentaje para que no calcule posiciones fuera del grid
		// (soltaria error al no encontrar nodos)
		percentX = Mathf.Clamp01 (percentX);
		percentY = Mathf.Clamp01 (percentY);

		// sacamos la posicion en el indice de la lista del nodo en el que estamos
		// NOTA: los offsets se calculan al iniciar, pues dependen del tamaño del grid.
		int x = Mathf.RoundToInt ((gridSizeX) * (percentX - percentXoffset));
		int y = Mathf.RoundToInt ((gridSizeY) * (percentY - percentYoffset));

		// le decimos que nos devuelva el nodo que hemos sacado
		return grid [x, y];
	}

	// Funcion para mostrar gizmos, para ver mejor el grid y los nodos
	void OnDrawGizmos() {

		// Dibuja un gizmo para ver el tamaño real del grid
		Gizmos.DrawWireCube (transform.position, new Vector2 (gridWorldSize.x, gridWorldSize.y));

		// Si el grid existe
		if (grid != null){

			// guardamos en enemyNode el nodo en el que esta el enemigo
			Node enemyNode = GetNodeFromWorldPosition (enemyTest.position);

			// Por cada nodo en el grid
			foreach (Node n in grid){

				// coloreale gris si es caminable, rojo si no lo es
				// si n.isWalkable es true, entonces (?) es gris, si no (:) es rojo
				Gizmos.color = n.isWalkable ? Color.grey : Color.red;

				// Colorea el nodo si el enemigo esta sobre el
				if (enemyNode == n){
					Gizmos.color = Color.blue;
				}

				// y dibuja un cubo un pelin mas pequeño que el nodo para representarlo
				Gizmos.DrawCube (n.worldPosition, Vector3.one * (nodeDiameter - 1f));
			}
		}
	}
}