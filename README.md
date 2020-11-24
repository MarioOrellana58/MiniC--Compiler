# Compilador de mini C#
> Análisis léxico de un archivo escrito en C#
> José Eduardo Tejeda León             1097218
> Mario Estuardo Gómez Orellana     1020618


[English version](https://github.com/MarioOrellana58/MiniC--Compiler/blob/master/README-en.md)

<a href="https://drive.google.com/file/d/18B-DVnjRqm_pjGAUgKhFbL_qr_79WvY5/view?usp=sharing" download="Descargar ejecutable"><img src="https://i.ibb.co/zSNLWLg/3.png" /></a>




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

Dentro de la clase de “LexicalAnalyzer” en el procedimiento “ReadFileAndAnalyzeDocument”, se crea un nodo que contendrá toda la información necesaria para poder mostrar el lexema luego del análisis (Valor, token, descripción, columna de inicio y fin, lína de inicio y fin) luego de crear el nodo  se inicia la lectura del archivo de entrada línea por línea. Cada línea es enviada al procedimiento “analyzeLine”.

Dentro de este procedimiento se recorre cada carácter de la línea. En cada carácter se pregunta si es algún separador de lexema (espacio, tab horizontal o nueva línea) de serlo se llama al procedimiento  “finishLexemeNodeAndAddToLexemes” que agrega lo que se tenga hasta ese momento en una lista de lexemas (“Lexemes”) y se crea un nuevo nodo para continuar con el siguiente carácter.

Si no es un separador, se pregunta si no está dentro de una lista definida de operadores y signos de puntuación (“OperatorsAndPuncChars”), si no es una letra o un dígito, si no es un guión bajo (“_”) o una comilla (‘“‘), si cumple con esto se procede a preguntar si el nodo actual ya contiene un lexema o si este está vacío. De no estar vacío se llama al procedimiento “finishLexemeNodeAndAddToLexemes” y luego se procede a escribir un error de “Carácter no reconocido” dado que no es válido para la gramática.  

La tercera validación pertenece a los caracteres que sean reconocidos como operadores de la gramática y no estén dentro de una cadena o comentario, por las mismas razones de la segunda validación se valida si el nodo está vacío, en dado caso no lo esté se guarda y se hace un retroceso para volver analizar el carácter que ya se había leído esto dado que al momento de que entre aquí otra vez y el nodo esté vacío ya se registrará el caracter leído como un operador. Dentro de este método se valida si es el comienzo de algún tipo de comentario viendo hacia adelante un carácter o bien que se trate de un fin de comentario sin emparejar.

Si no cumple con ninguna de las condiciones previas, se pregunta si el lexema actual está vacío, si está vacío se procede a evaluar dentro de una estructura selectiva que representa expresiones regulares. Al momento de encontrar la expresión cuyo inicio sea el carácter actual, se propone un token al lexema y se agrega al lexema el carácter actual. 

En dado caso todo lo anterior no se cumpla significa que ya se está analizando un lexema, esto se hace mediante una estructura selectiva que presenta comportamiento de un autómata finito determinista, este se estructuró analizando la composición de cada lexema, armando una expresión regular para el mismo y llevando esta expresión a la estructura selectiva. En esta sección se maneja la mayor parte de errores ya que con el flujo del autómata se determina el token definitivo del lexema. Con esto se puede analizar si el lexema cumple con las condiciones de la expresión regular definida para él.

## Justificación

Este analizador ha sido depurado con múltiples archivos de prueba por lo tanto es un analizador robusto y 100% funcional para la gramática explicada en la sección de “Explicación detallada del flujo del código” que obedece a las expresiones regulares de abajo.

## Expresiones regulares de la gramática

Identificadores: 
```sh
((a-z)| (A-Z) )((A-Z)|(a-z)|(0-9)|(_))*
```

Tipos de constantes:
```sh
Entero Hexa:   0 (x|X)(0-9|a-f|A-F)+
 
Entero: Dec (0-9)+
 
Bool: true|false

Exponencial  (0-9)+ (.)(0-9)*(e|E)(+|-)?(0-9)+ 
 
Double (0-9)+ (.)(0-9)*

Constante tipo string: (“)( ASCII(1-255)(”)
```

Operadores de la gramática
```sh
+ - * / % < <= > >= = == != && || ! ; , . [ ] ( ) { } [] () {}
```

Comentarios : 
```sh
(//)(ASCII(1-255)*)
(/*)(ASCII(1-255)|(\n))*(*/)
```

Palabras reservadas : 
```sh
void 
int 
double 
bool 
string 
class 
const 
interface 
null 
this 
for 
while 
foreach 
if 
else 
return 
break 
New 
NewArray 
```


# Analizador sintáctico

El siguiente paso es verificar la sintaxis del archivo de entrada, solo si el archivo no tiene ningún error léxico, el compilador verificará la sintaxis del programa. Obtendrá algo como el ejemplo de abajo si hay un error. En caso de que no los haya, recibirá un mensaje indicándole que todo salió bien. El manejo de errores de sintaxis se encuentra más detallado en la sección “Sintaxis y manejo de errores”. Para el mismo archivo utilizado anteriormente en el analizador léxico pero ahora sin el #, el resultado del analizador de sintaxis sería
```sh
Se esperaba "(", venía "()" en las columnas 10 hasta 11 en la línea número: 3


Se esperaba "bool", venía "{" en las columnas 13 hasta 13 en la línea número: 3


Se esperaba "class", venía "{" en las columnas 13 hasta 13 en la línea número: 3


Se esperaba "const", venía "{" en las columnas 13 hasta 13 en la línea número: 3


Se esperaba "double", venía "{" en las columnas 13 hasta 13 en la línea número: 3


Se esperaba "identificador", venía "{" en las columnas 13 hasta 13 en la línea número: 3


Se esperaba "int", venía "{" en las columnas 13 hasta 13 en la línea número: 3


Se esperaba "interface", venía "{" en las columnas 13 hasta 13 en la línea número: 3


Se esperaba "string", venía "{" en las columnas 13 hasta 13 en la línea número: 3


Se esperaba "void", venía "{" en las columnas 13 hasta 13 en la línea número: 3


Se esperaba "[]", venía "=" en las columnas 6 hasta 6 en la línea número: 8


Se esperaba "identificador", venía "=" en las columnas 6 hasta 6 en la línea número: 8


Se esperaba "bool", venía "2" en las columnas 8 hasta 8 en la línea número: 8


Se esperaba "class", venía "2" en las columnas 8 hasta 8 en la línea número: 8


Se esperaba "const", venía "2" en las columnas 8 hasta 8 en la línea número: 8


Se esperaba "double", venía "2" en las columnas 8 hasta 8 en la línea número: 8


Se esperaba "identificador", venía "2" en las columnas 8 hasta 8 en la línea número: 8


Se esperaba "int", venía "2" en las columnas 8 hasta 8 en la línea número: 8


Se esperaba "interface", venía "2" en las columnas 8 hasta 8 en la línea número: 8


Se esperaba "string", venía "2" en las columnas 8 hasta 8 en la línea número: 8


Se esperaba "void", venía "2" en las columnas 8 hasta 8 en la línea número: 8


Se esperaba "bool", venía "+" en las columnas 10 hasta 10 en la línea número: 8


Se esperaba "class", venía "+" en las columnas 10 hasta 10 en la línea número: 8


Se esperaba "const", venía "+" en las columnas 10 hasta 10 en la línea número: 8


Se esperaba "double", venía "+" en las columnas 10 hasta 10 en la línea número: 8


Se esperaba "identificador", venía "+" en las columnas 10 hasta 10 en la línea número: 8


Se esperaba "int", venía "+" en las columnas 10 hasta 10 en la línea número: 8


Se esperaba "interface", venía "+" en las columnas 10 hasta 10 en la línea número: 8


Se esperaba "string", venía "+" en las columnas 10 hasta 10 en la línea número: 8


Se esperaba "void", venía "+" en las columnas 10 hasta 10 en la línea número: 8


Se esperaba "bool", venía "3" en las columnas 12 hasta 12 en la línea número: 8


Se esperaba "class", venía "3" en las columnas 12 hasta 12 en la línea número: 8


Se esperaba "const", venía "3" en las columnas 12 hasta 12 en la línea número: 8


Se esperaba "double", venía "3" en las columnas 12 hasta 12 en la línea número: 8


Se esperaba "identificador", venía "3" en las columnas 12 hasta 12 en la línea número: 8


Se esperaba "int", venía "3" en las columnas 12 hasta 12 en la línea número: 8


Se esperaba "interface", venía "3" en las columnas 12 hasta 12 en la línea número: 8


Se esperaba "string", venía "3" en las columnas 12 hasta 12 en la línea número: 8


Se esperaba "void", venía "3" en las columnas 12 hasta 12 en la línea número: 8


Se esperaba "bool", venía "*" en las columnas 14 hasta 14 en la línea número: 8


Se esperaba "class", venía "*" en las columnas 14 hasta 14 en la línea número: 8


Se esperaba "const", venía "*" en las columnas 14 hasta 14 en la línea número: 8


Se esperaba "double", venía "*" en las columnas 14 hasta 14 en la línea número: 8


Se esperaba "identificador", venía "*" en las columnas 14 hasta 14 en la línea número: 8


Se esperaba "int", venía "*" en las columnas 14 hasta 14 en la línea número: 8


Se esperaba "interface", venía "*" en las columnas 14 hasta 14 en la línea número: 8


Se esperaba "string", venía "*" en las columnas 14 hasta 14 en la línea número: 8


Se esperaba "void", venía "*" en las columnas 14 hasta 14 en la línea número: 8


Se esperaba "bool", venía "4" en las columnas 16 hasta 16 en la línea número: 8


Se esperaba "class", venía "4" en las columnas 16 hasta 16 en la línea número: 8


Se esperaba "const", venía "4" en las columnas 16 hasta 16 en la línea número: 8


Se esperaba "double", venía "4" en las columnas 16 hasta 16 en la línea número: 8


Se esperaba "identificador", venía "4" en las columnas 16 hasta 16 en la línea número: 8


Se esperaba "int", venía "4" en las columnas 16 hasta 16 en la línea número: 8


Se esperaba "interface", venía "4" en las columnas 16 hasta 16 en la línea número: 8


Se esperaba "string", venía "4" en las columnas 16 hasta 16 en la línea número: 8


Se esperaba "void", venía "4" en las columnas 16 hasta 16 en la línea número: 8


Se esperaba "bool", venía "-" en las columnas 18 hasta 18 en la línea número: 8


Se esperaba "class", venía "-" en las columnas 18 hasta 18 en la línea número: 8


Se esperaba "const", venía "-" en las columnas 18 hasta 18 en la línea número: 8


Se esperaba "double", venía "-" en las columnas 18 hasta 18 en la línea número: 8


Se esperaba "identificador", venía "-" en las columnas 18 hasta 18 en la línea número: 8


Se esperaba "int", venía "-" en las columnas 18 hasta 18 en la línea número: 8


Se esperaba "interface", venía "-" en las columnas 18 hasta 18 en la línea número: 8


Se esperaba "string", venía "-" en las columnas 18 hasta 18 en la línea número: 8


Se esperaba "void", venía "-" en las columnas 18 hasta 18 en la línea número: 8


Se esperaba "bool", venía "6" en las columnas 20 hasta 20 en la línea número: 8


Se esperaba "class", venía "6" en las columnas 20 hasta 20 en la línea número: 8


Se esperaba "const", venía "6" en las columnas 20 hasta 20 en la línea número: 8


Se esperaba "double", venía "6" en las columnas 20 hasta 20 en la línea número: 8


Se esperaba "identificador", venía "6" en las columnas 20 hasta 20 en la línea número: 8


Se esperaba "int", venía "6" en las columnas 20 hasta 20 en la línea número: 8


Se esperaba "interface", venía "6" en las columnas 20 hasta 20 en la línea número: 8


Se esperaba "string", venía "6" en las columnas 20 hasta 20 en la línea número: 8


Se esperaba "void", venía "6" en las columnas 20 hasta 20 en la línea número: 8


Se esperaba "bool", venía ";" en las columnas 21 hasta 21 en la línea número: 8


Se esperaba "class", venía ";" en las columnas 21 hasta 21 en la línea número: 8


Se esperaba "const", venía ";" en las columnas 21 hasta 21 en la línea número: 8


Se esperaba "double", venía ";" en las columnas 21 hasta 21 en la línea número: 8


Se esperaba "identificador", venía ";" en las columnas 21 hasta 21 en la línea número: 8


Se esperaba "int", venía ";" en las columnas 21 hasta 21 en la línea número: 8


Se esperaba "interface", venía ";" en las columnas 21 hasta 21 en la línea número: 8


Se esperaba "string", venía ";" en las columnas 21 hasta 21 en la línea número: 8


Se esperaba "void", venía ";" en las columnas 21 hasta 21 en la línea número: 8


Se esperaba "[]", venía "=" en las columnas 6 hasta 6 en la línea número: 9


Se esperaba "identificador", venía "=" en las columnas 6 hasta 6 en la línea número: 9


Se esperaba "bool", venía "3" en las columnas 8 hasta 8 en la línea número: 9


Se esperaba "class", venía "3" en las columnas 8 hasta 8 en la línea número: 9


Se esperaba "const", venía "3" en las columnas 8 hasta 8 en la línea número: 9


Se esperaba "double", venía "3" en las columnas 8 hasta 8 en la línea número: 9


Se esperaba "identificador", venía "3" en las columnas 8 hasta 8 en la línea número: 9


Se esperaba "int", venía "3" en las columnas 8 hasta 8 en la línea número: 9


Se esperaba "interface", venía "3" en las columnas 8 hasta 8 en la línea número: 9


Se esperaba "string", venía "3" en las columnas 8 hasta 8 en la línea número: 9


Se esperaba "void", venía "3" en las columnas 8 hasta 8 en la línea número: 9


Se esperaba "bool", venía ";" en las columnas 9 hasta 9 en la línea número: 9


Se esperaba "class", venía ";" en las columnas 9 hasta 9 en la línea número: 9


Se esperaba "const", venía ";" en las columnas 9 hasta 9 en la línea número: 9


Se esperaba "double", venía ";" en las columnas 9 hasta 9 en la línea número: 9


Se esperaba "identificador", venía ";" en las columnas 9 hasta 9 en la línea número: 9


Se esperaba "int", venía ";" en las columnas 9 hasta 9 en la línea número: 9


Se esperaba "interface", venía ";" en las columnas 9 hasta 9 en la línea número: 9


Se esperaba "string", venía ";" en las columnas 9 hasta 9 en la línea número: 9


Se esperaba "void", venía ";" en las columnas 9 hasta 9 en la línea número: 9


Se esperaba "[]", venía "=" en las columnas 6 hasta 6 en la línea número: 10


Se esperaba "identificador", venía "=" en las columnas 6 hasta 6 en la línea número: 10


Se esperaba "[]", venía "+" en las columnas 10 hasta 10 en la línea número: 10


Se esperaba "identificador", venía "+" en las columnas 10 hasta 10 en la línea número: 10


Se esperaba "bool", venía "2" en las columnas 12 hasta 12 en la línea número: 10


Se esperaba "class", venía "2" en las columnas 12 hasta 12 en la línea número: 10


Se esperaba "const", venía "2" en las columnas 12 hasta 12 en la línea número: 10


Se esperaba "double", venía "2" en las columnas 12 hasta 12 en la línea número: 10


Se esperaba "identificador", venía "2" en las columnas 12 hasta 12 en la línea número: 10


Se esperaba "int", venía "2" en las columnas 12 hasta 12 en la línea número: 10


Se esperaba "interface", venía "2" en las columnas 12 hasta 12 en la línea número: 10


Se esperaba "string", venía "2" en las columnas 12 hasta 12 en la línea número: 10


Se esperaba "void", venía "2" en las columnas 12 hasta 12 en la línea número: 10


Se esperaba "bool", venía ";" en las columnas 13 hasta 13 en la línea número: 10


Se esperaba "class", venía ";" en las columnas 13 hasta 13 en la línea número: 10


Se esperaba "const", venía ";" en las columnas 13 hasta 13 en la línea número: 10


Se esperaba "double", venía ";" en las columnas 13 hasta 13 en la línea número: 10


Se esperaba "identificador", venía ";" en las columnas 13 hasta 13 en la línea número: 10


Se esperaba "int", venía ";" en las columnas 13 hasta 13 en la línea número: 10


Se esperaba "interface", venía ";" en las columnas 13 hasta 13 en la línea número: 10


Se esperaba "string", venía ";" en las columnas 13 hasta 13 en la línea número: 10


Se esperaba "void", venía ";" en las columnas 13 hasta 13 en la línea número: 10


Se esperaba "bool", venía "}" en las columnas 1 hasta 1 en la línea número: 11


Se esperaba "class", venía "}" en las columnas 1 hasta 1 en la línea número: 11


Se esperaba "const", venía "}" en las columnas 1 hasta 1 en la línea número: 11


Se esperaba "double", venía "}" en las columnas 1 hasta 1 en la línea número: 11


Se esperaba "identificador", venía "}" en las columnas 1 hasta 1 en la línea número: 11


Se esperaba "int", venía "}" en las columnas 1 hasta 1 en la línea número: 11


Se esperaba "interface", venía "}" en las columnas 1 hasta 1 en la línea número: 11


Se esperaba "string", venía "}" en las columnas 1 hasta 1 en la línea número: 11


Se esperaba "void", venía "}" en las columnas 1 hasta 1 en la línea número: 11


Se esperaba "bool", venía "Fin de archivo" en las columnas 1 hasta 1 en la línea número: 11


Se esperaba "class", venía "Fin de archivo" en las columnas 1 hasta 1 en la línea número: 11


Se esperaba "const", venía "Fin de archivo" en las columnas 1 hasta 1 en la línea número: 11


Se esperaba "double", venía "Fin de archivo" en las columnas 1 hasta 1 en la línea número: 11


Se esperaba "identificador", venía "Fin de archivo" en las columnas 1 hasta 1 en la línea número: 11


Se esperaba "int", venía "Fin de archivo" en las columnas 1 hasta 1 en la línea número: 11


Se esperaba "interface", venía "Fin de archivo" en las columnas 1 hasta 1 en la línea número: 11


Se esperaba "string", venía "Fin de archivo" en las columnas 1 hasta 1 en la línea número: 11


Se esperaba "void", venía "Fin de archivo" en las columnas 1 hasta 1 en la línea número: 11
```
## Sintaxis y manejo de errores
El análisis de sintaxis comienza en la clase "SyntaxAnalyzer" después de que se realiza el análisis léxico sin obtener ningún error en los lexemas analizados. El primer paso es agregar el lexema “$end” a la lista de lexemas, lo que significa el estado de final de archivo, después de esto, el recorrido de los lexemas comienza a ignorar los lexemas de los comentarios. Todo el proceso de análisis se basa en la gramática definida en los archivos “Grammar.txt” y “LR (0) parsing table.xlsx” dentro de la carpeta “Grammar and LR (0) parsing table”. El método de análisis de sintaxis implementado fue el analizador sintáctico LR(0).

El analizador (función ParseLexemes) comienza a buscar el encabezado especificado por el valor del lexema leído o el token en el caso de que el valor del lexema sea una constante.
 
Si el programa encuentra el encabezado, toma la cima de la pila StatusStack y la columna del encabezado para verificar si hay una instrucción en la posición encontrada.

Si hay una instrucción allí, entonces la opera, si esta instrucción es un conflicto, entonces guarda el estado actual de todo (variables globales y datos usados por el analizador) para restaurar este estado y avanzar en el conflicto en caso de que haya un error dentro de la ruta tomada.

Si no se encuentran instrucciones dentro de la posición de la tabla, el analizador verifica si hay un estado para restaurar, si hay al menos uno, lo restaura y el proceso de análisis continúa; si no las hay, este es el final del proceso de análisis de la entrada recibida, la lista de esperados se recrea si el índice máximo analizado es más pequeño que el índice del lexema actual, los esperados se detectan recorriendo la fila actual de la tabla de análisis en donde sí haya operaciones a realizar. Se imprimen los mensajes, todas las variables se vacían para comenzar nuevamente con el siguiente lexema dentro de la lista de lexemas.

Si el programa no encuentra el encabezado entonces este es el final del proceso de análisis de la entrada recibida, se recrea la lista esperada y se imprimen los mensajes, se vacían todas las variables para comenzar de nuevo con el siguiente lexema dentro de la lista de lexemas.

Para mantener la eficiencia alta cuando la producción se reduce por Decl o Program5, entonces las estructuras de conflictos y errores se configuran de nuevo por defecto, esto se hizo para eliminar antiguas rutas posibles que ya no son válidas para el contexto actual. Se seleccionaron estas producciones dado que las reducciones que se realicen aquí significan que todo lo anterior hasta ese punto es correcto.

##Tabla de Símbolos:
Estructura:
La tabla de símbolos (nombre de variable SymbolsTable) está definida como un diccionario cuya clave es un número (int) y su valor es un listado de símbolos (SymbolNode). La llave del diccionario representa el ámbito de todos los símbolos contenidos en ese valor del diccionario. El valor representa toda definición dentro del ámbito en cuestión.
Estructura de SymbolNode:
Esta estructura de datos es utilizada para almacenar información relevante del identificador que se está reconociendo. Se almacena el nombre del identificador, el valor de este, el ámbito donde se encuentra, su columna de inicio y fin, la línea donde se encuentra este identificador, una bandera que indica si esta activo o no, está se utiliza en la comprobación de tipos en la producción de CallStmt (ver sección de semántica para tener más detalles del uso de esta bandera). Por último, se almacena el tipo del símbolo, este puede ser de tipo “class”, “interface”, “void” o cualquiera de los tipos de dato que puedan darse por la producción “Type”, validando si se da la producción que se encuentra en la línea No.22 del archivo “Grammar.txt” que el identificador por el que se está dando la sustitución este definido en el listado de tipos de dato válidos para el archivo actual (estos tipos de dato se almacenan en un diccionario para facilitar la búsqueda de tipos de datos válidos (DataTypesFound).
Sí el valor del tipo del SymbolNode es “void” o bien se encuentra entre las producciones de declaración de funciones o prototipos, se inicializa un diccionario en SymbolNode, que almacenará todos los parámetros que tenga la definición de la función actual. Por cada parámetro de la función se almacenará el nombre de este y su tipo de dato, esto se almacenará en un diccionario para validar que no existan variables repetidas dentro de los parámetros de cada función. 


Análisis semántico:
Para el manejo de errores de esta fase, todos los errores detectados son los errores que encontró el analizador semántico tomando el camino correcto del análisis sintáctico. Debido a que solo por las producciones que sean sintácticamente correctas debe realizarse un análisis semántico. 
El análisis sintáctico y semántico se van dando en paralelo, pero los errores semánticos del archivo únicamente se mostrarán si el archivo ingresado es sintácticamente correcto, pero semánticamente incorrecto, si todo está en orden se le indicará al usuario que el archivo ingresado es léxica, sintáctica y semánticamente correcto.
Se imprimirá la tabla de símbolos independientemente del resultado de análisis semántico, es decir, aunque el archivo ingresado posea errores semánticos, siempre se mostrará la tabla de símbolos completa.


Clases:
Para la declaración de clases, se valida que no exista ninguna clase declarada con ese nombre antes de agregarla a la tabla de símbolos. Está validación se da en el ámbito donde se encuentra la clase y también en el diccionario de tipos de dato que ya han sido reconocidos por el compilador. Sí no se encuentra ninguna definición previa, esta es agregada como un símbolo válido a la tabla de símbolos, de lo contrario se agrega una descripción para el error en cuestión y este error es agregado a la lista de errores semánticos.
Se valida que, para la herencia de otras clases, la clase actual solo herede una única vez por cada clase reconocida hasta el momento. En cada clase que se está enviando para herencia, se valida que esa clase ya se encuentre entre los tipos de dato reconocidos por el compilador, caso contrario se indica el error.
Por cada clase se almacena como parámetros, todas las clases que reciba como herencia.
Procedimientos, prototipos y funciones: 
Se valida que no exista ningún otro método con el nombre del actual en el ámbito donde se encuentra el actual, si existe un método con el mismo nombre, se indicará el error. Se validará que cada parámetro tenga un tipo de dato que ya se haya reconocido como válido por el compilador. El nombre de cada variable dentro de los parámetros debe ser distinto, por lo que, en caso de encontrar alguna variable repetida en los parámetros, se indicará el error y se cambiará a falso el valor de la bandera para el método en cuestión.
Definición de variables y constantes:
Se valida que no exista ninguna otra variable o constante con ese nombre para el ámbito donde se encuentra la declaración, si existe una variable con el mismo nombre y ámbito se almacena el error. Se valida que el tipo de dato con el que se quiere declarar la variable sea alguno que ya reconoció el compilador, en caso contrario se indicará el error.
Asignación a variables y constantes:
Se debe validar que el elemento al que se le va a hacer la asignación exista dentro de la tabla de símbolos y este en un ámbito abierto o accesible, para la asignación de variables el compilador puede visitar todos los ámbitos que son accesibles desde ese punto del archivo en busca de una variable para asignarle su valor. Debido a que pueden declararse dos variables con el mismo nombre, pero diferente ámbito, se asignará el valor de la asignación a la variable más interna que se encuentre dentro del análisis. 
Sí se sabe que existe la variable, previo a la asignación debe validarse que el tipo de dato de la variable sea igual que el valor que va a recibir, en caso contrario indicar el error.

Operaciones:
Debe validarse que todos los operandos sean del mismo tipo o bien de tipos por los que se pueda manejar una coerción, caso contrario indicar el error. 
Si se utilizan variables para las operaciones, debe validarse que estas existan dentro del ámbito actual o bien que estas sean accesibles, caso contrario indicar el error.  Toda variable utilizada en una operación debe tener un valor definido, en caso contrario indicar el error.

