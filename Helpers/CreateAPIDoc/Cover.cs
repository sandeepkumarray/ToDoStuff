#region MigraDoc - Creating Documents on the Fly
//
// Authors:
//   PDFsharp Team (mailto:PDFsharpSupport@pdfsharp.de)
//
// Copyright (c) 2001-2009 empira Software GmbH, Cologne (Germany)
//
// http://www.pdfsharp.com
// http://www.migradoc.com
// http://sourceforge.net/projects/pdfsharp
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
#endregion

using System;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;

namespace CreateAPIDoc
{
    public class Cover
    {
        /// <summary>
        /// Defines the cover page.
        /// </summary>
        public static void DefineCover(Document document)
        {
            Section section = document.AddSection();

            Paragraph paragraph = section.AddParagraph();
            paragraph.Format.SpaceAfter = "0cm";

            Image image = section.AddImage("../../images/my_World_Logo.png");
            image.Left = ShapePosition.Center;
            image.Width = "8cm";

            paragraph = section.AddParagraph("My World API Documentation.");
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.Format.Font.Size = 40;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Font.Color = Colors.DarkRed;
            paragraph.Format.SpaceBefore = "4cm";
            paragraph.Format.SpaceAfter = "2cm";

            paragraph = section.AddParagraph("Created By :");
            paragraph.Format.Alignment = ParagraphAlignment.Right;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Font.Size = 18;
            paragraph.Format.Font.Color = Colors.DarkRed;
            paragraph.Format.SpaceBefore = "5cm";
            paragraph.Format.SpaceAfter = "0cm";


            paragraph = section.AddParagraph("Sandeep Ray");
            paragraph.Format.Alignment = ParagraphAlignment.Right;
            paragraph.Format.Font.Size = 10;
            paragraph.Format.Font.Color = Colors.DarkRed;

            paragraph = section.AddParagraph("Date: " + Convert.ToDateTime("05-01-2021").ToString("dd-mm-yyyy"));
            paragraph.Format.Alignment = ParagraphAlignment.Right;
            paragraph.Format.Font.Size = 10;
            paragraph.Format.Font.Color = Colors.DarkRed;

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
