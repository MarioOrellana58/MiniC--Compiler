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
                var fileLine = sr.ReadLine();
                while (fileLine != null)
                {
                    row++;
                    AnalyzeLine(fileLine, row);
                    fileLine = sr.ReadLine();
                }
            }
        }

        void AnalyzeLine(string fileLine, int row)
        {          
            var tempNode = new LexemeNode();
            for (int column = 0; column < fileLine.Length; column++)
            {
                
                //first it analyzes if the read character is a not valid character
                if (!OperatorsAndPuncChars.Contains(fileLine[column].ToString()) && 
                    !Char.IsLetterOrDigit(fileLine[column]))
                {
                    if (tempNode.Value != string.Empty)
                    {
                        //terminar nodo
                    }

                    tempNode = new LexemeNode();
                    tempNode.Value = fileLine[column].ToString();
                    tempNode.StartColumn = column + 1;
                    tempNode.EndColumn = column + 1;
                    tempNode.Row = row;
                    tempNode.Token = 'E';
                    tempNode.Description = tempNode.Value + " en la línea " + tempNode.Row + " cols " + tempNode.StartColumn + "-" + tempNode.EndColumn + " es un caracter no reconocido ";
                    Lexemes.Add(tempNode);
                }
                else if (OperatorsAndPuncChars.Contains(fileLine[column].ToString()) && tempNode.Token != 'O')//this last validation was made because of double operators
                {                    
                    if (tempNode.Value != string.Empty)
                    {
                        //if this is true then we already have a node to finish
                        tempNode.EndColumn = column;                        
                        Lexemes.Add(tempNode);
                        column--;//recoil
                    }
                    else
                    {
                        //if this is true then we are reading the first part of an operator
                        tempNode.Value += fileLine[column].ToString();
                        tempNode.StartColumn = column + 1;
                        tempNode.Row = row;
                        tempNode.Token = 'O';
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
                        else if (fileLine[column] == '/')
                        {
                            //comment
                            column++;
                            tempNode.Value = fileLine[column].ToString();
                            tempNode.StartColumn = column + 1;
                            tempNode.Row = row;
                            tempNode.Token = 'C';
                        }
                        
                    }
                    else
                    {
                        //Switch case
                    }
                }
            }
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
                    return "comentario";
                case 'O':
                    return "operador o signo de puntuación";
                default:
                    return string.Empty;
            }
        }
    }
}
