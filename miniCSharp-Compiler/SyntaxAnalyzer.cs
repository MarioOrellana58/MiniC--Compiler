using System;
using System.Collections.Generic;
using System.Linq;
using Z.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
namespace miniCSharp_Compiler
{
    class SyntaxAnalyzer
    {
        Stack<int> StatusStack { get; set; }
        Stack<string> ConsumedSymbols { get; set; }
        HelperStructures Helper { get; set; }
        Stack<ConflictNode> ConflictsStack { get; set; }
        int MaxLexemesIndex { get; set; }
        List<string> MaxExpected { get; set; }
        LexemeNode MaxReceived { get; set; }
        public bool IsSyntacticallyCorrect { get; set; }
        public bool EnglishVersion { get; set; }
        public Dictionary<int, List<SymbolNode>> SymbolsTable { get; set; }
        public Dictionary<string, char> DataTypesFound { get; set; }
        public Stack<int> OpenScopes { get; set; }
        public int ScopesIndex { get; set; }
        public string ActualDataType { get; set; }
        public List<string> SemanticErrors { get; set; }
        public SymbolNode TempNodeForSymbolsTable { get; set; }
        public string LastIdentValue { get; set; }
        public bool FillingParameters { get; set; }
        public bool FillingClassInheritance { get; set; }
        public bool FillingExpr { get; set; }
        public Dictionary<int, string> ExprDict { get; set; }
        public List<LexemeNode> Lexemes { get; set; }
        public string identToAssign { get; set; }
        public bool ErrorInInheritance { get; set; }
        public bool ErrorInParameters { get; set; }
        //public List<string> CallStmtParameters { get; set; }
        public SyntaxAnalyzer(bool englishVersion)
        {
            StatusStack = new Stack<int>();
            ConsumedSymbols = new Stack<string>();
            Helper = new HelperStructures();
            ConflictsStack = new Stack<ConflictNode>();
            MaxExpected = new List<string>();
            IsSyntacticallyCorrect = true;
            OpenScopes = new Stack<int>();
            OpenScopes.Push(0);
            this.EnglishVersion = englishVersion;
            ActualDataType = string.Empty;
            InitializeDataTypesFound();
            SemanticErrors = new List<string>();
            TempNodeForSymbolsTable = new SymbolNode();
            LastIdentValue = string.Empty;
            FillingParameters = false;
            FillingClassInheritance = false;
            ErrorInInheritance = false;
            FillingExpr = false;
            ExprDict = new Dictionary<int, string>();
            identToAssign = string.Empty;
        }
        void InitializeDataTypesFound()
        {
            DataTypesFound = new Dictionary<string, char>();
            DataTypesFound.Add("int", ' ');
            DataTypesFound.Add("double", ' ');
            DataTypesFound.Add("bool", ' ');
            DataTypesFound.Add("string", ' ');
            DataTypesFound.Add("void", ' ');
            DataTypesFound.Add("interface", ' ');
        }

        public void AnalyzeLexemesSyntax(List<LexemeNode> lexemes, List<SymbolNode> symbolsTable)
        {
            var dollarLexeme = new LexemeNode();
            Lexemes = new List<LexemeNode>(lexemes);
            initializeSymbolsTable(symbolsTable);
            dollarLexeme.Value = "$end";
            dollarLexeme.Token = ' ';
            dollarLexeme.StartRow = lexemes[lexemes.Count - 1].StartRow;
            dollarLexeme.StartColumn = lexemes[lexemes.Count - 1].StartColumn;
            dollarLexeme.EndColumn = lexemes[lexemes.Count - 1].EndColumn;
            lexemes.Add(dollarLexeme);
            StatusStack.Push(0);
            var alreadyRecognizingLexemes = false;
            for (int i = 0; i < lexemes.Count; i++)
            {
                if (lexemes[i].Token != 'M' && lexemes[i].Token != 'C')
                {
                    if (lexemes[i].Value != "$end" && lexemes[i].Token != ' ')
                    {//The grammar is not nullable
                        alreadyRecognizingLexemes = true;
                    }
                    if (lexemes[i].Value == "$end" && lexemes[i].Token == ' ' &&
                        StatusStack.Peek() == 0 && alreadyRecognizingLexemes)
                    {//Finished recognizing all the grammar
                        break;
                    }
                    ParseLexemes(lexemes[i], ref i);
                }
            }


            if (IsSyntacticallyCorrect)
            {
                Console.WriteLine(EnglishVersion ? "Your file is lexically correct :D" : "El archivo es lexicamente correcto :D");
                Console.WriteLine(EnglishVersion ? "Your file is syntactically correct :D" : "El archivo es sintacticamente correcto :D");
                if (SemanticErrors.Count == 0)
                {
                    Console.WriteLine(EnglishVersion ? "Your file is semantically correct :D" : "El archivo es semanticamente correcto :D");
                }
                else
                {
                    PrintSemanticErrors();
                }
                PrintSymbolsTable();
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine();
            }
        }
        void PrintSemanticErrors()
        {
            for (int i = 0; i < SemanticErrors.Count; i++)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine(SemanticErrors[i]);
                Console.WriteLine("\n");
            }
        }
        void PrintSymbolsTable()
        {
            var getMaxNameSize = 0;
            var getMaxValueSize = 0;
            var getMaxTypeSize = 0;
            var getMaxColumnSize = 0;
            var getMaxLineSize = 0;

            var outputFile = new StreamWriter("SymbolsTable.txt");
            for (int i = 0; i < SymbolsTable.Count(); i++)
            {
                for (int j = 0; j < SymbolsTable[i].Count(); j++)
                {
                    if (SymbolsTable[i][j] != null)
                    {
                        getMaxNameSize = getMaxNameSize < SymbolsTable[i][j].Name.Length ? SymbolsTable[i][j].Name.Length : getMaxNameSize;
                        getMaxTypeSize = getMaxTypeSize < SymbolsTable[i][j].Type.Length ? SymbolsTable[i][j].Type.Length : getMaxTypeSize;
                        getMaxLineSize = getMaxLineSize < SymbolsTable[i][j].StartRow.ToString().Length ? SymbolsTable[i][j].StartRow.ToString().Length : getMaxLineSize;
                        getMaxColumnSize = getMaxColumnSize < SymbolsTable[i][j].StartColumn.ToString().Length ? SymbolsTable[i][j].StartColumn.ToString().Length : getMaxLineSize;
                        getMaxColumnSize = getMaxColumnSize < SymbolsTable[i][j].EndColumn.ToString().Length ? SymbolsTable[i][j].EndColumn.ToString().Length : getMaxLineSize;
                        if (SymbolsTable[i][j].Value != null)
                        {
                            getMaxValueSize = getMaxValueSize < SymbolsTable[i][j].Value.ToString().Length ? SymbolsTable[i][j].Value.ToString().Length : getMaxValueSize;
                        }
                    }
                }
            }
            //var maxPerRow = getMaxNameSize + getMaxValueSize + getMaxTypeSize + getMaxColumnSize + getMaxLineSize;
            var consoleColors = Enum.GetValues(typeof(ConsoleColor));
            var maxChars = new int[] {getMaxNameSize, getMaxValueSize, getMaxTypeSize, getMaxColumnSize, getMaxLineSize};
            var standardizeColumns = maxChars.Max();
            var indexForColors = 0;
            var maxPerRow = standardizeColumns * 5;
            Console.ForegroundColor = ConsoleColor.White;
            //Printing headers for symbolsTable
            for (int i = 0; i < maxPerRow; i++)
            {
                if (i == 0)
                {
                    Console.Write("|");
                    outputFile.Write("|");
                }
                if (i == standardizeColumns || i + 1 == maxPerRow)
                {
                    Console.Write("|");
                    outputFile.Write("|");
                    standardizeColumns *= 2;
                }
                else
                {
                    Console.Write("-");
                    outputFile.Write("-");
                }
            }

            Console.WriteLine();
            outputFile.WriteLine();
            var headersVector = new string[] {EnglishVersion? "Name": "Nombre", EnglishVersion ? "type" : "tipo", EnglishVersion ? "value" : "valor/es"
                , EnglishVersion ? "Row" : "fila", EnglishVersion? "Columns": "Columnas" };
            for (int i = 0; i < headersVector.Count(); i++)
            {
                Console.Write("|  ");
                Console.Write(headersVector[i]);
                outputFile.Write("|  ");
                outputFile.Write(headersVector[i]);
                for (int j = 0; j < maxChars.Max() - headersVector[i].Length - 2; j++)
                {
                    Console.Write(" ");
                    outputFile.Write(" ");
                }

            }

            Console.WriteLine();
            outputFile.WriteLine();
            for (int i = 0; i < maxPerRow; i++)
            {
                if (i == 0)
                {
                    Console.Write("|");
                    outputFile.Write("|");
                }
                if (i == standardizeColumns || i + 1 == maxPerRow)
                {
                    Console.Write("|");
                    outputFile.Write("|");
                    standardizeColumns *= 2;
                }
                else
                {
                    Console.Write("-");
                    outputFile.Write("-");
                }
            }

            for (int i = 0; i < SymbolsTable.Count(); i++)
            {
                ConsoleColor actualColor = new ConsoleColor();
                foreach (ConsoleColor color in consoleColors)
                {
                    actualColor = color;
                    if (consoleColors.Length <= i)
                    {
                        indexForColors = 0;
                        break;
                    }
                    if (indexForColors == i)
                    {
                        indexForColors = 0;
                        break;
                    }
                    indexForColors++;
                }
                Console.BackgroundColor = actualColor;

                for (int j = 0; j < SymbolsTable[i].Count(); j++)
                {
                    Console.WriteLine();
                    outputFile.WriteLine();
                    var valueForType = string.Empty;
                    if (SymbolsTable[i][j].Value != null)
                    {
                        valueForType = SymbolsTable[i][j].Value.ToString();
                    }
                    else
                    {
                        valueForType = "-";
                    }
                    var actualValues = new string[] { SymbolsTable[i][j].Name, SymbolsTable[i][j].Type, valueForType
                        , SymbolsTable[i][j].StartRow.ToString(), SymbolsTable[i][j].StartColumn.ToString() + " - " + SymbolsTable[i][j].EndColumn.ToString()};
                    for (int k = 0; k < actualValues.Count(); k++)
                    {
                        Console.Write("|  ");
                        outputFile.Write("|  ");
                        if (actualValues[k] != null)
                        {
                            Console.Write(actualValues[k]);
                            outputFile.Write(actualValues[k]);
                            for (int h = 0; h < maxChars.Max() - actualValues[k].Length - 2; h++)
                            {
                                Console.Write(" ");
                                outputFile.Write(" ");
                            }
                        }
                        else
                        {
                            Console.Write("-");
                            outputFile.Write("-");
                            for (int h = 0; h < maxChars.Max() - 3; h++)
                            {
                                Console.Write(" ");
                                outputFile.Write(" ");
                            }
                        }

                    }
                }
            }
        }
        bool ParseLexemes(LexemeNode lexeme, ref int lexemesIndex)
        {
            var column = 0;
            var isLexemeValue = true;
            var headerFound = false;
            if (lexeme.Value == "B" || lexeme.Value == "D" || lexeme.Value == "I" || lexeme.Value == "N" || lexeme.Value == "S")
            {
                headerFound = Helper.ActionsDict.TryGetValue(lexeme.Token.ToString(), out column);
            }
            else
            {
                headerFound = Helper.ActionsDict.TryGetValue(lexeme.Value, out column);
            }
            if (!headerFound)
            {
                isLexemeValue = false;
                var actualToken = lexeme.Token.ToString();
                if (actualToken == "H")
                {
                    actualToken = "N";
                }
                else if (actualToken == "X")
                {
                    actualToken = "D";
                }
                headerFound = Helper.ActionsDict.TryGetValue(actualToken, out column);
            }

            if (headerFound)
            {
                if (Lexemes.Count > lexemesIndex + 1)
                {
                    if (Lexemes[lexemesIndex + 1].Value == "=")
                    {
                        identToAssign = Lexemes[lexemesIndex].Value;
                    }
                }
                var analysisTableIns = getAnalysisTableInstruction(column);
                if (analysisTableIns.Contains('/'))
                {
                    /*make a backup of the actual status
                      to restore later in case the
                      path of the conflict taken 
                      by the analyzer resulted in
                      an error
                     */
                    var conflicNode = new ConflictNode();
                    conflicNode.Instructions = analysisTableIns.Split('/');
                    conflicNode.LexemesIndex = lexemesIndex;
                    conflicNode.NextInstructionIndex = 1;
                    conflicNode.StatusStack = new Stack<int>(new Stack<int>(StatusStack));
                    conflicNode.ConsumedSymbolsStack = new Stack<string>(new Stack<string>(ConsumedSymbols));
                    conflicNode.IsLexemeValue = isLexemeValue;
                    conflicNode.Lexeme = lexeme;
                    conflicNode.SymbolsTable = new Dictionary<int, List<SymbolNode>>(SymbolsTable);
                    conflicNode.DataTypesFound = new Dictionary<string, char>(DataTypesFound);
                    conflicNode.SemanticErrors = new List<string>(SemanticErrors);
                    ConflictsStack.Push(conflicNode);

                    analysisTableIns = conflicNode.Instructions[0];

                }
                if (analysisTableIns == string.Empty)
                {
                    RestoreStatus(ref lexemesIndex, isLexemeValue, lexeme);
                }
                else
                {
                    ShiftOrReduce(analysisTableIns, isLexemeValue, lexeme, ref lexemesIndex);
                }

            }
            else
            {
                //Final error detected, unrecognized symbol
                MaxReceived = lexeme;
                LexemesExpected(true);
                PrintActualErrors();
                ClearVariables();
                lexemesIndex = lexemesIndex < MaxLexemesIndex ? MaxLexemesIndex : lexemesIndex;
            }

            return true;
        }
        void ClearVariables()
        {
            StatusStack = new Stack<int>();
            ConsumedSymbols = new Stack<string>();
            MaxReceived = new LexemeNode();
            MaxExpected = new List<string>();
            ConflictsStack = new Stack<ConflictNode>();
            StatusStack.Push(0);
        }
        void RestoreStatus(ref int lexemesIndex, bool isLexemeValue, LexemeNode lexeme)
        {
            //error
            if (MaxLexemesIndex < lexemesIndex)
            {
                MaxLexemesIndex = lexemesIndex;
                MaxReceived = lexeme;
                //go across the matrix looking for header against value in current state (row), until finding the $end
                LexemesExpected(true);//Clear list and add new values
            }
            else if (MaxLexemesIndex == lexemesIndex)
            {//MaxExpected
                LexemesExpected(false);//Add to list
            }

            if (ConflictsStack.Count == 0)
            {
                //Final error, input not admitted by parser
                PrintActualErrors();
                ClearVariables();
                lexemesIndex = MaxLexemesIndex;
            }
            else
            {
                var previousStatus = ConflictsStack.Pop();
                if (previousStatus.NextInstructionIndex < previousStatus.Instructions.Count())
                {
                    lexemesIndex = previousStatus.LexemesIndex;
                    isLexemeValue = previousStatus.IsLexemeValue;
                    lexeme = previousStatus.Lexeme;
                    var analysisTableIns = previousStatus.Instructions[previousStatus.NextInstructionIndex];
                    previousStatus.NextInstructionIndex++;
                    ConsumedSymbols = new Stack<string>(new Stack<string>(previousStatus.ConsumedSymbolsStack));
                    StatusStack = new Stack<int>(new Stack<int>(previousStatus.StatusStack));
                    SymbolsTable = new Dictionary<int, List<SymbolNode>>(previousStatus.SymbolsTable);
                    DataTypesFound = new Dictionary<string, char>(previousStatus.DataTypesFound);
                    SemanticErrors = new List<string>(previousStatus.SemanticErrors);
                    ConflictsStack.Push(previousStatus);
                    ShiftOrReduce(analysisTableIns, isLexemeValue, lexeme, ref lexemesIndex);
                }
                else
                {
                    //error, check the stack again
                    RestoreStatus(ref lexemesIndex, isLexemeValue, lexeme);
                }
            }
        }

        void PrintActualErrors()
        {
            var firstMessage = EnglishVersion ? "Was expecting " : "Se esperaba ";
            var secondMessage = EnglishVersion ? ", recieved " : ", venía ";
            var onColumns = EnglishVersion ? " on columns " : " en las columnas ";
            var onLineNumber = EnglishVersion ? " on line number: " : " en la línea número: ";
            var endColumn = EnglishVersion ? " to " : " hasta ";
            if (MaxReceived.Value == "$end")
            {
                MaxReceived.Value = EnglishVersion ? "End of file" : "Fin de archivo";
            }
            MaxExpected = MaxExpected.Distinct().ToList();
            for (int i = 0; i < MaxExpected.Count; i++)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine(firstMessage + "\"" + MaxExpected[i] + "\"" + secondMessage + "\"" + MaxReceived.Value + "\"" +
                                    onColumns + MaxReceived.StartColumn.ToString() + endColumn + MaxReceived.EndColumn.ToString() +
                                    onLineNumber + MaxReceived.StartRow);
                Console.WriteLine("\n");
            }
            IsSyntacticallyCorrect = false;
        }

        void LexemesExpected(bool createOrAdd)
        {
            if (createOrAdd)
            {
                MaxExpected = new List<string>();
            }

            var helperColumns = Helper.ActionsDict.Count;
            var actualExpected = string.Empty;
            if (StatusStack.Peek() == 1)
            {
                StatusStack.Push(0);
                for (int i = 2; i <= helperColumns; i++)
                {
                    actualExpected = getAnalysisTableInstruction(i);
                    var expected = Helper.AnalysisTable[0, i];
                    TranslateHeaders(ref expected);
                    if (actualExpected != string.Empty)
                    {
                        MaxExpected.Add(expected);
                    }
                }
                StatusStack.Pop();
            }
            actualExpected = getAnalysisTableInstruction(1);
            if (actualExpected != string.Empty)
            {
                MaxExpected.Add(EnglishVersion ? "End of File" : "Fin de archivo");
            }
            for (int i = 2; i <= helperColumns; i++)//i = 2 to ignore de $end symbol
            {
                actualExpected = getAnalysisTableInstruction(i);
                var expected = Helper.AnalysisTable[0, i];
                TranslateHeaders(ref expected);
                if (actualExpected != string.Empty)
                {
                    MaxExpected.Add(expected);
                }
            }
        }
        void TranslateHeaders(ref string expected)
        {
            switch (expected)
            {
                case "ident":
                    expected = !EnglishVersion ? "identificador" : "identifier";
                    break;
                case "doubleConstant":
                    expected = !EnglishVersion ? "constante tipo double" : "double constant ";
                    break;
                case "boolConstant":
                    expected = !EnglishVersion ? "constante tipo bool" : "bool constant";
                    break;
                case "intConstant":
                    expected = !EnglishVersion ? "constante tipo int" : "int constant";
                    break;
                case "stringConstant":
                    expected = !EnglishVersion ? "cadena de caracteres (string)" : "string constant";
                    break;
                default:
                    break;
            }
        }
        void ShiftOrReduce(string analysisTableIns, bool isLexemeValue, LexemeNode lexeme, ref int lexemesIndex)
        {
            if (FillingExpr)
            {
                if (!ExprDict.TryGetValue(lexemesIndex, out string value4))
                {
                    ExprDict.Add(lexemesIndex, lexeme.Value);
                }
            }
            if (analysisTableIns[0] == 's')
            {
                var stackTop = string.Empty;
                if (ConsumedSymbols.Count > 0)
                {
                    stackTop = ConsumedSymbols.Peek();
                }
                if (stackTop == "=")
                {
                    FillingExpr = true;
                    if (!ExprDict.TryGetValue(lexemesIndex, out string value4))
                    {
                        ExprDict.Add(lexemesIndex, lexeme.Value);
                    }
                }
                else if (FillingExpr && lexeme.Value == ";")//(|| lexeme.Value == ")" || lexeme.Value == "}")
                {
                    FillingExpr = false;
                    var temp = string.Empty;
                    var identProblemFound = false;
                    foreach (var item in ExprDict)
                    {
                        if (Lexemes[item.Key].Token == 'I')
                        {
                            var symbol = getVariableFromSymbolsTable(item.Value);
                            if (symbol != null)
                            {
                                if (symbol.Value != null)
                                {
                                    temp += symbol.Value + " ";
                                }
                                else
                                {
                                    identProblemFound = true;
                                    //variable no inicializada
                                    var lineError = EnglishVersion ? "In line number: " : "En la linea: ";
                                    var innitialColumnError = EnglishVersion ? ", on columns: " : ", en las columnas: ";
                                    var endColumn = EnglishVersion ? " to " : " hasta ";
                                    var identifier = EnglishVersion ? " this identifier: \" " : " el identificador: \" ";
                                    var errorMessage = EnglishVersion ? " is not initialized " : " no esta inicializado ";
                                    SemanticErrors.Add(lineError + lexeme.StartRow.ToString() + innitialColumnError + lexeme.StartColumn.ToString() + endColumn
                                        + lexeme.EndColumn + " **ERROR** " + identifier + item.Value + " \"" + errorMessage);
                                }
                            }
                            else
                            {
                                identProblemFound = true;
                                //variable no encontrada
                                var lineError = EnglishVersion ? "In line number: " : "En la linea: ";
                                var innitialColumnError = EnglishVersion ? ", on columns: " : ", en las columnas: ";
                                var endColumn = EnglishVersion ? " to " : " hasta ";
                                var identifier = EnglishVersion ? " this variable: \" " : " la variable: \" ";
                                var errorMessage = EnglishVersion ? " is not created in any open scopes " : " no esta creada en ningun ambito abierto ";
                                SemanticErrors.Add(lineError + lexeme.StartRow.ToString() + innitialColumnError + lexeme.StartColumn.ToString() + endColumn
                                    + lexeme.EndColumn + " **ERROR** " + identifier + item.Value + " \"" + errorMessage);
                            }
                            //buscar variable en tabla de símbolos
                        }
                        else
                        {
                            if (item.Value != ";")
                            {
                                temp += item.Value + " ";
                            }
                        }
                    }
                    if (!identProblemFound)
                    {
                        var symbol = getVariableFromSymbolsTable(identToAssign);
                        if (symbol != null)
                        {
                            var value = ProcessExpr(temp);
                            if (value != null)
                            {
                                for (int i = 0; i < SymbolsTable[symbol.Scope].Count; i++)
                                {
                                    if (SymbolsTable[symbol.Scope][i].Name == symbol.Name)
                                    {
                                        var tempType = value.GetType().Name;
                                        switch (value.GetType().Name)
                                        {
                                            case "Int32":
                                                tempType = "int";
                                                break;
                                            case "Int16":
                                                tempType = "int";
                                                break;
                                            case "Int64":
                                                tempType = "int";
                                                break;
                                            case "Boolean":
                                                tempType = "bool";
                                                break;
                                            case "Double":
                                                tempType = "double";
                                                break;
                                            default:
                                                tempType = "string";
                                                break;
                                        }

                                        if (SymbolsTable[symbol.Scope][i].Type == string.Empty)
                                        {
                                            SymbolsTable[symbol.Scope][i].Type = tempType;
                                            SymbolsTable[symbol.Scope][i].Value = value;
                                        }
                                        else
                                        {
                                            if (SymbolsTable[symbol.Scope][i].Type == tempType)
                                            {
                                                SymbolsTable[symbol.Scope][i].Value = value;
                                            }
                                            else
                                            {
                                                //error, se está tratando de guardar un tipo de dato diferente en un nodo con dato ya definido
                                                var lineError = EnglishVersion ? "In line number: " : "En la linea: ";
                                                var innitialColumnError = EnglishVersion ? ", on columns: " : ", en las columnas: ";
                                                var endColumn = EnglishVersion ? " to " : " hasta ";
                                                var identifier = EnglishVersion ? " this assign to the identifier: \" " : " la asignacion para el identificador: \" ";
                                                var errorMessage = EnglishVersion ? " does not match with the data type that is traying to assign " : " no tiene el mismo tipo de dato que el valor con que se esta haciendo la operacion ";
                                                var destinationIdent = SymbolsTable[symbol.Scope][i];
                                                SemanticErrors.Add(lineError + lexeme.StartRow.ToString() + innitialColumnError + lexeme.StartColumn.ToString() + endColumn
                                                    + lexeme.EndColumn + " **ERROR** " + identifier + destinationIdent.Name + " \"" + errorMessage);

                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //error en los tipos de dato
                                //--------------------------------------------------------------------------------------
                                var lineError = EnglishVersion ? "In line number: " : "En la linea: ";
                                var innitialColumnError = EnglishVersion ? ", on columns: " : ", en las columnas: ";
                                var endColumn = EnglishVersion ? " to " : " hasta ";
                                var identifier = EnglishVersion ? " this identifier: \" " : " el identificador: \" ";
                                var errorMessage = EnglishVersion ? " is not initialized " : " no esta inicializado ";
                                SemanticErrors.Add(lineError + lexeme.StartRow.ToString() + innitialColumnError + lexeme.StartColumn.ToString() + endColumn
                                    + lexeme.EndColumn + " **ERROR** " + identifier + symbol.Name + " \"" + errorMessage);

                            }
                        }
                        else
                        {
                            //se está tratando de asignar a una variable no accesible desde el ámbito actual
                            var lineError = EnglishVersion ? "In line number: " : "En la linea: ";
                            var innitialColumnError = EnglishVersion ? ", on columns: " : ", en las columnas: ";
                            var endColumn = EnglishVersion ? " to " : " hasta ";
                            var identifier = EnglishVersion ? " this variable: \" " : " la variable: \" ";
                            var errorMessage = EnglishVersion ? " is not created in any open scopes " : " no esta creada en ningun ambito abierto ";
                            SemanticErrors.Add(lineError + lexeme.StartRow.ToString() + innitialColumnError + lexeme.StartColumn.ToString() + endColumn
                                + lexeme.EndColumn + " **ERROR** " + identifier + identToAssign + " \"" + errorMessage);

                        }
                        //procesar EXPR
                    }
                    else
                    {
                        //asignación no posible
                        //-----------------------------------------------------------------------------------
                        /* var lineError = EnglishVersion ? "In line number: " : "En la linea: ";
                         var innitialColumnError = EnglishVersion ? ", on columns: " : ", en las columnas: ";
                         var endColumn = EnglishVersion ? " to " : " hasta ";
                         var identifier = EnglishVersion ? " you can't assignate this \" " : " no puede asignar esto \" ";
                         var errorMessage = EnglishVersion ? " the data types do not match " : " los tipos de dato no coinciden ";
                         SemanticErrors.Add(lineError + lexeme.StartRow.ToString() + innitialColumnError + lexeme.StartColumn.ToString() + endColumn
                                 + lexeme.EndColumn + " **ERROR** " + identifier + SymbolsTable[0][SymbolsTable[0].Count - 1].Name + " \"" + errorMessage);
                        */
                    }
                    ExprDict = new Dictionary<int, string>();
                }
                else
                if ((stackTop == ":" || stackTop == ",") && FillingClassInheritance && lexeme.Token == 'I')
                {
                    if (DataTypesFound.TryGetValue(lexeme.Value, out char value))
                    {
                        if (!SymbolsTable[0][SymbolsTable[0].Count - 1].Parameters.TryGetValue(lexeme.Value, out string value3))
                        {
                            SymbolsTable[0][SymbolsTable[0].Count - 1].Parameters.Add(lexeme.Value, "inheritance");
                        }
                        else
                        {
                            var lineError = EnglishVersion ? "In line number: " : "En la linea: ";
                            var innitialColumnError = EnglishVersion ? ", on columns: " : ", en las columnas: ";
                            var endColumn = EnglishVersion ? " to " : " hasta ";
                            var identifier = EnglishVersion ? " the class \" " : " la clase \" ";
                            var errorMessage = EnglishVersion ? " previously inherited by: " : " ya heredo anteriormente por: ";
                            SemanticErrors.Add(lineError + lexeme.StartRow.ToString() + innitialColumnError + lexeme.StartColumn.ToString() + endColumn
                                    + lexeme.EndColumn + " **ERROR** " + identifier + SymbolsTable[0][SymbolsTable[0].Count - 1].Name + " \"" + errorMessage + lexeme.Value);
                            //herencia repetida
                            ErrorInInheritance = true;
                        }
                    }
                    else
                    {
                        //tipo de dato para herencia no encontrado
                        var lineError = EnglishVersion ? "In line number: " : "En la linea: ";
                        var innitialColumnError = EnglishVersion ? ", on columns: " : ", en las columnas: ";
                        var endColumn = EnglishVersion ? " to " : " hasta ";
                        var identifier = EnglishVersion ? " the class \" " : " la clase \" ";
                        var errorMessage = EnglishVersion ? " doesn't exist" : " no existe";
                        SemanticErrors.Add(lineError + lexeme.StartRow.ToString() + innitialColumnError + lexeme.StartColumn.ToString() + endColumn
                                + lexeme.EndColumn + " **ERROR** " + identifier + lexeme.Value + " \"" + errorMessage);
                        ErrorInInheritance = true;
                    }
                    if (ErrorInInheritance)
                    {
                        SymbolsTable[0][SymbolsTable[0].Count - 1].IsActive = false;
                    }

                }
                else if (lexeme.Value == ";" || lexeme.Value == ")")
                {
                    if (TempNodeForSymbolsTable.StartRow != 0)
                    {
                        //if this value is different than 0 then 
                        //we're filling an ident
                        if (!DataTypesFound.TryGetValue(TempNodeForSymbolsTable.Name, out char value))
                        {

                            if (validateIdentDeclInScope(TempNodeForSymbolsTable))
                            {
                                if (ErrorInParameters)
                                {
                                    TempNodeForSymbolsTable.IsActive = false;
                                    ErrorInParameters = false;
                                }
                                SymbolsTable[TempNodeForSymbolsTable.Scope].Add(TempNodeForSymbolsTable);
                            }
                            else
                            {
                                var lineError = EnglishVersion ? "In line number: " : "En la linea: ";
                                var innitialColumnError = EnglishVersion ? ", on columns: " : ", en las columnas: ";
                                var endColumn = EnglishVersion ? " to " : " hasta ";
                                var identifier = string.Empty;
                                var errorMessage = string.Empty;
                                if (TempNodeForSymbolsTable.Parameters != null)
                                {
                                    if (TempNodeForSymbolsTable.Type == "void")
                                    {
                                        identifier = EnglishVersion ? " the procedure: \" " : " el procedimiento \" ";
                                        errorMessage = EnglishVersion ? " is already defined in this scope" : " ya esta definido en este ambito";
                                    }
                                    else
                                    {
                                        identifier = EnglishVersion ? " the function: \" " : " la funcion \" ";
                                        errorMessage = EnglishVersion ? " is already defined in this scope" : " ya esta definida en este ambito";
                                    }

                                }
                                else
                                {
                                    identifier = EnglishVersion ? " the identifier: \" " : " el identificador \" ";
                                    errorMessage = EnglishVersion ? " is already defined in this scope" : " ya esta definido en este ambito";
                                }
                                SemanticErrors.Add(lineError + TempNodeForSymbolsTable.StartRow.ToString() + innitialColumnError + TempNodeForSymbolsTable.StartColumn.ToString() + endColumn
                                    + TempNodeForSymbolsTable.EndColumn + " **ERROR** " + identifier + TempNodeForSymbolsTable.Name + " \"" + errorMessage);
                            }
                        }
                        else
                        {
                            var lineError = EnglishVersion ? "In line number: " : "En la linea: ";
                            var innitialColumnError = EnglishVersion ? ", on columns: " : ", en las columnas: ";
                            var endColumn = EnglishVersion ? " to " : " hasta ";
                            var identifier = string.Empty;
                            var errorMessage = string.Empty;
                            if (TempNodeForSymbolsTable.Parameters != null)
                            {
                                if (TempNodeForSymbolsTable.Type == "void")
                                {
                                    identifier = EnglishVersion ? " the procedure: \" " : " el procedimiento \" ";
                                    errorMessage = EnglishVersion ? " name is a data type" : " que está definiendo ya existe como tipo de dato";
                                }
                                else
                                {
                                    identifier = EnglishVersion ? " the function: \" " : " la funcion \" ";
                                    errorMessage = EnglishVersion ? " name is a data type" : " que está definiendo ya existe como tipo de dato";
                                }

                            }
                            else
                            {
                                identifier = EnglishVersion ? " the identifier: \" " : " el identificador \" ";
                                errorMessage = EnglishVersion ? " is already defined as a data type" : " ya esta definido como un tipo de dato";
                            }
                            SemanticErrors.Add(lineError + TempNodeForSymbolsTable.StartRow.ToString() + innitialColumnError + TempNodeForSymbolsTable.StartColumn.ToString() + endColumn
                                + TempNodeForSymbolsTable.EndColumn + " **ERROR** " + identifier + TempNodeForSymbolsTable.Name + " \"" + errorMessage);

                        }
                        TempNodeForSymbolsTable = new SymbolNode();
                        FillingParameters = false;
                    }
                    //revisar si sí aquí debería añadirse el nodo
                }
                else if (lexeme.Token == 'I' && stackTop == "class")
                {
                    TempNodeForSymbolsTable.Name = lexeme.Value;
                    TempNodeForSymbolsTable.Scope = OpenScopes.Peek();
                    TempNodeForSymbolsTable.StartColumn = lexeme.StartColumn;
                    TempNodeForSymbolsTable.EndColumn = lexeme.EndColumn;
                    TempNodeForSymbolsTable.StartRow = lexeme.StartRow;
                    TempNodeForSymbolsTable.Type = stackTop;
                    TempNodeForSymbolsTable.InitParameters();
                    if (validateIdentDeclInScope(TempNodeForSymbolsTable) && !DataTypesFound.TryGetValue(lexeme.Value, out char value))
                    {
                        DataTypesFound.Add(lexeme.Value, ' ');
                        SymbolsTable[TempNodeForSymbolsTable.Scope].Add(TempNodeForSymbolsTable);
                        FillingClassInheritance = true;
                    }
                    else
                    {
                        //Erorr trying to define as new an existing data type
                        var lineError = EnglishVersion ? "In line number: " : "En la linea: ";
                        var innitialColumnError = EnglishVersion ? ", on columns: " : ", en las columnas: ";
                        var endColumn = EnglishVersion ? " to " : " hasta ";
                        var identifier = EnglishVersion ? " this class: \" " : " esta clase \" ";
                        var errorMessage = EnglishVersion ? " is already defined in this scope" : " ya esta definida en este ambito";
                        SemanticErrors.Add(lineError + TempNodeForSymbolsTable.StartRow.ToString() + innitialColumnError + TempNodeForSymbolsTable.StartColumn.ToString() + endColumn
                            + TempNodeForSymbolsTable.EndColumn + " **ERROR** " + identifier + TempNodeForSymbolsTable.Name + " \"" + errorMessage);
                    }
                    TempNodeForSymbolsTable = new SymbolNode();


                }
                else if (lexeme.Token == 'I' && (stackTop == "Type" || stackTop == "ConstType" || stackTop == "void" || stackTop == "interface"))
                {
                    if (!FillingParameters)
                    {
                        TempNodeForSymbolsTable.Name = lexeme.Value;
                        TempNodeForSymbolsTable.Scope = OpenScopes.Peek();
                        TempNodeForSymbolsTable.StartColumn = lexeme.StartColumn;
                        TempNodeForSymbolsTable.EndColumn = lexeme.EndColumn;
                        TempNodeForSymbolsTable.StartRow = lexeme.StartRow;
                        TempNodeForSymbolsTable.Type = stackTop == "void" || stackTop == "interface" ? stackTop : ActualDataType;
                    }
                    else
                    {
                        var tempParameter = new SymbolNode();
                        tempParameter.Name = lexeme.Value;
                        tempParameter.Scope = OpenScopes.Peek();
                        tempParameter.StartColumn = lexeme.StartColumn;
                        tempParameter.EndColumn = lexeme.EndColumn;
                        tempParameter.StartRow = lexeme.StartRow;
                        tempParameter.Type = ActualDataType;
                        if (validateIdentDeclInScope(tempParameter))
                        {
                            if (!DataTypesFound.TryGetValue(tempParameter.Name, out char value))
                            {
                                if (ActualDataType == "NF")//not found datatype in reduction by ID
                                {
                                    TempNodeForSymbolsTable.IsActive = false;
                                    var lineError = EnglishVersion ? "In line number: " : "En la linea: ";
                                    var innitialColumnError = EnglishVersion ? ", on columns: " : ", en las columnas: ";
                                    var endColumn = EnglishVersion ? " to " : " hasta ";
                                    var identifier = EnglishVersion ? " this parameter: \" " : " el parametro: \" ";
                                    var errorMessage = EnglishVersion ? " makes reference to a non existing data type " : " hace referencia a un tipo de dato inexistente";
                                    SemanticErrors.Add(lineError + tempParameter.StartRow.ToString() + innitialColumnError + tempParameter.StartColumn.ToString() + endColumn
                                        + tempParameter.EndColumn + " **ERROR** " + identifier + tempParameter.Name + " \"" + errorMessage);
                                }
                                TempNodeForSymbolsTable.Parameters.Add(tempParameter.Name, tempParameter.Type);
                                SymbolsTable[tempParameter.Scope].Add(tempParameter);
                            }
                            else
                            {
                                var lineError = EnglishVersion ? "In line number: " : "En la linea: ";
                                var innitialColumnError = EnglishVersion ? ", on columns: " : ", en las columnas: ";
                                var endColumn = EnglishVersion ? " to " : " hasta ";
                                var identifier = EnglishVersion ? " the parameter: \" " : " el parametro \" ";
                                var errorMessage = EnglishVersion ? " is already defined as a data type" : " ya esta definido como un tipo de dato";
                                TempNodeForSymbolsTable.IsActive = false;
                                ErrorInParameters = true;
                                SemanticErrors.Add(lineError + tempParameter.StartRow.ToString() + innitialColumnError + tempParameter.StartColumn.ToString() + endColumn
                                    + tempParameter.EndColumn + " **ERROR** " + identifier + tempParameter.Name + " \"" + errorMessage);
                            }
                        }
                        else
                        {
                            var lineError = EnglishVersion ? "In line number: " : "En la linea: ";
                            var innitialColumnError = EnglishVersion ? ", on columns: " : ", en las columnas: ";
                            var endColumn = EnglishVersion ? " to " : " hasta ";
                            var identifier = EnglishVersion ? " the parameter: \" " : " el parametro \" ";
                            var errorMessage = EnglishVersion ? " is already defined in this scope" : " ya esta definido en este ambito";
                            TempNodeForSymbolsTable.IsActive = false;
                            ErrorInParameters = true;
                            SemanticErrors.Add(lineError + tempParameter.StartRow.ToString() + innitialColumnError + tempParameter.StartColumn.ToString() + endColumn
                                + tempParameter.EndColumn + " **ERROR** " + identifier + tempParameter.Name + " \"" + errorMessage);
                        }
                    }
                    //left value and parameters
                }
                else if (lexeme.Value == "(")
                {
                    if (ConsumedSymbols.Count > 1)
                    {
                        var tryIdentSymbol = ConsumedSymbols.Pop();
                        if ((ConsumedSymbols.Peek() == "Type" || ConsumedSymbols.Peek() == "void") && tryIdentSymbol == "I")
                        {
                            ScopesIndex++;
                            OpenScopes.Push(ScopesIndex);
                            SymbolsTable.Add(ScopesIndex, new List<SymbolNode>());
                            FillingParameters = true;
                            TempNodeForSymbolsTable.InitParameters();
                        }
                        ConsumedSymbols.Push(tryIdentSymbol);
                    }
                }
                else if (lexeme.Value == "{")
                {
                    FillingClassInheritance = false;
                    if (ErrorInInheritance)
                    {
                        //delete datatype
                        var className = SymbolsTable[0][SymbolsTable[0].Count - 1].Name;
                        if (DataTypesFound.TryGetValue(className, out char value0))
                        {
                            DataTypesFound.Remove(className);
                        }
                    }
                    ErrorInInheritance = false;

                    if (ConsumedSymbols.Peek() != ")")
                    {
                        if (TempNodeForSymbolsTable.Type == "interface")
                        {
                            if (validateIdentDeclInScope(TempNodeForSymbolsTable))
                            {
                                SymbolsTable[TempNodeForSymbolsTable.Scope].Add(TempNodeForSymbolsTable);
                            }
                            else
                            {
                                var lineError = EnglishVersion ? "In line number: " : "En la linea: ";
                                var innitialColumnError = EnglishVersion ? ", on columns: " : ", en las columnas: ";
                                var endColumn = EnglishVersion ? " to " : " hasta ";
                                var identifier = EnglishVersion ? " this interface: \" " : " esta interfaz \" ";
                                var errorMessage = EnglishVersion ? " is already defined in this scope" : " ya esta definida en este ambito";
                                SemanticErrors.Add(lineError + TempNodeForSymbolsTable.StartRow.ToString() + innitialColumnError + TempNodeForSymbolsTable.StartColumn.ToString() + endColumn
                                    + TempNodeForSymbolsTable.EndColumn + " **ERROR** " + identifier + TempNodeForSymbolsTable.Name + " \"" + errorMessage);
                            }
                            TempNodeForSymbolsTable = new SymbolNode();
                        }
                        ScopesIndex++;
                        OpenScopes.Push(ScopesIndex);
                        SymbolsTable.Add(ScopesIndex, new List<SymbolNode>());
                    }
                }
                else if (lexeme.Value == "}")
                {
                    if (OpenScopes.Count > 1)
                    {
                        OpenScopes.Pop();
                    }
                    else
                    {
                        //error??
                    }


                }
                if (lexeme.Token == 'I')
                {
                    LastIdentValue = lexeme.Value;
                }
                shiftTo(analysisTableIns, (isLexemeValue ? lexeme.Value : lexeme.Token.ToString()));
            }
            else if (analysisTableIns[0] == 'r')
            {
                var column = 0;
                var headerFound = false;
                if (analysisTableIns == "r16" || analysisTableIns == "r17" || analysisTableIns == "r18" || analysisTableIns == "r19"
                    || analysisTableIns == "r12" || analysisTableIns == "r13" || analysisTableIns == "r14" || analysisTableIns == "r15")
                {
                    ActualDataType = ConsumedSymbols.Peek();
                }
                else if (analysisTableIns == "r20")
                {
                    if (DataTypesFound.TryGetValue(LastIdentValue, out char value))
                    {
                        ActualDataType = LastIdentValue;
                    }
                    else
                    {
                        if (FillingClassInheritance)
                        {
                            ActualDataType = "NF";
                            //imprimir error que en la herencia no se encontró el tipo de dato
                            var lineError = EnglishVersion ? "In line number: " : "En la linea: ";
                            var innitialColumnError = EnglishVersion ? ", on columns: " : ", en las columnas: ";
                            var endColumn = EnglishVersion ? " to " : " hasta ";
                            var identifier = EnglishVersion ? " this identifier: \" " : " el identificador: \" ";
                            var errorMessage = EnglishVersion ? " has an invalid data type " : " se esta intentando crear con un tipo de dato que no existe";
                            SemanticErrors.Add(lineError + TempNodeForSymbolsTable.StartRow.ToString() + innitialColumnError + TempNodeForSymbolsTable.StartColumn.ToString() + endColumn
                                + TempNodeForSymbolsTable.EndColumn + " **ERROR** " + identifier + TempNodeForSymbolsTable.Name + " \"" + errorMessage);
                        }
                        else if (FillingParameters)
                        {
                            ActualDataType = "NF";
                        }


                    }
                }
                reduceBy(analysisTableIns);
                headerFound = Helper.GotoDict.TryGetValue(ConsumedSymbols.Peek(), out column);
                if (headerFound)
                {
                    analysisTableIns = getAnalysisTableInstruction(column);

                    gotoStatus(Convert.ToInt32(analysisTableIns));

                    lexemesIndex--;
                }
                if (ConsumedSymbols.Peek() == "Decl" || ConsumedSymbols.Peek() == "Program5")
                {
                    ConflictsStack = new Stack<ConflictNode>();
                    if (MaxLexemesIndex < lexemesIndex)
                    {
                        MaxLexemesIndex = lexemesIndex;
                        MaxReceived = lexeme;
                    }
                }
            }
        }
        bool validateIdentDeclInScope(SymbolNode actualIdent)
        {

            if (SymbolsTable[actualIdent.Scope].Count != 0)
            {
                foreach (var symbol in SymbolsTable[actualIdent.Scope])
                {
                    if (symbol.Name == actualIdent.Name)
                    {
                        return false;
                    }
                }
            }
            return true;


        }
        string getAnalysisTableInstruction(int column)
        {
            return Helper.AnalysisTable[StatusStack.Peek() + 1, column];//to sync with table row index
        }

        void shiftTo(string actualInstruction, string consumedSymbol)
        {
            var nextStatus = Convert.ToInt32(actualInstruction.Substring(1, actualInstruction.Length - 1));
            StatusStack.Push(nextStatus);
            ConsumedSymbols.Push(consumedSymbol);
        }

        void reduceBy(string actualInstruction)
        {
            var reductionProdId = Convert.ToInt32(actualInstruction.Substring(1, actualInstruction.Length - 1));

            var production = Helper.Productions[reductionProdId];

            popStacksAndReplaceStatus(production.SymbolsProducedQty, production.NonTerminalName);
        }

        void gotoStatus(int nextStatus)
        {
            StatusStack.Push(nextStatus);
        }

        void popStacksAndReplaceStatus(int elementsQtyToPop, string productionName)
        {
            for (int i = 0; i < elementsQtyToPop; i++)
            {
                if (StatusStack.Count > 0)
                {
                    StatusStack.Pop();
                }

                if (ConsumedSymbols.Count > 0)
                {
                    ConsumedSymbols.Pop();
                }
            }

            ConsumedSymbols.Push(productionName);
        }
        void initializeSymbolsTable(List<SymbolNode> symbolsTable)
        {
            SymbolsTable = new Dictionary<int, List<SymbolNode>>();
            SymbolsTable.Add(0, new List<SymbolNode>());
            for (int i = 0; i < symbolsTable.Count; i++)
            {
                symbolsTable[i] = symbolsTable[i];
            }
        }

        SymbolNode getVariableFromSymbolsTable(string name)
        {
            var auxStack = new Stack<int>(OpenScopes);

            while (OpenScopes.Count > 0)
            {
                foreach (var item in SymbolsTable[OpenScopes.Peek()])
                {
                    if (item.Name == name)
                    {
                        OpenScopes = new Stack<int>(auxStack);
                        return item;
                    }
                }
                OpenScopes.Pop();
            }

            OpenScopes = new Stack<int>(auxStack);
            return null;
        }

        dynamic ProcessExpr(string expr)
        {
            if (expr.Contains("True"))
            {
                string pattern = @"\bTrue\b";
                string replace = "true";
                expr = Regex.Replace(expr, pattern, replace);
            }
            if (expr.Contains("False"))
            {
                string pattern = @"\bFalse\b";
                string replace = "false";
                expr = Regex.Replace(expr, pattern, replace);
            }
            dynamic result;
            try
            {
                result = Eval.Execute<dynamic>(expr);//validar /0
                if (result.GetType().Name != "string")
                {
                    if (result != null)
                    {
                        return result;
                    }
                }
                else
                {
                    //división entre 0
                    var lineError = EnglishVersion ? "In line number: " : "En la linea: ";
                    var innitialColumnError = EnglishVersion ? ", on columns: " : ", en las columnas: ";
                    var endColumn = EnglishVersion ? " to " : " hasta ";
                    var identifier = EnglishVersion ? " this operations cannot be done: \" " : " esta operacion no puede hacerse: \" ";
                    var errorMessage = EnglishVersion ? " you can't divide by zero " : " se esta intentando no se sabe quien reinicio";
                    SemanticErrors.Add(lineError + TempNodeForSymbolsTable.StartRow.ToString() + innitialColumnError + TempNodeForSymbolsTable.StartColumn.ToString() + endColumn
                        + TempNodeForSymbolsTable.EndColumn + " **ERROR** " + identifier + TempNodeForSymbolsTable.Name + " \"" + errorMessage);

                }
            }
            catch (Exception)
            {
                if (expr.Contains("+"))
                {
                    var splitExpr = expr.Split('+');
                    var outPutString = string.Empty;
                    for (int i = 0; i < splitExpr.Length; i++)
                    {
                        outPutString += splitExpr[i];
                    }
                    return outPutString;
                }
                //else sería algún tipo de error porque no se pudo operar la entrada,
                //maybe en los tipos de dato
            }
            return null;
        }
    }
}
