using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace miniCSharp_Compiler
{
    public class LexicalAnalyzer
    {
        List<LexemeNode> Lexemes = new List<LexemeNode>();
        List<string> OperatorsAndPuncChars = new List<string>();
        List<string> ReservedWords = new List<string>();
        public LexicalAnalyzer()
        {
            OperatorsAndPuncChars.Add("+");
            OperatorsAndPuncChars.Add("-");
            OperatorsAndPuncChars.Add("*");
            OperatorsAndPuncChars.Add("/");
            OperatorsAndPuncChars.Add("%");
            OperatorsAndPuncChars.Add("(");
            OperatorsAndPuncChars.Add(")");
            OperatorsAndPuncChars.Add("[");
            OperatorsAndPuncChars.Add("]");
            OperatorsAndPuncChars.Add("{");
            OperatorsAndPuncChars.Add("}");
            OperatorsAndPuncChars.Add(",");
            OperatorsAndPuncChars.Add(".");
            OperatorsAndPuncChars.Add(";");
            OperatorsAndPuncChars.Add("!");
            OperatorsAndPuncChars.Add("<");
            OperatorsAndPuncChars.Add(">");
            OperatorsAndPuncChars.Add("=");
            OperatorsAndPuncChars.Add("<=");
            OperatorsAndPuncChars.Add(">=");
            OperatorsAndPuncChars.Add("==");
            OperatorsAndPuncChars.Add("!=");
            OperatorsAndPuncChars.Add("||");
            OperatorsAndPuncChars.Add("&&");
            OperatorsAndPuncChars.Add("{}");
            OperatorsAndPuncChars.Add("[]");
            OperatorsAndPuncChars.Add("()");

            ReservedWords.Add("void");
            ReservedWords.Add("int");
            ReservedWords.Add("double");
            ReservedWords.Add("bool");
            ReservedWords.Add("string");
            ReservedWords.Add("class");
            ReservedWords.Add("const");
            ReservedWords.Add("interface");
            ReservedWords.Add("null");
            ReservedWords.Add("this");
            ReservedWords.Add("for");
            ReservedWords.Add("while");
            ReservedWords.Add("foreach");
            ReservedWords.Add("if");
            ReservedWords.Add("else");
            ReservedWords.Add("return");
            ReservedWords.Add("break");
            ReservedWords.Add("New");
            ReservedWords.Add("NewArray");
            ReservedWords.Add("Console");
            ReservedWords.Add("WriteLine");
        }
        public void ReadFile(string path)
        {
            var row = 0;
            using (var sr = new StreamReader(path, Encoding.UTF8))
            {
                var fileLine = string.Empty;
                var tempNode = new LexemeNode();
                while ((fileLine = sr.ReadLine()) != null)
                {
                    row++;
                    analyzeLine(fileLine, row, ref tempNode);
                }
            }
        }

        void analyzeLine(string fileLine, int row, ref LexemeNode tempNode)
        {
            for (int column = 0; column < fileLine.Length; column++)
            {

                if ((fileLine[column] == ' ' || fileLine[column] == '\t' || fileLine[column] == '\n') && (tempNode.Token != 'M') && (tempNode.Token != 'S'))
                {
                    if (tempNode.Value != string.Empty)
                    {
                        finishLexemeNodeAndAddToLexemes(ref tempNode, column, string.Empty);
                    }
                }
                else if (!OperatorsAndPuncChars.Contains(fileLine[column].ToString()) &&
                        !Char.IsLetterOrDigit(fileLine[column]) &&
                        fileLine[column] != '_' &&
                        fileLine[column] != '"' &&
                        tempNode.Token != 'M' &&
                        tempNode.Token != 'S')
                {
                    if (tempNode.Value != string.Empty)
                    {
                        finishLexemeNodeAndAddToLexemes(ref tempNode, column, string.Empty);
                    }

                    tempNode = new LexemeNode();
                    tempNode.Value = fileLine[column].ToString();
                    tempNode.StartColumn = column + 1;
                    tempNode.EndColumn = column + 1;
                    tempNode.Row = row;
                    tempNode.Token = 'E';
                    tempNode.Description = tempNode.Value + " en la línea " + tempNode.Row + " cols " + tempNode.StartColumn + "-" + tempNode.EndColumn + " es un caracter no reconocido ";
                    Lexemes.Add(tempNode);
                    tempNode = new LexemeNode();

                }
                else
                if (OperatorsAndPuncChars.Contains(fileLine[column].ToString()) &&
                        tempNode.Token != 'M' &&
                        tempNode.Token != 'C' &&
                        tempNode.Token != 'O' &&//this validation was made because of double operators                      
                        tempNode.Token != 'N' &&
                        tempNode.Token != 'X')
                {
                    if (tempNode.Value != string.Empty)
                    {
                        //if this is true then we already have a node to finish                        
                        finishLexemeNodeAndAddToLexemes(ref tempNode, column, string.Empty);
                        column--;//recoil
                    }
                    else
                    {
                        //if this is true then we are reading the first part of an operator or the start of a comment
                        tempNode.Value = fileLine[column].ToString();
                        tempNode.StartColumn = column + 1;
                        tempNode.Row = row;
                        if (fileLine[column] == '/')
                        {
                            if (column + 1 < fileLine.Length)
                            {
                                if (fileLine[column + 1] == '*')
                                {
                                    tempNode.Token = 'M';
                                    column++;
                                    tempNode.Value += fileLine[column].ToString();
                                }
                                else
                                {
                                    tempNode.Token = 'O';
                                }
                            }
                            else
                            {
                                tempNode.Token = 'O';
                            }
                        }
                        else if (fileLine[column] == '*')
                        {
                            if (column + 1 < fileLine.Length)
                            {
                                if (fileLine[column + 1] == '/')
                                {
                                    column++;
                                    tempNode.Token = 'E';
                                    tempNode.Value += fileLine[column].ToString();
                                    finishLexemeNodeAndAddToLexemes(ref tempNode, column, "fin de comentario sin emparejar hallado");
                                }
                                else
                                {
                                    tempNode.Token = 'O';
                                }
                            }
                        }
                        else
                        {
                            tempNode.Token = 'O';
                        }
                    }

                }
                else
                {
                    if (tempNode.Value == string.Empty)
                    {
                        if (Char.IsLetter(fileLine[column]))
                        {
                            //identifier
                            tempNode.Value += fileLine[column].ToString();
                            tempNode.StartColumn = column + 1;
                            tempNode.Row = row;
                            tempNode.Token = 'I';
                        }
                        else if (Char.IsDigit(fileLine[column]))
                        {
                            //int constant
                            tempNode.Value = fileLine[column].ToString();
                            tempNode.StartColumn = column + 1;
                            tempNode.Row = row;
                            tempNode.Token = 'N';
                        }
                        else if (fileLine[column] == '"')
                        {
                            //string
                            tempNode.Value = fileLine[column].ToString();
                            tempNode.StartColumn = column + 1;
                            tempNode.Row = row;
                            tempNode.Token = 'S';
                        }

                    }
                    else
                    {
                        switch (tempNode.Token)
                        {
                            case 'I':
                                //validar si es identificador, bool o palabra reservada
                                if (char.IsLetterOrDigit(fileLine[column]) || fileLine[column] == '_')
                                {
                                    if (tempNode.Value.Length < 31)
                                    {
                                        tempNode.Value += fileLine[column];
                                    }
                                    else
                                    {
                                        if (tempNode.Description == string.Empty)
                                        {
                                            tempNode.EndColumn = column;
                                            tempNode.Description = tempNode.Value + " en la línea " + tempNode.Row + " cols " + tempNode.StartColumn + "-" + column + " identificador de longitud no permitida, se tomaron nada más los primeros 31 caracteres";
                                        }
                                    }
                                }
                                else
                                {
                                    finishLexemeNodeAndAddToLexemes(ref tempNode, column, string.Empty);
                                    column--;//recoil
                                }
                                break;
                            case 'O':
                                if (OperatorsAndPuncChars.Contains(fileLine[column].ToString()))
                                {

                                    if (OperatorsAndPuncChars.Contains(tempNode.Value + fileLine[column].ToString()))
                                    {
                                        //if is a double operator then it concatenates its value to the node value
                                        tempNode.Value += fileLine[column].ToString();
                                    }
                                    else
                                    {
                                        //else the value stays as it was and a recoil is made
                                        column--;
                                    }
                                    finishLexemeNodeAndAddToLexemes(ref tempNode, column + 1, string.Empty);
                                }
                                else
                                {
                                    finishLexemeNodeAndAddToLexemes(ref tempNode, column, string.Empty);
                                    column--;
                                }
                                break;
                            case 'N':
                                if (char.IsDigit(fileLine[column]))
                                {
                                    tempNode.Value += fileLine[column];
                                }
                                else if ((fileLine[column] == 'x' || fileLine[column] == 'X') && tempNode.Value == "0")
                                {
                                    tempNode.Value += fileLine[column];
                                    tempNode.Token = 'H';
                                }
                                else if (fileLine[column] == '.')
                                {
                                    tempNode.Value += fileLine[column];
                                    tempNode.Token = 'D';
                                }
                                else
                                {
                                    //if this is true then we already have a node to finish
                                    finishLexemeNodeAndAddToLexemes(ref tempNode, column, string.Empty);
                                    column--;//recoil
                                }
                                break;
                            case 'H':
                                if (char.IsLetter(fileLine[column]))
                                {
                                    var asciiCode = (int)(char)fileLine[column];
                                    if ((asciiCode >= 65 && asciiCode <= 70) || ((asciiCode >= 97 && asciiCode <= 102)))
                                    {
                                        tempNode.Value += fileLine[column];
                                    }
                                    else
                                    {
                                        finishLexemeNodeAndAddToLexemes(ref tempNode, column, string.Empty);
                                        column--;
                                    }
                                }
                                else if (char.IsDigit(fileLine[column]))
                                {
                                    tempNode.Value += fileLine[column];
                                }
                                break;
                            case 'D':
                                if (char.IsDigit(fileLine[column]))
                                {
                                    tempNode.Value += fileLine[column];
                                }
                                else if (fileLine[column] == 'e' || fileLine[column] == 'E')
                                {
                                    tempNode.Value += fileLine[column];
                                    tempNode.Token = 'X';
                                }
                                else
                                {
                                    //if this is true then we already have a node to finish
                                    finishLexemeNodeAndAddToLexemes(ref tempNode, column, string.Empty);
                                    column--;//recoil
                                }
                                break;
                            case 'X':
                                if (char.IsDigit(fileLine[column]) ||
                                    ((fileLine[column] == '+' || fileLine[column] == '-') &&
                                    (tempNode.Value[tempNode.Value.Length - 1] == 'e' || tempNode.Value[tempNode.Value.Length - 1] == 'E')))
                                {
                                    if ((tempNode.Value[tempNode.Value.Length - 1] == 'e' || tempNode.Value[tempNode.Value.Length - 1] == 'E') && char.IsDigit(fileLine[column]))
                                    {
                                        tempNode.Value += '+';
                                    }
                                    tempNode.Value += fileLine[column];
                                }
                                else
                                {
                                    //The character readed is a letter
                                    finishLexemeNodeAndAddToLexemes(ref tempNode, column, string.Empty);
                                    column--;//recoil
                                }
                                break;
                            case 'M':
                                if (fileLine[column] == '*')
                                {
                                    if (column + 1 < fileLine.Length)
                                    {
                                        if (fileLine[column + 1] == '/')
                                        {
                                            tempNode.Value += fileLine[column];
                                            column++;
                                            tempNode.Value += fileLine[column];
                                            finishLexemeNodeAndAddToLexemes(ref tempNode, column + 1, string.Empty);
                                        }
                                        else
                                        {
                                            validateAsciiInterval(ref tempNode, fileLine[column], column + 1);
                                        }
                                    }
                                    else
                                    {
                                        validateAsciiInterval(ref tempNode, fileLine[column], column + 1);
                                    }
                                }
                                else
                                {
                                    validateAsciiInterval(ref tempNode, fileLine[column], column + 1);
                                }
                                break;
                            case 'S':
                                if (fileLine[column] == '"')
                                {
                                    tempNode.Value += fileLine[column];
                                    finishLexemeNodeAndAddToLexemes(ref tempNode, column, string.Empty);
                                }
                                else
                                {
                                    validateAsciiInterval(ref tempNode, fileLine[column], column + 1);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            if (tempNode.Value != string.Empty && tempNode.Token != 'M')
            {
                if (tempNode.Token == 'S')
                {
                    tempNode.Token = 'E';
                    finishLexemeNodeAndAddToLexemes(ref tempNode, fileLine.Length, "cadena sin finalizar hallada");
                }
                else
                {
                    finishLexemeNodeAndAddToLexemes(ref tempNode, fileLine.Length, string.Empty);
                }

            }
            else if (tempNode.Token == 'M')
            {
                tempNode.Value += '\n';
            }


        }

        void finishLexemeNodeAndAddToLexemes(ref LexemeNode tempNode, int column, string error)
        {
            switch (tempNode.Token)
            {
                case 'E':
                    tempNode.Description = tempNode.Value + " en la línea " + tempNode.Row + " cols " + tempNode.StartColumn + "-" + column + " es un lexema no reconozido el error es: " + error;
                    break;
                case 'I':
                    if (ReservedWords.Contains(tempNode.Value))
                    {
                        tempNode.Token = 'R';
                    }
                    else if (tempNode.Value == "true" || tempNode.Value == "false")
                    {//is a bool variable
                        tempNode.Token = 'B';
                    }
                    break;
                case 'R':
                    break;
                case 'D':
                    break;
                case 'B':
                    break;
                case 'N':
                    break;
                case 'H':
                    if (tempNode.Value[tempNode.Value.Length - 1] == 'x' || tempNode.Value[tempNode.Value.Length - 1] == 'X')
                    {
                        tempNode.Token = 'E';
                        finishLexemeNodeAndAddToLexemes(ref tempNode, column, "En constantes hexadecimales, luego de la 'X' o 'x' debe escribir al menos un número o una letra de la a-f o A-F");
                    }
                    break;
                case 'X':
                    if (!char.IsDigit(tempNode.Value[tempNode.Value.Length - 1]))
                    {
                        tempNode.Token = 'E';
                        finishLexemeNodeAndAddToLexemes(ref tempNode, column, "En constantes exponenciales, luego de la E, e o del signo del exponencial debe escribir al menos un número.");
                    }
                    //if this is true then we already have a node to finish
                    break;
                case 'S':
                    break;
                case 'C':
                    break;
                default:
                    break;
            }
            tempNode.EndColumn = tempNode.EndColumn == 0 ? column : tempNode.EndColumn;
            if (tempNode.Description == string.Empty && error == string.Empty && tempNode.Token != 'E')
            {
                tempNode.Description = tempNode.Value + " en la línea " + tempNode.Row + " cols " + tempNode.StartColumn + "-" + column + " es un(a) " + getTokenDescription(tempNode.Token);
            }
            if (tempNode.Token != '\0')
            {
                Lexemes.Add(tempNode);
            }
            tempNode = new LexemeNode();
        }

        string getTokenDescription(char tokenID)
        {
            switch (tokenID)
            {
                case 'E':
                    return "error";
                case 'I':
                    return "identificador";
                case 'R':
                    return "palabra reservada";
                case 'D':
                    return "constante tipo double";
                case 'B':
                    return "constante tipo bool";
                case 'N':
                    return "constante tipo int";
                case 'H':
                    return "constante hexadecimal";
                case 'X':
                    return "constante exponencial";
                case 'S':
                    return "cadena de caracteres (string)";
                case 'C':
                    return "comentario de una línea";
                case 'M':
                    return "comentario multilínea";
                case 'O':
                    return "operador o signo de puntuación";
                default:
                    return string.Empty;
            }
        }

        void validateAsciiInterval(ref LexemeNode tempNode, char character, int column)
        {
            if (character >= 1 && character <= 255)
            {
                tempNode.Value += character;
            }
            else
            {
                var errorDescription = tempNode.Token == 'M' ? "caracter no reconocido en comentario multilínea" : "caracter no reconocido en cadena";
                tempNode.Token = 'E';
                finishLexemeNodeAndAddToLexemes(ref tempNode, column, errorDescription);
            }
        }
    }
}