using DataClasses;
using System.Xml;

namespace DistanceFileManagement
{
    public sealed class PlaylistDeserializer
    {
        public Playlist Deserialize(Stream stream)
        {
            XmlReaderSettings readerSettings = new XmlReaderSettings
            {
                IgnoreWhitespace = true,
            };

            using XmlReader reader = XmlReader.Create(stream, readerSettings);
            reader.ReadOrThrow();

            reader.EnsureIsElement("GameObject").EnsureAttribute("Name", "LevelPlaylist").ReadOrThrow();
            reader.EnsureIsElement("Transform").ReadOrThrow();
            reader.EnsureIsElement("LevelPlaylist").ReadOrThrow();

            string playlistName = reader.EnsureIsElement("PlaylistName").ReadElementContentAsString();
            int levelCount = reader.EnsureIsElement("NumberOfLevelsInPlaylist").ReadElementContentAsInt();

            reader.EnsureIsElement("ModeAndLevelInfoVersion").Skip();
            reader.EnsureIsElement("RequiredMedalCount").Skip();

            IList<PlaylistLevel> levels = ReadPlaylistLevels(reader, levelCount);

            reader.EnsureIsEndElement("LevelPlaylist").ReadOrThrow();
            reader.EnsureIsEndElement("GameObject");

            Playlist playlist = new Playlist
            {
                Name = playlistName,
                Levels = levels,
            };

            return playlist;
        }

        private IList<PlaylistLevel> ReadPlaylistLevels(XmlReader reader, int levelCount)
        {
            IList<PlaylistLevel> levels = new List<PlaylistLevel>(levelCount);

            while (reader.IsElement("GameMode"))
            {
                int gameModeLiteral = reader.ReadElementContentAsInt();
                GameMode gameMode = (GameMode)gameModeLiteral;

                string levelName = reader.EnsureIsElement("LevelName").ReadElementContentAsString();
                string levelPath = reader.EnsureIsElement("LevelPath").ReadElementContentAsString();

                PlaylistLevel playlistLevel = new PlaylistLevel
                {
                    GameMode = gameMode,
                    LevelName = levelName,
                    LevelPath = levelPath,
                };
                levels.Add(playlistLevel);
            }

            return levels;
        }
    }
}
