# LocalizationHelper

Helper to localize textures in your mods

## Summary

- [Usage](#Usage)
  - [Simple format](#Simple-format)
  - [Advanced format](#Advanced-format)
    - [Aliases](#Aliases)
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

> **BE CAREFUL** of the formatting. When using the advanced format, the yaml has to follow the structure metadatas > (aliases, etc.) and languages > (the languages where the localization happen)!  

> *There is currently only aliases in the advanced format, but more is planned to be added, such as frame handling! [See the related issue](https://github.com/Vireth4114/LocalizationHelper/issues/3).*  

#### Aliases

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