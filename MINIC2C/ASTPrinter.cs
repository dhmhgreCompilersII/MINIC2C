using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_C
{
    class ASTPrinter : ASTBaseVisitor<int>
    {
        private static int m_clusterSerial = 0;
        private StreamWriter m_ostream;
        private string m_dotName;

        // Constructor
        public ASTPrinter(string dotFileName)
        {
            m_ostream = new StreamWriter(dotFileName);
            m_dotName = dotFileName;
        }
         /// ###########################################################################|
        /// ExtractSubgraphs ##########################################################|
        /// ###########################################################################|
        /// Αυτή η μέδοδος χωρίζει τα παιδιά σε περιοχές, ανάλογα με τον γωνέα τους ###|
        /// ###########################################################################|
        private void ExtractSubgraphs(ASTComposite node, contextType context)
        {
            if (node.MChildren[node.GetContextIndex(context)].Count != 0)
            {
                m_ostream.WriteLine("\tsubgraph cluster" + m_clusterSerial++ + "{");
                m_ostream.WriteLine("\t\tnode [style=filled,color=white];");
                m_ostream.WriteLine("\t\tstyle=filled;");
                m_ostream.WriteLine("\t\tcolor=lightgrey;");
                m_ostream.Write("\t\t");
                for (int i = 0; i < node.MChildren[node.GetContextIndex(context)].Count; i++)
                {
                    m_ostream.Write(node.MChildren[node.GetContextIndex(context)][i].MNodeName + ";");
                }

                m_ostream.WriteLine("\n\t\tlabel=" + context + ";");
                m_ostream.WriteLine("\t}");
            }
        }

        public override int VisitCOMPILEUNIT(CASTCompileUnit node)
        {

            m_ostream.WriteLine("digraph {");

            ExtractSubgraphs(node, contextType.CT_COMPILEUNIT_MAINBODY);
            ExtractSubgraphs(node, contextType.CT_COMPILEUNIT_FUNCTIONDEFINITIONS);

            base.VisitCOMPILEUNIT(node);

            m_ostream.WriteLine("}");
            m_ostream.Close();

            // Prepare the process to run
            ProcessStartInfo start = new ProcessStartInfo();
            // Enter in the command line arguments, everything you would enter after the executable name itself
            start.Arguments = "-Tgif " + m_dotName + " -o" + m_dotName + ".gif";
            // Enter the executable to run, including the complete path
            start.FileName = "dot";
            // Do you want to show a console window?
            start.WindowStyle = ProcessWindowStyle.Hidden;
            start.CreateNoWindow = true;
            int exitCode;


            // Run the external process & wait for it to finish
            using (Process proc = Process.Start(start))
            {
                proc.WaitForExit();

                // Retrieve the app's exit code
                exitCode = proc.ExitCode;
            }

            return 0;
        }

        public override int VisitIDENTIFIER(CASTIDENTIFIER node)
        {
            m_ostream.WriteLine("{0}->{1}", node.MParent.MNodeName, node.MNodeName);
            return base.VisitIDENTIFIER(node);
        }

        public override int VisitNUMBER(CASTNUMBER node)
        {
            m_ostream.WriteLine("{0}->{1}", node.MParent.MNodeName, node.MNodeName);
            return base.VisitNUMBER(node);
        }

        public override int VisitAddition(CASTExpressionAddition node)
        {

            ExtractSubgraphs(node, contextType.CT_EXPRESSION_ADDITION_LEFT);
            ExtractSubgraphs(node, contextType.CT_EXPRESSION_ADDITION_RIGHT);

            base.VisitAddition(node);

            m_ostream.WriteLine("{0}->{1}", node.MParent.MNodeName, node.MNodeName);

            return 0;
        }

        public override int VisitSubtraction(CASTExpressionSubtraction node)
        {
            ExtractSubgraphs(node, contextType.CT_EXPRESSION_SUBTRACTION_LEFT);
            ExtractSubgraphs(node, contextType.CT_EXPRESSION_SUBTRACTION_RIGHT);

            base.VisitSubtraction(node);

            m_ostream.WriteLine("{0}->{1}", node.MParent.MNodeName, node.MNodeName);

            return 0;
        }

        public override int VisitMultiplication(CASTExpressionMultiplication node)
        {
            ExtractSubgraphs(node, contextType.CT_EXPRESSION_MULTIPLICATION_LEFT);
            ExtractSubgraphs(node, contextType.CT_EXPRESSION_MULTIPLICATION_RIGHT);

            base.VisitMultiplication(node);

            m_ostream.WriteLine("{0}->{1}", node.MParent.MNodeName, node.MNodeName);

            return 0;
        }

        public override int VisitDivision(CASTExpressionDivision node)
        {
            ExtractSubgraphs(node, contextType.CT_EXPRESSION_DIVISION_LEFT);
            ExtractSubgraphs(node, contextType.CT_EXPRESSION_DIVISION_RIGHT);

            base.VisitDivision(node);

            m_ostream.WriteLine("{0}->{1}", node.MParent.MNodeName, node.MNodeName);

            return 0;
        }

        public override int VisitASSIGN(CASTExpressionAssign node)
        {
            ExtractSubgraphs(node, contextType.CT_EXPRESSION_ASSIGN_LVALUE);
            ExtractSubgraphs(node, contextType.CT_EXPRESSION_ASSIGN_EXPRESSION);

            base.VisitASSIGN(node);

            m_ostream.WriteLine("{0}->{1}", node.MParent.MNodeName, node.MNodeName);

            return 0;
        }
        public override int VisitSTATEMENT(CASTStatement node)
        {

            ExtractSubgraphs(node, contextType.CT_STATEMENT);

            base.VisitSTATEMENT(node);
            m_ostream.WriteLine("{0}->{1}", node.MParent.MNodeName, node.MNodeName);
            return 0; 
        }

        public override int VisitFunctionDefinition(CASTFunctionDefinition node)
        {
            ExtractSubgraphs(node, contextType.CT_FUNCTION_IDENTIFIER);
            ExtractSubgraphs(node, contextType.CT_FUNCTION_FARGS);
            ExtractSubgraphs(node, contextType.CT_FUNCTION_BODY);
            base.VisitFunctionDefinition(node);

            m_ostream.WriteLine("{0}->{1}", node.MParent.MNodeName, node.MNodeName);

            return 0;
        }
        public override int VisitSTATEMENTLIST(CASTStatementList node)
        {
            ExtractSubgraphs(node, contextType.CT_STATEMENTLIST);
            base.VisitSTATEMENTLIST(node);
            m_ostream.WriteLine("{0}->{1}", node.MParent.MNodeName, node.MNodeName);
            return 0;
        }

        public override int VisitWHILESTATEMENT(CASTWhileStatement node)
        {
            ExtractSubgraphs(node, contextType.CT_WHILESTATEMENT_CONDITION);
            ExtractSubgraphs(node, contextType.CT_WHILESTATEMENT_BODY);
            base.VisitWHILESTATEMENT(node);
            m_ostream.WriteLine("{0}->{1}", node.MParent.MNodeName, node.MNodeName);
            return 0;
        }

        public override int VisitPLUS(CASTExpressionPlus node)
        {
            ExtractSubgraphs(node, contextType.CT_EXPRESSION_PLUS);
            base.VisitPLUS(node);
            m_ostream.WriteLine("{0}->{1}", node.MParent.MNodeName, node.MNodeName);
            return 0;
        }

        public override int VisitMINUS(CASTExpressionMinus node)
        {
            ExtractSubgraphs(node, contextType.CT_EXPRESSION_MINUS);
            base.VisitMINUS(node);
            m_ostream.WriteLine("{0}->{1}", node.MParent.MNodeName, node.MNodeName);
            return 0;
        }
        

        public override int VisitNOT(CASTExpressionNot node)
        {
            ExtractSubgraphs(node, contextType.CT_EXPRESSION_NOT);
            base.VisitNOT(node);
            m_ostream.WriteLine("{0}->{1}", node.MParent.MNodeName, node.MNodeName);
            return 0;
        }

        public override int VisitAND(CASTExpressionAnd node)
        {
            ExtractSubgraphs(node, contextType.CT_EXPRESSION_AND_LEFT);
            ExtractSubgraphs(node, contextType.CT_EXPRESSION_AND_RIGHT);
            base.VisitAND(node);
            m_ostream.WriteLine("{0}->{1}", node.MParent.MNodeName, node.MNodeName);
            return 0;
        }

        public override int VisitOR(CASTExpressionOr node)
        {
            ExtractSubgraphs(node, contextType.CT_EXPRESSION_OR_LEFT);
            ExtractSubgraphs(node, contextType.CT_EXPRESSION_OR_RIGHT);
            base.VisitOR(node);
            m_ostream.WriteLine("{0}->{1}", node.MParent.MNodeName, node.MNodeName);
            return 0;
        }

        public override int VisitGT(CASTExpressionGt node)
        {
            ExtractSubgraphs(node, contextType.CT_EXPRESSION_GT_LEFT);
            ExtractSubgraphs(node, contextType.CT_EXPRESSION_GT_RIGHT);
            base.VisitGT(node);
            m_ostream.WriteLine("{0}->{1}", node.MParent.MNodeName, node.MNodeName);
            return 0;
        }

        public override int VisitGTE(CASTExpressionGte node)
        {
            ExtractSubgraphs(node, contextType.CT_EXPRESSION_GTE_LEFT);
            ExtractSubgraphs(node, contextType.CT_EXPRESSION_GTE_RIGHT);
            base.VisitGTE(node);
            m_ostream.WriteLine("{0}->{1}", node.MParent.MNodeName, node.MNodeName);
            return 0;
        }

        public override int VisitLT(CASTExpressionLt node)
        {
            ExtractSubgraphs(node, contextType.CT_EXPRESSION_LT_LEFT);
            ExtractSubgraphs(node, contextType.CT_EXPRESSION_LT_RIGHT);
            base.VisitLT(node);
            m_ostream.WriteLine("{0}->{1}", node.MParent.MNodeName, node.MNodeName);
            return 0;
        }

        public override int VisitLTE(CASTExpressionLte node)
        {

            ExtractSubgraphs(node, contextType.CT_EXPRESSION_LTE_LEFT);
            ExtractSubgraphs(node, contextType.CT_EXPRESSION_LTE_RIGHT);
            base.VisitLTE(node);
            m_ostream.WriteLine("{0}->{1}", node.MParent.MNodeName, node.MNodeName);
            return 0;
        }

        public override int VisitEQUAL(CASTExpressionEqual node)
        {
            ExtractSubgraphs(node, contextType.CT_EXPRESSION_EQUAL_LEFT);
            ExtractSubgraphs(node, contextType.CT_EXPRESSION_EQUAL_RIGHT);
            base.VisitEQUAL(node);
            m_ostream.WriteLine("{0}->{1}", node.MParent.MNodeName, node.MNodeName);
            return 0;
        }

        public override int VisitNEQUAL(CASTExpressionNequal node)
        {
            ExtractSubgraphs(node, contextType.CT_EXPRESSION_NEQUAL_LEFT);
            ExtractSubgraphs(node, contextType.CT_EXPRESSION_NEQUAL_RIGHT);
            base.VisitNEQUAL(node);
            m_ostream.WriteLine("{0}->{1}", node.MParent.MNodeName, node.MNodeName);
            return 0;
        }
        public override int VisitCOMPOUNDSTATEMENT(CASTCompoundStatement node)
        {
            ExtractSubgraphs(node, contextType.CT_COMPOUNDSTATEMENT);
            base.VisitCOMPOUNDSTATEMENT(node);
            m_ostream.WriteLine("{0}->{1}", node.MParent.MNodeName, node.MNodeName);
            return 0;
        }

        public override int VisitIFSTATEMENT(CASTIfStatement node)
        {
            ExtractSubgraphs(node, contextType.CT_IFSTATEMENT_CONDITION);
            ExtractSubgraphs(node, contextType.CT_IFSTATEMENT_IFCLAUSE);
            ExtractSubgraphs(node, contextType.CT_IFSTATEMENT_ELSECLAUSE);
            base.VisitIFSTATEMENT(node);
            m_ostream.WriteLine("{0}->{1}", node.MParent.MNodeName, node.MNodeName);
            return 0;
        }
    }
}