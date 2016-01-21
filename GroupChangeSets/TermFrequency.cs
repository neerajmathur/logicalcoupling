using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GroupChangeSets
{
    static class TermFrequency
    {

        private static Dictionary<string, Dictionary<string, int>> tf = new Dictionary<string, Dictionary<string, int>>();

        public static void Add(string CommitID, String Comment)
        {
            Dictionary<String, int> tokens = GetTokens(Comment);
            foreach (var token in tokens)
            {
                if (tf.ContainsKey(token.Key))
                {
                    tf[token.Key].Add(CommitID, token.Value);
                }
                else
                {
                    tf.Add(token.Key, new Dictionary<string, int>() { { CommitID, token.Value } });
                }
            }
        }


        public static void Print()
        {

            int count = 0;

            foreach (var item in tf)
            {
                if (item.Key.Contains("#"))
                { System.Console.WriteLine(item.Key + ":" + item.Value.Count); }
                count++;

                if (count > 10)
                {
                    //System.Console.ReadLine();
                    count = 0;
                }

            }

        }

        public static double GetSimilarityValue(string comment1, string comment2)
        {

            if (IsContainSameHashTags(comment1, comment2))
            {
                return 1;
            }
            else
            {
                //Convert to tokens
                Dictionary<string, int> tokensC1 = GetTokens(comment1);
                Dictionary<string, int> tokensC2 = GetTokens(comment2);
                
                //Fetch term frequency score
                Dictionary<string, double> tokensC1TF = CalculateScore(tokensC1);
                Dictionary<string, double> tokensC2TF = CalculateScore(tokensC2);

                //Calculate Cosine Similarity
                double cosineScore = 0;
                foreach (var item in tokensC1TF)
                {
                    if (tokensC2TF.ContainsKey(item.Key))
                    {
                        cosineScore += item.Value * tokensC2TF[item.Key];
                    }
                }

                return cosineScore;

            }


        }

        private static Dictionary<string, double> CalculateScore(Dictionary<string, int> tokens)
        {
            
            //Calculate term frequency score
            Dictionary<string, double> tokensTF = new Dictionary<string, double>();
            foreach (var tokenC1 in tokens)
            {
                double score = 1 + Math.Log10(tokenC1.Value);
                tokensTF.Add(tokenC1.Key, score);
            }
            
            //calculate lenthg for normalization
            double length = 0;
            foreach (var item in tokensTF)
            {
                length += item.Value * item.Value;
            }
            length = Math.Sqrt(length);

            //Calculate TF score with length normalization
            Dictionary<string, double> tokensTFNormalized = new Dictionary<string, double>();
            foreach (var token in tokensTF)
            {
                tokensTFNormalized.Add(token.Key, (token.Value / length));
            }

            return tokensTFNormalized;
        }

        private static bool IsContainSameHashTags(string comment1, string comment2)
        {

            string HashTag1 = "-1";
               string HashTag2 = "-2";

               string sPattern = "#[0-9]+";
               Regex regex = new Regex(sPattern);

               if (System.Text.RegularExpressions.Regex.IsMatch(comment1, sPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
               {
                    Match match = regex.Match(comment1);
                    HashTag1= match.Groups[0].Value;
               }

               if (System.Text.RegularExpressions.Regex.IsMatch(comment2, sPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
               {
                   Match match = regex.Match(comment2);
                   HashTag2 = match.Groups[0].Value;
               }
               
            return HashTag1.Equals(HashTag2,StringComparison.OrdinalIgnoreCase);

        }

        private static Dictionary<string, int> GetTokens(string str)
        {

            str = CleanInput(str);
            string[] tokens = str.Split(' ');
            Dictionary<string, int> tokensWithCount = new Dictionary<string, int>();
            foreach (var item in tokens)
            {
                string item1 = item.Trim().Trim('.');
                if (item1.Length > 2)
                {
                    if (!tokensWithCount.ContainsKey(item1))
                    {
                        tokensWithCount.Add(item1, 1);
                    }
                    else
                    {
                        tokensWithCount[item1] = tokensWithCount[item1] + 1;
                    }
                }
            }
            return tokensWithCount;
        }


       public static string CleanInput(string strIn)
        {
            strIn = strIn.ToLower();

            // Replace invalid characters with empty strings. 
            try
            {
                return Regex.Replace(strIn, @"[^#0-9a-z]", " ",
                                     RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            // If we timeout when replacing invalid characters,  
            // we should return Empty. 
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }
    }
}
