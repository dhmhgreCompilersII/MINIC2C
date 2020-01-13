using System;
using System.Collections.Generic;
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

        public CodeBlockType MNodeType
        {
            get => m_nodeType;
         }

        /// <summary>
        /// This method is specialized in concrete nodes and converts the composite
        /// structure of this node to a CodeContainer object having the text for this
        /// Code object
        /// </summary>
        /// <returns></returns>
        public abstract CodeContainer AssemblyCodeContainer();
        public abstract void AddCode(String code, CodeContextType context);
        public abstract string EmmitStdout();
    }

    public abstract class CComboContainer : CEmmitableCodeContainer
    {
        private List<CEmmitableCodeContainer>[] m_repository;
        public override void AddCode(string code, CodeContextType context)
        {
            CodeContainer container = new CodeContainer();
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
    }

    public class CodeContainer : CEmmitableCodeContainer
    {
        StringBuilder m_repository = new StringBuilder();
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
    }
   
}
