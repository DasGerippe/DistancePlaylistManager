# Distance Playlist Manager

A tool for the racing game Distance that offers advanced playlist management features.

## Current Features

- Create a playlist with the levels from a workshop collection.

## Planned Features

- Add levels from a workshop collection to an existing playlist.
- Randomize levels in a playlist.
- Remove duplicate levels from a playlist.
- Simple playlist management:
  - create new
  - copy
  - rename
  - delete
- Advanced playlist management (probably implemented with a GUI):
  - Add or insert levels to a playlist.
  - Remove levels from a playlist.
  - Manually order levels in a playlist.
- GUI

I'm also open to feature requests, though I wont promise implementing any of the requested or planned features.

## How To

### Preparation

Downloads can be found on the [Releases](https://github.com/DasGerippe/DistancePlaylistManager/releases) page.

If you don't want to install anything, download the self-contained DistancePlaylistManager.exe for your operating system and run it.

***OR***

If you don't have it installed already, download and install the [.NET 8 runtime](https://dotnet.microsoft.com/en-us/download). Next, download the DistancePlaylistManager.zip, extract it to any location, and execute the DistancePlaylistManager.exe.

### Usage

Once the console window has opened, simply enter the ID or URL of a Distance workshop collection, the game mode, and optionally a name for the playlist.

The tool will collect all the information necessary and create a playlist file in your \Distance\LevelPlaylists folder.

You will still need to subscribe to the collection manually.

**To those using Linux:** I have only tested this tool on Windows, though it *might* work on Linux as well. If you tried using it on Linux, let me know how it went.

### Tips and Tricks... or something like that

Distance seems to (re)load your playlists in the following situations:

- When you select a game mode in Arcade.
- When you open the level selector in a multiplayer lobby.
- When you return to main menu from a level.

Keep that in mind if you're using the tool while Distance is running.

Also, I had no problems with cloud sync when using the tool, whether Distance was running or not. Though I suspect this will change with future features like renaming or deleting... But that's a problem future-me will have to figure out.
