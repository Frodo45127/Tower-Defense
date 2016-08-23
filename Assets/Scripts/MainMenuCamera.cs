using UnityEngine;
using System.Collections;
// hay que añadir el siguiente namespace para poder manipular la UI
using UnityEngine.UI;

//-----------------------------------------------------------------------
// MainMenuCamera.cs
//
// Este script es el que controla la camara del menu principal.
// Esto incluye el movimiento de un menu a otro y el fade del logo.
//
//-----------------------------------------------------------------------

public class MainMenuCamera : MonoBehaviour {

	// cacheo del transform de la cámara y de los canvas que hemos cogido con el gameobject
	private Transform myTransform;
	private Transform mainMenuTransform;
	private Transform highScoreTableTransform;
	private Transform creditsTransform;

	// pillamos los canvas del menú principal
	public GameObject mainMenu;
	public GameObject highScoreTable;
	public GameObject credits;

	// variables para controlar los giros
	private bool moveToMainMenu;
	private bool moveToHighScoreList;
	private bool moveToCredits;

	// variables para el logo
	// variable para sabes si está saliendo (false) o está desapareciendo (true)
	private bool isEnding;

	// variable para saber si hay que desactivar el canvas del intro
	private bool isTimeToShowMainMenu;

	// variables para guardar el valor del alpha
	private float alpha;
	private float minAlpha;
	private float maxAlpha;
	private float alphaLogoBackground;
	private float minAlphaLogoBackground;

	// cacheos
	private Image Logo;
	private Image LogoBackground;
	private GameObject CanvasIntro;


	void Start() {

		//inicializamos las variables
		myTransform = transform;
		mainMenuTransform = mainMenu.transform;
		highScoreTableTransform = highScoreTable.transform;
		creditsTransform = credits.transform;

		// cacheamos el logo, el fondo del logo y el canvas e inicializamos los parámateros del logo
		CanvasIntro = GameObject.FindGameObjectWithTag ("CanvasIntro");
		LogoBackground = GameObject.FindGameObjectWithTag("LogoBackground").GetComponent<Image> ();
		Logo = GameObject.FindGameObjectWithTag("Logo").GetComponent<Image> ();
		alpha = 0;
		minAlpha = 0f;
		maxAlpha = 1.5f;
		alphaLogoBackground = 1f;
		minAlphaLogoBackground = 0f;
		isEnding = false;
		isTimeToShowMainMenu = false;
	}

	void Update (){

		// si la intro no ha salido por primera vez
		if (GameManager.Instance.IsFirstStart) {
			LogoFadeInAndOut ();
		}
		else {
			CanvasIntro.SetActive (false);
		}
			
		// A partir de aquí es código para que funcione el giro de la cámara

		// TODO: mover todo este tocho de codigo a una funcion aparte
		// si moveToHighScoreList es true nos movemos hacia ella
		if (moveToHighScoreList == true) {

			// si no esta pegada muevete suavizado
			// esto es para que no se quede moviendose infinitamente
			if (Vector2.Distance(myTransform.position, highScoreTableTransform.position) > 0.2f){

				// sacamos la posicion en vector 2 de la tabla de puntuaciones y de nosotros
				Vector2 highScoreTablePosition = new Vector2 (highScoreTableTransform.position.x, highScoreTableTransform.position.y);
				Vector2 myPosition = new Vector2 (myTransform.position.x, myTransform.position.y);

				// sacamos la proxima posicion con movimiento suavizado
				Vector2 myNewPosition = Vector2.Lerp (myPosition, highScoreTablePosition, 0.1f);

				// nos movemos a la proxima posicion sin cambiar nuestra Z
				myTransform.position = new Vector3 (myNewPosition.x, myNewPosition.y, myTransform.position.z);
			}

			// si esta muy cerca del destino
			else {
				
				// muevelo directamente a la coordenada de destino y desactiva el movimiento
				myTransform.position = new Vector3 (highScoreTableTransform.position.x, highScoreTableTransform.position.y, myTransform.position.z);
				moveToHighScoreList = false;
			}
		}

		// si lo es moveToMainMenu nos movemos hacia el menu principal
		else if (moveToMainMenu == true){
			
			// si no esta pegada muevete suavizado
			// esto es para que no se quede moviendose infinitamente
			if (Vector2.Distance(myTransform.position, mainMenuTransform.position) > 0.2f){

				// sacamos la posicion en vector 2 del menu principal y de nosotros
				Vector2 mainMenuPosition = new Vector2 (mainMenuTransform.position.x, mainMenuTransform.position.y);
				Vector2 myPosition = new Vector2 (myTransform.position.x, myTransform.position.y);

				// sacamos la proxima posicion con movimiento suavizado
				Vector2 myNewPosition = Vector2.Lerp (myPosition, mainMenuPosition, 0.1f);

				// nos movemos a la proxima posicion sin cambiar nuestra Z
				myTransform.position = new Vector3 (myNewPosition.x, myNewPosition.y, myTransform.position.z);
			}

			// si esta muy cerca del destino
			else {

				// muevelo directamente a la coordenada de destino y desactiva el movimiento
				myTransform.position = new Vector3 (mainMenuTransform.position.x, mainMenuTransform.position.y, myTransform.position.z);
				moveToMainMenu = false;
			}
		}
		// si lo es moveToCredits nos movemos hacia los creditos
		else if (moveToCredits == true){
			
			// si no esta pegada muevete suavizado
			// esto es para que no se quede moviendose infinitamente
			if (Vector2.Distance(myTransform.position, creditsTransform.position) > 0.2f){

				// sacamos la posicion en vector 2 de la pantalla de creditos y de nosotros
				Vector2 creditsPosition = new Vector2 (creditsTransform.position.x, creditsTransform.position.y);
				Vector2 myPosition = new Vector2 (myTransform.position.x, myTransform.position.y);

				// sacamos la proxima posicion con movimiento suavizado
				Vector2 myNewPosition = Vector2.Lerp (myPosition, creditsPosition, 0.1f);

				// nos movemos a la proxima posicion sin cambiar nuestra Z
				myTransform.position = new Vector3 (myNewPosition.x, myNewPosition.y, myTransform.position.z);
			}

			// si esta muy cerca del destino
			else {

				// muevelo directamente a la coordenada de destino y desactiva el movimiento
				myTransform.position = new Vector3 (creditsTransform.position.x, creditsTransform.position.y, myTransform.position.z);
				moveToCredits = false;
			}
		}
	}

	// funcion para hacer el fade in y el fade out del logo
	void LogoFadeInAndOut(){
		// si el alpha es menor que 2 y está saliendo el logo
		if (isEnding == false && alpha <= maxAlpha) {
			// aumenta el alfa en cada frame
			alpha = alpha + 0.01f;
			// aplica el nuevo valor de alpha
			// ANALOGIA DE PLSQL
			// CREA UN CURSOR CON LOS DATOS DE COLOR DEL LOGO (r,g,b,a)
			Color newalpha = Logo.color;
			// A LA (a) LE APLICA EL NUEVO VALOR
			newalpha.a = alpha;
			// ESCRIBE EN EL COLOR DEL LOGO LOS DATOS ALMACENADOS EN EL CURSOR
			Logo.color = newalpha;
			// FIN DE LA ANALOGÍA
		}
		// si se ha pasado con el alfa (el logo se está mostrando un par de segundos) empieza el final
		else if (isEnding == false && alpha > maxAlpha) {
			isEnding = true;
		}
		// si el alpha es positivo y el logo ha estado en pantalla un rato, empieza a ocultarlo
		else if (isEnding == true && alpha > minAlpha) {
			// reduce el alfa en cada frame
			alpha = alpha - 0.01f;
			// aplica el nuevo valor de alpha
			// ANALOGIA DE PLSQL
			// CREA UN CURSOR CON LOS DATOS DE COLOR DEL LOGO (r,g,b,a)
			Color newalpha = Logo.color;
			// A LA (a) LE APLICA EL NUEVO VALOR
			newalpha.a = alpha;
			// ESCRIBE EN EL COLOR DEL LOGO LOS DATOS ALMACENADOS EN EL CURSOR
			Logo.color = newalpha;
			// FIN DE LA ANALOGÍA
		}
		// tras ocultar el logo y esperar un poco en negro, reduce el negro y muestra el menu
		else if (isEnding == true && alphaLogoBackground > minAlphaLogoBackground && isTimeToShowMainMenu == false) {
			// reduce el alfa del panel en cada frame
			alphaLogoBackground = alphaLogoBackground - 0.06f;
			// aplica el nuevo valor de alpha
			// ANALOGIA DE PLSQL
			Color newalpha = LogoBackground.color;
			// A LA (a) LE APLICA EL NUEVO VALOR
			newalpha.a = alphaLogoBackground;
			// ESCRIBE EN EL COLOR DEL LOGO LOS DATOS ALMACENADOS EN EL CURSOR
			LogoBackground.color = newalpha;
			// FIN DE LA ANALOGÍA
		}
		// después, desactiva el canvas para que el menu sea clickeable
		else if (isEnding == true && alphaLogoBackground <= minAlphaLogoBackground && isTimeToShowMainMenu == false) {
			isTimeToShowMainMenu = true;
		} 
		else if (isTimeToShowMainMenu == true) {
			CanvasIntro.SetActive (false);
			// le dice al gameManager que ya ha mostrado la intro una vez, que no la muestre mas
			GameManager.Instance.IsFirstStart = false;
		}
	}
		
	// función para girar la cámara para ver las puntuaciones
	void MoveCameraToHighScores () {
		moveToMainMenu = false;
		moveToHighScoreList = true;
		moveToCredits = false;
	}

	// función para girar la cámara para ver los créditos
	void MoveCameraToCredits () {
		moveToMainMenu = false;
		moveToHighScoreList = false;
		moveToCredits = true;
	}

	// función para girar la cámara hacia el menú
	void MoveCameraToMainMenu () {
		moveToHighScoreList = false;
		moveToMainMenu = true;
		moveToCredits = false;
	}
}
