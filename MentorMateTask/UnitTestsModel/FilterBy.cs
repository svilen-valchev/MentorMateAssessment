using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorMateTask.UnitTestsModel
{
    public class FilterBy
    {
        #region Private Members
        private const string _XPathFilter = "//span[text() = '{0}']";
        #endregion

        #region Public Properties
        public string Criteria
        {
            get; private set;
        }

        public string Name
        {
            get; private set;
        }

        public string Xpath
        {
            get { return _XPathFilter; }
        }
        #endregion

        #region Public Static Methods
        public static FilterBy StorageType(string storageType)
        {
            if (storageType == null)
            {
                throw new ArgumentNullException("storage", "Cannot filter when storageType is null.");

            }
            FilterBy b = new FilterBy();
            b.Criteria = storageType;
            b.Name = "Storage Type";

            return b;
        }

        public static FilterBy CPUType(string cpuType)
        {
            if (cpuType == null)
            {
                throw new ArgumentNullException("storage", "Cannot filter when storageType is null.");

            }
            FilterBy b = new FilterBy();
            b.Criteria = cpuType;
            b.Name = "CPU Type";

            return b;
        }

        public static FilterBy DisplaySize(string displaySize)
        {
            if (displaySize == null)
            {
                throw new ArgumentNullException("storage", "Cannot filter when storageType is null.");

            }
            FilterBy b = new FilterBy();
            b.Criteria = displaySize;
            b.Name = "Display Size";

            return b;
        }
        #endregion
    }
}
