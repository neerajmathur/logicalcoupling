using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DetectLogicalDependency.StopWords
{
    class CSHTMLStopWord: StopWord
    {

        public CSHTMLStopWord()
            : base(".cshtml")
        {

            #region StopWords
            StopWords.Add("xml");
            StopWords.Add("Xml");
            StopWords.Add("name");
            StopWords.Add("Name");
            StopWords.Add("version");
            StopWords.Add("Version");
            StopWords.Add("value");
            StopWords.Add("Value");
            StopWords.Add("LocaleResource");
            #endregion
        
        }

        
    }
}
