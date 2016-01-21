using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DetectLogicalDependency.StopWords
{
    public abstract class StopWord
    {

        public static Dictionary<string, List<string>> StopWordIndex = new Dictionary<string, List<string>>();

        public List<string> StopWords = new List<string>();


        public StopWord(string fileExtension)
        {
            //Load stop word index first, if index does not contain any thing
            if (StopWordIndex.Count == 0)
            {
                LoadStopWordIndex();
            }

            //Assing StopWord list before returning it.
            if (StopWordIndex.ContainsKey(fileExtension))
            {
                StopWords.AddRange(StopWordIndex[fileExtension]);
            }
        }

        private void LoadStopWordIndex()
        {
            using (System.IO.StreamReader idnexfile = new System.IO.StreamReader(@"StopWordIndex.txt"))
            {
                string line;
                string fileExtension = string.Empty;
                while ((line = idnexfile.ReadLine()) != null)
                {
                    if (line.Contains("FileExtension"))
                    {
                        fileExtension = line.Split(':')[1];
                        StopWordIndex.Add(fileExtension, new List<string>());
                        
                    }
                    else if (line.Trim() != string.Empty)
                    {
                        StopWordIndex[fileExtension].Add(line.Trim());
                    }

                }

            }

        }



        public string Remove(string str)
        {

            foreach (var stopword in StopWords)
            {
                string pattern = @"\b" + stopword.Trim() + @"\b";
                str = Regex.Replace(str, pattern, " ", RegexOptions.IgnoreCase);
            }

            return str;
        }
    }
}
