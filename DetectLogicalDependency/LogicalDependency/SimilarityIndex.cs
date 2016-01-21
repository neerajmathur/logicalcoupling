using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetectLogicalDependency.LogicalDependency
{
    class SimilarityIndex
    {

        public int Support { get; set; }

        public int Confidence { get; set; }

        public List<string> KeywordsList { get; set; }

    }
}
