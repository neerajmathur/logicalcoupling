using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DetectLogicalDependency.StopWords
{
    class CSharpStopWord: StopWord
    {

        public CSharpStopWord(): base(".cs")
        {

            #region StopWords
            StopWords.Add("abstract");
            StopWords.Add("async");
            StopWords.Add("break");
            StopWords.Add("catch");
            StopWords.Add("const");
            StopWords.Add("delegate");
            StopWords.Add("dynamic");
            StopWords.Add("explicit");
            StopWords.Add("fixed");
            StopWords.Add("from");
            StopWords.Add("group");
            StopWords.Add("int");
            StopWords.Add("is");
            StopWords.Add("long");
            StopWords.Add("object");
            StopWords.Add("out");
            StopWords.Add("private");
            StopWords.Add("ref");
            StopWords.Add("sealed");
            StopWords.Add("sizeof");
            StopWords.Add("struct");
            StopWords.Add("true");
            StopWords.Add("ulong");
            StopWords.Add("using");
            StopWords.Add("void");
            StopWords.Add("yield");
            StopWords.Add("add");
            StopWords.Add("await");
            StopWords.Add("by");
            StopWords.Add("char");
            StopWords.Add("continue");
            StopWords.Add("descending");
            StopWords.Add("else");
            StopWords.Add("extern");
            StopWords.Add("float");
            StopWords.Add("get");
            StopWords.Add("if");
            StopWords.Add("interface");
            StopWords.Add("join");
            StopWords.Add("namespace");
            StopWords.Add("on");
            StopWords.Add("override");
            StopWords.Add("protected");
            StopWords.Add("remove");
            StopWords.Add("select");
            StopWords.Add("stackalloc");
            StopWords.Add("switch");
            StopWords.Add("try");
            StopWords.Add("unchecked");
            StopWords.Add("value");
            StopWords.Add("volatile");
            StopWords.Add("as");
            StopWords.Add("base");
            StopWords.Add("byte");
            StopWords.Add("checked");
            StopWords.Add("decimal");
            StopWords.Add("do");
            StopWords.Add("enum");
            StopWords.Add("false");
            StopWords.Add("for");
            StopWords.Add("global");
            StopWords.Add("implicit");
            StopWords.Add("internal");
            StopWords.Add("let");
            StopWords.Add("new");
            StopWords.Add("operator");
            StopWords.Add("params");
            StopWords.Add("public");
            StopWords.Add("return");
            StopWords.Add("set");
            StopWords.Add("static");
            StopWords.Add("this");
            StopWords.Add("typeof");
            StopWords.Add("unsafe");
            StopWords.Add("var");
            StopWords.Add("where");
            StopWords.Add("ascending");
            StopWords.Add("bool");
            StopWords.Add("case");
            StopWords.Add("class");
            StopWords.Add("default");
            StopWords.Add("double");
            StopWords.Add("equals");
            StopWords.Add("finally");
            StopWords.Add("foreach");
            StopWords.Add("goto");
            StopWords.Add("in");
            StopWords.Add("into");
            StopWords.Add("lock");
            StopWords.Add("null");
            StopWords.Add("orderby");
            StopWords.Add("partial");
            StopWords.Add("readonly");
            StopWords.Add("sbyte");
            StopWords.Add("short");
            StopWords.Add("string");
            StopWords.Add("throw");
            StopWords.Add("uint");
            StopWords.Add("ushort");
            StopWords.Add("virtual");
            StopWords.Add("while");
            StopWords.Add("set");
            StopWords.Add("get");
            #endregion
        }

      
    }
}
