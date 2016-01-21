using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DetectLogicalDependency.StopWords
{
    class JavaStopWord: StopWord
    {
        public JavaStopWord(): base(".java")
        {
            #region StopWords
            StopWords.Add("abstract");
            StopWords.Add("assert");
            StopWords.Add("boolean");
            StopWords.Add("break");
            StopWords.Add("byte");
            StopWords.Add("case");
            StopWords.Add("catch");
            StopWords.Add("char");
            StopWords.Add("class");
            StopWords.Add("const");
            StopWords.Add("continue");
            StopWords.Add("default");
            StopWords.Add("do");
            StopWords.Add("double");
            StopWords.Add("else");
            StopWords.Add("enum");
            StopWords.Add("extends");
            StopWords.Add("final");
            StopWords.Add("finally");
            StopWords.Add("float");
            StopWords.Add("for");
            StopWords.Add("goto");
            StopWords.Add("if");
            StopWords.Add("implements");
            StopWords.Add("import");
            StopWords.Add("instanceof");
            StopWords.Add("int");
            StopWords.Add("interface");
            StopWords.Add("long");
            StopWords.Add("native");
            StopWords.Add("new");
            StopWords.Add("package");
            StopWords.Add("private");
            StopWords.Add("protected");
            StopWords.Add("public");
            StopWords.Add("return");
            StopWords.Add("short");
            StopWords.Add("static");
            StopWords.Add("strictfp");
            StopWords.Add("super");
            StopWords.Add("switch");
            StopWords.Add("synchronized");
            StopWords.Add("this");
            StopWords.Add("throw");
            StopWords.Add("throws");
            StopWords.Add("transient");
            StopWords.Add("try");
            StopWords.Add("void");
            StopWords.Add("volatile");
            StopWords.Add("while");
            #endregion
        
        }

       
    }
}
