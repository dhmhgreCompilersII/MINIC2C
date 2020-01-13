using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_C
{
    public class CCFunctionDefinition : CComboContainer {
        //public CCFunctionDefinition(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override CodeContainer AssemblyCodeContainer() {
            throw new NotImplementedException();
        }
    }

    public class CCFile : CComboContainer {
        public override CodeContainer AssemblyCodeContainer() {
            throw new NotImplementedException();
        }
    }

    public class CCPreprocessorDirectives : CComboContainer
    {
        //public CCPreprocessorDirectives(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override CodeContainer AssemblyCodeContainer() {
            throw new NotImplementedException();
        }
    }

    public class CCGlobalVariables : CComboContainer
    {
        //public CCGlobalVariables(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override CodeContainer AssemblyCodeContainer() {
            throw new NotImplementedException();
        }
    }
   
}
