namespace DataStoring
{
    public class DefaultPathsProvider : IPathsProvider
    {
        private const string LevelPlaylists = "LevelPlaylists";

        private readonly char[] _ValidPathChars = [' ', '_', '.'];

        public string GetPlaylistFilePath(string playlistName)
        {
            char[] playlistFileNameChars = playlistName
                .Where(nameChar => char.IsLetterOrDigit(nameChar) || _ValidPathChars.Contains(nameChar))
                .ToArray();

            string playlistFileName = new string(playlistFileNameChars);
            string playlistFilePath = GetDistancePath(LevelPlaylists, $"{playlistFileName}.xml");
            return playlistFilePath;
        }

        private string GetDistancePath(params string[] subPaths)
        {
            string myDocumentsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string distancePath = Path.Combine([myDocumentsFolderPath, "My Games", "Distance", .. subPaths]);
            return distancePath;
        }
    }
}
