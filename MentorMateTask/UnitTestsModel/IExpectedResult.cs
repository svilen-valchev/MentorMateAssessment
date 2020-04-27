using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorMateTask.UnitTestsModel
{
    public interface IExpectedResult<T> where T : new()
    {
        T ExpectedResult { get; set; }
    }
}
