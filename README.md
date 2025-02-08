# Distance Playlist Manager

A tool for the racing game Distance that offers advanced playlist management features.

## Features

- Retrieve the levels of a workshop collection and add them to a new or existing playlist.
- Randomize levels in a playlist.

## How To Use

Downloads can be found on the [Releases](https://github.com/DasGerippe/DistancePlaylistManager/releases) page.

The tool works like a classic command line tool, meaning you give it a command which specifies what the tool should do. Commands can be supplied in two ways:

  - Run (double-click) the tool and enter your command in the interactive mode. (The rest of the documentation assumes this method.)
  - Call the tool from a command line/script/... and pass your comamnd as an argument.

## Commands

Commands can have `<argument>`s that are required and/or `[option]`s that are... *option*al. When an argument contains spaces, it needs to be enclosed in "double quotes".

### collection

- Syntax: `collection <collection-url-or-id> [--playlist <name>] [--gamemode <mode>]`
- Arguments/Options:
  - `<collection-url-or-id>`: The URL or Steam ID of a workshop collection.
  - `[--playlist <name>]`: The name of the playlist to which the levels are added. If omitted, the collection name and game mode are used to generate a name.
  - `[--gamemode <mode>]`: Only levels that support the specified game mode will be added to the playlist. Possible parameter values are Sprint, Challenge, Stunt, or ReverseTag. Default is Sprint.
- Examples:
  - `collection 2799461592`
    - Retrieves all levels from the collection "Competitive Levels" and adds them to the playlist "Competitive Levels (Sprint)".
  - `collection 2087489130 --playlist "Air Control Challenges" --gamemode Challenge`
    - Retrieves all Challenge levels from the collection "All air control style maps v2" and adds them to the playlist "Air Control Challenges".

### playlist shuffle

- Syntax: `playlist shuffle <playlist>`
- Arguments/Options:
  - `<playlist>`: The name of the playlist to be shuffled.
- Examples:
  - `playlist shuffle Favorites`
  - `playlist shuffle "Competitive Levels"`

## Additional Notes

Distance seems to (re)load your playlists in the following situations:

- When you select a game mode in Arcade.
- When you open the level selector in a multiplayer lobby.
- When you return to main menu from a level.

Keep that in mind if you're using the tool while Distance is running.

Also, I had no problems with cloud sync when using the tool, whether Distance was running or not.
