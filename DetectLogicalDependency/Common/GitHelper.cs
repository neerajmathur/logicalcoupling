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
            //Fetch diffrence of commit
            OutputStream @out = new ByteArrayOutputStream();
            DiffCommand diff = git.Diff().SetPathFilter(PathFilter.Create(file)).SetOldTree(GetTreeIterator(rev.GetParent(0).Tree.Name, repository)).SetNewTree(GetTreeIterator(rev.Tree.Name, repository)).SetOutputStream(@out);
            IList<DiffEntry> entries = diff.Call();
            return @out.ToString();
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
