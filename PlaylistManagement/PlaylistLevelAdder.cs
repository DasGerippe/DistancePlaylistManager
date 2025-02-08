using DataClasses;

namespace PlaylistManagement
{
    public class PlaylistLevelAdder
    {
        private readonly IReadOnlyCollection<GameMode> _GameModesByPreference = new List<GameMode>
        {
            GameMode.Sprint,
            GameMode.Challenge,
            GameMode.Stunt,
            GameMode.ReverseTag,
        }.AsReadOnly();

        public void AddLevelsToPlaylist(Playlist playlist, IEnumerable<Level> levels, GameMode? gameMode)
        {
            levels = gameMode.HasValue
                ? levels.Where(level => level.GameModes.Contains(gameMode.Value))
                : levels.Where(level => level.GameModes.Count > 0);

            IEnumerable<PlaylistLevel> levelsToAdd = levels
                .Select(level => new PlaylistLevel
                {
                    GameMode = gameMode ?? GetPreferredGameMode(level),
                    LevelName = level.Name,
                    LevelPath = level.GetLevelPath(),
                })
                .Distinct()
                .Except(playlist.Levels);

            foreach (PlaylistLevel playlistLevel in levelsToAdd)
            {
                playlist.Levels.Add(playlistLevel);
            }
        }

        private GameMode GetPreferredGameMode(Level level)
        {
            return _GameModesByPreference.FirstOrDefault(gameMode => level.GameModes.Contains(gameMode));
        }
    }
}
