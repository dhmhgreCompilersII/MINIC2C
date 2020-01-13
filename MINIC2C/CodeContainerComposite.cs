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
        CB_PREPROCESSORDIRECTIVES=3,
        CB_GLOBALVARIABLES=4,
        CB_FUNCTIONDEFINITION=5
    };

    public enum CodeContextType
    {
        CC_NA,
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

        protected CEmmitableCodeContainer(CodeBlockType nodeType, CComboContainer parent) {
            m_nodeType = nodeType;
            m_serialNumber = m_serialNumberCounter++;
            m_nodeName = m_nodeType + "_" + m_serialNumber;
            m_parent = parent;
        }

        /// <summary>
        /// This method is specialized in concrete nodes and converts the composite
        /// structure of this node to a CodeContainer object having the text for this
        /// Code object
        /// </summary>
        /// <returns></returns>
        public abstract CodeContainer AssemblyCodeContainer();
        public abstract void AddCode(String code, CodeContextType context);
        public abstract void PrintStructure(StreamWriter m_ostream);
        public abstract string EmmitStdout();
    }

    public abstract class CComboContainer : CEmmitableCodeContainer
    {
        protected List<CEmmitableCodeContainer>[] m_repository;
        
        private static int m_clusterSerial=0;

       
        protected CComboContainer(CodeBlockType nodeType,CComboContainer parent) : base(nodeType,parent) {
            
        }

        public override void AddCode(string code, CodeContextType context)
        {
            CodeContainer container = new CodeContainer(CodeBlockType.CB_NA,this);
            container.AddCode(code,CodeContextType.CC_NA);
            m_repository[GetContextIndex(context)].Add(container);
        }

        public void AddCode(CEmmitableCodeContainer code, CodeContextType context)        {                       
            m_repository[GetContextIndex(context)].Add(code);
        }
        
        public override string EmmitStdout()
        {
            int i = 0;
            StringBuilder s =new StringBuilder();
            for(i = 0; i < m_repository.Length; i++) {
                foreach (CEmmitableCodeContainer codeContainer in m_repository[i])
                {
                    s.Append(codeContainer.EmmitStdout());

                }
            }
            return s.ToString();
        }

        internal int GetContextIndex(CodeContextType ct)
        {
            int index;
            index = (int)ct - (int)MNodeType;
            return index;
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

        public override void AddCode(string code, CodeContextType context)
        {
            m_repository.Append(code);
        }

        public override string EmmitStdout()
        {
           System.Console.WriteLine(m_repository.ToString());
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
