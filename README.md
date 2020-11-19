# AnimeDiscordRichPresence
Anime websites don't have discord rich presence... so I created one.

![Showcase Image](https://raw.githubusercontent.com/PlayingSpree/AnimeDiscordRichPresence/master/Doc/DiscordActivity.png)

(Update: Actually there is [one](https://github.com/MALSync/MALSync/wiki/Discord-Rich-Presence). But this was already created so... ¯\\\_(ツ)_/¯)

## Feature

* Automatically scan window title on specific process for keyword that identify website name then use it to set discord activity (Rich presence).
* Auto hide console window in system tray.
* config.json for adding new website.

## Download
https://github.com/PlayingSpree/AnimeDiscordRichPresence/releases

## How to use
1. Extract .zip to any folder.
1. Edit `config.json` to add website you want to show ([See Adding your website](https://github.com/PlayingSpree/AnimeDiscordRichPresence#adding-your-website))
1. Run AnimeDiscordRichPresence.exe

A console window will pop up and then auto hide in system tray. You can exit the program there.

![System Tray Image](https://raw.githubusercontent.com/PlayingSpree/AnimeDiscordRichPresence/master/Doc/System%20Tray.png)

Want to run at start up? [Guide for windows 10](https://support.microsoft.com/en-us/help/4558286/windows-10-add-an-app-to-run-automatically-at-startup)

## Adding your website
Find pattern from your anime website title

![Example Image](https://raw.githubusercontent.com/PlayingSpree/AnimeDiscordRichPresence/master/Doc/NewWebsite.png)

From above image. Open `config.json` in text editor and add this in the list of `AnimeWebsites`
```
"AnimeWebsites": [
  {
      "Website": "Gogoanime",
      "MatchText": ["Gogoanime"],
      "MatchAnimeNameStartText": ["Watch"],
      "MatchAnimeNameEndText": ["Episode"],
      "MatchEpisodeStartText": ["Episode"],
      "MatchEpisodeEndText": ["English"]
  }
]
```
### Note
`"Website":` set the text that will display below your anime title on discord for that website.

You can put more text in the list if there are multiple pattern.

You can put empty list if there is no infomation or you want to hide it.

Discord activity accept max 128 character.

### More Example

You want to add this 2 website...

**Example website 1**: Website that website title start with anime title and multiple episode ending pattern

Website 1 example title: `Boku no Pico Ep.1 Sub ExampleAnime` and `Yosuga no Sora Ep.1 Dub ExampleAnime`

**Example website 2**: Website that website title end with anime title and no episode info

Website 2 example title: `ExampleAnime2 Steins;Gate`

In `config.json`
```
"AnimeWebsites": [
  {
      "Website": "Example1",
      "MatchText": ["ExampleAnime"],
      "MatchAnimeNameStartText": [],
      "MatchAnimeNameEndText": ["Ep."],
      "MatchEpisodeStartText": ["Ep."],
      "MatchEpisodeEndText": ["Sub","Dub"]
  },
  {
      "Website": "Example2",
      "MatchText": ["ExampleAnime2"],
      "MatchAnimeNameStartText": ["ExampleAnime2"],
      "MatchAnimeNameEndText": [],
      "MatchEpisodeStartText": [],
      "MatchEpisodeEndText": []
  }
]
```

## Add web browser
Add your browser process name to `ProcessNames` in `config.json`.

```
"ProcessNames": [
  "chrome",
  "msedge"
],
```
