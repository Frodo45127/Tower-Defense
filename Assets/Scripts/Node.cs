using UnityEngine;
using System.Collections;

//-----------------------------------------------------------------------
// Node.cs
// 
// Este script es el que se usa para construir cada nodo del grid.
// Basicamente, es para que el pathfinding rule.
//
//-----------------------------------------------------------------------

public class Node {

	// variables para saber si el nodo es caminable, construible, 
	// si tiene una torreta o barricada y para saber donde esta
	public bool isWalkable;
	public bool isBuildable;
	public bool isBuildableAndHasATurret;
	public bool isBuildableAndHasABarricade;
	public Vector3 worldPosition;

	// variables para guardar la posicion del nodo
	public int gridX;
	public int gridY;

	// variable para saber el padre del nodo y poder trazar un camino
	public Node parentNode;

	// variables para los costes del algoritmo de pathfinding
	// coste desde el inicio al nodo
	public int gCost;

	// coste desde el final al nodo
	public int hCost;

	// suma de los costes del nodo
	public int fCost{
		get{
			return gCost + hCost;
		}
	}

	// constructor de los nodos
	// le pasamos los parametros de si es caminable y de donde esta el nodo
	// desde el creador del grid
	public Node(Vector3 _worldPosition, int _gridX, int _gridY, bool _isWalkable, bool _isBuildable) {
		worldPosition = _worldPosition;
		gridX = _gridX;
		gridY = _gridY;
		isWalkable = _isWalkable;
		isBuildable = _isBuildable;
	}
}
