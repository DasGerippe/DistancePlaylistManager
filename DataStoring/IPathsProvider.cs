namespace DataStoring
{
    public interface IPathsProvider
    {
        string GetPlaylistsFolderPath();

        string GetPlaylistFilePath(string playlistName);
    }
}
