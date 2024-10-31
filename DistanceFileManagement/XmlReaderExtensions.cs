using System.Xml;

namespace DistanceFileManagement
{
    internal static class XmlReaderExtensions
    {
        internal static XmlReader ReadOrThrow(this XmlReader reader)
        {
            if (!reader.Read())
                throw new XmlReaderException("Could not read next node.", reader);

            return reader;
        }

        private static bool Is(this XmlReader reader, XmlNodeType nodeType, string name)
        {
            return reader.NodeType == nodeType && reader.Name == name;
        }

        private static XmlReader EnsureIs(this XmlReader reader, XmlNodeType nodeType, string name)
        {
            if (!reader.Is(nodeType, name))
                throw new XmlReaderException($"Expected current node to be {nodeType} '{name}'.", reader);

            return reader;
        }

        internal static bool IsElement(this XmlReader reader, string elementName)
        {
            return reader.Is(XmlNodeType.Element, elementName);
        }

        internal static XmlReader EnsureIsElement(this XmlReader reader, string elementName)
        {
            return reader.EnsureIs(XmlNodeType.Element, elementName);
        }

        internal static bool IsEndElement(this XmlReader reader, string elementName)
        {
            return reader.Is(XmlNodeType.EndElement, elementName);
        }

        internal static XmlReader EnsureIsEndElement(this XmlReader reader, string elementName)
        {
            return reader.EnsureIs(XmlNodeType.EndElement, elementName);
        }

        internal static XmlReader EnsureAttribute(this XmlReader reader, string attributeName, string value)
        {
            string? attributeValue = reader.GetAttribute(attributeName);
            if (attributeValue != value)
                throw new XmlReaderException(
                    $"Expected attribute '{attributeName}' with value '{value}' but got value '{attributeValue}' instead.",
                    reader);

            return reader;
        }
    }
}
