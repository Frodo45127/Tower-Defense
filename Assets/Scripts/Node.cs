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

	// variables para saber si el nodo es caminable y para saber donde esta
	public bool isWalkable;
	public Vector3 worldPosition;

	// constructor de los nodos
	// le pasamos los parametros de si es caminable y de donde esta el nodo
	// desde el creador del grid
	public Node(bool _isWalkable, Vector3 _worldPosition) {
		isWalkable = _isWalkable;
		worldPosition = _worldPosition;
	}
}
