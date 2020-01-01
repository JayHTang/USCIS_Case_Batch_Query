using System.Collections.Generic;

namespace USCIS_Case_Batch_Query
{
    class CaseComparer : IComparer<CaseStatus>
    {
        public int Compare(CaseStatus x, CaseStatus y)
        {
            return x.ReceiptNumber.CompareTo(y.ReceiptNumber);
        }
    }
}
