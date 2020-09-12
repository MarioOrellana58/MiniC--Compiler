using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace miniCSharp_Compiler
{
    public class SyntaxTrees
    {
        public Dictionary<string, SyntaxNode> SyntaxTreesDic = new Dictionary<string, SyntaxNode>();
        public SyntaxTrees()
        {
            var root0 = new SyntaxNode("Program");
            var firstSon0 = new SyntaxNode("Decl");
            var firstS0F1 = new SyntaxNode("Program'");
            root0.Sons.Add(firstSon0);
            firstSon0.Sons.Add(firstS0F1);
            SyntaxTreesDic.Add("Program", root0);


            //-------------------------------------------------


            var root1 = new SyntaxNode("Program'");
            var firstSon1 = new SyntaxNode("Decl");
            var firstS1F1 = new SyntaxNode("Program'");
            var secondSon1 = new SyntaxNode("Eps");
            root1.Sons.Add(firstSon1);
            root1.Sons.Add(secondSon1);
            firstSon1.Sons.Add(firstS1F1);
            SyntaxTreesDic.Add("Program'", root1);


            //-------------------------------------------------


            var root2 = new SyntaxNode("Decl");
            var firstSon2 = new SyntaxNode("VariableDecl");
            var secondSon2 = new SyntaxNode("FunctionDecl");
            root2.Sons.Add(firstSon2);
            root2.Sons.Add(secondSon2);
            SyntaxTreesDic.Add("Decl", root2);


            //-------------------------------------------------

            var root3 = new SyntaxNode("VariableDecl");
            var leftson3 = new SyntaxNode("Variable");
            var leftS3F1 = new SyntaxNode(";");

            root3.Sons.Add(leftson3);
            leftson3.Sons.Add(leftS3F1);
            SyntaxTreesDic.Add("VariableDecl", root3);


            //-------------------------------------------------


            var root4 = new SyntaxNode("Variable");
            var leftson4 = new SyntaxNode("Type");
            var leftS4F1 = new SyntaxNode("I");

            root4.Sons.Add(leftson4);
            leftson4.Sons.Add(leftS4F1);
            SyntaxTreesDic.Add("Variable", root4);



            //-------------------------------------------------

            var root5 = new SyntaxNode("Type");
            var firstSon5 = new SyntaxNode("int");
            var firstS5F1 = new SyntaxNode("Type'");
            var secondSon5 = new SyntaxNode("double");
            var secondS5F1 = new SyntaxNode("Type'");
            var thirdSon5 = new SyntaxNode("bool");
            var thirdS5F1 = new SyntaxNode("Type'");
            var fourthSon5 = new SyntaxNode("string");
            var fourthS5F1 = new SyntaxNode("Type'");
            var fifthSon5 = new SyntaxNode("I");
            var fifthS5F1 = new SyntaxNode("Type'");

            root5.Sons.Add(firstSon5);
            root5.Sons.Add(secondSon5);
            root5.Sons.Add(thirdSon5);
            root5.Sons.Add(fourthSon5);
            root5.Sons.Add(fifthSon5);
            firstSon5.Sons.Add(firstS5F1);
            secondSon5.Sons.Add(secondS5F1);
            thirdSon5.Sons.Add(thirdS5F1);
            fourthSon5.Sons.Add(fourthS5F1);
            fifthSon5.Sons.Add(fifthS5F1);

            SyntaxTreesDic.Add("Type", root5);


            //-------------------------------------------------

            var root6 = new SyntaxNode("Type'");
            var firstSon6 = new SyntaxNode("[]");
            var firstS6F1 = new SyntaxNode("Type'");
            var secondSon6 = new SyntaxNode("Eps");

            root6.Sons.Add(firstSon6);
            root6.Sons.Add(secondSon6);
            firstSon6.Sons.Add(firstS6F1);

            SyntaxTreesDic.Add("Type'", root6);


            //-------------------------------------------------

            var root7 = new SyntaxNode("FunctionDecl");
            var leftSon7 = new SyntaxNode("Type");
            var leftS7F1 = new SyntaxNode("I");
            var leftS7F2 = new SyntaxNode("(");
            var leftS7F3 = new SyntaxNode("Formals");
            var leftS7F7 = new SyntaxNode(")");
            var leftS7F5 = new SyntaxNode("FunctionDecl'");

            var rightSon7 = new SyntaxNode("void");
            var rightS7F1 = new SyntaxNode("I");
            var rightS7F2 = new SyntaxNode("(");
            var rightS7F3 = new SyntaxNode("Formals");
            var rightS7F7 = new SyntaxNode(")");
            var rightS7F5 = new SyntaxNode("FunctionDecl'");

            root7.Sons.Add(leftSon7);
            root7.Sons.Add(rightSon7);
            leftSon7.Sons.Add(leftS7F1);
            leftS7F1.Sons.Add(leftS7F2);
            leftS7F2.Sons.Add(leftS7F3);
            leftS7F3.Sons.Add(leftS7F7);
            leftS7F7.Sons.Add(leftS7F5);
            rightSon7.Sons.Add(rightS7F1);
            rightS7F1.Sons.Add(rightS7F2);
            rightS7F2.Sons.Add(rightS7F3);
            rightS7F3.Sons.Add(rightS7F7);
            rightS7F7.Sons.Add(rightS7F5);
            SyntaxTreesDic.Add("FunctionDecl", root7);



            //-------------------------------------------------

            var root8 = new SyntaxNode("Formals");
            var firstSon8 = new SyntaxNode("Variable");
            var firstS8F1 = new SyntaxNode("Formals'");
            var secondSon8 = new SyntaxNode("Eps");
            root8.Sons.Add(firstSon8);
            firstSon8.Sons.Add(firstS8F1);
            root8.Sons.Add(secondSon8);
            SyntaxTreesDic.Add("Formals", root8);


            //-------------------------------------------------

            var root9 = new SyntaxNode("Formals'");
            var firstSon9 = new SyntaxNode(",");
            var firstS9F1 = new SyntaxNode("Variable");
            var firstS9F2 = new SyntaxNode("Formals'");
            var secondSon9 = new SyntaxNode("Eps");

            root9.Sons.Add(firstSon9);
            root9.Sons.Add(secondSon9);
            firstSon9.Sons.Add(firstS9F1);
            firstS9F1.Sons.Add(firstS9F2);

            SyntaxTreesDic.Add("Formals'", root9);

            //-------------------------------------------------
            var root10 = new SyntaxNode("FunctionDecl'");
            var firstSon10 = new SyntaxNode("Stmt");
            var firstS10F1 = new SyntaxNode("FunctionDecl'");
            var secondSon10 = new SyntaxNode("Eps");

            root10.Sons.Add(firstSon10);
            root10.Sons.Add(secondSon10);
            firstSon10.Sons.Add(firstS10F1);

            SyntaxTreesDic.Add("FunctionDecl'", root10);

            //-------------------------------------------------

            var root11 = new SyntaxNode("Stmt");
            var firstSon11 = new SyntaxNode("ReturnStmt");
            var secondSon11 = new SyntaxNode("PrintStmt");
            var thirdSon11 = new SyntaxNode("Expr");
            var thirdS11F1 = new SyntaxNode(";");

            root11.Sons.Add(firstSon11);
            root11.Sons.Add(secondSon11);
            root11.Sons.Add(thirdSon11);
            thirdSon11.Sons.Add(thirdS11F1);

            SyntaxTreesDic.Add("Stmt", root11);


            //-------------------------------------------------

            var root12 = new SyntaxNode("ReturnStmt");
            var firstSon12 = new SyntaxNode("return");
            var firstS12F1 = new SyntaxNode("ReturnStmt'");
            var firstS12F2 = new SyntaxNode(";");

            root12.Sons.Add(firstSon12);
            firstSon12.Sons.Add(firstS12F1);
            firstS12F1.Sons.Add(firstS12F2);

            SyntaxTreesDic.Add("ReturnStmt", root12);

            //-------------------------------------------------

            var root13 = new SyntaxNode("ReturnStmt'");
            var firstSon13 = new SyntaxNode("Expr");
            var secondSon13 = new SyntaxNode("Eps");

            root13.Sons.Add(firstSon13);
            root13.Sons.Add(secondSon13);

            SyntaxTreesDic.Add("ReturnStmt'", root13);

            //-------------------------------------------------


            var root14 = new SyntaxNode("PrintStmt");
            var firstSon14 = new SyntaxNode("Print");
            var firstS14F1 = new SyntaxNode("(");
            var firstS14F2 = new SyntaxNode("Expr");
            var firstS14F3 = new SyntaxNode("PrintStmt'");
            var firstS14F4 = new SyntaxNode(")");
            var firstS14F5 = new SyntaxNode(";");

            root14.Sons.Add(firstSon14);
            firstSon14.Sons.Add(firstS14F1);
            firstS14F1.Sons.Add(firstS14F2);
            firstS14F2.Sons.Add(firstS14F3);
            firstS14F3.Sons.Add(firstS14F4);
            firstS14F4.Sons.Add(firstS14F5);

            SyntaxTreesDic.Add("PrintStmt", root14);


            //-------------------------------------------------


            var root15 = new SyntaxNode("PrintStmt'");
            var firstSon15 = new SyntaxNode(",");
            var firstS15F1 = new SyntaxNode("Expr");
            var firstS15F2 = new SyntaxNode("PrintStmt'");
            var secondSon15 = new SyntaxNode("Eps");

            root15.Sons.Add(firstSon15);
            root15.Sons.Add(secondSon15);
            firstSon15.Sons.Add(firstS15F1);
            firstS15F1.Sons.Add(firstS15F2);
            SyntaxTreesDic.Add("PrintStmt'", root15);


            //-------------------------------------------------

            var root16 = new SyntaxNode("Expr");
            var firstSon16 = new SyntaxNode("AA");
            var firstS16F1 = new SyntaxNode("Expr'");

            root16.Sons.Add(firstSon16);
            firstSon16.Sons.Add(firstS16F1);

            SyntaxTreesDic.Add("Expr", root16);


            //-------------------------------------------------

            var root17 = new SyntaxNode("Expr'");
            var firstSon17 = new SyntaxNode("||");
            var firstS17F1 = new SyntaxNode("AA");
            var firstS17F2 = new SyntaxNode("Expr'");
            var secondSon17 = new SyntaxNode("Eps");

            root17.Sons.Add(firstSon17);
            root17.Sons.Add(secondSon17);
            firstSon17.Sons.Add(firstS17F1);
            firstS17F1.Sons.Add(firstS17F2);
            SyntaxTreesDic.Add("Expr'", root17);



            //-------------------------------------------------


            var root18 = new SyntaxNode("AA");
            var firstSon18 = new SyntaxNode("BB");
            var firstS18F1 = new SyntaxNode("AA'");

            root18.Sons.Add(firstSon18);
            firstSon18.Sons.Add(firstS18F1);

            SyntaxTreesDic.Add("AA", root18);


            //-------------------------------------------------


            var root19 = new SyntaxNode("AA'");
            var firstSon19 = new SyntaxNode("&&");
            var secondSon19 = new SyntaxNode("Eps");
            var firstS19F1 = new SyntaxNode("BB");
            var firstS19F2 = new SyntaxNode("AA'");

            root19.Sons.Add(firstSon19);
            root19.Sons.Add(secondSon19);
            firstSon19.Sons.Add(firstS19F1);
            firstS19F1.Sons.Add(firstS19F2);

            SyntaxTreesDic.Add("AA'", root19);


        }
    }
}
