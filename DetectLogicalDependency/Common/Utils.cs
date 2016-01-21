using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DetectLogicalDependency.Common
{
    class Utils
    {
        
        public static string RemoveStopWords(string str)
        {

            #region CSharpStopWords
            List<string> CSharpStopWords = new List<string>();
            CSharpStopWords.Add("abstract");
            CSharpStopWords.Add("async");
            CSharpStopWords.Add("break");
            CSharpStopWords.Add("catch");
            CSharpStopWords.Add("const");
            CSharpStopWords.Add("delegate");
            CSharpStopWords.Add("dynamic");
            CSharpStopWords.Add("explicit");
            CSharpStopWords.Add("fixed");
            CSharpStopWords.Add("from");
            CSharpStopWords.Add("group");
            CSharpStopWords.Add("int");
            CSharpStopWords.Add("is");
            CSharpStopWords.Add("long");
            CSharpStopWords.Add("object");
            CSharpStopWords.Add("out");
            CSharpStopWords.Add("private");
            CSharpStopWords.Add("ref");
            CSharpStopWords.Add("sealed");
            CSharpStopWords.Add("sizeof");
            CSharpStopWords.Add("struct");
            CSharpStopWords.Add("true");
            CSharpStopWords.Add("ulong");
            CSharpStopWords.Add("using");
            CSharpStopWords.Add("void");
            CSharpStopWords.Add("yield");
            CSharpStopWords.Add("add");
            CSharpStopWords.Add("await");
            CSharpStopWords.Add("by");
            CSharpStopWords.Add("char");
            CSharpStopWords.Add("continue");
            CSharpStopWords.Add("descending");
            CSharpStopWords.Add("else");
            CSharpStopWords.Add("extern");
            CSharpStopWords.Add("float");
            CSharpStopWords.Add("get");
            CSharpStopWords.Add("if");
            CSharpStopWords.Add("interface");
            CSharpStopWords.Add("join");
            CSharpStopWords.Add("namespace");
            CSharpStopWords.Add("on");
            CSharpStopWords.Add("override");
            CSharpStopWords.Add("protected");
            CSharpStopWords.Add("remove");
            CSharpStopWords.Add("select");
            CSharpStopWords.Add("stackalloc");
            CSharpStopWords.Add("switch");
            CSharpStopWords.Add("try");
            CSharpStopWords.Add("unchecked");
            CSharpStopWords.Add("value");
            CSharpStopWords.Add("volatile");
            CSharpStopWords.Add("as");
            CSharpStopWords.Add("base");
            CSharpStopWords.Add("byte");
            CSharpStopWords.Add("checked");
            CSharpStopWords.Add("decimal");
            CSharpStopWords.Add("do");
            CSharpStopWords.Add("enum");
            CSharpStopWords.Add("false");
            CSharpStopWords.Add("for");
            CSharpStopWords.Add("global");
            CSharpStopWords.Add("implicit");
            CSharpStopWords.Add("internal");
            CSharpStopWords.Add("let");
            CSharpStopWords.Add("new");
            CSharpStopWords.Add("operator");
            CSharpStopWords.Add("params");
            CSharpStopWords.Add("public");
            CSharpStopWords.Add("return");
            CSharpStopWords.Add("set");
            CSharpStopWords.Add("static");
            CSharpStopWords.Add("this");
            CSharpStopWords.Add("typeof");
            CSharpStopWords.Add("unsafe");
            CSharpStopWords.Add("var");
            CSharpStopWords.Add("where");
            CSharpStopWords.Add("ascending");
            CSharpStopWords.Add("bool");
            CSharpStopWords.Add("case");
            CSharpStopWords.Add("class");
            CSharpStopWords.Add("default");
            CSharpStopWords.Add("double");
            CSharpStopWords.Add("equals");
            CSharpStopWords.Add("finally");
            CSharpStopWords.Add("foreach");
            CSharpStopWords.Add("goto");
            CSharpStopWords.Add("in");
            CSharpStopWords.Add("into");
            CSharpStopWords.Add("lock");
            CSharpStopWords.Add("null");
            CSharpStopWords.Add("orderby");
            CSharpStopWords.Add("partial");
            CSharpStopWords.Add("readonly");
            CSharpStopWords.Add("sbyte");
            CSharpStopWords.Add("short");
            CSharpStopWords.Add("string");
            CSharpStopWords.Add("throw");
            CSharpStopWords.Add("uint");
            CSharpStopWords.Add("ushort");
            CSharpStopWords.Add("virtual");
            CSharpStopWords.Add("while");
            CSharpStopWords.Add("set");
            CSharpStopWords.Add("get");
            #endregion
            foreach (var stopword in CSharpStopWords)
            {
                string pattern = @"\b"+stopword+"\b";
                str = Regex.Replace(str, pattern, " ");
            }

           

            str = str.Replace("public", " ");
            str = str.Replace("gets", " ");
            str = str.Replace("sets", " ");
            str = str.Replace("get", " ");
            str = str.Replace("set", " ");
            str = str.Replace("int", " ");
            str = str.Replace("float", " ");
            str = str.Replace("decimal", " ");
            str = str.Replace("private", " ");
            str = str.Replace("try", " ");
            str = str.Replace("catch", " ");
            str = str.Replace("finally", " ");
            str = str.Replace("return", " ");
            str = str.Replace("string", " ");
            str = str.Replace("bool", " ");
            str = str.Replace("static", " ");
            str = str.Replace("value", " ");
            str = str.Replace("diff", " ");
            str = str.Replace("git", " ");
            str = str.Replace("src", " ");
            str = str.Replace("libraries", " ");
            str = str.Replace("Nop", " ");
            str = str.Replace("Web", " ");
            str = str.Replace("Views", " ");
            str = str.Replace("Presentation", " ");
            str = str.Replace("Tests", " ");
            str = str.Replace("Core", " ");
            str = str.Replace("Data", " ");
            str = str.Replace("Services", " ");

            return str;

        }


        public static bool AllowedFileExtentions(String extension)
        {
            extension = extension.ToLower();
            return //extension.EndsWith(".js")    || 
                //extension.EndsWith(".css")   || 
                // extension.EndsWith(".xml")   || 
                   extension.EndsWith(".cs");   // || 
                  // extension.EndsWith(".cshtml") || 
                  // extension.EndsWith(".sql")   ||
                   //extension.EndsWith(".config") ||
                  // extension.EndsWith(".java") ||
                   //extension == null;
        }

        public static List<string> SplitCamalCase(string str)
        {
            str=str.Trim();
            if (str.Length > 0)
            {
                return System.Text.RegularExpressions.Regex.Split(str, @"(?<!(^|[A-Z]))(?=[A-Z])|(?<!^)(?=[A-Z][a-z])").ToList();
            }

            return null;
        }


        public static string GetExtension(string FilePath)
        {
            
            return System.IO.Path.GetExtension(FilePath);
        
        }

        public static string CleanString(string data)
        {
            data = System.Text.RegularExpressions.Regex.Replace(data, "[^a-zA-Z]+", " ");//[^0-9a-zA-Z.]+
            return data;
        }

        public static List<string> Split(string data)
        {

            List<string> splitList = new List<string>();

            foreach (var item in data.Split(' ').ToList())
            {

                if (item.Trim() != string.Empty)
                {
                    splitList.Add(item);
                    var IdentifierList = SplitCamalCase(item);
                    if (IdentifierList.Count > 1) 
                    {
                        splitList.AddRange(IdentifierList);
                    }
                }

            }
            return splitList;      
        }

    }
}
