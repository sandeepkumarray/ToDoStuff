using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;

namespace CreateAPIDoc
{
    public class APIDocInfoModel
    {

        public List<DocInfoModel> DocInfoList { get; set; }

        public string Title { get; set; }
        public string Author { get; set; }
        public string Keywords { get; set; }
        public string Subject { get; set; }
        public string Comment { get; set; }
        public string FilePath { get; set; }
        public string Version { get; set; }

    }

    public class DocInfoModel
    {
        public List<DocumentObject> SectionObjects { get; set; }

        public DocInfoModel()
        {
            SectionObjects = new List<DocumentObject>();
        }
    }
}
