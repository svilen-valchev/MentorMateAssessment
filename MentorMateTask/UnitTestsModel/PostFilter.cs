using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorMateTask.UnitTestsModel
{
    public enum SortOrder
    {
        None,
        LowestPrice,
        HighestPrice
    }
    public class PostFilter
    {
        public string Rating { get; set; }

        public SortOrder Sort { get; set; }
    }
}
