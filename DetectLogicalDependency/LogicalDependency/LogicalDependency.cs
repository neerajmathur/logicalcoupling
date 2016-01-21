using DetectLogicalDependency.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetectLogicalDependency.LogicalDependency
{

    public class DependencyDiff
    {
        public string MainArtifactDiff { get; set; }
        public  Dictionary<string,string> DependentArtifactsDiff { get; set; }
    }

    class LogicalDependency
    {

        private static Dictionary<string, DependencyDiff> _LogicalDependencyDiff = new Dictionary<string, DependencyDiff>();
        
        //<FileName,Count>
        private static Dictionary<string, int> _LogicalDependencyArtifacts = new Dictionary<string, int>();

        //<FileName,SimilarityIndex>
        private static Dictionary<string, SimilarityIndex> _SimilarityIndex = new Dictionary<string, SimilarityIndex>();


        public static void AddArtifact(string RevisionID, string FileName, string DiffContent)
        {

            if (!Common.GitHelper.HasFile(new List<string> { FileName }))
            {
                //Add file to dependency count (for association rule support/confidence calculation)
                if (_LogicalDependencyArtifacts.ContainsKey(FileName))
                {
                    _LogicalDependencyArtifacts[FileName] = _LogicalDependencyArtifacts[FileName] + 1;
                }
                else
                {
                    _LogicalDependencyArtifacts.Add(FileName, 1);
                }


                // Add file and diff content for semantic similarity index calculation
                if (_LogicalDependencyDiff.ContainsKey(RevisionID))
                {
                    _LogicalDependencyDiff[RevisionID].DependentArtifactsDiff.Add(FileName, DiffContent);

                }
                else
                {

                    _LogicalDependencyDiff.Add(RevisionID,new DependencyDiff()
                                                                  {
                                                                      MainArtifactDiff = null,
                                                                      DependentArtifactsDiff = new Dictionary<string,string>(){{FileName, DiffContent}}
                                                                  });
                }

            }
            else
            {


                if (_LogicalDependencyDiff.ContainsKey(RevisionID))
                {
                    _LogicalDependencyDiff[RevisionID].MainArtifactDiff = DiffContent;

                }
                else
                {

                    _LogicalDependencyDiff.Add(RevisionID, new DependencyDiff()
                    {
                        MainArtifactDiff = DiffContent,
                        DependentArtifactsDiff = new Dictionary<string,string>()
                    });
                }


            }


        }

        public static void CalculateSimilarityIndex()
        {
            foreach (var item in _LogicalDependencyArtifacts)
            {
                SortedDictionary<string, int> TotalMatchedKeywords = new SortedDictionary<string, int>();
                foreach (var diffitem in _LogicalDependencyDiff)
                {
                    string mainArtifactDiff = Utils.CleanString(diffitem.Value.MainArtifactDiff);
                    mainArtifactDiff = StopWords.StopWordFactory.GetStopWord(GitHelper.BaseFileForDependency).Remove(mainArtifactDiff);

                    List<string> MainArtifactKeywords = Utils.Split(mainArtifactDiff);
                    foreach (var dependentDiff in diffitem.Value.DependentArtifactsDiff)
                    {
                        if (dependentDiff.Key.Contains(item.Key))
                        {
                            string dependentArtifactDiff = Utils.CleanString(dependentDiff.Value);
                            dependentArtifactDiff = StopWords.StopWordFactory.GetStopWord(dependentDiff.Key).Remove(dependentArtifactDiff);
                            List<string> DependentArtifactKeywords = Utils.Split(dependentArtifactDiff);
                            var MatchedKeywords = MainArtifactKeywords.Select(i => i.ToString()).Intersect(DependentArtifactKeywords);

                            foreach (string match in MatchedKeywords)
                            {
                                if (match.Trim() != string.Empty)
                                {
                                    if (TotalMatchedKeywords.ContainsKey(match))
                                    {
                                        TotalMatchedKeywords[match] = TotalMatchedKeywords[match] + 1;
                                    }
                                    else
                                    {
                                        TotalMatchedKeywords.Add(match, 1);
                                    }
                                }
                            }
                            
                            //TotalMatchedKeywords.AddRange(MatchedKeywords);
                        }
                    }
                }

                decimal confidence = ((item.Value / _LogicalDependencyDiff.Count()) * 100);

                string Keywords = "";
                foreach (var keyword in TotalMatchedKeywords)
                {
                    Keywords += keyword.Key + " " + keyword.Value + ", ";
                }

                Console.WriteLine("FILE: " + item.Key + "; SUPPORT: " + item.Value + "; CONFIDENCE:" + confidence.ToString() +  "; SMIndex:"+ TotalMatchedKeywords.Values.Sum() +"; KEYWORDS: " + Keywords);
                //TotalMatchedKeywords.ForEach(i => Console.Write(i + ";"));
                
                Console.WriteLine("");
                Console.WriteLine("");
            }

            Console.ReadLine();

        }
    }
}
