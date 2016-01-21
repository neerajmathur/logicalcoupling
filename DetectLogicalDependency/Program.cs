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
using DetectLogicalDependency.Common;


namespace DetectLogicalDependency
{
    class Program
    {
        private static int totalCommitCount = 0;

        private static SortedDictionary<string, int> fileCount = new SortedDictionary<string, int>();

        static void Main(string[] args)
        {


           

            //string input = "group, and test but not testing.  But yes to test";
            //string val="Group";
            //string pattern = @"\b" + val + @"\b";
            //string replace = " ";
            //string result = System.Text.RegularExpressions.Regex.Replace(input, pattern, replace, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            //Console.WriteLine(result);

            FileRepositoryBuilder builder = new FileRepositoryBuilder();
            Repository repository = builder.SetGitDir(new FilePath(@"D:\Personal\E Books\Higher Education\Research\SampleProjects\NopCommerce\.git"))
                //Repository repository = builder.SetGitDir(new FilePath(@"C:\Users\neemathu\Documents\GitHub\angular.js\.git"))
                // Repository repository = builder.SetGitDir(new FilePath(@"D:\Personal\E Books\Higher Education\RefactoringActivity\ganttproject\.git"))
            .ReadEnvironment() // scan environment GIT_* variables
            .FindGitDir() // scan up the file system tree
            .Build();

            RevWalk rw = new RevWalk(repository);

            Git git = new Git(repository);

            Iterable<RevCommit> log = git.Log().Call();


            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "buildstopwordindex":
                        BuildStopWordIndex(log, repository, git);
                        return;

                        break;

                    default:
                        break;
                }

            }


            // Iterat over revisions
            for (Iterator<RevCommit> iterator = log.Iterator(); iterator.HasNext(); )
            {
                RevCommit rev = iterator.Next();

                var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(rev.CommitTime).ToLocalTime();

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

                        string filePath=diff.GetNewPath();

                        //if (fileCount.ContainsKey(filePath))
                        //{
                        //    fileCount[filePath] = fileCount[filePath] + 1;
                        
                        //}
                        //else
                        //{
                        //    fileCount.Add(filePath, 1);

                        //}

                        files.Add(filePath);
                        
                        //System.Console.WriteLine(String.Format("FilePath: {0} {1}", diff.GetNewMode().GetBits(), diff.GetNewPath()));
                    }
                    //continue;
                }

                if (GitHelper.HasFile(files))
                {
                    foreach (String file in files)
                    {
                        String FileName = file.Substring(file.LastIndexOf("/") + 1, file.Length - file.LastIndexOf("/") - 1);
                        if (Utils.AllowedFileExtentions(FileName))
                        {
                            string DiffContent = GitHelper.GetCommitDiff(repository, git, rev, file);
                            //data = Common.Utils.RemoveStopWords(data);

                            LogicalDependency.LogicalDependency.AddArtifact(rev.Id.Name, file, DiffContent);

                            //StopWords.StopWordIndexBuilder.BuildWordIndex(file, data);
                        }
                    }
                }

            }

            //var sortedElements = fileCount.OrderByDescending(kvp => kvp.Value);

            //foreach (var item in sortedElements)
            //{
            //    Console.WriteLine(item.Key + ": " + item.Value);
            //}

            LogicalDependency.LogicalDependency.CalculateSimilarityIndex();

        }

        private static void BuildStopWordIndex(Iterable<RevCommit> log, Repository repository, Git git)
        {
            Console.WriteLine("Index build start -" + System.DateTime.Now);
            DateTime startTime = DateTime.Now;

            for (Iterator<RevCommit> iterator = log.Iterator(); iterator.HasNext(); )
            {
                RevCommit rev = iterator.Next();
                var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(rev.CommitTime).ToLocalTime();

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
                        string filename = diff.GetNewPath();
                        if (Utils.AllowedFileExtentions(filename))
                        {
                            string data = GitHelper.GetCommitDiff(repository, git, rev, filename);
                            StopWords.StopWordIndexBuilder.BuildWordIndex(filename, data);
                        }
                    }
                }

                if (DateTime.Now.Subtract(startTime).TotalMinutes > 60)
                {
                    break;             
                }             

            }

            Console.WriteLine("Index build end -" + System.DateTime.Now);

             StopWords.StopWordIndexBuilder.BuildStopWordsIndex();
             StopWords.StopWordIndexBuilder.WriteToFile();

            Console.WriteLine("Stop word keyword index build completed..");
        
        }


    }
}
