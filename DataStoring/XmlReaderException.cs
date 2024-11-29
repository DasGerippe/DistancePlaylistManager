using System.Xml;

namespace DataStoring
{
    public class XmlReaderException : Exception
    {
        private readonly XmlReader _Reader;

        public override string Message =>
            $"""
             {base.Message}
             XmlReader: ReadState = {_Reader.ReadState}; NodeType = {_Reader.NodeType}; Name = {_Reader.Name}
             """;

        internal XmlReaderException(string message, XmlReader reader) : base(message)
        {
            _Reader = reader;
        }
    }
}
