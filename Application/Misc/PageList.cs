using System.Collections.Generic;
using SharedLibraryCore.Interfaces;

namespace IW4MAdmin.Application.Misc
{
    /// <summary>
    /// implementatin of IPageList that supports basic
    /// pages title and page location for webfront
    /// </summary>
    class PageList : IPageList
    {
        /// <summary>
        /// Pages dictionary
        /// Key = page name
        /// Value = page location (url)
        /// </summary>
        public IDictionary<string, string> Pages { get; set; }

        public PageList()
        {
            Pages = new Dictionary<string, string>();
        }
    }
}
