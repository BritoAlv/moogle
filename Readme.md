# Moogle!

![](moogle.png)

> Proyecto de Programación I. Facultad de Matemática y Computación. Universidad de La Habana. Curso 2021.



Moogle! es una aplicación *totalmente original* cuyo propósito es buscar inteligentemente un texto en un conjunto de documentos.

Es una aplicación web, desarrollada con tecnología .NET Core 6.0, específicamente usando Blazor como *framework* web para la interfaz gráfica, y en el lenguaje C#.
La aplicación está dividida en dos componentes fundamentales:

- `MoogleServer` es un servidor web que renderiza la interfaz gráfica y sirve los resultados.
- `MoogleEngine` es una biblioteca de clases donde está... ehem... casi implementada la lógica del algoritmo de búsqueda.

Hasta el momento hemos logrado implementar gran parte de la interfaz gráfica (que es lo fácil), pero nos está causando graves problemas la lógica. Aquí es donde entras tú.


## Ejecutando el proyecto

Lo primero que tendrás que hacer para poder trabajar en este proyecto es instalar .NET Core 6.0 (lo que a esta altura imaginamos que no sea un problema, ¿verdad?). Luego, solo te debes parar en la carpeta del proyecto y ejecutar en la terminal de Linux:

```bash
make dev
```

Si estás en Windows, debes poder hacer lo mismo desde la terminal del WSL (Windows Subsystem for Linux). Si no tienes WSL ni posibilidad de instalarlo, deberías considerar seriamente instalar Linux, pero si de todas formas te empeñas en desarrollar el proyecto en Windows, el comando *ultimate* para ejecutar la aplicación es (desde la carpeta raíz del proyecto):

```bash
dotnet watch run --project MoogleServer
```
## Tu misión

Tu misión (si decides aceptarla) es ayudarnos a implementar el motor de búsqueda de Moogle! (sí, el nombre es así con ! al final). Para ello, deberás modificar el método `Moogle.Query` que está en la clase `Moogle` del proyecto `MoogleEngine`.

Este método devuelve un objeto de tipo `SearchResult`. Este objeto contiene los resultados de la búsqueda realizada por el usuario, que viene en un parámetro de tipo `string` llamado `query`.

Esto es lo que hay ahora en este método:

```cs
public static class Moogle
{
    public static SearchResult Query(string query) {
        // Modifique este método para responder a la búsqueda

        SearchItem[] items = new SearchItem[3] {
            new SearchItem("Hello World", "Lorem ipsum dolor sit amet", 0.9f),
            new SearchItem("Hello World", "Lorem ipsum dolor sit amet", 0.5f),
            new SearchItem("Hello World", "Lorem ipsum dolor sit amet", 0.1f),
        };

        return new SearchResult(items, query);
    }
}
```

Como puedes ver, dado que no sabemos implementarlo, hemos cableado la solución para que al menos devuelva algo.

El tipo `SearchResult` recibe en su constructor dos argumentos: `items` y `suggestion`. El parámetro `items` es un array de objetos de tipo `SearchItem`. Cada uno de estos objetos representa un posible documento que coincide al menos parcialmente con la consulta en `query`.

Cada `SearchItem` recibe 3 argumentos en su constructor: `title`, `snippet` y `score`. El parámetro `title` debe ser el título del documento (el nombre del archivo de texto correspondiente). El parámetro `snippet` debe contener una porción del documento donde se encontró el contenido del `query`. El parámetro `score` tendrá un valor de tipo `float` que será más alto mientras más relevante sea este item.

> ⚠️ Por supuesto, debes devolver los `items` ordenados de mayor a menor por este valor de `score`!

El parámetro `suggestion` de la clase `SearchResult` es para darle una sugerencia al usuario cuando su búsqueda da muy pocos resultados (tú debes decidir qué serían pocos resultados en este contexto). Esta sugerencia debe ser algo similar a la consulta del usuario pero que sí exista, de forma que si el usuario se equivoca, por ejemplo, escribiendo `"reculsibidá"`, y no aparece (evidentemente) ningún documento con ese contenido, le podamos sugerir la palabra `"recursividad"`.

## Sobre la búsqueda

Queremos que la búsqueda sea lo más inteligente posible, por ese motivo no podemos limitarnos a los documentos donde aparece exactamente la frase introducida por el usuario. Aquí van algunos requisitos que debe cumplir esta búsqueda, pero eres libre de adicionar cualquier otra funcionalidad que ayude a mejorar y hacer más inteligente la búsqueda.

- En primer lugar, el usuario puede buscar no solo una palabra sino en general una frase cualquiera.
- Si no aparecen todas las palabras de la frase en un documento, pero al menos aparecen algunas, este documento también queremos que sea devuelto, pero con un
`score` menor mientras menos palabras aparezcan.
- El orden en que aparezcan en el documento los términos del `query` en general no debe importar, ni siquiera que aparezcan en lugares totalmente diferentes del documento.
- Si en diferentes documentos aparecen la misma cantidad de palabras de la consulta, (por ejemplo, 2 de las 3 palabras de la consulta `"algoritmos de ordenación"`), pero uno de ellos contiene una palabra más rara (por ejemplo, `"ordenación"` es más rara que `"algoritmos"` porque aparece en menos documentos), el documento con palabras más raras debe tener un `score` más alto, porque es una respuesta más específica.
- De la misma forma, si un documento tiene más términos de la consulta que otro, en general debería tener un `score` más alto (a menos que sean términos menos relevantes).
- Algunas palabras excesivamente comunes como las preposiciones, conjunciones, etc., deberían ser ignoradas por completo ya que aparecerán en la inmensa mayoría de los documentos (esto queremos que se haga de forma automática, o sea, que no haya una lista cableada de palabras a ignorar, sino que se computen de los documentos).

### Operadores de búsqueda

Con estas ideas ya podemos hacer algo, pero para mejorar la búsqueda aún más queremos adicionar operadores a la consulta que permitan darle más control al usuario. Por ejemplo:

- Un símbolo `!` delante de una palabra (e.j., `"algoritmos de búsqueda !ordenación"`) indica que esa palabra **no debe aparecer** en ningún documento que sea devuelto.
- Un símbolo `^` delante de una palabra (e.j., `"algoritmos de ^ordenación"`) indica que esa palabra **tiene que aparecer** en cualquier documento que sea devuelto.
- Un símbolo `~` entre dos o más términos indica que esos términos deben **aparecer cerca**, o sea, que mientras más cercanos estén en el documento mayor será la relevancia. Por ejemplo, para la búsqueda `"algoritmos ~ ordenación"`, mientras más cerca están las palabras `"algoritmo"` y `"ordenación"`, más alto debe ser el `score` de ese documento.
- Cualquier cantidad de símbolos `*` delante de un término indican que ese término es más importante, por lo que su influencia en el `score` debe ser mayor que la tendría normalmente (este efecto será acumulativo por cada `*`, por ejemplo `"algoritmos de **ordenación"` indica que la palabra `"ordenación"` tiene dos veces más prioridad que `"algoritmos"`).

### Ideas extras

Nuestros usuarios son muy exigentes, pero no podemos darles todo. Algunas ideas que no creemos que sean estrictamente necesarias pero que harían nuestra aplicación mucho mejor son:

- Si las palabras exactas no aparecen, pero aparecen palabras derivadas de la misma raíz, también queremos devolver esos documentos (por ejemplo, si no está `"ordenación"` pero estar `"ordenados"`, ese documento puede devolverse pero con un `score` menor).
- Si aparecen palabras relacionadas aunque no tengan la misma raíz (por ejemplo si la búsqueda es `"computadora"` y el documento tiene `"ordenador"`), también queremos devolver esos documentos pero con menor `score` que si apareciera la palabra exacta o una de la misma raíz.

### Evaluación del `score`

De manera general el valor de `score` debe corresponder a cuán relevante es el documento devuelto para la búsqueda realizada. Como te hemos explicado antes, hay muchos factores que aumentan o disminuyen esta relevancia.

Como todos estos factores están en oposición unos con otros, debes encontrar una forma de balancearlos en alguna fórmula que permita evaluar todo documento con respecto a toda consulta posible. Si un documento no tiene ningún término de la consulta, y no es para nada relevante, entonces su `score` sería `0` como mínimo, pero no debe haber ningún error o excepción en estos casos. Tú debes decidir cómo dar peso a cada elemento que puede influir en el `score` para que los documentos devueltos tengan la mayor relevancia posible.

### Algoritmos de búsqueda

Te hemos dado este proyecto justamente a tí porque sabemos que ustedes en MatCom tienen conocimientos que el resto de nosotros ni imaginamos. En particular, sabemos que hay algo llamado "modelo vectorial" que aparentemente tiene que ver con un arte arcano llamado "álgebra" que permite hacer estas búsquedas muchísimo más rápido que con un simple ciclo `for` por cada documento. De más está decir que esperamos que hagas gala de estos poderes extraordinarios que la matemática te concedió, porque para hacer esto con un doble `for` hubiéramos contratado a cualquier otro.

Si te sirve de algo, hace unos meses contratamos a un gurú de los algoritmos de búsqueda para ver si nos podía enseñar a implementar este proyecto por nosotros mismos, y nos dio una conferencia de 4 horas de la que no entendimos casi nada (debía ser uno de ustedes, porque parecía llevar meses sin afeitar y hablaba solo consigo mismo, susurrando cosas como "turing completo" y "subespacio propio"). En fin, aunque de poco sirvió, al menos uno de nosotros recordó, luego de la conferencia, que había algo llamado "TF-IDF" que aparentemente era la clave para resolver este problema de búsqueda. Seguro que tu sabes de qué se trata.

## Sobre la interfaz gráfica

Como verás cuando ejecutes la aplicación (que se explica más abajo), la interfaz gráfica es bastante pobre. En principio, no tienes obligación de trabajar en esta parte del proyecto (sabemos que ustedes los científicos de la computación están por encima de estas mundeces).

Pero si nos quieres ayudar, eres libre de modificar la interfaz gráfica todo lo que desees, eso sí, siempre que se mantenga la idea original de la aplicación. Si te interesa aprender Blazor, HTML, o CSS, eres libre de jugar con el código de la interfaz gráfica, que está en el proyecto `MoogleServer`.

## Sobre el contenido a buscar

La idea original del proyecto es buscar en un conjunto de archivos de texto (con extensión `.txt`) que estén en la carpeta `Content`. Desgraciadamente, nuestro último programador que sabía cargar y leer archivos fue contratado por nuestra compañía enemiga *MoneySoft*. Por lo tanto, tendrás que lidiar con esta parte tú mismo.



