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
using System.Data.SqlClient;

namespace GroupChangeSets
{
    class Program
    {

        static SqlConnection myConnection = new SqlConnection();
            

        static void Main(string[] args)
        {



        

            Iterable<RevCommit> gitLogs = GetRevCommitList();

            int count = 0;

            DateTime startTime = System.DateTime.Now;

            Dictionary<string, string> gitLogDictionary = new Dictionary<string, string>();


            Dictionary<string, Dictionary<string, string>> groupedChangeSets = new Dictionary<string, Dictionary<string, string>>();

            HashSet<string> commitIDs = new HashSet<string>();




            for (Iterator<RevCommit> iterator = gitLogs.Iterator(); iterator.HasNext(); )
            {
                RevCommit rev = iterator.Next();

                //System.Console.WriteLine(rev.Id.FirstByte);
                //System.Console.WriteLine(rev.GetFullMessage());
                // TermFrequency.Add(rev.Id.Name, rev.GetFullMessage());

                gitLogDictionary.Add(rev.Id.Name, TermFrequency.CleanInput(rev.GetFullMessage()));
                //System.Console.ReadLine();

                count++;
            }


            int loopCount = 0;
            Iterable<RevCommit> gitLogs1 = GetRevCommitList();
            //for (Iterator<RevCommit> iterator2 = gitLogs1.Iterator(); iterator2.HasNext(); )
            foreach (var itemParent in gitLogDictionary)
            {
                //RevCommit rev = iterator2.Next();

                bool first = true;
                if (!itemParent.Value.ToLower().Contains("merge branch") && !commitIDs.Contains(itemParent.Key))
                    foreach (var item in gitLogDictionary)
                    {

                        if (!item.Value.ToLower().Contains("merge branch"))
                            if (item.Key != itemParent.Key && TermFrequency.GetSimilarityValue(itemParent.Value, item.Value) > 0.8)
                            {

                                if (first)
                                {
                                    groupedChangeSets.Add(itemParent.Key, new Dictionary<string, string>() { { itemParent.Key, itemParent.Value } });
                                    commitIDs.Add(itemParent.Key);
                                    first = false;
                                }

                                groupedChangeSets[itemParent.Key].Add(item.Key, item.Value);
                                commitIDs.Add(item.Key);

                                string Test1 = "1";

                            }
                    }

                loopCount++;
                System.Console.Clear();
                System.Console.WriteLine(loopCount);
            }

            TimeSpan TotalTime = (System.DateTime.Now - startTime);


            foreach (var item in groupedChangeSets)
            {

                foreach (var item1 in item.Value)
                {
                    //System.Console.WriteLine(item.Key + ";" + item1.Key + ";" + item1.Value);
                    WriteData(item.Key, item1.Key, item1.Value);
                }
            }

            System.Console.WriteLine(TotalTime);
            // System.Console.ReadLine();
            //TermFrequency.Print();

        }


        private static void WriteData(string parentCommitID, string childCommitID, string Comment)
        {
            
            
            if (myConnection.State == System.Data.ConnectionState.Closed)
            {
                myConnection.ConnectionString =
                @"Data Source=.\MSSQLSERVER1;" +
                "Initial Catalog=GroupChangeSet;" +
                "Integrated Security=SSPI;";
                myConnection.Open();
            }

            SqlCommand myCommand = new SqlCommand("Command String", myConnection);

            myCommand.Connection = myConnection;

            myCommand.CommandText = "INSERT INTO AngularJS (ParentCommit, ChildCommit,Comment) " +
                        "Values ('" + parentCommitID + "','" + childCommitID + "','" + Comment + "')";
            myCommand.ExecuteNonQuery();

            myCommand.Dispose();

            myConnection.Close();
        }


        private static Iterable<RevCommit> GetRevCommitList()
        {

            string GitPath = "";
            //GitPath = @"D:\Personal\E Books\Higher Education\Research\SampleProjects\NopCommerce\.git";
            GitPath = @"C:\Users\neemathu\Documents\GitHub\angular.js\.git";
            //GitPath = @"C:\Users\neemathu\Documents\GitHub\Umbraco-CMS\.git";
            //GitPath = @" D:\Personal\E Books\Higher Education\Research\SampleProjects\SignalR\SignalR\.git";
            //GitPath = @"D:\Personal\E Books\Higher Education\RefactoringActivity\ganttproject\.git";

            FileRepositoryBuilder builder = new FileRepositoryBuilder();
            Repository repository = builder.SetGitDir(new FilePath(GitPath))
                                           .ReadEnvironment() // scan environment GIT_* variables
                                           .FindGitDir() // scan up the file system tree
                                           .Build();

            Git git = new Git(repository);

            Iterable<RevCommit> log = git.Log().Call();
            return log;
        }
    }
}
