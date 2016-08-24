using UnityEngine;
using System.Collections;
// necesario para usar las listas
using System.Collections.Generic;

//-----------------------------------------------------------------------
// Pathfinder.cs
// 
// Este script es el que se encarga del propio pathfinding.
//
//-----------------------------------------------------------------------

public class Pathfinder : MonoBehaviour {

	// variable para el cacheo del grid
	Grid grid;

	// en el awake cacheamos lo necesario
	void Awake(){
		grid = GetComponent<Grid>();
	}

	// Probamos el path
	// TODO: Borrar esto cuando todo rule
	public Transform startPath;
	public Transform targetPath;

	void Update(){
		FindShortestPath (startPath.position, targetPath.position);
	}

	// Función para encontrar el camino más corto entre dos puntos
	// si sabemos los dos puntos
	void FindShortestPath(Vector3 startWorldPosition, Vector3 targetWorldPosition){
		
		// Primero pasamos los dos puntos de posicion global a posicion en el grid
		Node startNode = grid.GetNodeFromWorldPosition (startWorldPosition);
		Node targetNode = grid.GetNodeFromWorldPosition (targetWorldPosition);

		// Creamos las listas para los grupos de nodos abierto (no comprobados) y cerrado (comprobados)
		List<Node> openSet = new List<Node> ();
		List<Node> closedSet = new List<Node> ();

		// Añadimos el nodo inicial a la lista abierta
		openSet.Add (startNode);

		// Si en la lista abierta hay algun nodo
		while (openSet.Count > 0) {
			// Ponemos ese nodo (el nodo inicial) como nodo actual
			Node currentNode = openSet [0];

			// TODO: Este bucle es horriblemente lento, hay que hacer una optimizacion Heap
			// Por cada nodo dentro del openset
			for (int i = 1; i < openSet.Count; i++){

				// Comprobamos si el coste del nodo del openset es menor que el del nodo actual
				// y si es igual, comparamos su hcost
				if ((openSet[i].fCost < currentNode.fCost) || (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)){

					// si se cumple alguna de las condiciones, es que nuestro nuevo nodo esta
					// mas cerca del final que el anterior, asi que le ponemos como actual
					currentNode = openSet [i];
				}
			}

			// Si hemos terminado de comprobar los nodos y hemos obtenido uno nuevo, lo quitamos
			// del openset y lo metemos en el closedset
			openSet.Remove (currentNode);
			closedSet.Add (currentNode);

			// si nuestro currentnode es el nodo que buscamos
			if (currentNode == targetNode){

				// paramos de buscar y devolvemos el camino buscado
				FollowTheTrail (startNode, targetNode);
				return;
			}

			// si no es el nodo final, empezamos a comprobar todos sus vecinos
			foreach (Node neighbour in grid.GetNeighbours(currentNode)){
				// si el vecino no es caminable o esta en la lista de comprobados
				if (!neighbour.isWalkable || closedSet.Contains(neighbour)) {

					// pasamos de el y probamos con el siguiente nodo
					continue;
				}

				// comprobamos si el vecino resulta ser una ruta mas corta a la ya calculada antes
				int newNeighbourGCost = currentNode.gCost + GetDistanceBetweenNodes (currentNode, neighbour);

				// si el nuevo coste es menor y no esta en la lista de comprobables
				if (newNeighbourGCost < neighbour.gCost || !openSet.Contains(neighbour)){
					neighbour.gCost = newNeighbourGCost;
					neighbour.hCost = GetDistanceBetweenNodes (neighbour, targetNode);
					neighbour.parentNode = currentNode;

					// si el vecino no esta en el openset, añadelo
					if (!openSet.Contains(neighbour)){
						openSet.Add (neighbour);
					}
				}
			}
		}
	}

	// Funcion para rastrear el camino desde el principio hasta el final, una vez
	// que tenemos todos los nodos del camino localizados
	void FollowTheTrail(Node startNode, Node targetNode){

		// Creamos la lista con el camino de nodos
		List<Node> Trail = new List<Node>();

		// Como primer nodo cogemos el nodo del final
		Node currentNode = targetNode;

		// Y ahora seguimos el rastro de padres hasta el nodo original
		while (currentNode != startNode){
			Trail.Add (currentNode);
			currentNode = currentNode.parentNode;
		}

		// Por ultimo, invertimos la lista (de principio a fin)
		Trail.Reverse();

		// Asignamos esto para hacer las pruebas del pathfinding
		grid.path = Trail;

	}

	// Funcion para sacar la distancia entre un nodo y otro
	int GetDistanceBetweenNodes(Node nodeA, Node nodeB){

		// sacamos la distancia en las X y en las Y en enteros absolutos
		int distanceX = Mathf.Abs (nodeA.gridX - nodeB.gridX);
		int distanceY = Mathf.Abs (nodeA.gridY - nodeB.gridY);

		// dependiendo de cual es mayor, devolvemos una ecuacion u otra
		if (distanceX < distanceY){
			return 10 * (distanceY - distanceX);
		}
		else {
			return 10 * (distanceX - distanceY);
		}
	}
}
