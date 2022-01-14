using HelloMigraDoc;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateAPIDoc
{
    class Paragraphs
    {
        public static void DefineParagraphs(Document document)
        {
            Paragraph paragraph = document.LastSection.AddParagraph("Paragraph Layout Overview", "Heading1");
            paragraph.AddBookmark("Paragraphs");

            DemonstrateAlignment(document);
            DemonstrateIndent(document);
            DemonstrateFormattedText(document);
            DemonstrateBordersAndShading(document);
        }

        static void DemonstrateAlignment(Document document)
        {
            document.LastSection.AddParagraph("Alignment", "Heading2");

            document.LastSection.AddParagraph("Left Aligned", "Heading3");

            Paragraph paragraph = document.LastSection.AddParagraph();
            paragraph.Format.Alignment = ParagraphAlignment.Left;
            paragraph.AddText(FillerText.Text);

            document.LastSection.AddParagraph("Right Aligned", "Heading3");

            paragraph = document.LastSection.AddParagraph();
            paragraph.Format.Alignment = ParagraphAlignment.Right;
            paragraph.AddText(FillerText.Text);

            document.LastSection.AddParagraph("Centered", "Heading3");

            paragraph = document.LastSection.AddParagraph();
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.AddText(FillerText.Text);

            document.LastSection.AddParagraph("Justified", "Heading3");

            paragraph = document.LastSection.AddParagraph();
            paragraph.Format.Alignment = ParagraphAlignment.Justify;
            paragraph.AddText(FillerText.MediumText);

            TextFrame tf = document.LastSection.AddTextFrame();
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

        static void DemonstrateIndent(Document document)
        {
            document.LastSection.AddParagraph("Indent", "Heading2");

            document.LastSection.AddParagraph("Left Indent", "Heading3");

            Paragraph paragraph = document.LastSection.AddParagraph();
            paragraph.Format.LeftIndent = "2cm";
            paragraph.AddText(FillerText.Text);

            document.LastSection.AddParagraph("Right Indent", "Heading3");

            paragraph = document.LastSection.AddParagraph();
            paragraph.Format.RightIndent = "1in";
            paragraph.AddText(FillerText.Text);

            document.LastSection.AddParagraph("First Line Indent", "Heading3");

            paragraph = document.LastSection.AddParagraph();
            paragraph.Format.FirstLineIndent = "12mm";
            paragraph.AddText(FillerText.Text);

            document.LastSection.AddParagraph("First Line Negative Indent", "Heading3");

            paragraph = document.LastSection.AddParagraph();
            paragraph.Format.LeftIndent = "1.5cm";
            paragraph.Format.FirstLineIndent = "-1.5cm";
            paragraph.AddText(FillerText.Text);

            TextFrame tf = document.LastSection.AddTextFrame();
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

        static void DemonstrateFormattedText(Document document)
        {
            document.LastSection.AddParagraph("Formatted Text", "Heading2");

            //document.LastSection.AddParagraph("Left Aligned", "Heading3");

            Paragraph paragraph = document.LastSection.AddParagraph();
            paragraph.AddText("Text can be formatted ");
            paragraph.AddFormattedText("bold", TextFormat.Bold);
            paragraph.AddText(", ");
            paragraph.AddFormattedText("italic", TextFormat.Italic);
            paragraph.AddText(", or ");
            paragraph.AddFormattedText("bold & italic", TextFormat.Bold | TextFormat.Italic);
            paragraph.AddText(".");
            paragraph.AddLineBreak();
            paragraph.AddText("You can set the ");
            FormattedText formattedText = paragraph.AddFormattedText("size ");
            formattedText.Size = 15;
            paragraph.AddText("the ");
            formattedText = paragraph.AddFormattedText("color ");
            formattedText.Color = Colors.Firebrick;
            paragraph.AddText("the ");
            paragraph.AddFormattedText("font", new Font("Verdana"));
            paragraph.AddText(".");
            paragraph.AddLineBreak();
            paragraph.AddText("You can set the ");
            formattedText = paragraph.AddFormattedText("subscript");
            formattedText.Subscript = true;
            paragraph.AddText(" or ");
            formattedText = paragraph.AddFormattedText("superscript");
            formattedText.Superscript = true;
            paragraph.AddText(".");


            TextFrame tf = document.LastSection.AddTextFrame();
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

        static void DemonstrateBordersAndShading(Document document)
        {
            document.LastSection.AddPageBreak();
            document.LastSection.AddParagraph("Borders and Shading", "Heading2");

            document.LastSection.AddParagraph("Border around Paragraph", "Heading3");

            Paragraph paragraph = document.LastSection.AddParagraph();
            paragraph.Format.Borders.Width = 2.5;
            paragraph.Format.Borders.Color = Colors.Navy;
            paragraph.Format.Borders.Distance = 3;
            paragraph.AddText(FillerText.MediumText);

            document.LastSection.AddParagraph("Shading", "Heading3");

            paragraph = document.LastSection.AddParagraph();
            paragraph.Format.Shading.Color = Colors.LightCoral;
            paragraph.AddText(FillerText.Text);

            document.LastSection.AddParagraph("Borders & Shading", "Heading3");

            paragraph = document.LastSection.AddParagraph();
            paragraph.Style = "TextBox";
            paragraph.AddText(FillerText.MediumText);


            TextFrame tf = document.LastSection.AddTextFrame();
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
