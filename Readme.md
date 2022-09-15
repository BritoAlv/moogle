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
## Sobre el contenido a buscar

La idea original del proyecto es buscar en un conjunto de archivos de texto (con extensión `.txt`) que estén en la carpeta `Content`.



