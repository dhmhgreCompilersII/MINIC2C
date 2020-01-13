using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace ANTLR_Startup_Project
{
    public static class ANTLRVisitorExtensions
    {

        private static ITerminalNode GetTerminalNode<Result>(this AbstractParseTreeVisitor<Result> t, ParserRuleContext node, IToken terminal)
        {

            for (int i = 0; i < node.ChildCount; i++)
            {
                ITerminalNode child = node.GetChild(i) as ITerminalNode;
                if (child != null)
                {
                    if (child.Symbol == terminal)
                    {
                        return child;
                    }
                }
            }
            return null;
        }

        public static Result VisitElementInContext<E, Result>(this AbstractParseTreeVisitor<Result> t, ParserRuleContext node, Stack<E> s, E context) where E : System.Enum
        {
            s.Push(context);
            Result res = t.Visit(node);
            s.Pop();
            return res;
        }

        public static Result VisitElementsInContext<E, Result>(this AbstractParseTreeVisitor<Result> t, IEnumerable<IParseTree> nodeset, Stack<E> s, E context) where E : Enum
        {
            Result res = default(Result);
            s.Push(context);
            foreach (IParseTree node in nodeset)
            {
                res = t.Visit(node);
            }
            s.Pop();
            return res;
        }

        public static Result VisitTerminalInContext<E, Result>(this AbstractParseTreeVisitor<Result> t, ParserRuleContext tokenParent, IToken node, Stack<E> s, E context) where E : System.Enum
        {
            s.Push(context);
            Result res = t.Visit(GetTerminalNode<Result>(t, tokenParent, node));
            s.Pop();
            return res;
        }

    }
}