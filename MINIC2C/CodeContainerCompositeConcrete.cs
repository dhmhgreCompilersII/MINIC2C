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
        public CCFunctionDefinition(CComboContainer parent) : base(CodeBlockType.CB_FUNCTIONDEFINITION,parent) {
        }

        public override CodeContainer AssemblyCodeContainer() {
            throw new NotImplementedException();
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

    public class CCFile : CComboContainer {
        public CCFile() : base(CodeBlockType.CB_FILE,null) {
        }

        public override CodeContainer AssemblyCodeContainer() {
            CodeContainer rep = new CodeContainer(CodeBlockType.CB_NA,null);

            return rep;
        }

        public override void PrintStructure(StreamWriter m_ostream) {

            m_ostream.WriteLine("digraph {");

            ExtractSubgraphs(m_ostream, CodeContextType.CC_FILE_GLOBALVARS);
            ExtractSubgraphs(m_ostream, CodeContextType.CC_FILE_PREPROCESSOR);
            ExtractSubgraphs(m_ostream, CodeContextType.CC_FUNCTIONDEFINITION_DECLARATIONS);

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
            start.Arguments = "-Tgif FileStructure.dot" + " -o" + "FileStructure.gif";
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
