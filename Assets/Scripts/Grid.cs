using UnityEngine;
using System.Collections;
// necesario para usar las listas
using System.Collections.Generic;

//-----------------------------------------------------------------------
// Grid.cs
// 
// Este script es el que se encarga de crear el grid de nodos para el pathfinding.
// Es un grid de 20x20, con 2 celdas en cada margen que no se pueden usar (24x24).
// Para colocar las cosas en el grid hay que usar multiplos impares de 17.
//
// Nota:
// - grid...: posicion dentro del grid en enteros.
// - gridWorld...: posicion del grid en coordenadas del mundo.
//
//-----------------------------------------------------------------------

public class Grid : MonoBehaviour {

	// variables que necesitamos
	// lista del pathfinding, y transforms para provar el pathfinding
	public List<List<Node>> pathList;

	// - Las capas a usar para el grid
	public LayerMask walkableTerrain;
	public LayerMask unbuildableTerrain;

	// - El tamaño del grid
	public Vector2 gridWorldSize;

	// - El radio de cada nodo del grid
	public float nodeRadius;

	// - Una lista de dos dimensiones con todos los nodos del grid
	public Node[,] grid;

	// - Diámetro de los nodos, necesarios para ciertas operaciones
	private float nodeDiameter;

	// - Tamaño de la X y de la Y del Grid en funcion del numero de nodos 
	// que podemos meter en integers, para que no quede medio nodo dentro 
	// del grid en los bordes y cosas asi
	private int gridSizeX, gridSizeY;

	// - Offset de los porcentajes para la deteccion del nodo en el que se
	// encuentra un enemigo
	private float percentXoffset, percentYoffset;

	// lista de sprites para pintar el fondo
	public Sprite[] tileList;

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

				// sacamos si el nodo esta tocando terreno caminable, construible, etc
				bool isWalkable = Physics.CheckSphere (gridWorldNodeCenter, nodeRadius, walkableTerrain);
				bool isBuildable = !Physics.CheckSphere (gridWorldNodeCenter, nodeRadius, unbuildableTerrain);

				// y añadimos el nodo a la lista de nodos en el grid 
				grid [x, y] = new Node (gridWorldNodeCenter, x, y, isWalkable, isBuildable);
			}
		}

		// y despues pintamos el fondo
		PaintMeLikeOneOfYourFrenchGirls ();
	}

	// Funcion para poner las texturas de las narices al fondo
	void PaintMeLikeOneOfYourFrenchGirls(){

		// primero creamos un anillo para gobernarlos a todos
		GameObject tileGroup = new GameObject ("TileList");

		// por cada columna hasta llegar a la del nodo del final del grid
		for (int x = 0; x < gridSizeX; x++){

			// si esa celda esta fuera de la pantalla por un lado (x < 1 o > 38)
			if (x < 1 || x > 38) {

				// nos la saltamos
				continue;
			}
				
			// si la celda esta en la pantalla la cogemos
			for (int y = 0; y < gridSizeY; y++){

				// si la celda se sale de la pantalla por arriba o abajo
				if (y < 8 || y > 31) {

					// nos la saltamos
					continue;
				}

				// si esa celda esta en un lado de la pantalla
				if (x < 4 || x > 35) {

					// guardamos la celda o nodo
					Node currentNode = grid [x, y];

					// creamos un plano en blanco que ocupe lo que ocupa el nodo
					GameObject tile = new GameObject("Mountain");
					SpriteRenderer sprite = tile.AddComponent<SpriteRenderer> ();
					Transform tileTransform = tile.transform;
					tileTransform.position = new Vector3(currentNode.worldPosition.x, currentNode.worldPosition.y, 0.1f);

					// escalamos los cuadrados para que se ajusten al grid
					tileTransform.localScale = new Vector2 (nodeDiameter * 1.6f, nodeDiameter * 1.6f);

					// lo metemos como hijo de TileList
					tileTransform.SetParent(tileGroup.transform);

					// seteamos el nodo como no construible
					currentNode.isBuildable = false;

					// y le pintamos un muro
					sprite.sprite = tileList [7];
				}

				// si la celda esta cerca del borde, es un muro
				else if (y == 10 || y == 29) {
					
					// guardamos la celda o nodo
					Node currentNode = grid [x, y];

					// creamos un plano en blanco que ocupe lo que ocupa el nodo
					GameObject tile = new GameObject("Wall");
					SpriteRenderer sprite = tile.AddComponent<SpriteRenderer> ();
					Transform tileTransform = tile.transform;
					tileTransform.position = new Vector3(currentNode.worldPosition.x, currentNode.worldPosition.y, 0.1f);

					// escalamos los cuadrados para que se ajusten al grid
					tileTransform.localScale = new Vector2 (nodeDiameter * 1.6f, nodeDiameter * 1.6f);

					// lo metemos como hijo de TileList
					tileTransform.SetParent(tileGroup.transform);

					// seteamos el nodo como no construible
					currentNode.isBuildable = false;

					// y le pintamos un muro
					sprite.sprite = tileList [6];
				}

				// y si no cumple ninguna de las anteriores, esta en la pantalla y hay que pintarlo
				else {
					// guardamos la celda o nodo
					Node currentNode = grid [x, y];

					// creamos un plano en blanco que ocupe lo que ocupa el nodo
					GameObject tile = new GameObject("Tile");
					SpriteRenderer sprite = tile.AddComponent<SpriteRenderer> ();
					Transform tileTransform = tile.transform;
					tileTransform.position = new Vector3(currentNode.worldPosition.x, currentNode.worldPosition.y, 0.1f);

					// escalamos los cuadrados para que se ajusten al grid
					tileTransform.localScale = new Vector2 (nodeDiameter * 1.6f, nodeDiameter * 1.6f);

					// lo metemos como hijo de TileList
					tileTransform.SetParent(tileGroup.transform);

					// si la celda esta por encima del muro superior o por debajo del inferior
					if (y < 10 || y > 29) {
						// seteamos el nodo como no construible
						currentNode.isBuildable = false;
					}

					// sacamos todos sus vecinos y los guardamos en una lista
					List<Node> neighboursList = GetNeighbours(currentNode);

					// ponemos una variable para ver el numero de conexiones caminables
					int walkableConnection = 0;

					// ahora que tenemos todos los nodos vecinos, empieza lo divertido
					// primero miramos cuantos de los lados son caminables
					foreach (Node n in neighboursList){
						if (n.isWalkable){
							walkableConnection++;
						}	
					}
					// ATENCION: el orden de los nodos es el siguiente:
					// - 0: izquierda
					// - 1: abajo
					// - 2: arriba
					// - 3: derecha
					// si no tiene conexiones con nada caminable o solo una
					// FIXME: Arreglar el crash si un camino toca el borde de la grilla
					if (walkableConnection == 0 || (walkableConnection == 1 && !currentNode.isWalkable) || !currentNode.isWalkable){
						// es cesped
						sprite.sprite = tileList [0];
					}
					// solo tiene una conexion y es caminable
					else if (walkableConnection == 1 && currentNode.isWalkable){
						// es un tope de camino
						// si el de la izquierda es caminable
						if (neighboursList [0].isWalkable) {
							// es un tope desde la izquierda
							sprite.sprite = tileList [4];
							tileTransform.Rotate(Vector3.forward, 90f);
						}
						// si el de la derecha es caminable
						else if (neighboursList [3].isWalkable) {
							// es un tope desde la derecha
							sprite.sprite = tileList [4];
							tileTransform.Rotate(Vector3.forward, -90f);
						}
						// si el de arriba es caminable
						else if (neighboursList [2].isWalkable) {
							// es un tope desde arriba
							sprite.sprite = tileList [4];
						}
						// si el caminable es el de abajo
						else {
							// es un tope desde abajo
							sprite.sprite = tileList [4];
						}
					}
					// si tiene dos conexiones es carretera recta o curva
					else if (walkableConnection == 2 && currentNode.isWalkable){
						// si el de la izquierda es caminable
						if (neighboursList [0].isWalkable) {
							// y el de la derecha tambien
							if (neighboursList [3].isWalkable) {
								// es una recta --
								sprite.sprite = tileList [2];
							}
							// y si la de arriba tambien
							else if (neighboursList [2].isWalkable){
								// es una curva -!
								sprite.sprite = tileList [1];
								tileTransform.Rotate (Vector3.forward, 180f);
							}
							// y si la de abajo tambien
							else if (neighboursList [1].isWalkable){
								// es una curva -¡
								sprite.sprite = tileList [1];
								tileTransform.Rotate (Vector3.forward, -90f);
							}
							else {
								Debug.Log("Error.");
							}
						}
						// si el de la derecha es caminable
						else if (neighboursList [3].isWalkable) {
							// y si la de arriba tambien
							if (neighboursList [2].isWalkable) {
								// es una curva !-
								sprite.sprite = tileList [1];
								tileTransform.Rotate (Vector3.forward, 90f);
							}
							// y si la de abajo tambien
							else if (neighboursList [1].isWalkable) {
								// es una curva ¡-
								sprite.sprite = tileList [1];
							} 
							else {
								Debug.Log ("Error.");
							}
						}
						// si el de arriba es caminable
						else if (neighboursList [2].isWalkable) {
							// y el de la abajo tambien
							if (neighboursList [1].isWalkable) {
								// es una recta I
								sprite.sprite = tileList [2];
								tileTransform.Rotate (Vector3.forward, 90f);
							}
							else {
								Debug.Log ("Error.");
							}
						}
						else {
							Debug.Log ("Error.");
						}
					}
					// si tiene 3 conexiones, es una T
					else if (walkableConnection == 3 && currentNode.isWalkable){
						// si el de la izquierda es caminable
						if (neighboursList [0].isWalkable) {
							// y el de la derecha tambien
							if (neighboursList [3].isWalkable) {
								//y el de arriba tambien lo es
								if (neighboursList [2].isWalkable) {
									// es un -!-
									sprite.sprite = tileList [3];
									tileTransform.Rotate (Vector3.forward, 90f);
								}
								// si no, es un -¡-
								else {
									sprite.sprite = tileList [3];
									tileTransform.Rotate (Vector3.forward, -90f);
								}
							}
							// y el de la derecha no lo es, es una -I
							else {
								sprite.sprite = tileList [3];
								tileTransform.Rotate (Vector3.forward, 180f);
							}
						}
						// si el de la izquierda no es caminable, es una I-
						else {
							sprite.sprite = tileList [3];
						}
					}
					// si tiene 4, es un cruce de calles en -I-
					else if (walkableConnection == 4 && currentNode.isWalkable){
						sprite.sprite = tileList [5];
					}
					// si no cumple ninguna condicion especial, esta roto
					else {
						Debug.Log ("Algo hemos jodio al pintar el fondo.");
					}
				}
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

	//Funcion para saber cuales son los vecinos de un nodo conocido
	public List<Node> GetNeighbours(Node node){

		// Creamos la nueva lista de nodos
		List<Node> neighboursList = new List<Node>();

		// Comprobamos cada nodo en sus cercanias
		for (int x = -1; x <= 1; x++){
			for (int y = -1; y <= 1; y++){

				// no queremos que compruebe ni diagonales ni el centro
				if ((x == 0 && y == 0) || (x == -1 && y == -1) || (x == -1 && y == 1) || (x == 1 && y == -1) || (x == 1 && y == 1)) {
					continue;
				}

				// cogemos la posicion del nodo en el grid y comprobamos sus alrededores
				int nodeX = node.gridX + x;
				int nodeY = node.gridY + y;

				// si los nodos de los lados no existen no queremos que los compruebe
				// usease, evitamos que compruebe nodos fuera de los bordes
				if ( nodeX >= 0 && nodeX < gridSizeX && nodeY >= 0 && nodeY < gridSizeY ){

					// si el nodo es valido, añadelo a la lista
					neighboursList.Add(grid[nodeX,nodeY]);
				}
			}
		}

		// y ahora devuelve la lista
		return neighboursList;
	}

	// Funcion para mostrar gizmos, para ver mejor el grid y los nodos
	void OnDrawGizmos() {

		// Dibuja un gizmo para ver el tamaño real del grid
		Gizmos.DrawWireCube (transform.position, new Vector2 (gridWorldSize.x, gridWorldSize.y));

		// Si el grid existe
		if (grid != null){

			// Por cada nodo en el grid
			foreach (Node n in grid){

				// coloreale gris si es caminable, rojo si no lo es
				// si n.isWalkable es true, entonces (?) es gris, si no (:) es rojo
				Gizmos.color = n.isWalkable ? Color.white : Color.red;

				// colorea el camino encontrado por el pathfinder
				if (pathList != null){
					foreach (List<Node> l in pathList){
						if (l.Contains(n)){
							Gizmos.color = Color.yellow;
						}	
					}
				}

				// y dibuja un cubo un pelin mas pequeño que el nodo para representarlo
				Gizmos.DrawCube (n.worldPosition, Vector3.one * (nodeDiameter - 1f));
			}
		}
	}
}
