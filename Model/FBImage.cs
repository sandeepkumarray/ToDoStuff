using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff.Model
{
    public class Author
    {
        public string @type { get; set; }
        public string name { get; set; }
        public long identifier { get; set; }
        public string url { get; set; }
        public string image { get; set; }
        public string sameAs { get; set; }
        public DateTime foundingDate { get; set; }
    }

    public class InteractionStatistic
    {
        public string @type { get; set; }
        public string interactionType { get; set; }
        public int userInteractionCount { get; set; }
    }

    public class Image
    {
        public string @type { get; set; }
        public int width { get; set; }
        public string contentUrl { get; set; }
        public string caption { get; set; }
    }

    public class FBImage
    {
        public string @type { get; set; }
        public string headline { get; set; }
        public string identifier { get; set; }
        public Author author { get; set; }
        public DateTime dateCreated { get; set; }
        public DateTime dateModified { get; set; }
        public IList<InteractionStatistic> interactionStatistic { get; set; }
        public int commentCount { get; set; }
        public IList<object> comment { get; set; }
        public Image image { get; set; }
        public string url { get; set; }
        public string isPartOf { get; set; }
        public string @context { get; set; }
    }


}
