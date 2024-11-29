namespace DataStoring
{
    public class DefaultPathsProvider : IPathsProvider
    {
        private const string LevelPlaylists = "LevelPlaylists";

        public string GetPlaylistFilePath(string playlistName)
        {
            return GetDistancePath(LevelPlaylists, $"{playlistName}.xml");
        }

        private string GetDistancePath(params string[] subDirs)
        {
            string myDocumentsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string path = Path.Combine([myDocumentsFolderPath, "My Games", "Distance", .. subDirs]);
            return path;
        }
    }
}
