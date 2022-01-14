using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateAPIDoc
{
    class Contents
    {
        public static void DefineTableOfContents(Document document)
        {
            Section section = document.LastSection;

            section.AddPageBreak();
            Paragraph paragraph = section.AddParagraph("Contents");
            paragraph.Format.Font.Size = 14;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.SpaceAfter = 24;
            paragraph.Format.OutlineLevel = OutlineLevel.Level1;

            paragraph = section.AddParagraph();
            paragraph.Style = "TOC";
            Hyperlink hyperlink = paragraph.AddHyperlink("API Description");
            hyperlink.AddText("API Description\t");
            hyperlink.AddPageRefField("API Description");

            //paragraph = section.AddParagraph();
            //paragraph.Style = "TOC";
            //hyperlink = paragraph.AddHyperlink("Tables");
            //hyperlink.AddText("Tables\t");
            //hyperlink.AddPageRefField("Tables");

            //paragraph = section.AddParagraph();
            //paragraph.Style = "TOC";
            //hyperlink = paragraph.AddHyperlink("Charts");
            //hyperlink.AddText("Charts\t");
            //hyperlink.AddPageRefField("Charts");


            TextFrame tf = section.AddTextFrame();
            tf.WrapFormat.Style = WrapStyle.Through;
            tf.WrapFormat.DistanceLeft = "5mm";
            tf.WrapFormat.DistanceTop = "5mm";
            tf.RelativeHorizontal = RelativeHorizontal.Page;
            tf.RelativeVertical = RelativeVertical.Page;
            tf.Width = "200mm"; // A 4 is 210
            tf.Height = "287mm"; // A 4 is 297
            tf.LineFormat.Width = "5pt";
            tf.LineFormat.Color = new Color(204, 0, 51);// Colors.Red;
        }
    }
}
