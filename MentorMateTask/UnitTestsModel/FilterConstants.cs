using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorMateTask.UnitTestsModel
{
    public class FilterConstants
    {
        public class DisplaySize
        {
            public const string UpTo_10_inch = "up to 10\"";

            public const string Size_11_12_inch = "11\" - 12\"";

            public const string Size_13_14_inch = "13\" - 14\"";

            public const string Size_15_16_inch = "15\" - 16\"";

            public const string Size_17_and_More = "17\" & More";
        }

        public class CPUType
        {
            public const string Intel_Core_i5 = "Intel Core i5";

            public const string Intel_Core_i7 = "Intel Core i7";

            public const string Intel_Pentium = "Intel Pentium";

            public const string Intel_Core_i3 = "Intel Core i3";

            public const string Intel_Atom = "Intel Atom";

            public const string Intel_Celeron = "Intel Celeron";

            public const string AMD_A_Series = "AMD A-Series";
        }

        public class StorageType
        {
            public const string HDD = "HDD";
            public const string SSD = "SSD";
            public const string SSHD = "SSHD";
        }
    }
}

