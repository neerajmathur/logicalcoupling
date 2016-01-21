using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NGit;
using NGit.Storage.File;
using Sharpen;
using NGit.Revwalk;
using NGit.Api;
using NGit.Treewalk;
using NGit.Diff;
using NGit.Util.IO;
using NGit.Treewalk.Filter;


namespace LogicalCoupling1
{
    class Program
    {

        private static readonly int aTypicalCommitThreshold = 50;
        private static int totalCommitCount = 0;
        private static Dictionary<string, Dictionary<string, Dictionary<string, double>>> tfIdfBeforeData = new Dictionary<string, Dictionary<string, Dictionary<string, double>>>();
        private static Dictionary<string, Dictionary<string, Dictionary<string, double>>> tfIdfCalculated ;


        static void Main(string[] args)
        {

            
            FileRepositoryBuilder builder = new FileRepositoryBuilder();
            Repository repository = builder.SetGitDir(new FilePath(@"D:\Personal\E Books\Higher Education\Research\SampleProjects\NopCommerce\.git"))
                //Repository repository = builder.SetGitDir(new FilePath(@"C:\Users\neemathu\Documents\GitHub\angular.js\.git"))
                // Repository repository = builder.SetGitDir(new FilePath(@"D:\Personal\E Books\Higher Education\RefactoringActivity\ganttproject\.git"))
            .ReadEnvironment() // scan environment GIT_* variables
            .FindGitDir() // scan up the file system tree
            .Build();

            
            
            HashSet<String> uniqueFile = new HashSet<string>();
            Dictionary<String, int> logicalCoupling = new Dictionary<string, int>();

            RevWalk rw = new RevWalk(repository);


            Git git = new Git(repository);

            Iterable<RevCommit> log = git.Log().Call();
            for (Iterator<RevCommit> iterator = log.Iterator(); iterator.HasNext(); )
            {
                RevCommit rev = iterator.Next();


                //RevWalk revWalk = new RevWalk(git.GetRepository());
                //RevTree revTree = revWalk.ParseTree(rev.Tree.Id);
                //TreeWalk treeWalk = new TreeWalk(git.GetRepository());
                //treeWalk.AddTree(revTree);

                //while (treeWalk.Next())
                //{
                //    //compare treeWalk.NameString yourself


                //    byte[] bytes = treeWalk.ObjectReader.Open(treeWalk.GetObjectId(0)).GetBytes();
                //    string result1 = System.Text.Encoding.UTF8.GetString(bytes);


                //}



                // Sharpen.OutputStream os = new Sharpen.OutputStream();

                //rev.CopyRawTo(os);

                //System.Console.WriteLine("Author: "+rev.GetAuthorIdent().GetName());
                //System.Console.WriteLine("ID:" + rev.Id);


                var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(rev.CommitTime).ToLocalTime();
                //var ts = new TimeSpan(DateTime.UtcNow.Ticks - rev.CommitTime);
                //System.Console.WriteLine("Date:" + dt.ToString());
                //System.Console.WriteLine("Description:" + rev.GetFullMessage());

                DiffFormatter df = new DiffFormatter(DisabledOutputStream.INSTANCE);
                df.SetRepository(repository);
                df.SetDiffComparator(RawTextComparator.DEFAULT);
                df.SetDetectRenames(true);

                List<String> files = new List<string>();

                if (rev.ParentCount > 0)
                {
                    List<DiffEntry> diffs = df.Scan(rev.GetParent(0).Tree, rev.Tree).ToList();


                    foreach (DiffEntry diff in diffs)
                    {

                        // Fetch data from the commited new file
                        //ObjectLoader loader = repository.Open(diff.GetNewId().ToObjectId());
                        //OutputStream @out = new ByteArrayOutputStream();
                        ////loader.CopyTo(@out);

                        ////Fetch diffrence of commit
                        //DiffCommand diff1 = git.Diff().SetPathFilter(PathFilter.Create(diff.GetNewPath())).SetOldTree(GetTreeIterator(rev.GetParent(0).Tree.Name, repository)).SetNewTree(GetTreeIterator(rev.Tree.Name, repository)).SetOutputStream(@out);
                        //IList<DiffEntry>  entries = diff1.Call();
                        //string data = @out.ToString();

                        files.Add(diff.GetNewPath());
                        uniqueFile.Add(diff.GetNewPath());
                        //System.Console.WriteLine(String.Format("FilePath: {0} {1}", diff.GetNewMode().GetBits(), diff.GetNewPath()));
                    }
                }




                if (isContainFile(rev, files))
                {
                    //System.Console.WriteLine(rev.Id);
                    //System.Console.WriteLine(dt);
                    //System.Console.WriteLine(rev.GetAuthorIdent().GetName());
                    //System.Console.WriteLine(rev.GetFullMessage());

                    tfIdfBeforeData.Add(rev.Id.Name, new Dictionary<string, Dictionary<string, double>>());

                    
                    foreach (String file in files)
                    {
                        String fileName = file.Substring(file.LastIndexOf("/") + 1, file.Length - file.LastIndexOf("/") - 1);

                        if (IsFileExtentionAllowed(fileName))
                        {


                            string data = GetCommitDiff(repository, git, rev, file);
                            Dictionary<string, double> tokensTF = GetTokensWithTF(data);

                            tfIdfBeforeData[rev.Id.Name].Add(file, tokensTF);


                            //System.Console.WriteLine("File path: " + file);
                            //System.Console.WriteLine(data);
                            //System.Console.WriteLine("------------------");

                            if (!logicalCoupling.ContainsKey(fileName))
                            {
                                logicalCoupling.Add(fileName, 1);
                            }
                            else
                            {

                                logicalCoupling[fileName] += 1;
                            }
                        }
                    }

                    //System.Console.WriteLine("###################################");

                }


                //foreach (var item in uniqueFile)
                //{

                //    System.Console.WriteLine(item);
                //}

                //System.Console.WriteLine("--------------------");


                //http://stackoverflow.com/questions/11869412/jgit-using-revwalk-to-get-revcommit-returns-nothing

                ////ObjectId head = repository.Resolve("master");
                //RevWalk walk = new RevWalk(repository);

                //foreach (var commit in walk)
                //{
                //    String email = commit.GetAuthorIdent().GetEmailAddress();
                //}

            }

            CalculateTfIdfScore("defaultResources.nopres.xml");

            CalculateLogicalDependency(logicalCoupling);

            System.Console.WriteLine("----------Done----------");
            System.Console.ReadLine();
        }

        private static string GetCommitDiff(Repository repository, Git git, RevCommit rev, String file)
        {
            //Fetch diffrence of commit
            OutputStream @out = new ByteArrayOutputStream();
            DiffCommand diff1 = git.Diff().SetPathFilter(PathFilter.Create(file)).SetOldTree(GetTreeIterator(rev.GetParent(0).Tree.Name, repository)).SetNewTree(GetTreeIterator(rev.Tree.Name, repository)).SetOutputStream(@out);
            IList<DiffEntry> entries = diff1.Call();
            string data = @out.ToString();

            data = System.Text.RegularExpressions.Regex.Replace(data, "[^a-zA-Z.]+", " ");//[^0-9a-zA-Z.]
            data = RemoveStopWords(data);
            return data;
        }

        private static void CalculateTfIdfScore(string filename)
        {

            tfIdfCalculated = new Dictionary<string, Dictionary<string, Dictionary<string, double>>>(tfIdfBeforeData);

            foreach (var commit in tfIdfBeforeData)
            {

                if (!commit.Key.Contains(filename))
                {

                    #region Count Total Terms in a Commit
                    //count total terms in documents
                    Dictionary<String, HashSet<string>> tokensInDocs = new Dictionary<string, HashSet<string>>();
                    foreach (var documents in commit.Value)
                    {
                        foreach (var token in documents.Value)
                        {
                            if (!tokensInDocs.ContainsKey(token.Key))
                            {
                                tokensInDocs.Add(token.Key, new HashSet<string>());
                            }
                            tokensInDocs[token.Key].Add(documents.Key);
                            
                        }
                    }
                    #endregion

                    #region Jaccard Similarity
                    var result= commit.Value["src/Presentation/Nop.Web/App_Data/Localization/defaultResources.nopres.xml"].Keys
                        .Intersect(commit.Value["upgradescripts/3.50-the next version/upgrade.sql"].Keys)
                        .ToDictionary(t => t, t => commit.Value["src/Presentation/Nop.Web/App_Data/Localization/defaultResources.nopres.xml"]);

                  var resultUnion = commit.Value["src/Presentation/Nop.Web/App_Data/Localization/defaultResources.nopres.xml"].Keys
                      .Union(commit.Value["upgradescripts/3.50-the next version/upgrade.sql"].Keys)
                      .ToDictionary(t => t, t => commit.Value["src/Presentation/Nop.Web/App_Data/Localization/defaultResources.nopres.xml"]);

                  var result1 = commit.Value["src/Presentation/Nop.Web/App_Data/Localization/defaultResources.nopres.xml"].Keys
                      .Intersect(commit.Value["src/Libraries/Nop.Core/Domain/Topics/Topic.cs"].Keys)
                      .ToDictionary(t => t, t => commit.Value["src/Presentation/Nop.Web/App_Data/Localization/defaultResources.nopres.xml"]);

                  var result1Union = commit.Value["src/Presentation/Nop.Web/App_Data/Localization/defaultResources.nopres.xml"].Keys
                    .Union(commit.Value["src/Libraries/Nop.Core/Domain/Topics/Topic.cs"].Keys)
                    .ToDictionary(t => t, t => commit.Value["src/Presentation/Nop.Web/App_Data/Localization/defaultResources.nopres.xml"]);

                  var result2 = commit.Value["src/Presentation/Nop.Web/App_Data/Localization/defaultResources.nopres.xml"].Keys
                   .Intersect(commit.Value["src/Libraries/Nop.Services/Installation/CodeFirstInstallationService.cs"].Keys)
                   .ToDictionary(t => t, t => commit.Value["src/Presentation/Nop.Web/App_Data/Localization/defaultResources.nopres.xml"]);

                  var result3 = commit.Value["src/Presentation/Nop.Web/App_Data/Localization/defaultResources.nopres.xml"].Keys
                   .Intersect(commit.Value["src/Libraries/Nop.Services/Topics/TopicService.cs"].Keys)
                   .ToDictionary(t => t, t => commit.Value["src/Presentation/Nop.Web/App_Data/Localization/defaultResources.nopres.xml"]);


                  var result4 = commit.Value["src/Presentation/Nop.Web/App_Data/Localization/defaultResources.nopres.xml"].Keys
                .Intersect(commit.Value["src/Presentation/Nop.Web/Administration/Controllers/TopicController.cs"].Keys)
                .ToDictionary(t => t, t => commit.Value["src/Presentation/Nop.Web/App_Data/Localization/defaultResources.nopres.xml"]);

                  var result5 = commit.Value["src/Presentation/Nop.Web/App_Data/Localization/defaultResources.nopres.xml"].Keys
                .Intersect(commit.Value["src/Presentation/Nop.Web/Administration/Models/Topics/TopicModel.cs"].Keys)
                .ToDictionary(t => t, t => commit.Value["src/Presentation/Nop.Web/App_Data/Localization/defaultResources.nopres.xml"]);

                  var result6 = commit.Value["src/Presentation/Nop.Web/App_Data/Localization/defaultResources.nopres.xml"].Keys
                .Intersect(commit.Value["src/Tests/Nop.Data.Tests/Topics/TopicPersistenceTests.cs"].Keys)
                .ToDictionary(t => t, t => commit.Value["src/Presentation/Nop.Web/App_Data/Localization/defaultResources.nopres.xml"]);
                #endregion


                    foreach (var documents in commit.Value)
                    {
                        foreach (var token in documents.Value)
                        {
                            double tf = (double)token.Value / (double)documents.Value.Values.Sum();
                            double idf =Math.Log((double)commit.Value.Count() / (double)tokensInDocs[token.Key].Count(),2);
                            double tf_idf=tf*idf;

                            tfIdfCalculated[commit.Key][documents.Key][token.Key] = tf_idf;
                        }
                    }
                }
            }

        }

        private static void CalculateLogicalDependency(Dictionary<String, int> logicalCoupling)
        {
            var list = logicalCoupling.Keys.ToList();
            list.Sort();

            System.Console.WriteLine("FileName".PadRight(60) + "|  Support | Confidence");


            foreach (var key in list)
            {
                System.Console.WriteLine(key.PadRight(50) + "|  " + logicalCoupling[key].ToString().PadRight(7) + "| " + string.Format("{0:0.00}", Convert.ToDouble(((Convert.ToDouble(logicalCoupling[key]) / Convert.ToDouble(totalCommitCount)) * 100))).PadRight(10));
            }

            System.Console.WriteLine("--------------------");
        }

        private static bool isContainFile(RevCommit rev, List<String> files)
        {
            bool hasFile = false;

            foreach (String file in files)
            {

                if (file.Contains("defaultResources.nopres.xml"))
                // if ((rev.GetFullMessage().Contains("miss") || rev.GetFullMessage().Contains("forgot")) && !rev.GetFullMessage().Contains("permission"))
                {
                    hasFile = true;
                    totalCommitCount += 1;
                }

            }
            return hasFile && (files.Count < aTypicalCommitThreshold);
        }

        private static bool IsFileExtentionAllowed(String key)
        {
            key = key.ToLower();
            return key.EndsWith(".js") || key.EndsWith(".css") || key.EndsWith(".xml") || key.EndsWith(".cs") || key.EndsWith(".chtml") || key.EndsWith(".sql") || key == null;
        }

        private static AbstractTreeIterator GetTreeIterator(string name, Repository db)
        {
            ObjectId id = db.Resolve(name);
            if (id == null)
            {
                throw new ArgumentException(name);
            }
            CanonicalTreeParser p = new CanonicalTreeParser();
            ObjectReader or = db.NewObjectReader();
            try
            {
                p.Reset(or, new RevWalk(db).ParseTree(id));
                return p;
            }
            finally
            {
                or.Release();
            }
        }

        private static string RemoveStopWords(string str)
        {
            str = str.ToLower();
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

            return str;

        }

        private static Dictionary<string, double> GetTokensWithTF(string str)
        {
            string[] tokens = str.Split(' ');

            Dictionary<string, double> tokensTF = new Dictionary<string, double>();

            foreach (var item in tokens)
            {
                string item1 = item.Trim().Trim('.');
                if (item1.Length > 2)
                {
                    if (!tokensTF.ContainsKey(item1))
                    {
                        tokensTF.Add(item1, 1);
                    }
                    else
                    {
                        tokensTF[item1] = tokensTF[item1] + 1;
                    }

                }
            }

            return tokensTF;


        }
    }
}
