using NGit;
using NGit.Api;
using NGit.Diff;
using NGit.Revwalk;
using NGit.Treewalk;
using NGit.Treewalk.Filter;
using Sharpen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetectLogicalDependency.Common
{
    class GitHelper
    {

        public const string BaseFileForDependency ="CatalogController.cs";// "defaultResources.nopres.xml";
        public static string GetCommitDiff(Repository repository, Git git, RevCommit rev, String file)
        {

            string snapshotN = GetSnapshot(repository, git, rev.GetParent(0).Tree, file);
            string snapshotNPlusOne = GetSnapshot(repository, git, rev.Tree, file);

            //Fetch diffrence of commit
            OutputStream @out = new ByteArrayOutputStream();
            DiffCommand diff = git.Diff().SetPathFilter(PathFilter.Create(file)).SetOldTree(GetTreeIterator(rev.GetParent(0).Tree.Name, repository)).SetNewTree(GetTreeIterator(rev.Tree.Name, repository)).SetOutputStream(@out);
            IList<DiffEntry> entries = diff.Call();
            return @out.ToString();
        }

        private static string GetSnapshot(Repository repository, Git git, RevTree revTree, String file)
        {
            //Get snapshot , Retrive older version of an object
            TreeWalk treeWalk = TreeWalk.ForPath(git.GetRepository(), file, revTree);
            byte[] data = repository.Open(treeWalk.GetObjectId(0)).GetBytes();
            return System.Text.Encoding.UTF8.GetString(data);


            //ObjectLoader loader = repository.Open(rev.ToObjectId());
            //InputStream ttt = loader.OpenStream();
            //string result = ttt.ToString();
            //Retrive older version of an object
            //RevTree revTree = rev.Tree;
            //TreeWalk treeWalk = new TreeWalk(git.GetRepository());
            //treeWalk.AddTree(revTree);
            //while (treeWalk.Next())
            //{
            //    //compare treeWalk.NameString yourself
            //    byte[] bytes = treeWalk.ObjectReader.Open(rev.ToObjectId()).GetCachedBytes();
            //    string result = System.Text.Encoding.UTF8.GetString(rev.RawBuffer);
            //}
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


        public static bool HasFile(List<String> files)
        {
            bool hasFile = false;

            foreach (String file in files)
            {
                if (file.Contains(BaseFileForDependency))
                // if ((rev.GetFullMessage().Contains("miss") || rev.GetFullMessage().Contains("forgot")) && !rev.GetFullMessage().Contains("permission"))
                {
                    hasFile = true;
                }
            }
            int aTypicalCommitThreshold = 50;
            return hasFile && (files.Count < aTypicalCommitThreshold);
        }

    }
}
