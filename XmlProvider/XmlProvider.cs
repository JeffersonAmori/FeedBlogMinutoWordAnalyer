using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Providers
{
    public class XmlProvider
    {
        public rss Deserialize(string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            rss result;

            var serializer = new XmlSerializer(typeof(rss));

            using (var stream = new StreamReader(filePath))
            using (var reader = XmlReader.Create(stream))
            {
                result = (rss)serializer.Deserialize(reader);
            }

            return result;
        }
    }
}
