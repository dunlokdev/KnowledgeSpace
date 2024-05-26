using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeSpace.ViewModels
{
    public class Pagination<T>
    {
        public List<T> Data { get; set; }
        public int TotalRecord { get; set; }
    }
}
