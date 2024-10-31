using DataClasses;
using System.Text;
using System.Xml;

namespace DistanceFileManagement
{
    public sealed class PlaylistSerializer
    {
        public void Serialize(Stream stream, Playlist playlist)
        {
            XmlWriterSettings writerSettings = new XmlWriterSettings()
            {
                Encoding = Encoding.Unicode,
                OmitXmlDeclaration = true,
                Indent = true,
            };

            using XmlWriter writer = XmlWriter.Create(stream, writerSettings);

            WriteGameObjectElement(writer, playlist);

            writer.Flush();
        }

        private void WriteGameObjectElement(XmlWriter writer, Playlist playlist)
        {
            writer.WriteStartElement("GameObject");
            writer.WriteAttributeString("Name", "LevelPlaylist");
            writer.WriteAttributeString("GUID", "0");

            WriteTransformElement(writer);
            WriteLevelPlaylistElement(writer, playlist);

            writer.WriteEndElement();
        }

        private void WriteTransformElement(XmlWriter writer)
        {
            writer.WriteStartElement("Transform");
            writer.WriteAttributeString("Version", "0");
            writer.WriteAttributeString("GUID", "0");
            writer.WriteEndElement();
        }

        private void WriteLevelPlaylistElement(XmlWriter writer, Playlist playlist)
        {
            writer.WriteStartElement("LevelPlaylist");
            writer.WriteAttributeString("Version", "2");
            writer.WriteAttributeString("GUID", "0");

            writer.WriteElementString("PlaylistName", playlist.Name);
            writer.WriteElementString("NumberOfLevelsInPlaylist", playlist.Levels.Count.ToString());
            writer.WriteElementString("ModeAndLevelInfoVersion", "0");
            writer.WriteElementString("RequiredMedalCount", "0");

            WriteLevelsElements(writer, playlist);

            writer.WriteEndElement();
        }

        private void WriteLevelsElements(XmlWriter writer, Playlist playlist)
        {
            foreach (PlaylistLevel level in playlist.Levels)
            {
                writer.WriteElementString("GameMode", ((int)level.GameMode).ToString());
                writer.WriteElementString("LevelName", level.LevelName);
                writer.WriteElementString("LevelPath", level.LevelPath);
            }
        }
    }
}
