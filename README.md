# Compilador de mini C#
> Análisis léxico de un archivo escrito en C#
> José Eduardo Tejeda León             1097218
> Mario Estuardo Gómez Orellana     1020618




<a href="https://drive.google.com/file/d/1LuXkIy3qcK6N0ac09yaUVioggTmSz76k/view?usp=sharing" download="Descargar ejecutable"><img src="https://i.ibb.co/zSNLWLg/3.png" /></a>




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

Este analizador ha sido depurado con múltiple cantidad de archivos de prueba por lo tanto es un analizador robusto y 100% funcional para la gramática explicada en la sección de “Explicación detallada del flujo del código” que obedece a las expresiones regulares de abajo.

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
