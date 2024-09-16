using DataClasses;
using System.Text;
using System.Xml;

namespace DistanceFileManagement
{
    public class PlaylistSerializer
    {
        public void Serialize(Playlist playlist, Stream stream)
        {
            using XmlTextWriter writer = new XmlTextWriter(stream, Encoding.Unicode);
            writer.Formatting = Formatting.Indented;

            WriteGameObjectElement(writer, playlist);

            writer.Flush();
        }

        private void WriteGameObjectElement(XmlWriter writer, Playlist playlist)
        {
            writer.WriteStartElement("GameObject");
            writer.WriteAttributeString("Name", "LevelPlaylist");
            writer.WriteAttributeString("GUID", "0");

            WriteTransformElement(writer, playlist);
            WriteLevelPlaylistElement(writer, playlist);

            writer.WriteEndElement();
        }

        private void WriteTransformElement(XmlWriter writer, Playlist playlist)
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
            string serializedGameMode = ((int)playlist.GameMode).ToString();

            foreach (Level level in playlist.Levels)
            {
                writer.WriteElementString("GameMode", serializedGameMode);
                writer.WriteElementString("LevelName", level.Name);
                writer.WriteElementString("LevelPath", GetLevelPath(level));
            }
        }

        private string GetLevelPath(Level level)
        {
            return level.Source != LevelSource.Workshop
                ? $"{level.Source}Levels/{level.FileName}"
                : $"{level.Source}Levels/{level.CreatorId}/{level.FileName}";
        }
    }
}
