using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_C
{
    public class CReturnStatement : CComboContainer {
        public CReturnStatement(CEmmitableCodeContainer parent) : base(CodeBlockType.CB_RETURNSTATEMENT, parent, 1) {
        }

        public override CodeContainer AssemblyCodeContainer() {
            CodeContainer rep = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY, M_Parent);
            rep.AddCode("return ");
            rep.AddCode(AssemblyContext(CodeContextType.CC_RETURNSTATEMENT_BODY));
            rep.AddCode(";");
            rep.AddNewLine();
            return rep;
        }

        public override void PrintStructure(StreamWriter m_ostream) {
            ExtractSubgraphs(m_ostream, CodeContextType.CC_RETURNSTATEMENT_BODY);
            
            foreach (List<CEmmitableCodeContainer> cEmmitableCodeContainers in m_repository) {
                foreach (CEmmitableCodeContainer codeContainer in cEmmitableCodeContainers) {
                    codeContainer.PrintStructure(m_ostream);
                }
            }
            m_ostream.WriteLine("\"{0}\"->\"{1}\"", M_Parent.M_NodeName, M_NodeName);
        }
    }

    public class CIfStatement : CComboContainer
    {
        public CIfStatement(CEmmitableCodeContainer parent) : base(CodeBlockType.CB_IFSTATEMENT, parent, 3) {
        }

        public override CodeContainer AssemblyCodeContainer()
        {
            CodeContainer rep = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY, M_Parent);
            rep.AddCode("if ( ");
            rep.AddCode(AssemblyContext(CodeContextType.CC_IFSTATEMENT_CONDITION));
            rep.AddCode(" )");
            rep.AddCode(AssemblyContext(CodeContextType.CC_IFSTATEMENT_IFBODY));
            if (GetContextChildren(CodeContextType.CC_IFSTATEMENT_ELSEBODY).Length != 0)
            {
                rep.AddCode("else");
                rep.AddCode(AssemblyContext(CodeContextType.CC_IFSTATEMENT_ELSEBODY));
            }

            return rep;
        }

        public override void PrintStructure(StreamWriter m_ostream)
        {
            ExtractSubgraphs(m_ostream, CodeContextType.CC_IFSTATEMENT_CONDITION);
            ExtractSubgraphs(m_ostream, CodeContextType.CC_IFSTATEMENT_IFBODY);
            ExtractSubgraphs(m_ostream, CodeContextType.CC_IFSTATEMENT_ELSEBODY);

            foreach (List<CEmmitableCodeContainer> cEmmitableCodeContainers in m_repository)
            {
                foreach (CEmmitableCodeContainer codeContainer in cEmmitableCodeContainers)
                {
                    codeContainer.PrintStructure(m_ostream);
                }
            }
            m_ostream.WriteLine("\"{0}\"->\"{1}\"", M_Parent.M_NodeName, M_NodeName);
        }
    }
    public class CExpressionStatement : CComboContainer
    {
        public CExpressionStatement(CEmmitableCodeContainer parent) : base(CodeBlockType.CB_EXPRESSIONSTATEMENT, parent, 1) {
        }

        public override CodeContainer AssemblyCodeContainer()
        {
            CodeContainer rep = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY, M_Parent);
            rep.AddCode("printf(\"res=%f\\n\",");
            rep.AddCode(AssemblyContext(CodeContextType.CC_EXPRESSIONSTATEMENT_BODY));
            rep.AddCode(");");
            rep.AddNewLine();
            return rep;
        }

        public override void PrintStructure(StreamWriter m_ostream)
        {
            ExtractSubgraphs(m_ostream, CodeContextType.CC_EXPRESSIONSTATEMENT_BODY);

            foreach (List<CEmmitableCodeContainer> cEmmitableCodeContainers in m_repository)
            {
                foreach (CEmmitableCodeContainer codeContainer in cEmmitableCodeContainers)
                {
                    codeContainer.PrintStructure(m_ostream);
                }
            }
            m_ostream.WriteLine("\"{0}\"->\"{1}\"", M_Parent.M_NodeName, M_NodeName);
        }
    }
    public class CCompoundStatement : CComboContainer {
        public CCompoundStatement(CEmmitableCodeContainer parent) : base(CodeBlockType.CB_COMPOUNDSTATEMENT, parent, 2) {
        }

        public override CodeContainer AssemblyCodeContainer() {
            CodeContainer rep = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY, M_Parent);
            rep.AddCode("{");
            rep.EnterScope();
            rep.AddCode("//  ***** Local declarations *****");
            rep.AddNewLine();
            rep.AddCode(AssemblyContext(CodeContextType.CC_COMPOUNDSTATEMENT_DECLARATIONS));
            rep.AddCode("//  ***** Code Body *****");
            rep.AddNewLine();
            rep.AddCode(AssemblyContext(CodeContextType.CC_COMPOUNDSTATEMENT_BODY));
            rep.LeaveScope();
            rep.AddCode("}");
            return rep;
        }

        public override void PrintStructure(StreamWriter m_ostream) {
            ExtractSubgraphs(m_ostream, CodeContextType.CC_COMPOUNDSTATEMENT_BODY);
            ExtractSubgraphs(m_ostream, CodeContextType.CC_COMPOUNDSTATEMENT_DECLARATIONS);
            foreach (List<CEmmitableCodeContainer> cEmmitableCodeContainers in m_repository)
            {
                foreach (CEmmitableCodeContainer codeContainer in cEmmitableCodeContainers)
                {
                    codeContainer.PrintStructure(m_ostream);
                }
            }
            m_ostream.WriteLine("\"{0}\"->\"{1}\"", M_Parent.M_NodeName, M_NodeName);
        }
    }

    public class CWhileStatement : CComboContainer {
        public CWhileStatement(CEmmitableCodeContainer parent) : base(CodeBlockType.CB_WHILESTATEMENT, parent, 2) {
        }

        public override CodeContainer AssemblyCodeContainer() {
            CodeContainer rep =new CodeContainer(CodeBlockType.CB_CODEREPOSITORY, M_Parent);
            rep.AddCode("while ( ");
            rep.AddCode(AssemblyContext(CodeContextType.CC_WHILESTATEMENT_CONDITION));
            rep.AddCode(" )");
            rep.AddCode(AssemblyContext(CodeContextType.CC_WHILESTATEMENT_BODY));
            return rep;
        }

        public override void PrintStructure(StreamWriter m_ostream) {
            ExtractSubgraphs(m_ostream, CodeContextType.CC_WHILESTATEMENT_CONDITION);
            ExtractSubgraphs(m_ostream, CodeContextType.CC_WHILESTATEMENT_BODY);

            foreach (List<CEmmitableCodeContainer> cEmmitableCodeContainers in m_repository) {
                foreach (CEmmitableCodeContainer codeContainer in cEmmitableCodeContainers) {
                    codeContainer.PrintStructure(m_ostream);
                }
            }
            m_ostream.WriteLine("\"{0}\"->\"{1}\"", M_Parent.M_NodeName, M_NodeName);
        }
    }

    public class CCFunctionDefinition : CComboContainer {
        private HashSet<string> m_localSymbolTable = new HashSet<string>();

        
        public virtual void DeclareVariable(string varname) {
            CodeContainer rep;
            if (!m_localSymbolTable.Contains(varname)) {
                rep = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY, this);
                m_localSymbolTable.Add(varname);

                rep.AddCode("float "+varname+";\n",CodeContextType.CC_NA);
                CEmmitableCodeContainer compoundst = GetChild(CodeContextType.CC_FUNCTIONDEFINITION_BODY);
                compoundst.AddCode(rep, CodeContextType.CC_COMPOUNDSTATEMENT_DECLARATIONS);
            }
        }

        public void AddVariableToLocalSymbolTable(string varname) {
            if (!m_localSymbolTable.Contains(varname)) {
                m_localSymbolTable.Add(varname);
            }
        }

        public CCFunctionDefinition(CEmmitableCodeContainer parent) : base(CodeBlockType.CB_FUNCTIONDEFINITION,parent,2) {
        }

        public override CodeContainer AssemblyCodeContainer() {
            CodeContainer rep =new CodeContainer(CodeBlockType.CB_NA,M_Parent);
            // 1. Emmit Header
            rep.AddCode(AssemblyContext(CodeContextType.CC_FUNCTIONDEFINITION_HEADER));
            
           // 4. Emmit Code Body
            rep.AddCode(AssemblyContext(CodeContextType.CC_FUNCTIONDEFINITION_BODY));
            rep.AddNewLine();

            return rep;
        }

        public override void PrintStructure(StreamWriter m_ostream) {
            ExtractSubgraphs(m_ostream,CodeContextType.CC_FUNCTIONDEFINITION_BODY);
            ExtractSubgraphs(m_ostream,CodeContextType.CC_FUNCTIONDEFINITION_HEADER);

            foreach (List<CEmmitableCodeContainer> cEmmitableCodeContainers in m_repository) {
                foreach (CEmmitableCodeContainer codeContainer in cEmmitableCodeContainers) {
                    codeContainer.PrintStructure(m_ostream);
                }
            }
            m_ostream.WriteLine("\"{0}\"->\"{1}\"", M_Parent.M_NodeName, M_NodeName);
        }
    }

    public class CMainFunctionDefinition : CCFunctionDefinition {
        public CMainFunctionDefinition(CComboContainer parent) : base(parent) {
            CCompoundStatement cmpst = new CCompoundStatement(this);
            string mainheader = "void main(int argc, char* argv[])";
            AddCode(mainheader,CodeContextType.CC_FUNCTIONDEFINITION_HEADER);
            AddCode(cmpst,CodeContextType.CC_FUNCTIONDEFINITION_BODY);
        }

        public override void DeclareVariable(string varname) {
            CCFile file = M_Parent as CCFile;
            file.DeclareGlobalVariable(varname);
        }
    }

    public class CCFile : CComboContainer {
        private HashSet<string> m_globalVarSymbolTable = new HashSet<string>();
        private HashSet<string> m_FunctionsSymbolTable = new HashSet<string>();

        private CCFunctionDefinition m_mainDefinition=null;
        public CCFunctionDefinition MainDefinition => m_mainDefinition;

        public void DeclareGlobalVariable(string varname) {
            CodeContainer rep;
            if (!m_globalVarSymbolTable.Contains(varname)) {
                m_globalVarSymbolTable.Add(varname);
                rep = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY,this);
                rep.AddCode("float " + varname + ";\n",CodeContextType.CC_FILE_GLOBALVARS);
                AddCode(rep,CodeContextType.CC_FILE_GLOBALVARS);
            }
        }

        public void DeclareFunction(string funname,string funheader) {
            CodeContainer rep;
            if (!m_FunctionsSymbolTable.Contains(funname)) {
                rep = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY, this);
                m_globalVarSymbolTable.Add(funname);
                rep.AddCode(funheader+";\n",CodeContextType.CC_FILE_GLOBALVARS);
                AddCode(rep, CodeContextType.CC_FILE_GLOBALVARS);
            }
        }

        public CCFile(bool withStartUpFunction) : base(CodeBlockType.CB_FILE,null,3) {

            if (withStartUpFunction) {
                m_mainDefinition= new CMainFunctionDefinition(this);
                AddCode(m_mainDefinition, CodeContextType.CC_FILE_FUNDEF);
            }
        }

        public override CodeContainer AssemblyCodeContainer() {
            CodeContainer rep = new CodeContainer(CodeBlockType.CB_NA,null);

            rep.AddCode(AssemblyContext(CodeContextType.CC_FILE_PREPROCESSOR));
            rep.AddCode(AssemblyContext(CodeContextType.CC_FILE_GLOBALVARS));
            rep.AddCode(AssemblyContext(CodeContextType.CC_FILE_FUNDEF));
            return rep;
        }

        public override void PrintStructure(StreamWriter m_ostream) {

            m_ostream.WriteLine("digraph {");

            ExtractSubgraphs(m_ostream, CodeContextType.CC_FILE_GLOBALVARS);
            ExtractSubgraphs(m_ostream, CodeContextType.CC_FILE_PREPROCESSOR);
            ExtractSubgraphs(m_ostream, CodeContextType.CC_FILE_FUNDEF);

            foreach (List<CEmmitableCodeContainer> cEmmitableCodeContainers in m_repository) {
                foreach (CEmmitableCodeContainer codeContainer in cEmmitableCodeContainers) {
                    codeContainer.PrintStructure(m_ostream);
                }
            }
            m_ostream.WriteLine("}");
            m_ostream.Close();

            // Prepare the process to run
            ProcessStartInfo start = new ProcessStartInfo();
            // Enter in the command line arguments, everything you would enter after the executable name itself
            start.Arguments = "-Tgif CodeStructure.dot " + " -o" + " CodeStructure.gif";
            // Enter the executable to run, including the complete path
            start.FileName = "dot";
            // Do you want to show a console window?
            start.WindowStyle = ProcessWindowStyle.Hidden;
            start.CreateNoWindow = true;
            int exitCode;
            
            // Run the external process & wait for it to finish
            using (Process proc = Process.Start(start)) {
                proc.WaitForExit();

                // Retrieve the app's exit code
                exitCode = proc.ExitCode;
            }
        }
    }
}
