using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetectLogicalDependency.StopWords
{
    class StopWordFactory
    {

        public static StopWord GetStopWord(string filename)
        {
            filename = filename.ToLower();

            if (filename.EndsWith(".cs"))
            {
                return new CSharpStopWord();
            }
            else if (filename.EndsWith(".java"))
            {
                return new JavaStopWord();
            }
            else if (filename.EndsWith(".js"))
            {
                return new JsStopWord();               
            }
            else if (filename.EndsWith(".xml"))
            {
                return new XMLStopWord();
            }
            else if (filename.EndsWith(".cshtml"))
            {
                return new CSHTMLStopWord();
            }
            else if (filename.EndsWith(".css"))
            {
                return new CssStopWord();
            }
            else if (filename.EndsWith(".sql"))
            {
                return new CssStopWord();
            }

            return null;

        }
    }
}
