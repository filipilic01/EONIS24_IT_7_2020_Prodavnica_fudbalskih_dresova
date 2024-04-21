using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class DresSpecParams
    {
        private const int MaxPageSize = 50;
        public int PageIndex { get; set; }

        private int _pageSize = 6;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string? Tim { get; set; }
        public string? Brend { get; set; }
        public string? Takmicenje { get; set; }
        public string? Player { get; set; }
        public string? Sort { get; set; }

        private string _search;

        public string? Search
        {
            get => _search;
            set => _search = value.ToLower();
        }
    }
}
