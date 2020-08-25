# Compilador de mini C#
> Análisis léxico de un archivo escrito en C#
> José Eduardo Tejeda León             1097218
> Mario Estuardo Gómez Orellana     1020618


<a href="https://drive.google.com/file/d/1LuXkIy3qcK6N0ac09yaUVioggTmSz76k/view?usp=sharing" download="Descargar ejecutable"><img src="data:image/jpeg;base64,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" /></a>


![](https://media1.giphy.com/media/W5TVax7yZ79xhfpwei/giphy.gif)

## Requerimientos

Framework: .NET Framework 4.7.2

Librerías adicionales si se desea imprimir el archivo de salida: iTextSharp
Programas adicionales si se desea imprimir el archivo de salida: Adobe Acrobat Reader DC https://get.adobe.com/es/reader/ 


## Instalación de dependencias

Para instalar la librería de iTextSharp sigue los siguientes pasos.



#Dirígete al manejador de NuGets


![image](https://drive.google.com/uc?export=view&id=1SxLEJA44EppRLstOHSCy3FXSBLyK1KNX)



#Instala la librería


![image](https://drive.google.com/uc?export=view&id=1phPiR__vSdldWj1qiPsdT66oi6gSjE0I)




## Ejemplo de uso

Carga un archivo que contenga código que cumpla con la gramática de mini c# para que sea analizado, abajo puedes ver un ejemplo.

```sh
int a;

void main() {
   int b;
   int a;
   int d;

   d = 2 + 3 * 4 - 6;
   b = 3;
   a = b + 2;
}
#
```

El analizador escaneará este archivo como se describió en la sección de flujo del código, como salida obtendrás algo similar a esto. Los e

```sh
int    	---      	en la línea 1 cols 1-3 es un(a) palabra reservada


a    	---      	en la línea 1 cols 5-5 es un(a) identificador


;    	---      	en la línea 1 cols 6-6 es un(a) operador o signo de puntuación


void    	---      	en la línea 3 cols 1-4 es un(a) palabra reservada


main    	---      	en la línea 3 cols 6-9 es un(a) identificador


()    	---      	en la línea 3 cols 10-11 es un(a) operador o signo de puntuación


{    	---      	en la línea 3 cols 13-13 es un(a) operador o signo de puntuación


int    	---      	en la línea 4 cols 4-6 es un(a) palabra reservada


b    	---      	en la línea 4 cols 8-8 es un(a) identificador


;    	---      	en la línea 4 cols 9-9 es un(a) operador o signo de puntuación


int    	---      	en la línea 5 cols 4-6 es un(a) palabra reservada


a    	---      	en la línea 5 cols 8-8 es un(a) identificador


;    	---      	en la línea 5 cols 9-9 es un(a) operador o signo de puntuación


int    	---      	en la línea 6 cols 4-6 es un(a) palabra reservada


d    	---      	en la línea 6 cols 8-8 es un(a) identificador


;    	---      	en la línea 6 cols 9-9 es un(a) operador o signo de puntuación


d    	---      	en la línea 8 cols 4-4 es un(a) identificador


=    	---      	en la línea 8 cols 6-6 es un(a) operador o signo de puntuación


2    	---      	en la línea 8 cols 8-8 es un(a) constante tipo int


+    	---      	en la línea 8 cols 10-10 es un(a) operador o signo de puntuación


3    	---      	en la línea 8 cols 12-12 es un(a) constante tipo int


*    	---      	en la línea 8 cols 14-14 es un(a) operador o signo de puntuación


4    	---      	en la línea 8 cols 16-16 es un(a) constante tipo int


-    	---      	en la línea 8 cols 18-18 es un(a) operador o signo de puntuación


6    	---      	en la línea 8 cols 20-20 es un(a) constante tipo int


;    	---      	en la línea 8 cols 21-21 es un(a) operador o signo de puntuación


b    	---      	en la línea 9 cols 4-4 es un(a) identificador


=    	---      	en la línea 9 cols 6-6 es un(a) operador o signo de puntuación


3    	---      	en la línea 9 cols 8-8 es un(a) constante tipo int


;    	---      	en la línea 9 cols 9-9 es un(a) operador o signo de puntuación


a    	---      	en la línea 10 cols 4-4 es un(a) identificador


=    	---      	en la línea 10 cols 6-6 es un(a) operador o signo de puntuación


b    	---      	en la línea 10 cols 8-8 es un(a) identificador


+    	---      	en la línea 10 cols 10-10 es un(a) operador o signo de puntuación


2    	---      	en la línea 10 cols 12-12 es un(a) constante tipo int


;    	---      	en la línea 10 cols 13-13 es un(a) operador o signo de puntuación


}    	---      	en la línea 11 cols 1-1 es un(a) operador o signo de puntuación


#    	---      	en la línea 12 cols 1-1 posee error el cual es: es un caracter no reconocido

```



## Explicación detallada del flujo del código y manejo de errores

Dentro de la clase de “LexicalAnalyzer” en el procedimiento “ReadFileAndAnalyzeDocument”, en este procedimiento se crea un nodo que contendrá toda la información necesaria para poder mostrar el lexema luego del análisis (Valor, token, descripción, columna de inicio y fin, lína de inicio y fin) luego de llenar los datos del nodo  se inicia la lectura del archivo de entrada línea por línea. Cada línea es enviada al procedimiento “analyzeLine”.

Dentro de este procedimiento se recorre cada carácter de la línea. En cada carácter se pregunta si es algún separador de lexema (espacio, tab horizontal o nueva línea) de serlo se llama al procedimiento  “finishLexemeNodeAndAddToLexemes” que agrega lo que se tenga hasta ese momento en una lista de lexemas (“Lexemes”) y se crea un nuevo nodo para continuar con el siguiente carácter.

Si no es un separador, se pregunta si no está dentro de una lista definida de operadores y signos de puntuación (“OperatorsAndPuncChars”), si no es una letra o un dígito, si no es un guión bajo (“_”) o una comilla (‘“‘), si cumple con esto se procede a preguntar si el nodo actual ya contiene un lexema o si este está vacío. De no estar vacío se llama al procedimiento “finishLexemeNodeAndAddToLexemes” y luego se procede a escribir un error de “Carácter no reconocido” dado que no es válido para la gramática.  

La tercera validación pertenece a los caracteres que sean reconocidos como operadores de la gramática y no estén dentro de una cadena o comentario, por las mismas razones de la segunda validación se valida si el nodo está vacío, en dado caso no lo esté se guarda y se hace un retroceso para volver analizar el carácter que ya se había leído esto dado que al momento de que entre aquí otra vez y el nodo esté vacío ya se registrará el caracter leído como un operador. Dentro de este método se valida si es el comienzo de algún tipo de comentario viendo hacia adelante un carácter o bien que se trate de un fin de comentario sin emparejar.

Si no cumple con ninguna de las condiciones previas, se pregunta si el lexema actual está vacío, si está vacío se procede a evaluar dentro de una estructura selectiva que representa expresiones regulares. Al momento de encontrar la expresión cuyo inicio sea el carácter actual, se propone un token al lexema y se agrega al lexema el carácter actual. 

En dado caso todo lo anterior no se cumpla significa que ya se está analizando un lexema, esto se hace mediante una estructura selectiva que presenta comportamiento de un autómata finito determinista, este se estructuró analizando la composición de cada lexema, armando una expresión regular para el mismo y llevando esta expresión a la estructura selectiva. En esta sección se maneja la mayor parte de errores ya que con el flujo del autómata se determina el token definitivo del lexema. Con esto se puede analizar si el lexema cumple con las condiciones de la expresión regular definida para él.

## Justificación

Este analizador ha sido depurado con múltiple cantidad de archivos de prueba por lo tanto es un analizador robusto y 100% funcional para la gramática explicada en la sección de “Explicación detallada del flujo del código”

## Expresiones regulares de la gramática

Identificadores: ((a-z)| (A-Z) )((A-Z)|(a-z)|(0-9)|(_))*

Tipos de constantes:
Entero Hexa:   0 (x|X)(0-9|a-f|A-F)+
 
Entero: Dec (0-9)+
 
Bool: true|false

Exponencial  (0-9)+ (.)(0-9)*(e|E)(+|-)?(0-9)+ 
 
Double (0-9)+ (.)(0-9)*

Constante tipo string: (“)( ASCII(1-255)(”)

Operadores de la gramática

+ - * / % < <= > >= = == != && || ! ; , . [ ] ( ) { } [] () {}

Comentarios : 
(//)(ASCII(1-255)*)
	(/*)(ASCII(1-255)|(\n))*(*/)

