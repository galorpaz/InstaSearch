using InstaSearchWebApp.Models;
using System.Collections.Generic;

namespace InstaSearchWebApp.Common
{
    public class ViewModelSearch
    {
        public List<Search> SearchHistory { get; set; }
        public SearchResult Result { get; set; }
        public string StringResults { get; set; }
        public string ErrorMsg { get; set; }
    }
}