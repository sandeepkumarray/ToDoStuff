using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ToDoStuff.Model
{
    class ContentXMLDataModel
    {
    }

	// using System.Xml.Serialization;
	// XmlSerializer serializer = new XmlSerializer(typeof(Ul));
	// using (StringReader reader = new StringReader(xml))
	// {
	//    var test = (Ul)serializer.Deserialize(reader);
	// }

	[XmlRoot(ElementName = "i")]
	public class I
	{

		[XmlAttribute(AttributeName = "class")]
		public string Class { get; set; }

		[XmlAttribute(AttributeName = "translate")]
		public string Translate { get; set; }

		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "div")]
	public class Div
	{

		[XmlElement(ElementName = "i")]
		public I I { get; set; }

		[XmlAttribute(AttributeName = "class")]
		public string Class { get; set; }

		[XmlAttribute(AttributeName = "tabindex")]
		public int Tabindex { get; set; }

		[XmlText]
		public string Text { get; set; }

		[XmlElement(ElementName = "strong")]
		public string Strong { get; set; }

		[XmlElement(ElementName = "span")]
		public Span Span { get; set; }

		[XmlElement(ElementName = "div")]
		public List<Div> Divs { get; set; }

		[XmlElement(ElementName = "p")]
		public List<string> P { get; set; }
	}

	[XmlRoot(ElementName = "span")]
	public class Span
	{

		[XmlAttribute(AttributeName = "class")]
		public string Class { get; set; }

		[XmlElement(ElementName = "em")]
		public string Em { get; set; }

		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "li")]
	public class Li
	{

		[XmlElement(ElementName = "div")]
		public List<Div> Div { get; set; }
	}

	[XmlRoot(ElementName = "ul")]
	public class Ul
	{

		[XmlElement(ElementName = "li")]
		public List<Li> Li { get; set; }

		[XmlAttribute(AttributeName = "class")]
		public string Class { get; set; }

		[XmlText]
		public string Text { get; set; }
	}


}
