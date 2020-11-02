using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace miniCSharp_Compiler
{
    class HelperStructures
    {
        public string[,] AnalysisTable { get; set; }
        public Dictionary<string, int> ActionsDict { get; set; }
        public Dictionary<string, int> GotoDict { get; set; }
        public List<ProductionNode> Productions { get; set; }

        public HelperStructures()
        {

            ActionsDict = new Dictionary<string, int>();
            ActionsDict.Add(";", 1);
            ActionsDict.Add("I", 2);//ident
            ActionsDict.Add("const", 3);
            ActionsDict.Add("int", 4);
            ActionsDict.Add("double", 5);
            ActionsDict.Add("bool", 6);
            ActionsDict.Add("string", 7);
            ActionsDict.Add("[]", 8);
            ActionsDict.Add("(", 9);
            ActionsDict.Add(")", 10);
            ActionsDict.Add("void", 11);
            ActionsDict.Add(",", 12);
            ActionsDict.Add("class", 13);
            ActionsDict.Add("{", 14);
            ActionsDict.Add("}", 15);
            ActionsDict.Add(":", 16);
            ActionsDict.Add("interface", 17);
            ActionsDict.Add("if", 18);
            ActionsDict.Add("else", 19);
            ActionsDict.Add("while", 20);
            ActionsDict.Add("for", 21);
            ActionsDict.Add("return", 22);
            ActionsDict.Add("break", 23);
            ActionsDict.Add("Console", 24);
            ActionsDict.Add(".", 25);
            ActionsDict.Add("Writeline", 26);
            ActionsDict.Add("=", 27);
            ActionsDict.Add("this", 28);
            ActionsDict.Add("+", 29);
            ActionsDict.Add("*", 30);
            ActionsDict.Add("%", 31);
            ActionsDict.Add("-", 32);
            ActionsDict.Add("<", 33);
            ActionsDict.Add("<=", 34);
            ActionsDict.Add("==", 35);
            ActionsDict.Add("&&", 36);
            ActionsDict.Add("!", 37);
            ActionsDict.Add("New", 38);
            ActionsDict.Add("N", 39);//intConstant
            ActionsDict.Add("D", 40);//doubleConstant
            ActionsDict.Add("B", 41);//boolConstant
            ActionsDict.Add("S", 42);//stringConstant
            ActionsDict.Add("null", 43);
            ActionsDict.Add("$", 44);

            GotoDict = new Dictionary<string, int>();
            GotoDict.Add("Start", 45);
            GotoDict.Add("Program", 46);
            GotoDict.Add("Program’", 47);
            GotoDict.Add("Decl", 48);
            GotoDict.Add("VariableDecl", 49);
            GotoDict.Add("Variable", 50);
            GotoDict.Add("ConstDecl", 51);
            GotoDict.Add("ConstType", 52);
            GotoDict.Add("Type", 53);
            GotoDict.Add("FunctionDecl", 54);
            GotoDict.Add("Formals", 55);
            GotoDict.Add("ClassDecl", 56);
            GotoDict.Add("ClassDecl1", 57);
            GotoDict.Add("ClassDecl2", 58);
            GotoDict.Add("ClassDecl3", 59);
            GotoDict.Add("Field", 60);
            GotoDict.Add("InterfaceDecl", 61);
            GotoDict.Add("InterfaceDecl’", 62);
            GotoDict.Add("Prototype", 63);
            GotoDict.Add("StmtBlock", 64);
            GotoDict.Add("StmtBlock1", 65);
            GotoDict.Add("StmtBlock2", 66);
            GotoDict.Add("StmtBlock3", 67);
            GotoDict.Add("Stmt", 68);
            GotoDict.Add("Stmt’", 69);
            GotoDict.Add("IfStmt", 70);
            GotoDict.Add("IfStmt’", 71);
            GotoDict.Add("WhileStmt", 72);
            GotoDict.Add("ForStmt", 73);
            GotoDict.Add("ReturnStmt", 74);
            GotoDict.Add("BreakStmt", 75);
            GotoDict.Add("PrintStmt", 76);
            GotoDict.Add("PrintStmt’", 77);
            GotoDict.Add("Expr", 78);
            GotoDict.Add("LValue", 79);
            GotoDict.Add("Constant", 80);

            Productions = new List<ProductionNode>();
            Productions.Add(new ProductionNode { NonTerminalName = "Start", SymbolsProducedQty = 1 });//1 ver si se quita

            Productions.Add(new ProductionNode { NonTerminalName = "Program", SymbolsProducedQty = 2 });//2

            Productions.Add(new ProductionNode { NonTerminalName = "Program’", SymbolsProducedQty = 2 });//3
            Productions.Add(new ProductionNode { NonTerminalName = "Program’", SymbolsProducedQty = 0 });//4

            Productions.Add(new ProductionNode { NonTerminalName = "Decl", SymbolsProducedQty = 1 });//5
            Productions.Add(new ProductionNode { NonTerminalName = "Decl", SymbolsProducedQty = 1 });//6
            Productions.Add(new ProductionNode { NonTerminalName = "Decl", SymbolsProducedQty = 1 });//7
            Productions.Add(new ProductionNode { NonTerminalName = "Decl", SymbolsProducedQty = 1 });//8
            Productions.Add(new ProductionNode { NonTerminalName = "Decl", SymbolsProducedQty = 1 });//9

            Productions.Add(new ProductionNode { NonTerminalName = "VariableDecl", SymbolsProducedQty = 2 });//10

            Productions.Add(new ProductionNode { NonTerminalName = "Variable", SymbolsProducedQty = 2 });//11

            Productions.Add(new ProductionNode { NonTerminalName = "ConstDecl", SymbolsProducedQty = 4 });//12

            Productions.Add(new ProductionNode { NonTerminalName = "ConstType", SymbolsProducedQty = 1 });//13
            Productions.Add(new ProductionNode { NonTerminalName = "ConstType", SymbolsProducedQty = 1 });//14
            Productions.Add(new ProductionNode { NonTerminalName = "ConstType", SymbolsProducedQty = 1 });//15
            Productions.Add(new ProductionNode { NonTerminalName = "ConstType", SymbolsProducedQty = 1 });//16

            Productions.Add(new ProductionNode { NonTerminalName = "Type", SymbolsProducedQty = 1 });//17
            Productions.Add(new ProductionNode { NonTerminalName = "Type", SymbolsProducedQty = 1 });//18
            Productions.Add(new ProductionNode { NonTerminalName = "Type", SymbolsProducedQty = 1 });//19
            Productions.Add(new ProductionNode { NonTerminalName = "Type", SymbolsProducedQty = 1 });//20
            Productions.Add(new ProductionNode { NonTerminalName = "Type", SymbolsProducedQty = 1 });//21
            Productions.Add(new ProductionNode { NonTerminalName = "Type", SymbolsProducedQty = 2 });//22

            Productions.Add(new ProductionNode { NonTerminalName = "FunctionDecl", SymbolsProducedQty = 6 });//23
            Productions.Add(new ProductionNode { NonTerminalName = "FunctionDecl", SymbolsProducedQty = 6 });//24

            Productions.Add(new ProductionNode { NonTerminalName = "Formals", SymbolsProducedQty = 3 });//25
            Productions.Add(new ProductionNode { NonTerminalName = "Formals", SymbolsProducedQty = 1 });//26

            Productions.Add(new ProductionNode { NonTerminalName = "ClassDecl", SymbolsProducedQty = 7 });//27  
            
            Productions.Add(new ProductionNode { NonTerminalName = "ClassDecl1", SymbolsProducedQty = 2 });//28
            Productions.Add(new ProductionNode { NonTerminalName = "ClassDecl1", SymbolsProducedQty = 0 });//29

            Productions.Add(new ProductionNode { NonTerminalName = "ClassDecl2", SymbolsProducedQty = 3 });//30
            Productions.Add(new ProductionNode { NonTerminalName = "ClassDecl2", SymbolsProducedQty = 0 });//31  
            
            Productions.Add(new ProductionNode { NonTerminalName = "ClassDecl3", SymbolsProducedQty = 2 });//32
            Productions.Add(new ProductionNode { NonTerminalName = "ClassDecl3", SymbolsProducedQty = 0 });//33

            Productions.Add(new ProductionNode { NonTerminalName = "Field", SymbolsProducedQty = 1 });//34
            Productions.Add(new ProductionNode { NonTerminalName = "Field", SymbolsProducedQty = 1 });//35
            Productions.Add(new ProductionNode { NonTerminalName = "Field", SymbolsProducedQty = 1 });//36

            Productions.Add(new ProductionNode { NonTerminalName = "InterfaceDecl", SymbolsProducedQty = 5 });//37

            Productions.Add(new ProductionNode { NonTerminalName = "InterfaceDecl’", SymbolsProducedQty = 2 });//38
            Productions.Add(new ProductionNode { NonTerminalName = "InterfaceDecl’", SymbolsProducedQty = 0 });//39

            Productions.Add(new ProductionNode { NonTerminalName = "Prototype", SymbolsProducedQty = 6 });//40
            Productions.Add(new ProductionNode { NonTerminalName = "Prototype", SymbolsProducedQty = 6 });//41

            Productions.Add(new ProductionNode { NonTerminalName = "StmtBlock", SymbolsProducedQty = 5 });//42

            Productions.Add(new ProductionNode { NonTerminalName = "StmtBlock1", SymbolsProducedQty = 2 });//43
            Productions.Add(new ProductionNode { NonTerminalName = "StmtBlock1", SymbolsProducedQty = 0 });//44

            Productions.Add(new ProductionNode { NonTerminalName = "StmtBlock2", SymbolsProducedQty = 2 });//45
            Productions.Add(new ProductionNode { NonTerminalName = "StmtBlock2", SymbolsProducedQty = 0 });//46

            Productions.Add(new ProductionNode { NonTerminalName = "StmtBlock3", SymbolsProducedQty = 2 });//47
            Productions.Add(new ProductionNode { NonTerminalName = "StmtBlock3", SymbolsProducedQty = 0 });//48

            Productions.Add(new ProductionNode { NonTerminalName = "Stmt", SymbolsProducedQty = 2 });//49
            Productions.Add(new ProductionNode { NonTerminalName = "Stmt", SymbolsProducedQty = 1 });//50
            Productions.Add(new ProductionNode { NonTerminalName = "Stmt", SymbolsProducedQty = 1 });//51
            Productions.Add(new ProductionNode { NonTerminalName = "Stmt", SymbolsProducedQty = 1 });//52
            Productions.Add(new ProductionNode { NonTerminalName = "Stmt", SymbolsProducedQty = 1 });//53
            Productions.Add(new ProductionNode { NonTerminalName = "Stmt", SymbolsProducedQty = 1 });//54
            Productions.Add(new ProductionNode { NonTerminalName = "Stmt", SymbolsProducedQty = 1 });//55
            Productions.Add(new ProductionNode { NonTerminalName = "Stmt", SymbolsProducedQty = 1 });//56

            Productions.Add(new ProductionNode { NonTerminalName = "Stmt’", SymbolsProducedQty = 1 });//57
            Productions.Add(new ProductionNode { NonTerminalName = "Stmt’", SymbolsProducedQty = 0 });//58

            Productions.Add(new ProductionNode { NonTerminalName = "IfStmt", SymbolsProducedQty = 6 });//59

            Productions.Add(new ProductionNode { NonTerminalName = "IfStmt’", SymbolsProducedQty = 2 });//60
            Productions.Add(new ProductionNode { NonTerminalName = "IfStmt’", SymbolsProducedQty = 0 });//61

            Productions.Add(new ProductionNode { NonTerminalName = "WhileStmt", SymbolsProducedQty = 5 });//62

            Productions.Add(new ProductionNode { NonTerminalName = "ForStmt", SymbolsProducedQty = 9 });//63

            Productions.Add(new ProductionNode { NonTerminalName = "ReturnStmt", SymbolsProducedQty = 3 });//64

            Productions.Add(new ProductionNode { NonTerminalName = "BreakStmt", SymbolsProducedQty = 2 });//65

            Productions.Add(new ProductionNode { NonTerminalName = "PrintStmt", SymbolsProducedQty = 8 });//66

            Productions.Add(new ProductionNode { NonTerminalName = "PrintStmt’", SymbolsProducedQty = 3 });//67
            Productions.Add(new ProductionNode { NonTerminalName = "PrintStmt’", SymbolsProducedQty = 0 });//68

            Productions.Add(new ProductionNode { NonTerminalName = "Expr", SymbolsProducedQty = 3 });//69
            Productions.Add(new ProductionNode { NonTerminalName = "Expr", SymbolsProducedQty = 1 });//70
            Productions.Add(new ProductionNode { NonTerminalName = "Expr", SymbolsProducedQty = 1 });//71
            Productions.Add(new ProductionNode { NonTerminalName = "Expr", SymbolsProducedQty = 1 });//72
            Productions.Add(new ProductionNode { NonTerminalName = "Expr", SymbolsProducedQty = 3 });//73
            Productions.Add(new ProductionNode { NonTerminalName = "Expr", SymbolsProducedQty = 3 });//74
            Productions.Add(new ProductionNode { NonTerminalName = "Expr", SymbolsProducedQty = 3 });//75
            Productions.Add(new ProductionNode { NonTerminalName = "Expr", SymbolsProducedQty = 3 });//76
            Productions.Add(new ProductionNode { NonTerminalName = "Expr", SymbolsProducedQty = 2 });//77
            Productions.Add(new ProductionNode { NonTerminalName = "Expr", SymbolsProducedQty = 3 });//78
            Productions.Add(new ProductionNode { NonTerminalName = "Expr", SymbolsProducedQty = 3 });//79
            Productions.Add(new ProductionNode { NonTerminalName = "Expr", SymbolsProducedQty = 3 });//80
            Productions.Add(new ProductionNode { NonTerminalName = "Expr", SymbolsProducedQty = 3 });//81
            Productions.Add(new ProductionNode { NonTerminalName = "Expr", SymbolsProducedQty = 2 });//82
            Productions.Add(new ProductionNode { NonTerminalName = "Expr", SymbolsProducedQty = 4 });//83

            Productions.Add(new ProductionNode { NonTerminalName = "LValue", SymbolsProducedQty = 1 });//84
            Productions.Add(new ProductionNode { NonTerminalName = "LValue", SymbolsProducedQty = 3 });//85

            Productions.Add(new ProductionNode { NonTerminalName = "Constant", SymbolsProducedQty = 1 });//86
            Productions.Add(new ProductionNode { NonTerminalName = "Constant", SymbolsProducedQty = 1 });//87
            Productions.Add(new ProductionNode { NonTerminalName = "Constant", SymbolsProducedQty = 1 });//88
            Productions.Add(new ProductionNode { NonTerminalName = "Constant", SymbolsProducedQty = 1 });//89
            Productions.Add(new ProductionNode { NonTerminalName = "Constant", SymbolsProducedQty = 1 });//90
            
        }
    }
}
