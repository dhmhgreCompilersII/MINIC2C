

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

            //2. Create the repositories for Function Definition object
            CodeContainer body  = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY,fundef);
            fundef.AddCode(body,CodeContextType.CC_FUNCTIONDEFINITION_BODY);
            CodeContainer declarations = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY, fundef);
            fundef.AddCode(declarations, CodeContextType.CC_FUNCTIONDEFINITION_DECLARATIONS);
            CodeContainer header = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY, fundef);
            fundef.AddCode(header, CodeContextType.CC_FUNCTIONDEFINITION_HEADER);

            //3. Add Function Definition to the File in the appropriate context
            param.M_Parent.AddCode(fundef,param.M_ContextType);

            VisitContext(node, contextType.CT_FUNCTIONDEFINITION_IDENTIFIER, new TranslationParameters() {
                M_Parent = fundef,
                M_ContextType = CodeContextType.CC_FUNCTIONDEFINITION_HEADER
            });

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
            CodeContainer rep = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY,param.M_Parent);
            param.M_Parent.AddCode(rep,param.M_ContextType);

            
            rep.AddCode("=");

            return rep;
            
        }
    }
}