# LocalizationHelper

Helper to localize textures in your mods

## Summary

- [Usage](#Usage)
  - [Simple format](#Simple-format)
  - [Advanced format](#Advanced-format)
    - [Aliases](#Aliases)
    - [Frame handling](#Frame-handling)
- [Language IDs](#Language-IDs)

## Usage

Put a `LocalizationTextures.yaml` in your mod root folder to specify the new texture for each language.

### Simple format

It has this format:
```yaml
french:
  Graphics/Atlases/Gameplay/decals/ExampleMap/stuffs/stuff1: Graphics/Atlases/Gameplay/decals/ExampleMap/stuffs/french_stuff1
  Graphics/Atlases/Portraits/stuff/ExampleMap/memo_title: Graphics/Atlases/Portraits/stuff/ExampleMap/french_memo_title
german:
  Graphics/Atlases/Gameplay/decals/ExampleMap/stuffs/stuff1: Graphics/Atlases/Gameplay/decals/ExampleMap/stuffs/german_stuff1
```
You can name the localized textures however you want, but you have to put them in the same data path than the original texture.

For example: if in Lönn the path for the texture is `ExampleMap/stuffs/stuff1` and it points to `Graphics/Atlases/Gameplay/decals/ExampleMap/stuffs/stuff1.png`, the data path is `Graphics/Atlases/Gameplay/decals`, so you have to put your localized textures for this texture somewhere in `Graphics/Atlases/Gameplay/decals`

### Advanced format

Depending on your usage, you may need more advanced features to ease your Localization work.

> **BE CAREFUL** of the formatting. When using the advanced format, the yaml has to follow the structure metadatas > (aliases, etc.) and languages > (the languages where the localization happen) if specified!  

#### Aliases

*⚠️ Aliases require the metadatas structure ⚠️*

If you start to have a lot of languages localized in your file, you can end up in the case where you specify the same original texture in your `.yaml` file.  
This can lead to issues when moving assets to different folders or merging multiple folders, as it can break the localization. Fortunately, **aliases** are here to solve this issue.  

For example, you have this starting `LocalizationTextures.yaml` file:

```yaml
brazilian:
  Graphics/Atlases/Gameplay/decals/ExampleMap/stuffs/stuff1: Graphics/Atlases/Gameplay/decals/ExampleMap/stuffs/brazilian_stuff1
french:
  Graphics/Atlases/Gameplay/decals/ExampleMap/stuffs/stuff1: Graphics/Atlases/Gameplay/decals/ExampleMap/stuffs/french_stuff1
german:
  Graphics/Atlases/Gameplay/decals/ExampleMap/stuffs/stuff1: Graphics/Atlases/Gameplay/decals/ExampleMap/stuffs/german_stuff1
italian:
  Graphics/Atlases/Gameplay/decals/ExampleMap/stuffs/stuff1: Graphics/Atlases/Gameplay/decals/ExampleMap/stuffs/italian_stuff1
```

You can see that the texture `Graphics/Atlases/Gameplay/decals/ExampleMap/stuffs/stuff1` is repeated in each language, but if you use aliases...  

```yaml
metadatas:
  aliases:
    stuff1: Graphics/Atlases/Gameplay/decals/ExampleMap/stuffs/stuff1
languages:
  brazilian:
    stuff1: Graphics/Atlases/Gameplay/decals/ExampleMap/stuffs/brazilian_stuff1
  french:
    stuff1: Graphics/Atlases/Gameplay/decals/ExampleMap/stuffs/french_stuff1
  german:
    stuff1: Graphics/Atlases/Gameplay/decals/ExampleMap/stuffs/german_stuff1
  italian:
    stuff1: Graphics/Atlases/Gameplay/decals/ExampleMap/stuffs/italian_stuff1
```

Less repetition! The name is clear, the path is only written one time, making it easier to update it if needed.

#### Frame handling

If, during your localization work, you happen to localize an animation, you may have noticed that giving all the frame can be quite tedious and annoying. But fear not, LocalizationHelper can handle the frames for you!

Take this yaml example:

```yaml
...
french:
  flags_canada: Graphics/Atlases/Gameplay/decals/Vireth/LocalizationHelper/flags/france
  country_text: Graphics/Atlases/Gameplay/decals/Vireth/LocalizationHelper/countryTexts/france
  Graphics/Atlases/Gameplay/bgs/Vireth/LocalizationHelper/background: Graphics/Atlases/Gameplay/bgs/Vireth/LocalizationHelper/french_background
  Graphics/Atlases/Gameplay/characters/Vireth/LocalizationHelper/bleh/idle00: Graphics/Atlases/Gameplay/characters/Vireth/LocalizationHelper/french_bleh/idle00
  Graphics/Atlases/Gameplay/characters/Vireth/LocalizationHelper/bleh/idle01: Graphics/Atlases/Gameplay/characters/Vireth/LocalizationHelper/french_bleh/idle01
  Graphics/Atlases/Gameplay/characters/Vireth/LocalizationHelper/bleh/idle02: Graphics/Atlases/Gameplay/characters/Vireth/LocalizationHelper/french_bleh/idle02
  Graphics/Atlases/Gameplay/characters/Vireth/LocalizationHelper/bleh/idle03: Graphics/Atlases/Gameplay/characters/Vireth/LocalizationHelper/french_bleh/idle03
  Graphics/Atlases/Gameplay/characters/Vireth/LocalizationHelper/bleh/idle04: Graphics/Atlases/Gameplay/characters/Vireth/LocalizationHelper/french_bleh/idle04
  Graphics/Atlases/Gameplay/characters/Vireth/LocalizationHelper/bleh/idle05: Graphics/Atlases/Gameplay/characters/Vireth/LocalizationHelper/french_bleh/idle05
  Graphics/Atlases/Gameplay/characters/Vireth/LocalizationHelper/bleh/idle06: Graphics/Atlases/Gameplay/characters/Vireth/LocalizationHelper/french_bleh/idle06
  Graphics/Atlases/Gameplay/characters/Vireth/LocalizationHelper/bleh/idle07: Graphics/Atlases/Gameplay/characters/Vireth/LocalizationHelper/french_bleh/idle07
  Graphics/Atlases/Gameplay/characters/Vireth/LocalizationHelper/bleh/idle08: Graphics/Atlases/Gameplay/characters/Vireth/LocalizationHelper/french_bleh/idle08
```

Annoying to write, right? Well, you can specify a *parameter*. For frame handling, it has to follow this format: `{FRAME:STARTING_FRAME_NUMBER-ENDING_FRAME_NUMBER}`.   
Here's the updated `LocalizationTextures.yaml` with the parameter added:

```yaml
...
french:
  flags_canada: Graphics/Atlases/Gameplay/decals/Vireth/LocalizationHelper/flags/france
  country_text: Graphics/Atlases/Gameplay/decals/Vireth/LocalizationHelper/countryTexts/france
  Graphics/Atlases/Gameplay/bgs/Vireth/LocalizationHelper/background: Graphics/Atlases/Gameplay/bgs/Vireth/LocalizationHelper/french_background
  Graphics/Atlases/Gameplay/characters/Vireth/LocalizationHelper/bleh/idle{FRAME:00-08}: Graphics/Atlases/Gameplay/characters/Vireth/LocalizationHelper/french_bleh/idle{FRAME:00-08}
```

Much simpler right?  
To use the parameter properly, you have to follow a few formatting rules (but don't be scared, they are fairly simple!).
- Your frames **must** all have the same amount of digit in their name! In our example, they all have 2 digits from 00 to 08. For `{FRAME:000-008}`, it would have been 3 digits, etc.  
- Parameters **have** to match for both side to make it to work. For example, `Graphics/Atlases/Gameplay/characters/Vireth/LocalizationHelper/bleh/idle{FRAME:00-08}: Graphics/Atlases/Gameplay/characters/Vireth/LocalizationHelper/french_bleh/idle{FRAME:01-07}` won't work.  
- **Be careful when writing the parameter.** The helper deduce the digit number by looking at the number of digits in the parameter for the starting or ending frame.
  
If your parameter isn't working, check the logs, when a mismatch happen, the helper logs it.
Note that the FRAME parameter works with aliases values! So writing something like this will work:
```yaml
metadatas:
  aliases:
    idle_anim: my/path/idle{FRAME:00-08}
languages:
  french:
    idle_anim: my/path/idle_french{FRAME:00-08}
```

## Language IDs

The language IDs are the ones used by Celeste, that is:

| Language             | id        |
|----------------------|-----------|
| English              | english   |
| Brazilian Portuguese | brazilian |
| French               | french    |
| German               | german    |
| Italian              | italian   |
| Japanese             | japanese  |
| Korean               | korean    |
| Russian              | russian   |
| Simplified Chinese   | schinese  |
| Spanish              | spanish   |