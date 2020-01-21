using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System.IO;
using Mini_C;

namespace MINIC2C{
    class Program
    {
        static void Main(string[] args) {

            StreamReader astream = new StreamReader(args[0]);

            AntlrInputStream antlrStream = new AntlrInputStream(astream);

            MINICLexer lexer = new MINICLexer(antlrStream);

            CommonTokenStream tokens = new CommonTokenStream(lexer);

            MINICParser parser = new MINICParser(tokens);

            IParseTree tree = parser.compileUnit();

            Console.WriteLine(tree.ToStringTree());

            STPrinter ptPrinter = new STPrinter();
            ptPrinter.Visit(tree);

            ASTGenerator astGenerator = new ASTGenerator();
            astGenerator.Visit(tree);

            ASTPrinter astPrinter = new ASTPrinter("test.ast.dot");
            astPrinter.Visit(astGenerator.M_Root);

            MINIC2CTranslation tr = new MINIC2CTranslation();
            tr.VisitCOMPILEUNIT(astGenerator.M_Root as CASTCompileUnit, new TranslationParameters());
            tr.M_TranslatedFile.EmmitStdout();
            StreamWriter trFile = new StreamWriter(Path.GetFileName(args[0]+".c"));
            tr.M_TranslatedFile.EmmitToFile(trFile);
            trFile.Close();
            StreamWriter m_streamWriter =new StreamWriter("CodeStructure.dot");
            tr.M_TranslatedFile.PrintStructure(m_streamWriter);


            
        }
    }
}
