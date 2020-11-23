using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
        public Dictionary<int,List<SymbolNode>> SymbolsTable { get; set; }          
        public Dictionary<string, char> DataTypesFound { get; set; }                   
        public Stack<int> OpenScopes { get; set; }
        public int ScopesIndex { get; set; }
        public string ActualDataType { get; set; }
        public List<string> SemanticErrors { get; set; }
        public SymbolNode TempNodeForSymbolsTable { get; set; }
        public string LastIdentValue { get; set; }
        public bool FillingParameters { get; set; }
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
            }
        }
        bool ParseLexemes(LexemeNode lexeme, ref int lexemesIndex)
        {
            var column = 0;
            var isLexemeValue = true;
            //consume terminal symbol
            var headerFound = Helper.ActionsDict.TryGetValue(lexeme.Value, out column);
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
            if (analysisTableIns[0] == 's')
            {
                var stackTop = string.Empty;
                if (ConsumedSymbols.Count > 0)
                {
                    stackTop = ConsumedSymbols.Peek();
                }
                if (lexeme.Value == ";" || lexeme.Value == ")")
                {
                    if (TempNodeForSymbolsTable.StartRow != 0)
                    {
                        //if this value is different than 0 then 
                        //we're filling an ident
                        if (!DataTypesFound.TryGetValue(TempNodeForSymbolsTable.Name, out char value))
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
                    if (validateIdentDeclInScope(TempNodeForSymbolsTable) && !DataTypesFound.TryGetValue(lexeme.Value, out char value))
                    {
                        DataTypesFound.Add(lexeme.Value, ' ');
                        SymbolsTable[TempNodeForSymbolsTable.Scope].Add(TempNodeForSymbolsTable);
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
                        if ((ConsumedSymbols.Peek() == "Type" || ConsumedSymbols.Peek() == "void" ) && tryIdentSymbol == "I")
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
                    if (ScopesIndex > 0 && OpenScopes.Count > 0)
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
                        //Erorr trying to define variable with non existing data type
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
        bool validateIdentDeclInScope (SymbolNode actualIdent)
        {
            if (SymbolsTable[actualIdent.Scope].Count != 0 )
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
        void  initializeSymbolsTable(List<SymbolNode> symbolsTable) 
        { 
            SymbolsTable = new Dictionary<int, List<SymbolNode>>();
            SymbolsTable.Add(0, new List<SymbolNode>());
            for (int i = 0; i < symbolsTable.Count; i++)
            {
                symbolsTable[i] = symbolsTable[i];
            }
        }
    }
}
