using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;


namespace miniCSharp_Compiler
{
    public class LexicalAnalyzer
    {
        public List<LexemeNode> Lexemes = new List<LexemeNode>();
        List<string> OperatorsAndPuncChars = new List<string>();
        List<string> ReservedWords = new List<string>();
        bool EnglishVersion = false;
        public LexicalAnalyzer(bool englishVersion)
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
            OperatorsAndPuncChars.Add(":");
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
            ReservedWords.Add("Writeline");

            this.EnglishVersion = englishVersion;
        }
        int CountFileLines(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string[] lines;
                var WholeFile = sr.ReadToEnd();
                lines = WholeFile.Split('\n');
                return lines.Count(); 
            }
        }
        public void ReadFileAndAnalyzeDocument(string path)
        {
            var row = 0;
            var tempNode = new LexemeNode();
            var totalRows = CountFileLines(path);           

            using (var sr = new StreamReader(path, Encoding.UTF8))
            {
                var fileLine = string.Empty;

                while ((fileLine = sr.ReadLine()) != null)
                {
                    row++;
                    AnalyzeLine(fileLine, row, totalRows, ref tempNode);
                }
                if (tempNode.Token == 'M')
                {
                    tempNode.Token = 'E';
                    tempNode.EndRow = totalRows;
                    FinishLexemeNodeAndAddToLexemes(ref tempNode, totalRows, !EnglishVersion ? "EOF en un comentario" : "EOF found in a comment");
                }
            }

        }

        void AnalyzeLine(string fileLine, int row, int totalRows, ref LexemeNode tempNode)
        {
            for (int column = 0; column < fileLine.Length; column++)
            {
                var isNotACommentOrStr = tempNode.Token != 'M' && tempNode.Token != 'C' && tempNode.Token != 'S' ?
                                         true :
                                         false;

                if ((fileLine[column] == ' ' ||
                    fileLine[column] == '\t' ||
                    fileLine[column] == '\n') &&
                    isNotACommentOrStr)
                {
                    if (tempNode.Value != string.Empty)
                    {
                        FinishLexemeNodeAndAddToLexemes(ref tempNode, column, string.Empty);
                    }
                }
                else
                if (!OperatorsAndPuncChars.Contains(fileLine[column].ToString()) &&
                        !Char.IsLetterOrDigit(fileLine[column]) &&
                        fileLine[column] != '_' &&
                        fileLine[column] != '"' &&
                        isNotACommentOrStr)
                {
                    if (tempNode.Value != string.Empty)
                    {
                        FinishLexemeNodeAndAddToLexemes(ref tempNode, column, string.Empty);
                    }
                    else
                    {
                        tempNode = new LexemeNode();
                    }


                    tempNode.Value += fileLine[column];
                    tempNode.StartColumn = column + 1;
                    tempNode.StartRow = row;

                    if (column + 1 < fileLine.Length)
                    {
                        //look ahead
                        if (fileLine[column + 1] == '&' && fileLine[column] == '&')
                        {
                            tempNode.Token = 'O';
                            column++;
                            tempNode.Value += fileLine[column];
                            FinishLexemeNodeAndAddToLexemes(ref tempNode, column + 1, string.Empty);
                        }
                        else if (fileLine[column + 1] == '|' && fileLine[column] == '|')
                        {
                            tempNode.Token = 'O';
                            column++;
                            tempNode.Value += fileLine[column];
                            FinishLexemeNodeAndAddToLexemes(ref tempNode, column + 1, string.Empty);
                        }
                        else
                        {
                            tempNode.Token = 'E';
                            FinishLexemeNodeAndAddToLexemes(ref tempNode, column + 1, !EnglishVersion ? "es un caracter no reconocido" : "unrecognized character");
                        }
                    }
                    else
                    {
                        tempNode.Token = 'E';
                        FinishLexemeNodeAndAddToLexemes(ref tempNode, column + 1, !EnglishVersion ? "es un caracter no reconocido" : "unrecognized character");
                    }
                }
                else
                if (OperatorsAndPuncChars.Contains(fileLine[column].ToString()) &&
                        isNotACommentOrStr &&
                        tempNode.Token != 'O' &&//this validation was made because of double operators                      
                        tempNode.Token != 'N' &&
                        tempNode.Token != 'X')
                {
                    if (tempNode.Value != string.Empty)
                    {
                        //if this is true then we already have a node to finish                        
                        FinishLexemeNodeAndAddToLexemes(ref tempNode, column, string.Empty);
                        column--;//backtrack
                    }
                    else
                    {
                        //if this is true then we are reading the first part of an operator or the start of a comment
                        tempNode.Value += fileLine[column];
                        tempNode.StartColumn = column + 1;
                        tempNode.StartRow = row;
                        if (fileLine[column] == '/')
                        {
                            //look ahead
                            if (column + 1 < fileLine.Length)
                            {
                                if (fileLine[column + 1] == '*')
                                {
                                    tempNode.Token = 'M';
                                    tempNode.StartColumn = -1;
                                    column++;
                                    tempNode.Value += fileLine[column];
                                }
                                else if (fileLine[column + 1] == '/')
                                {
                                    tempNode.Token = 'C';
                                    column++;
                                    tempNode.Value += fileLine[column];
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
                            //look ahead
                            if (column + 1 < fileLine.Length)
                            {
                                if (fileLine[column + 1] == '/')
                                {
                                    column++;
                                    tempNode.Token = 'E';
                                    tempNode.Value += fileLine[column];
                                    FinishLexemeNodeAndAddToLexemes(ref tempNode, column, !EnglishVersion ? "fin de comentario sin emparejar hallado" : "unmatched comment end found");
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
                            tempNode.Value += fileLine[column];
                            tempNode.StartColumn = column + 1;
                            tempNode.StartRow = row;
                            tempNode.Token = 'I';
                        }
                        else if (Char.IsDigit(fileLine[column]))
                        {
                            //int constant
                            tempNode.Value += fileLine[column];
                            tempNode.StartColumn = column + 1;
                            tempNode.StartRow = row;
                            tempNode.Token = 'N';
                        }
                        else if (fileLine[column] == '"')
                        {
                            //string
                            tempNode.Value += fileLine[column];
                            tempNode.StartColumn = column + 1;
                            tempNode.StartRow = row;
                            tempNode.Token = 'S';
                        }
                        else
                        {
                            tempNode.Value += fileLine[column];
                            tempNode.StartColumn = column + 1;
                            tempNode.StartRow = row;
                            tempNode.Token = 'E';
                            FinishLexemeNodeAndAddToLexemes(ref tempNode, column + 1, !EnglishVersion ? "es un caracter no reconocido" : "unrecognized character");
                        }

                    }
                    else
                    {
                        switch (tempNode.Token)
                        {
                            case 'I':
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
                                            tempNode.Description = tempNode.Value;
                                            var message = !EnglishVersion ? " en la línea " + tempNode.StartRow + " cols " + tempNode.StartColumn + "-" + column + " identificador de longitud no permitida, se tomaron nada más los primeros 31 caracteres"
                                                                          : " in the line " + tempNode.StartRow + " col " + tempNode.StartColumn + "-" + column + " not allowed length identifier, just the first 31st characters were taken";
                                            tempNode.Description += message;
                                        }
                                    }
                                }
                                else
                                {
                                    FinishLexemeNodeAndAddToLexemes(ref tempNode, column, string.Empty);
                                    column--;//backtrack
                                }
                                break;
                            case 'O':
                                if (OperatorsAndPuncChars.Contains(fileLine[column].ToString()))
                                {

                                    if (OperatorsAndPuncChars.Contains(tempNode.Value + fileLine[column]))
                                    {
                                        //if is a double operator then it concatenates its value to the node value
                                        tempNode.Value += fileLine[column];
                                    }
                                    else
                                    {
                                        //else the value stays as it was and a backtrack is made
                                        column--;
                                    }
                                    FinishLexemeNodeAndAddToLexemes(ref tempNode, column + 1, string.Empty);
                                }
                                else
                                {
                                    FinishLexemeNodeAndAddToLexemes(ref tempNode, column, string.Empty);
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
                                    if (column + 1 < fileLine.Length)
                                    {
                                        var asciiCode = (int)(char)fileLine[column + 1];                                        
                                        if (Char.IsDigit(fileLine[column + 1]) || 
                                            (asciiCode >= 65 && asciiCode <= 70) ||
                                            ((asciiCode >= 97 && asciiCode <= 102)))
                                        {
                                            tempNode.Value += fileLine[column];
                                            tempNode.Token = 'H';
                                        }
                                        else
                                        {
                                            FinishLexemeNodeAndAddToLexemes(ref tempNode, column, string.Empty);
                                            column--;//backtrack
                                        }
                                    }
                                    else
                                    {
                                        FinishLexemeNodeAndAddToLexemes(ref tempNode, column, string.Empty);
                                        column--;//backtrack
                                    }
                                    
                                }
                                else if (fileLine[column] == '.')
                                {
                                    tempNode.Value += fileLine[column];
                                    tempNode.Token = 'D';
                                }
                                else
                                {
                                    //if this is true then we already have a node to finish
                                    FinishLexemeNodeAndAddToLexemes(ref tempNode, column, string.Empty);
                                    column--;//backtrack
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
                                        FinishLexemeNodeAndAddToLexemes(ref tempNode, column, string.Empty);
                                        column--;
                                    }
                                }
                                else if (char.IsDigit(fileLine[column]))
                                {
                                    tempNode.Value += fileLine[column];
                                }
                                else
                                {
                                    FinishLexemeNodeAndAddToLexemes(ref tempNode, column, string.Empty);
                                    column--;
                                }
                                break;
                            case 'D':
                                if (char.IsDigit(fileLine[column]))
                                {
                                    tempNode.Value += fileLine[column];
                                }
                                else if (fileLine[column] == 'e' || fileLine[column] == 'E')
                                {
                                    if (column + 1 < fileLine.Length)
                                    {
                                        if (fileLine[column + 1] == '+' ||
                                            fileLine[column + 1] == '-')
                                        {
                                            if (column + 2 < fileLine.Length)
                                            {
                                                if (!char.IsDigit(fileLine[column + 2]))
                                                {
                                                    FinishLexemeNodeAndAddToLexemes(ref tempNode, column, string.Empty);
                                                    column--;//backtrack
                                                }
                                                else
                                                {
                                                    tempNode.Value += fileLine[column];
                                                    tempNode.Token = 'X';
                                                }
                                            }
                                            else
                                            {
                                                FinishLexemeNodeAndAddToLexemes(ref tempNode, column, string.Empty);
                                                column--;//backtrack
                                            }
                                        }
                                        else if (char.IsDigit(fileLine[column + 1]))
                                        {
                                            tempNode.Value += fileLine[column];
                                            tempNode.Token = 'X';
                                        }
                                        else
                                        {
                                            FinishLexemeNodeAndAddToLexemes(ref tempNode, column, string.Empty);
                                            column--;//backtrack
                                        }
                                    }
                                    else
                                    {
                                        FinishLexemeNodeAndAddToLexemes(ref tempNode, column, string.Empty);
                                        column--;//backtrack
                                    }
                                }
                                else
                                {
                                    //if this is true then we already have a node to finish
                                    column--;//backtrack
                                    FinishLexemeNodeAndAddToLexemes(ref tempNode, column, string.Empty);
                                }
                                break;
                            case 'X':
                                if (char.IsDigit(fileLine[column]) ||
                                    ((fileLine[column] == '+' || fileLine[column] == '-') &&
                                    (tempNode.Value[tempNode.Value.Length - 1] == 'e' ||
                                    tempNode.Value[tempNode.Value.Length - 1] == 'E')))
                                {
                                    if ((tempNode.Value[tempNode.Value.Length - 1] == 'e' ||
                                        tempNode.Value[tempNode.Value.Length - 1] == 'E') &&
                                        char.IsDigit(fileLine[column]))
                                    {
                                        tempNode.Value += '+';
                                    }
                                    tempNode.Value += fileLine[column];
                                }
                                else
                                {
                                    //The character readed is a letter
                                    FinishLexemeNodeAndAddToLexemes(ref tempNode, column, string.Empty);
                                    column--;//backtrack
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
                                            tempNode.EndRow = row;
                                            if (tempNode.Description != string.Empty)
                                            {
                                                tempNode.Token = 'E';
                                                FinishLexemeNodeAndAddToLexemes(ref tempNode, column + 1, tempNode.Description);
                                            }
                                            else
                                            {
                                                FinishLexemeNodeAndAddToLexemes(ref tempNode, column + 1, string.Empty);
                                            }
                                        }
                                        else
                                        {
                                            ValidateAsciiInterval(ref tempNode, fileLine[column], column + 1);
                                        }
                                    }
                                    else
                                    {
                                        ValidateAsciiInterval(ref tempNode, fileLine[column], column + 1);
                                    }
                                }
                                else
                                {
                                    ValidateAsciiInterval(ref tempNode, fileLine[column], column + 1);
                                }
                                break;
                            case 'S':
                                if (fileLine[column] == '"')
                                {
                                    tempNode.Value += fileLine[column];
                                    if (tempNode.Description != string.Empty)
                                    {
                                        tempNode.Token = 'E';
                                        FinishLexemeNodeAndAddToLexemes(ref tempNode, column + 1, tempNode.Description);
                                    }
                                    else
                                    {
                                        FinishLexemeNodeAndAddToLexemes(ref tempNode, column + 1, string.Empty);
                                    }
                                }
                                else
                                {
                                    ValidateAsciiInterval(ref tempNode, fileLine[column], column + 1);
                                }
                                break;
                            case 'C':
                                ValidateAsciiInterval(ref tempNode, fileLine[column], column + 1);
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
                    if (row != totalRows)
                    {
                        FinishLexemeNodeAndAddToLexemes(ref tempNode, fileLine.Length, !EnglishVersion ? "cadena sin finalizar hallada" : "unfinished string found");
                    }
                    else
                    {
                        FinishLexemeNodeAndAddToLexemes(ref tempNode, fileLine.Length, !EnglishVersion ? "EOF en una cadena" : "EOF found in a string");
                    }
                }
                if (tempNode.Token == 'C')
                {
                    if (tempNode.Description != string.Empty)
                    {
                        tempNode.Token = 'E';
                        FinishLexemeNodeAndAddToLexemes(ref tempNode, fileLine.Length, tempNode.Description);
                    }
                    else
                    {
                        FinishLexemeNodeAndAddToLexemes(ref tempNode, fileLine.Length, string.Empty);
                    }
                }
                else
                {
                    FinishLexemeNodeAndAddToLexemes(ref tempNode, fileLine.Length, string.Empty);
                }

            }
            else if (tempNode.Token == 'M')
            {
                if (row != totalRows)
                {
                    tempNode.Value += '\n';
                }
                else
                {                    
                    tempNode.EndRow = totalRows;
                    tempNode.Token = 'E';
                    FinishLexemeNodeAndAddToLexemes(ref tempNode, fileLine.Length, !EnglishVersion ? "EOF en un comentario" : "EOF found in a comment");
                }
            }


        }

        void FinishLexemeNodeAndAddToLexemes(ref LexemeNode tempNode, int column, string error)
        {
            if (tempNode.Token != '\0')
            {
                var message = string.Empty;
                switch (tempNode.Token)
                {
                    case 'E':
                        if (tempNode.StartColumn != -1)
                        {
                            tempNode.Description = tempNode.Value;
                            message = !EnglishVersion ? "        ---         " + " en la línea " + tempNode.StartRow + " cols " + tempNode.StartColumn + "-" + column + " posee error el cual es: " + error
                                                      : "        ---         " + " in the line " + tempNode.StartRow + " col " + tempNode.StartColumn + "-" + column + " there's an error which is: " + error;
                        }
                        else
                        {
                            tempNode.Description = tempNode.Value;
                            message = !EnglishVersion ? "        ---         " + " en las líneas " + tempNode.StartRow + "-" + tempNode.EndRow + " posee error el cual es: " + error
                                                      : "        ---          " + " in the lines " + tempNode.StartRow + "-" + tempNode.EndRow + " there's an error which is: " + error;
                        }
                        break;
                    case 'I':
                        if (ReservedWords.Contains(tempNode.Value))
                        {
                            tempNode.Token = 'R';
                        }
                        else if (tempNode.Value == "true" || tempNode.Value == "false")
                        {
                            tempNode.Token = 'B';
                        }
                        break;
                    case 'M':
                        tempNode.Description = tempNode.Value;
                        message = !EnglishVersion ? "        ---         " + " en las líneas " + tempNode.StartRow + "-" + tempNode.EndRow + " es un(a) " + GetTokenDescription(tempNode.Token)
                                                  : "        ---         " + " in the lines " + tempNode.StartRow + "-" + tempNode.EndRow + " is a " + GetTokenDescription(tempNode.Token);
                        break;
                    case 'H':
                        if (tempNode.Value[tempNode.Value.Length - 1] == 'x' || tempNode.Value[tempNode.Value.Length - 1] == 'X')
                        {
                            tempNode.Token = 'E';
                            var e = !EnglishVersion ? "En constantes hexadecimales, luego de la 'X' o 'x' debe escribir al menos un número o una letra de la a-f o A-F"
                                                    : "In hexadecimal constants, you should write at least one number or a letter from a to f, or A to F after 'x' or 'X'";
                            FinishLexemeNodeAndAddToLexemes(ref tempNode, column, e);
                        }
                        break;
                    case 'X':
                        if (!char.IsDigit(tempNode.Value[tempNode.Value.Length - 1]))
                        {
                            tempNode.Token = 'E';
                            var e = !EnglishVersion ? "En constantes exponenciales, luego de la E, e o del signo del exponencial debe escribir al menos un número."
                                                    : "In exponential constants, you should write at least one number after 'e', 'E' or exponential symbol";
                            FinishLexemeNodeAndAddToLexemes(ref tempNode, column, e);
                        }
                        break;
                    default:
                        break;
                }

                tempNode.EndColumn = tempNode.EndColumn == 0 ? column : tempNode.EndColumn;

                if (tempNode.Description == string.Empty && error == string.Empty && tempNode.Token != 'E')
                {
                    tempNode.Description = tempNode.Value;
                    message = !EnglishVersion ? "        ---         " + " en la línea " + tempNode.StartRow + " cols " + tempNode.StartColumn + "-" + column + " es un(a) " + GetTokenDescription(tempNode.Token)
                                              : "        ---         " + " in the line " + tempNode.StartRow + " col " + tempNode.StartColumn + "-" + column + " is a(n) " + GetTokenDescription(tempNode.Token);
                }
                tempNode.Description += message;
                if (tempNode.Token != '\0')
                {
                    Lexemes.Add(tempNode);
                }
                tempNode = new LexemeNode();
            }
        }

        public string GetTokenDescription(char tokenID)
        {
            switch (tokenID)
            {
                case 'E':
                    return "error";
                case 'I':
                    return !EnglishVersion ? "identificador" : "identifier";
                case 'R':
                    return !EnglishVersion ? "palabra reservada" : "reserved word";
                case 'D':
                    return !EnglishVersion ? "constante tipo double" : "double constant ";
                case 'B':
                    return !EnglishVersion ? "constante tipo bool" : "bool constant";
                case 'N':
                    return !EnglishVersion ? "constante tipo int" : "int constant";
                case 'H':
                    return !EnglishVersion ? "constante hexadecimal" : "hexadecimal constant";
                case 'X':
                    return !EnglishVersion ? "constante double exponencial" : "double exponential constant";
                case 'S':
                    return !EnglishVersion ? "cadena de caracteres (string)" : "string constant";
                case 'C':
                    return !EnglishVersion ? "comentario de una línea" : "one line comment";
                case 'M':
                    return !EnglishVersion ? "comentario multilínea" : "multiline comment";
                case 'O':
                    return !EnglishVersion ? "operador o signo de puntuación" : "operator or punctuation symbol";
                default:
                    return string.Empty;
            }
        }

        void ValidateAsciiInterval(ref LexemeNode tempNode, char character, int column)
        {
            if (character >= 1 && character <= 255)
            {
                tempNode.Value += character;
            }
            else
            {
                var errorDescription = string.Empty;
                if (tempNode.Token == 'M')
                {
                    tempNode.Description = !EnglishVersion ? "caracter(es) no reconocido(s) en comentario multilínea, se omitieron" : "unrecognized character(s) in multiline comment, not added to value";
                }
                else if (tempNode.Token == 'C')
                {
                    tempNode.Description = !EnglishVersion ? "caracter(es) no reconocido(s) en comentario simple, se omitieron" : "unrecognized character(s) in single comment, not added to value";
                }
                else
                {
                    tempNode.Description = !EnglishVersion ? "caracter(es) no reconocido(s) en cadena" : "unrecognized character(s) in string constant, not added to value";
                }

            }
        }

        public void PrintResultAndSaveToFile(string path, ref bool isFileLexicallyCorrect)
        {

            if (!Directory.Exists("C:/lexicalAnalyzer/"))
            {
                Directory.CreateDirectory("C:/lexicalAnalyzer/");
            }
            else
            {
                if (File.Exists(path))
                {
                    try
                    {
                        File.Delete(path);
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }

            using (var sw = File.CreateText(path))
            {
                foreach (var lexeme in Lexemes)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Red;
                    if (lexeme.Token == 'E')
                    {
                        isFileLexicallyCorrect = false;
                        Console.WriteLine(lexeme.Description);
                        Console.WriteLine("\n");
                    }

                    if (lexeme.Token != 'C' && lexeme.Token != 'M')
                    {
                        sw.WriteLine(lexeme.Description);
                        sw.WriteLine("\n");
                    }

                }

                if (isFileLexicallyCorrect)
                {
                    Console.WriteLine("\n");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(!EnglishVersion ? "El archivo no posee ningún error :D" : "No error was found in the file :D");
                    Console.WriteLine("\n");
                }
            }
        }

        public string PrintFile(string path)
        {
            var fileIsOpen = true;
            do
            {
                if (File.Exists(Path.ChangeExtension(path, ".pdf")))
                {
                    try
                    {
                        File.Delete(Path.ChangeExtension(path, ".pdf"));
                        fileIsOpen = false;
                    }
                    catch (Exception)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(!EnglishVersion ? "Cierra el archivo " : "Close the file ");
                        Console.Write(Path.ChangeExtension(path, ".pdf"));
                        Console.WriteLine(!EnglishVersion ? " D: luego presiona enter :D" : "D: then press enter :D");
                        Console.ReadKey();
                    }
                }
                else
                {
                    fileIsOpen = false;
                }
            } while (fileIsOpen);
            try
            {
                var pdfPath = Path.ChangeExtension(path, ".pdf");
                //convert to pdf
                //Read the Data from Input File

                var rdr = new StreamReader(path);

                //Create a New instance on Document Class

                var doc = new Document();

                //Create a New instance of PDFWriter Class for Output File

                PdfWriter.GetInstance(doc, new FileStream(pdfPath, FileMode.Create));

                //Open the Document

                doc.Open();

                //Add the content of Text File to PDF File

                doc.Add(new Paragraph(rdr.ReadToEnd()));

                //Close the Document

                doc.Close();

                //Open the Converted PDF File

                Process.Start(pdfPath);

                var p = new Process();
                p.StartInfo = new ProcessStartInfo()
                {
                    CreateNoWindow = true,
                    Verb = "print",
                    FileName = pdfPath,
                    Arguments = pdfPath
                };
                p.Start();
                Console.ForegroundColor = ConsoleColor.Green;
                return (!EnglishVersion ? "El archivo se ha añadido a tu cola de impresión... :D" : "The file has been added to your printing queue :D");

            }
            catch (Exception)
            {
                return (!EnglishVersion ? "La impresión del archivo falló :(" : "The file printing failed :(");
            }

        }
    }
}