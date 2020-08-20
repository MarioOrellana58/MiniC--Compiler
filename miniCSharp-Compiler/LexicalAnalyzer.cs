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
                if (!OperatorsAndPuncChars.Contains(fileLine[column].ToString()) || 
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
            }
        }
    }
}
