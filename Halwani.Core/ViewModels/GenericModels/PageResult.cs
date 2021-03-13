using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ViewModels.GenericModels
{
    public class PageResult<T>
    {
        public PageResult()
        {
            PageData = new List<T>();
        }
        public long TotalCount { get; set; }
        public List<T> PageData { get; set; }
    }
}
