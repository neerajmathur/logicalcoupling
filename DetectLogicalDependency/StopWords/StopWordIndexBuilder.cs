using DetectLogicalDependency.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetectLogicalDependency.StopWords
{
    class StopWordIndexBuilder
    {

        // <FileExtention, <Word,FilePath/Document>>
        private static Dictionary<string, Dictionary<string, HashSet<string>>> KewordIndex = new Dictionary<string, Dictionary<string, HashSet<string>>>();

        //<FileExtension, UniqueDocuments>
        private static Dictionary<string, HashSet<string>> DocumentCount = new Dictionary<string, HashSet<string>>();

        // List of stop words, <FileExtension, StopWord>
        private static Dictionary<string, HashSet<string>> StopWords = new Dictionary<string, HashSet<string>>();

        public static void BuildWordIndex(string FilePath, string content)
        {
            string FileExtension = Utils.GetExtension(FilePath).ToLower();
            content = Utils.CleanString(content);
            List<string> Keywords = Utils.Split(content);

            //check if fileExtention already present in the dictionary
            if (!KewordIndex.ContainsKey(FileExtension))
            {
                Dictionary<string, HashSet<string>> dic = new Dictionary<string, HashSet<string>>();
                KewordIndex.Add(FileExtension, dic);
            }


            Dictionary<string, HashSet<string>> index;
            index = KewordIndex[FileExtension];
            foreach (var item in Keywords)
            {
                string Keyword = item.Trim().Trim('.');
                if (Keyword != string.Empty)
                {
                    if (index.ContainsKey(Keyword))
                    {
                        index[Keyword].Add(FilePath);
                    }
                    else
                    {
                        HashSet<string> DocumentList = new HashSet<string>();
                        DocumentList.Add(FilePath);
                        index.Add(Keyword, DocumentList);
                    }
                }
            }

            //calculate unique documents
            if (DocumentCount.ContainsKey(FileExtension))
            {
                DocumentCount[FileExtension].Add(FilePath);
            }
            else
            {
                DocumentCount.Add(FileExtension, new HashSet<string> { FilePath });
            }

        }


        public static void BuildStopWordsIndex()
        {
            foreach (var fileextension in KewordIndex)
            {
                int totaldocuments = DocumentCount[fileextension.Key].Count;
                HashSet<string> stopwordslist = new HashSet<string>();

                foreach (var word in fileextension.Value)
                {
                    double IDF = (word.Value.Count / totaldocuments);

                    if (IDF > 0.2)
                    {
                        if (word.Key != String.Empty)
                        {
                            stopwordslist.Add(word.Key);

                        }
                    }
                }
                StopWords.Add(fileextension.Key, stopwordslist);
            }
        }


        public static void WriteToFile()
        {

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"StopWordIndex.txt",false))
            {
                foreach (var StopWordIndex in StopWords)
                {
                        file.WriteLine("FileExtension:" + StopWordIndex.Key);

                    foreach (var StopWord in StopWordIndex.Value)
                    {
                        file.WriteLine(StopWord);
                    }
                }
            }

        
        }

    }
}
