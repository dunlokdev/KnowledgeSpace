using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeSpace.Data.Common
{
    public class Pagination<T>
    {
        public List<T> Data { get; set; }
        public int TotalRecords { get; set; }
    }
}
