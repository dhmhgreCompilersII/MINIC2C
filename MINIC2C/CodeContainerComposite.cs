using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_C
{
    public enum CodeBlockType
    {
        CB_NA=-1,
        CB_FILE=0,
        CB_FUNCTIONDEFINITION=3,
        CB_CODEREPOSITORY=6
    };

    public enum CodeContextType
    {
        CC_NA=-1,
        CC_FILE_PREPROCESSOR,
        CC_FILE_GLOBALVARS,
        CC_FILE_FUNDEF,
        CC_FUNCTIONDEFINITION_HEADER,
        CC_FUNCTIONDEFINITION_DECLARATIONS,
        CC_FUNCTIONDEFINITION_BODY
    };
    public abstract class CEmmitableCodeContainer
    {
        private CodeBlockType m_nodeType;
        private int m_serialNumber;
        private static int m_serialNumberCounter=0;
        private string m_nodeName;
        private CComboContainer m_parent;
        protected int m_nestingLevel = 0;

        protected CComboContainer M_Parent {
            get => m_parent;
        }

        public int M_SerialNumber {
            get => m_serialNumber;
        }

        public CodeBlockType MNodeType{
            get => m_nodeType;
        }

        public string M_NodeName {
            get => m_nodeName;
        }
        protected int M_NestingLevel {
            get => m_nestingLevel;
            set => m_nestingLevel = value;
        }

        protected CEmmitableCodeContainer(CodeBlockType nodeType, CComboContainer parent) {
            m_nodeType = nodeType;
            m_serialNumber = m_serialNumberCounter++;
            m_nodeName = m_nodeType + "_" + m_serialNumber;
            m_parent = parent;
            m_nestingLevel = parent?.M_NestingLevel??0;
        }

        /// <summary>
        /// This method is specialized in concrete nodes and converts the composite
        /// structure of this node to a CodeContainer object having the text for this
        /// Code object
        /// </summary>
        /// <returns></returns>
        public abstract CodeContainer AssemblyCodeContainer();
        public abstract void AddCode(String code, CodeContextType context=CodeContextType.CC_NA);
        public abstract void AddCode(CodeContainer code, CodeContextType context=CodeContextType.CC_NA);
        public abstract void PrintStructure(StreamWriter m_ostream);
        public abstract string EmmitStdout();
        public abstract void EmmitToFile(StreamWriter f);
        public virtual void EnterScope() {
            m_nestingLevel++; }

        public virtual void LeaveScope() {
            if (m_nestingLevel > 0) {
                m_nestingLevel--;
            }
            else {
                throw new Exception("Non-matched nesting");
            }
        }
        public abstract void AddNewLine(CodeContextType context=CodeContextType.CC_NA);
    }

    public abstract class CComboContainer : CEmmitableCodeContainer
    {
        protected List<CEmmitableCodeContainer>[] m_repository;
        
        private static int m_clusterSerial=0;

       
        protected CComboContainer(CodeBlockType nodeType,CComboContainer parent,int numcontexts) : base(nodeType,parent) {
            m_repository = new List<CEmmitableCodeContainer>[numcontexts];
            for (int i = 0; i < numcontexts; i++) {
                m_repository[i] = new List<CEmmitableCodeContainer>();
            }
        }

        public override void AddCode(string code, CodeContextType context)
        {
            CodeContainer container = new CodeContainer(CodeBlockType.CB_NA,this);
            container.AddCode(code,CodeContextType.CC_NA);
            m_repository[GetContextIndex(context)].Add(container);
        }

        public override void AddCode(CodeContainer code, CodeContextType context) {
            m_repository[GetContextIndex(context)].Add(code);
        }

        public void AddCode(CEmmitableCodeContainer code, CodeContextType context)        {                       
            m_repository[GetContextIndex(context)].Add(code);
        }
        public override void AddNewLine(CodeContextType context) {
            CodeContainer container = new CodeContainer(CodeBlockType.CB_NA, this);
            container.AddNewLine();
            m_repository[GetContextIndex(context)].Add(container);
        }

        public override string EmmitStdout() {
            string s = AssemblyCodeContainer().ToString();
            Console.WriteLine(s);
            return s;
        }

        public override void EmmitToFile(StreamWriter f) {
            string s = AssemblyCodeContainer().ToString();
            f.WriteLine(s);
        }

        internal int GetContextIndex(CodeContextType ct)
        {
            int index;
            index = (int)ct - (int)MNodeType;
            return index;
        }

        protected CEmmitableCodeContainer GetChild(CodeContextType ct, int index=0) {
            return m_repository[GetContextIndex(ct)][index];
        }
        
        protected void ExtractSubgraphs(StreamWriter m_ostream, CodeContextType context) {
            if (m_repository[GetContextIndex(context)].Count != 0) {
                m_ostream.WriteLine("\tsubgraph cluster" + m_clusterSerial++ + "{");
                m_ostream.WriteLine("\t\tnode [style=filled,color=white];");
                m_ostream.WriteLine("\t\tstyle=filled;");
                m_ostream.WriteLine("\t\tcolor=lightgrey;");
                m_ostream.Write("\t\t");
                for (int i = 0; i < m_repository[GetContextIndex(context)].Count; i++) {
                    m_ostream.Write(m_repository[GetContextIndex(context)][i].M_NodeName + ";");
                }

                m_ostream.WriteLine("\n\t\tlabel=" + context + ";");
                m_ostream.WriteLine("\t}");
            }
        }
    }

    public class CodeContainer : CEmmitableCodeContainer
    {
        StringBuilder m_repository = new StringBuilder();

        public CodeContainer(CodeBlockType nodeType,CComboContainer parent) : base(nodeType,parent) {
        }

        public override void AddCode(string code, CodeContextType context=CodeContextType.CC_NA) {
            string[] lines = code.Split(new[] {'\n', '\r'});
            foreach (string line in lines) {
                m_repository.Append(line);
                if (lines.Length > 1) {
                    m_repository.Append("\n");
                    m_repository.Append(new string('\t', m_nestingLevel));
                }
            }
        }

        public override void AddCode(CodeContainer code, CodeContextType context = CodeContextType.CC_NA) {
            string str = code.ToString();
            AddCode(str,context);
        }
        public override void AddNewLine(CodeContextType context=CodeContextType.CC_NA) {
            m_repository.Append("\n");
            m_repository.Append(new string('\t', m_nestingLevel));
        }
        public override void EnterScope() {
            base.EnterScope();
            AddNewLine();
        }

        public override void LeaveScope() {
            base.LeaveScope();
            AddNewLine();
        }

        public override string EmmitStdout()
        {
           System.Console.WriteLine(m_repository.ToString());
           return m_repository.ToString();
        }

        public override void EmmitToFile(StreamWriter f) {
            f.WriteLine(m_repository.ToString());
        }

        public override string ToString() {
            return m_repository.ToString();
        }

        public override CodeContainer AssemblyCodeContainer() {
            return this;
        }
        public override void PrintStructure(StreamWriter m_ostream) {
            m_ostream.WriteLine("\"{0}\"->\"{1}\"", M_Parent.M_NodeName, M_NodeName);
        }
    }
   
}
