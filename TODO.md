### Mínimo de requisitos para publicar una "beta" de juego:
#### - Gráficos
- [ ] Todos los assets gráficos (sprites y esas cosas) terminados.

#### - Sonido
- [ ] Sonidos hechos para los siguientes eventos:
    - [ ] Disparo de torreta de disparo único
    - [ ] Disparo de torreta de disparo parabólico (¿cañón?)
    - [ ] Disparo de torreta de daño constante de zona.
    - [ ] Disparo de torreta de disparo aéreo (si al final es similar a la de disparo único, usamos el suyo).
    - [ ] Impacto del disparo de la torreta de disparo único.
    - [ ] Impacto del disparo de la torreta de disparo parabólico.
    - [ ] Impacto del disparo de la torreta de daño constante de zona.
    - [ ] Impacto del disparo de la torreta de disparo aéreo (si al final es similar a la de disparo único, usamos el suyo).
    - [ ] Muerte de los enemigos (este podemos usar uno común para todos salvo, tal vez, para el jefe).
    - [ ] \(Opcional)Victoria.
    - [ ] \(Opcional)Derrota.
    - [ ] \(Opcional)Musica de fondo para el menú.
    - [ ] \(Opcional)Musica de fondo para la pantalla de juego.

#### - Jugabilidad
- [ ] Tener terminadas las siguientes torretas:
    - [ ] Torreta de disparo único (¿balista?).
    - [ ] Torreta de disparo parabólico con impacto de zona (¿cañón?).
    - [ ] Torreta de daño constante de zona (¿veneno o rayos?).
    - [ ] Torreta de disparo a unidades aéreas (¿balista, u otra cosa parecida?).
- [ ] Tener terminados los siguientes enemigos:
    - [ ] Enemigo básico 1 (lento y con poca vida).
    - [ ] Enemigo básico 2 (muy lento pero con bastante vida).
    - [ ] Enemigo básico 3 (rápido y con muy poca vida).
    - [ ] Enemigo básico 4 (enemigo volador, velocidad media o lenta, muy poca vida).
    - [ ] Jefe final (nostringval, muy lento y con mucha vida).
- [ ] Tener terminados 10 niveles (layout de muy simple a muy complejo, con jefe solo en el último nivel).

#### - Interfaz
- [ ] Hacer que la interfaz del juego muestre todos los datos necesarios (puntuación, dinero, contador de oleadas, etc...) durante la partida.
- [ ] Hacer que, tras terminar una partida puedas salir o continuar al siguiente nivel.
- [ ] Hacer que solicite un nombre al jugador antes de comenzar el nivel, y que lo recuerde al continuar la partida más tarde.
- [ ] Hacer un menú de selección de nivel, donde los niveles se desbloqueen según se van completando.
- [ ] Hacer un menú principal decente y que funcione.
- [ ] Hacer un archivo txt de créditos, que sea mostrado en el apartado créditos del menú principal.
- [ ] \(Opcional)Hacer que la UI tenga una temática medieval, acorde con los demás sprites.

#### - Código
- [ ] Optimizar el algoritmo de pathfinding.
- [ ] Hacer que la cantidad de barricadas que se tienen sólo se actualice cuando pones o quitas una barricada.
- [ ] Implementar interfaces para evitar joder los "Spawner" de los distintos niveles.
- [ ] Hacer que al poner el ratón sobre una torreta se muestre un círculo verde (o de otro color) que muestre el alcance de la torreta.
- [ ] Mejorar el sistema de pintado automático del mapa para que genere distintas listas de Tiles dependiendo del terreno, y que añada la posición en el grid de la Tile al nombre del Tile, para poder localizarla mejor una vez creada.

#### - Miscelánea
- [ ] Reorganizar la carpeta "Sprites" para separar los sprites que usamos para las pruebas ("Test"), los que usamos para ver las cosas en el editor ("Editor") y los que vamos a poner definitivos en el juego ("Game").
- [ ] Licenciar todo el código bajo alguna licencia (posiblemente GPL3, por decidir).
- [ ] Ser millonarios.
