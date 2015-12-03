using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MediaRss.Optional
{
    [XmlRoot(ElementName = "thumbnail", Namespace = "http://search.yahoo.com/mrss/")]
    public class Thumbnail : MediaRssBase, IXmlSerializable
    {
        internal const string ELEMENT_NAME = "thumbnail";

        public Uri Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        #region IXmlSerializable Members

        public void ReadXml(XmlReader reader)
        {
            bool isEmpty = reader.IsEmptyElement;

            if (reader.HasAttributes)
            {
                for (int i = 0; i < reader.AttributeCount; i++)
                {
                    reader.MoveToNextAttribute();

                    if (reader.NamespaceURI == "")
                    {
                        if (reader.LocalName == "url")
                        {
                            Url = new Uri(reader.Value);
                        }
                        else if (reader.LocalName == "width")
                        {
                            Width = int.Parse(reader.Value);
                        }
                        else if (reader.LocalName == "height")
                        {
                            Height = Int32.Parse(reader.Value);
                        }
                        else
                        {
                            AttributeExtensions.Add(new XmlQualifiedName(reader.LocalName, reader.NamespaceURI), reader.Value);
                        }
                    }
                }
            }

            reader.ReadStartElement();

            AddElementExtensions(reader, isEmpty);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendFormat("Url: {0}\n", Url);
            builder.AppendFormat("Width: {0}\n", Width);
            builder.AppendFormat("Height: {0}\n", Height);

            if (ElementExtensions.Count > 0)
            {
                foreach (XElement elt in ElementExtensions)
                {
                    builder.AppendLine(elt.ToString());
                }
            }
            return builder.ToString();
        }

        public void WriteXml(XmlWriter writer)
        {
            if (Url != null)
            {
                writer.WriteAttributeString("url", null, Url.ToString());
            }
            if (Width > 0) writer.WriteAttributeString("width", null, Width.ToString());
            if (Height > 0) writer.WriteAttributeString("height", null, Height.ToString());

            foreach (KeyValuePair<XmlQualifiedName, string> kvp in AttributeExtensions)
            {
                writer.WriteAttributeString(kvp.Key.Name, kvp.Key.Namespace, kvp.Value);
            }

            foreach (XElement element in ElementExtensions)
            {
                element.WriteTo(writer);
            }
        }

        #endregion
    }
}