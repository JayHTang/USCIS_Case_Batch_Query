using System;
using System.Collections.Generic;
using System.Text;

namespace USCIS_Case_Batch_Query
{
    class SearchQuery
    {
        public SearchQuery()
        {
            ReceiptNumber = "";
            NextCases = 0;
        }
        public string ReceiptNumber { get; set; }
        public int NextCases { get; set; }
    }
}
