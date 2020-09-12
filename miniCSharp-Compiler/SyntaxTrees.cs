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



            //-------------------------------------------------


            var root20 = new SyntaxNode("BB");
            var firstSon20 = new SyntaxNode("CC");
            var firstS20F1 = new SyntaxNode("BB'");

            root20.Sons.Add(firstSon20);
            firstSon20.Sons.Add(firstS20F1);
            SyntaxTreesDic.Add("BB", root20);


            //-------------------------------------------------


            var root21 = new SyntaxNode("BB'");
            var firstSon21 = new SyntaxNode("!=");
            var secondSon21 = new SyntaxNode("==");
            var thirdSon21 = new SyntaxNode("Eps");
            var firstS21F1 = new SyntaxNode("CC");
            var firstS21F2 = new SyntaxNode("BB'");
            var secondS21F1 = new SyntaxNode("CC");
            var secondS21F2 = new SyntaxNode("BB'");

            root21.Sons.Add(firstSon21);
            root21.Sons.Add(secondSon21);
            root21.Sons.Add(thirdSon21);
            secondSon21.Sons.Add(firstS21F1);
            firstS21F1.Sons.Add(firstS21F2);
            thirdSon21.Sons.Add(secondS21F1);
            secondS21F1.Sons.Add(secondS21F2);

            SyntaxTreesDic.Add("BB'", root21);


            //-------------------------------------------------


            var root22 = new SyntaxNode("CC");
            var firstSon22 = new SyntaxNode("DD");
            var firstS22F1 = new SyntaxNode("CC'");

            root22.Sons.Add(firstSon22);
            firstSon22.Sons.Add(firstS22F1);

            SyntaxTreesDic.Add("CC", root22);


            //-------------------------------------------------


            var root23 = new SyntaxNode("CC'");
            var firstSon23 = new SyntaxNode(">=");
            var secondSon23 = new SyntaxNode(">");
            var thirdSon23 = new SyntaxNode("<=");
            var fourthSon23 = new SyntaxNode("<");
            var fifthSon23 = new SyntaxNode("Eps");
            var firstS23F1 = new SyntaxNode("DD");
            var firstS23F2 = new SyntaxNode("CC'");
            var secondS23F1 = new SyntaxNode("DD");
            var secondS23F2 = new SyntaxNode("CC'");
            var thirdS23F1 = new SyntaxNode("DD");
            var thirdS23F2 = new SyntaxNode("CC'");
            var fourthS23F1 = new SyntaxNode("DD");
            var fourthS23F2 = new SyntaxNode("CC'");


            root23.Sons.Add(firstSon23);
            root23.Sons.Add(secondSon23);
            root23.Sons.Add(thirdSon23);
            root23.Sons.Add(fourthSon23);
            root23.Sons.Add(fifthSon23);

            firstSon23.Sons.Add(firstS23F1);
            firstS23F1.Sons.Add(firstS23F2);
            secondSon23.Sons.Add(secondS23F1);
            secondS23F1.Sons.Add(secondS23F2);
            thirdSon23.Sons.Add(thirdS23F1);
            thirdS23F1.Sons.Add(thirdS23F2);
            fourthSon23.Sons.Add(fourthS23F1);
            fourthS23F1.Sons.Add(fourthS23F2);

            SyntaxTreesDic.Add("CC'", root23);


            //-------------------------------------------------


            var root24 = new SyntaxNode("DD");
            var firstSon24 = new SyntaxNode("EE");
            var firstS24F1 = new SyntaxNode("DD'");

            root24.Sons.Add(firstSon24);
            firstSon24.Sons.Add(firstS24F1);

            SyntaxTreesDic.Add("DD", root24);


            //-------------------------------------------------


            var root25 = new SyntaxNode("DD'");
            var firstSon25 = new SyntaxNode("+");
            var secondSon25 = new SyntaxNode("-");
            var firstS25F1 = new SyntaxNode("EE");
            var firstS25F2 = new SyntaxNode("DD'");
            var thirdSon25 = new SyntaxNode("Eps");
            var secondS25F1 = new SyntaxNode("EE");
            var secondS25F2 = new SyntaxNode("DD'");

            root25.Sons.Add(firstSon25);
            root25.Sons.Add(secondSon25);
            root25.Sons.Add(thirdSon25);
            firstSon25.Sons.Add(firstS25F1);
            firstS25F1.Sons.Add(firstS25F2);
            secondSon25.Sons.Add(secondS25F1);
            secondS25F1.Sons.Add(secondS25F2);

            SyntaxTreesDic.Add("DD'", root25);


            //-------------------------------------------------


            var root26 = new SyntaxNode("EE");
            var firstSon26 = new SyntaxNode("FF");
            var firstS26F1 = new SyntaxNode("EE'");

            root26.Sons.Add(firstSon26);
            firstSon26.Sons.Add(firstS26F1);
            SyntaxTreesDic.Add("EE", root26);


            //-------------------------------------------------

            var root27 = new SyntaxNode("EE'");
            var firstSon27 = new SyntaxNode("*");
            var secondSon27 = new SyntaxNode("/");
            var thirdSon27 = new SyntaxNode("%");
            var fourthSon27 = new SyntaxNode("Eps");
            var firstS27F1 = new SyntaxNode("FF");
            var firstS27F2 = new SyntaxNode("EE'");
            var secondS27F1 = new SyntaxNode("FF");
            var secondS27F2 = new SyntaxNode("EE'");
            var thirdS27F1 = new SyntaxNode("FF");
            var thirdS27F2 = new SyntaxNode("EE'");


            root27.Sons.Add(firstSon27);
            root27.Sons.Add(secondSon27);
            root27.Sons.Add(thirdSon27);
            root27.Sons.Add(fourthSon27);

            firstSon27.Sons.Add(firstS27F1);
            firstS27F1.Sons.Add(firstS27F2);
            secondSon27.Sons.Add(secondS27F1);
            secondS27F1.Sons.Add(secondS27F2);
            thirdSon27.Sons.Add(thirdS27F1);
            thirdS27F1.Sons.Add(thirdS27F2);

            SyntaxTreesDic.Add("EE'", root27);


            //-------------------------------------------------


            var root28 = new SyntaxNode("FF");
            var firstSon28 = new SyntaxNode("!");
            var secondSon28 = new SyntaxNode("-");
            var firstS28F1 = new SyntaxNode("GG");
            var secondS28F1 = new SyntaxNode("GG");
            var thirdSon28 = new SyntaxNode("GG");

            root28.Sons.Add(firstSon28);
            root28.Sons.Add(secondSon28);
            root28.Sons.Add(thirdSon28);
            firstSon28.Sons.Add(firstS28F1);
            secondSon28.Sons.Add(secondS28F1);

            SyntaxTreesDic.Add("FF", root28);


            //-------------------------------------------------


            var root29 = new SyntaxNode("GG");
            var firstSon29 = new SyntaxNode("HH");
            var firstS29F1 = new SyntaxNode("GG'");

            root29.Sons.Add(firstSon29);

            firstSon29.Sons.Add(firstS29F1);

            SyntaxTreesDic.Add("GG", root29);

            //-------------------------------------------------

            var root30 = new SyntaxNode("GG'");
            var firstSon30 = new SyntaxNode(".");
            var secondSon30 = new SyntaxNode("[");
            var thirdSon30 = new SyntaxNode("Eps");

            var firstS30F1 = new SyntaxNode("I");
            var firstS30F2 = new SyntaxNode("GG'");

            var secondS30F1 = new SyntaxNode("Expr");
            var secondS30F2 = new SyntaxNode("]");
            var secondS30F3 = new SyntaxNode("=");
            var secondS30F4 = new SyntaxNode("Expr");
            var secondS30F5 = new SyntaxNode("GG'");

            root30.Sons.Add(firstSon30);
            root30.Sons.Add(secondSon30);
            root30.Sons.Add(thirdSon30);

            firstSon30.Sons.Add(firstS30F1);
            firstS30F1.Sons.Add(firstS30F2);

            secondSon30.Sons.Add(secondS30F1);
            secondS30F1.Sons.Add(secondS30F2);
            secondS30F2.Sons.Add(secondS30F3);
            secondS30F3.Sons.Add(secondS30F4);
            secondS30F4.Sons.Add(secondS30F5);

            SyntaxTreesDic.Add("GG'", root30);
            //-------------------------------------------------

            var root31 = new SyntaxNode("HH");
            var firstSon31 = new SyntaxNode("(");
            var secondSon31 = new SyntaxNode("Constant");
            var thirdSon31 = new SyntaxNode("New");
            var fourthSon31 = new SyntaxNode("this");
            var fifthSon31 = new SyntaxNode("I");

            var firstS31F1 = new SyntaxNode("Expr");
            var firstS31F2 = new SyntaxNode(")");

            var thirdS31F1 = new SyntaxNode("(");
            var thirdS31F2 = new SyntaxNode("I");
            var thirdS31F3 = new SyntaxNode(")");

            root31.Sons.Add(firstSon31);
            root31.Sons.Add(secondSon31);
            root31.Sons.Add(thirdSon31);
            root31.Sons.Add(fourthSon31);
            root31.Sons.Add(fifthSon31);

            firstSon31.Sons.Add(firstS31F1);
            firstS31F1.Sons.Add(firstS31F2);

            thirdSon31.Sons.Add(thirdS31F1);
            thirdS31F1.Sons.Add(thirdS31F2);
            thirdS31F2.Sons.Add(thirdS31F3);

            SyntaxTreesDic.Add("HH", root31);

            //-------------------------------------------------


            //cambiar valores hojas para hacer match con token
            var root32 = new SyntaxNode("Constant");
            var firstSon32 = new SyntaxNode("N");       //N-- > int
            var secondSon32 = new SyntaxNode("D");      //D --> double
            var thirdSon32 = new SyntaxNode("B");       //B --> bool
            var fourthSon32 = new SyntaxNode("S");      //S --> String
            var fifthSon32 = new SyntaxNode("null");
            var sixthSon32 = new SyntaxNode("H");       //H --> int hexadecimal
            var seventhSon32 = new SyntaxNode("X");       //H --> double exponencial

            root32.Sons.Add(firstSon32);
            root32.Sons.Add(secondSon32);
            root32.Sons.Add(thirdSon32);
            root32.Sons.Add(fourthSon32);
            root32.Sons.Add(fifthSon32);
            root32.Sons.Add(sixthSon32);
            root32.Sons.Add(seventhSon32);

            SyntaxTreesDic.Add("Constant", root32);
        }
    }
}
