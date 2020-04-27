using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorMateTask.UnitTestsModel
{
    public interface IErrorLogger
    {
        void AddError(string error);
        string GetLastError();
    }
}
