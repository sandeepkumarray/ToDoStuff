using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigraDoc.DocumentObjectModel.Tables;

namespace CreateAPIDoc
{
    class APIDocument
    {
        public static Document CreateDocument(APIDocInfoModel apiDocInfoModel)
        {
            // Create a new MigraDoc document
            Document document = new Document();
            document.Info.Title = apiDocInfoModel.Title;
            document.Info.Subject = apiDocInfoModel.Subject;
            document.Info.Author = apiDocInfoModel.Author;

            Styles.DefineStyles(document);

            Cover.DefineCover(document);
            Contents.DefineTableOfContents(document);
            DefineContentSection(document);
            //Paragraphs.DefineParagraphs(document);

            //Tables.DefineTables(document);

            if (apiDocInfoModel.DocInfoList != null && apiDocInfoModel.DocInfoList.Count > 0)
            {
                foreach (var docInfo in apiDocInfoModel.DocInfoList)
                {
                    foreach (var obj in docInfo.SectionObjects)
                    {
                        Paragraph para;
                        Table tbl;

                        if(obj.GetType() ==typeof(Paragraph))
                        { 
                            para = (Paragraph)obj;
                            document.LastSection.Add(para);
                        }

                        if (obj.GetType() == typeof(Table))
                        {
                            tbl = (Table)obj;
                            document.LastSection.Add(tbl);
                        }

                    }
                }
            }
            //Charts.DefineCharts(document);

            return document;
        }

        static void DefineContentSection(Document document)
        {
            Section section = document.AddSection();
            section.PageSetup.OddAndEvenPagesHeaderFooter = true;
            section.PageSetup.StartingNumber = 1;

            HeaderFooter header = section.Headers.Primary;
            //header.AddParagraph("\tOdd Page Header");

            header = section.Headers.EvenPage;
            //header.AddParagraph("Even Page Header");

            // Create a paragraph with centered page number. See definition of style "Footer".
            Paragraph paragraph = new Paragraph();
            paragraph.AddTab();
            paragraph.AddPageField();

            // Add paragraph to footer for odd pages.
            section.Footers.Primary.Add(paragraph);
            // Add clone of paragraph to footer for odd pages. Cloning is necessary because an object must
            // not belong to more than one other object. If you forget cloning an exception is thrown.
            section.Footers.EvenPage.Add(paragraph.Clone());

        }
    }
}
