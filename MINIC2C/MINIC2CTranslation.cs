﻿

using System.Linq;

namespace Mini_C {

    public class TranslationParameters {
        private CComboContainer m_parent=null;
        private CodeContextType m_contextType=CodeContextType.CC_NA;

        public CComboContainer M_Parent {
            get => m_parent;
            set => m_parent = value;
        }

        public CodeContextType M_ContextType {
            get => m_contextType;
            set => m_contextType = value;
        }
    }

    class MINIC2CTranslation : ASTBaseVisitor<CEmmitableCodeContainer,TranslationParameters> {
        private CCFile m_translatedFile;

        public CCFile M_TranslatedFile => m_translatedFile;

        public override CEmmitableCodeContainer VisitFunctionDefinition(CASTFunctionDefinition node,TranslationParameters param) {

            //1. Create Output File
            CCFunctionDefinition fundef = new CCFunctionDefinition(param.M_Parent);
            
            //2. Add Function Definition to the File in the appropriate context
            param.M_Parent.AddCode(fundef,param.M_ContextType);

            //3. Assemble the function header
            CASTIDENTIFIER id = node.GetChild(contextType.CT_FUNCTIONDEFINITION_IDENTIFIER, 0) as CASTIDENTIFIER;

            CEmmitableCodeContainer header = fundef.GetHeader();
            header.AddCode("float "+id.M_Text +"(");
            string last = node.GetFunctionArgs().Last();
            foreach (string s in node.GetFunctionArgs()) {
                header.AddCode("float "+s);
                if (!s.Equals(last)) {
                    header.AddCode(", ");
                }
            }
            header.AddCode(")");



            VisitContext(node, contextType.CT_FUNCTIONDEFINITION_FARGS, new TranslationParameters() {
                M_Parent = fundef,
                M_ContextType = CodeContextType.CC_FUNCTIONDEFINITION_HEADER
            });

            VisitContext(node, contextType.CT_FUNCTIONDEFINITION_BODY, new TranslationParameters() {
                M_Parent = fundef,
                M_ContextType = CodeContextType.CC_FUNCTIONDEFINITION_BODY
            });

            return fundef;
        }

        public override CEmmitableCodeContainer VisitCOMPILEUNIT(CASTCompileUnit node, TranslationParameters param) {

            //1. Create Output File
            m_translatedFile = new CCFile();

            //2. Visit CT_COMPILEUNIT_MAINBODY and create main function
            CMainFunctionDefinition mainf = new CMainFunctionDefinition(m_translatedFile);
            m_translatedFile.AddCode(mainf,CodeContextType.CC_FILE_FUNDEF);

            VisitContext(node, contextType.CT_COMPILEUNIT_MAINBODY,
                new TranslationParameters() {
                    M_Parent = mainf ,
                    M_ContextType = CodeContextType.CC_FUNCTIONDEFINITION_BODY
                });

            // 3. Visit CT_COMPILEUNIT_FUNCTIONDEFINITIONS
            VisitContext(node, contextType.CT_COMPILEUNIT_FUNCTIONDEFINITIONS,
                new TranslationParameters() {
                    M_Parent = m_translatedFile,
                    M_ContextType = CodeContextType.CC_FILE_FUNDEF
                });

            //3. Visit CT

            return m_translatedFile;
        }

        public override CEmmitableCodeContainer VisitASSIGN(CASTExpressionAssign node, TranslationParameters param = default(TranslationParameters)) {
            CCFunctionDefinition fun = param.M_Parent as CCFunctionDefinition;

            CEmmitableCodeContainer rep = fun.GetBody();

            CASTIDENTIFIER id = node.GetChild(contextType.CT_EXPRESSION_ASSIGN_LVALUE, 0) as CASTIDENTIFIER;
            fun.DeclareVariable(id.M_Text);
            rep.AddCode(id.M_Text);
            rep.AddCode("=");
            rep.AddCode(node.GetChild(contextType.CT_EXPRESSION_ASSIGN_EXPRESSION,0).M_Text);
            rep.AddCode(";");
            rep.AddNewLine();
            return rep;
            
        }
    }
}