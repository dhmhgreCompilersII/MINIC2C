using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_C
{
    public class CCFunctionDefinition : CComboContainer {
        private HashSet<string> m_localSymbolTable = new HashSet<string>();

        public CEmmitableCodeContainer GetHeader() {
            return GetChild(CodeContextType.CC_FUNCTIONDEFINITION_HEADER);
        }

        public CEmmitableCodeContainer GetDeclarations() {
            return GetChild(CodeContextType.CC_FUNCTIONDEFINITION_DECLARATIONS);
        }

        public CEmmitableCodeContainer GetBody() {
            return GetChild(CodeContextType.CC_FUNCTIONDEFINITION_BODY);
        }

        public virtual void DeclareVariable(string varname) {
            if (!m_localSymbolTable.Contains(varname)) {
                m_localSymbolTable.Add(varname);
                GetDeclarations().AddCode("float "+varname+";\n");
            }
        }
        
        public CCFunctionDefinition(CComboContainer parent) : base(CodeBlockType.CB_FUNCTIONDEFINITION,parent,3) {

            CodeContainer body = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY, this);
            AddCode(body, CodeContextType.CC_FUNCTIONDEFINITION_BODY);
            CodeContainer declarations = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY, this);
            AddCode(declarations, CodeContextType.CC_FUNCTIONDEFINITION_DECLARATIONS);
            CodeContainer header = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY, this);
            AddCode(header, CodeContextType.CC_FUNCTIONDEFINITION_HEADER);
        }

        public override CodeContainer AssemblyCodeContainer() {
            CodeContainer rep =new CodeContainer(CodeBlockType.CB_NA,this);
            // 1. Emmit Header
            rep.AddCode(GetChild(CodeContextType.CC_FUNCTIONDEFINITION_HEADER,0).AssemblyCodeContainer());
            // 2. Emmit {
            rep.AddCode("{");
            rep.EnterScope();
            
            // 3. Emmit Declarations
            rep.AddCode(GetChild(CodeContextType.CC_FUNCTIONDEFINITION_DECLARATIONS,0).AssemblyCodeContainer());
            rep.AddNewLine();
            // 4. Emmit Code Body
            rep.AddCode(GetChild(CodeContextType.CC_FUNCTIONDEFINITION_BODY, 0).AssemblyCodeContainer());
            rep.AddNewLine();

            // 5. Emmit }
            rep.LeaveScope();
            rep.AddCode("}");
            return rep;
        }

        public override void PrintStructure(StreamWriter m_ostream) {
            ExtractSubgraphs(m_ostream, CodeContextType.CC_FUNCTIONDEFINITION_DECLARATIONS);
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
            string mainheader = "void main(int argc, char* argv[])";
            GetHeader().AddCode(mainheader);
        }

        public override void DeclareVariable(string varname) {
            CCFile file = M_Parent as CCFile;
            file.DeclareGlobalVariable(varname);
        }
    }

    public class CCFile : CComboContainer {
        private HashSet<string> m_globalVarSymbolTable = new HashSet<string>();
        private HashSet<string> m_FunctionsSymbolTable = new HashSet<string>();

        CEmmitableCodeContainer GetPreprocessorDefs() {
            return GetChild(CodeContextType.CC_FILE_PREPROCESSOR);
        }

        CEmmitableCodeContainer GetGlobalsVarDeclarations() {
            return GetChild(CodeContextType.CC_FILE_GLOBALVARS);
        }

        CEmmitableCodeContainer[] GetFunDefs() {
            return m_repository[GetContextIndex(CodeContextType.CC_FILE_FUNDEF)].ToArray();
        }

        public void DeclareGlobalVariable(string varname) {
            if (!m_globalVarSymbolTable.Contains(varname)) {
                m_globalVarSymbolTable.Add(varname);
                GetGlobalsVarDeclarations().AddCode("float " + varname + ";\n");
            }
        }

        public void DeclareFunction(string funname,string funheader) {
            if (!m_FunctionsSymbolTable.Contains(funname)) {
                m_globalVarSymbolTable.Add(funname);
                GetGlobalsVarDeclarations().AddCode(funheader+";\n");
            }
        }

        public CCFile() : base(CodeBlockType.CB_FILE,null,3) {

            CodeContainer prepro = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY,this);
            AddCode(prepro,CodeContextType.CC_FILE_PREPROCESSOR);
            CodeContainer globals =new CodeContainer(CodeBlockType.CB_CODEREPOSITORY,this);
            AddCode(globals,CodeContextType.CC_FILE_GLOBALVARS);
        }

        public override CodeContainer AssemblyCodeContainer() {
            CodeContainer rep = new CodeContainer(CodeBlockType.CB_NA,null);

            rep.AddCode(GetPreprocessorDefs().AssemblyCodeContainer());
            rep.AddCode(GetGlobalsVarDeclarations().AssemblyCodeContainer());
            foreach (CEmmitableCodeContainer codeContainer in GetFunDefs()) {
                rep.AddCode(codeContainer.AssemblyCodeContainer());
            }
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
