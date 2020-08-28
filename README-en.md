# Mini C# compiler
> C# written file lexical analysis
> José Eduardo Tejeda León             1097218
> Mario Estuardo Gómez Orellana     1020618


[Regresar versión en español](https://github.com/MarioOrellana58/MiniC--Compiler)

<a href="https://drive.google.com/file/d/1_QoMg-7Bq7fSuKf3kKp38MXlGy0VWKzO/view?usp=sharing" download="Descargar ejecutable"><img src="https://i.ibb.co/LhqG3hP/4.jpg" /></a>




![](https://media1.giphy.com/media/W5TVax7yZ79xhfpwei/giphy.gif)



## Requirements

Framework: .NET Framework 4.7.2

Additional libraries if you want to print the output file: iTextSharp

Additional programs if you want to print the output file: Adobe Acrobat Reader DC https://get.adobe.com/es/reader/


## Dependencies installation

To install the iTextSharp library follow these steps.



#Go to NuGet Package Manager


![image](https://drive.google.com/uc?export=view&id=1SxLEJA44EppRLstOHSCy3FXSBLyK1KNX)



#Install the library


![image](https://drive.google.com/uc?export=view&id=1phPiR__vSdldWj1qiPsdT66oi6gSjE0I)




## Usage example

Upload a file that fits mini c# grammar to be analyzed, down here you can see an example.

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
The analyzer will scan this file as described in Deep code explanation and error handling
section, as an output you will have something like this
```sh
int        ---          in the line 1 col 1-3 is a(n) reserved word


a        ---          in the line 1 col 5-5 is a(n) identifier


;        ---          in the line 1 col 6-6 is a(n) operator or punctuation symbol


void        ---          in the line 3 col 1-4 is a(n) reserved word


main        ---          in the line 3 col 6-9 is a(n) identifier


()        ---          in the line 3 col 10-11 is a(n) operator or punctuation symbol


{        ---          in the line 3 col 13-13 is a(n) operator or punctuation symbol


int        ---          in the line 4 col 4-6 is a(n) reserved word


b        ---          in the line 4 col 8-8 is a(n) identifier


;        ---          in the line 4 col 9-9 is a(n) operator or punctuation symbol


int        ---          in the line 5 col 4-6 is a(n) reserved word


a        ---          in the line 5 col 8-8 is a(n) identifier


;        ---          in the line 5 col 9-9 is a(n) operator or punctuation symbol


int        ---          in the line 6 col 4-6 is a(n) reserved word


d        ---          in the line 6 col 8-8 is a(n) identifier


;        ---          in the line 6 col 9-9 is a(n) operator or punctuation symbol


d        ---          in the line 8 col 4-4 is a(n) identifier


=        ---          in the line 8 col 6-6 is a(n) operator or punctuation symbol


2        ---          in the line 8 col 8-8 is a(n) int constant


+        ---          in the line 8 col 10-10 is a(n) operator or punctuation symbol


3        ---          in the line 8 col 12-12 is a(n) int constant


*        ---          in the line 8 col 14-14 is a(n) operator or punctuation symbol


4        ---          in the line 8 col 16-16 is a(n) int constant


-        ---          in the line 8 col 18-18 is a(n) operator or punctuation symbol


6        ---          in the line 8 col 20-20 is a(n) int constant


;        ---          in the line 8 col 21-21 is a(n) operator or punctuation symbol


b        ---          in the line 9 col 4-4 is a(n) identifier


=        ---          in the line 9 col 6-6 is a(n) operator or punctuation symbol


3        ---          in the line 9 col 8-8 is a(n) int constant


;        ---          in the line 9 col 9-9 is a(n) operator or punctuation symbol


a        ---          in the line 10 col 4-4 is a(n) identifier


=        ---          in the line 10 col 6-6 is a(n) operator or punctuation symbol


b        ---          in the line 10 col 8-8 is a(n) identifier


+        ---          in the line 10 col 10-10 is a(n) operator or punctuation symbol


2        ---          in the line 10 col 12-12 is a(n) int constant


;        ---          in the line 10 col 13-13 is a(n) operator or punctuation symbol


}        ---          in the line 11 col 1-1 is a(n) operator or punctuation symbol


#        ---          in the line 12 col 1-1 there's an error which is: unrecognized character
```



## Deep code explanation and error handling

Inside the class “LexicalAnalyzer” in the procedure “ReadFileAndAnalyzeDocument”, a node is created, this node will contain all the necessary information to show the lexeme after the analysis (token value, description, start and end column, start and end line). After creating the node, the reading of the input file begins line by line. Every line is sent to the “analyzeLine” procedure.

Inside this procedure, each character in the line is scanned. In each character, it’s asked if it’s a lexeme separator (space, horizontal tab, or newline). If it is, the procedure “finishLexemeNodeAndAddToLexemes” is called. This procedure adds what we have up to that moment in a list of lexemes (“Lexemes”) and a new node is created to continue with the next character.

If it’s not a separator, it asks if is not contained in a defined list of operators and punctuation marks (“OperatorsAndPuncChars”), if it’s not a letter or a digit, if it’s not an underscore (“_”) or a quotation mark (“ “ ”). If it complies with this, it proceeds to ask if the actual node already contains a lexeme or if it’s empty. If it is not empty the procedure “finishLexemeNodeAndAddToLexemes” is called and then an “Unrecognized character”  error is written since it is not valid for the grammar.

The third validation is for characters that are recognized as grammar operators and are not within a string or comment. For the same reasons of the second validation, it’s validated if the node is empty if it isn’t, it’s stored and backtrack is made so it can re-analyze this character since when it enters here again and the node is empty, the character will be registered with an operator token. This method validates if is the beginning of commentary with a character look ahead or if it’s the end of an unpaired comment.

If it doesn't accomplish any of the previous conditions, it asks if the current lexeme is empty, if it is, it proceeds to evaluate inside of a selective structure which represents the beginning of a regular expression. When it finds out which regular expression begins with the current character, it proposes an initial token for the lexeme, and the current character is added to the lexeme.

In case all the above is not accomplished, it means that there's already a lexeme being analyzed. This is done by a selective structure that represents the behavior of deterministic finite automata (DFA). This was structured by analyzing the composition of every lexeme, structuring a regular expression for it, and taking this expression to the selective structure. In this section, most of the errors are handled using the DFA to determine which is going to be the definitive token for the lexeme. With this, it can analyze if the current lexeme meets with the conditions of the regular expression defined for it.
## Justification
This analyzer has been tested with multiple test files, therefore is a robust and 100% functional parser for the grammar explained in the section “Deep code explanation and error handling”  which follows the regular expressions below.

## Regular expressions for the grammar

Identifiers:
```sh
((a-z)| (A-Z) )((A-Z)|(a-z)|(0-9)|(_))*
```

Types of constants:
```sh
Hexadecimal integer:   0 (x|X)(0-9|a-f|A-F)+
 
Integer: Dec (0-9)+
 
Bool: true|false

Exponential:  (0-9)+ (.)(0-9)*(e|E)(+|-)?(0-9)+
 
Double (0-9)+ (.)(0-9)*

string type: (“)( ASCII(1-255)(”)
```



Grammar Operators
```sh
+ - * / % < <= > >= = == != && || ! ; , . [ ] ( ) { } [] () {}
```



Commentaries :
```sh
(//)(ASCII(1-255)*)
(/*)(ASCII(1-255)|(\n))*(*/)
```



Reserved words :
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
