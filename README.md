# LocalizationHelper

Helper to localize textures in your mods

## Usage

Put a `LocalizationTextures.json` in your mod root folder to specify the new texture for each language.

It has this format:
```
{
  Graphics/Atlases/Gameplay/decals/ExampleMap/stuffs/stuff1": {
    "french": "Graphics/Atlases/Gameplay/decals/ExampleMap/stuffs/french_stuff1",
    "german": "Graphics/Atlases/Gameplay/decals/ExampleMap/stuffs/german_stuff1"
  },
  "Graphics/Atlases/Portraits/stuff/ExampleMap/memo_title": {
    "french": "Graphics/Atlases/Portraits/stuff/ExampleMap/french_memo_title"
  }
}
```
You can name the localized textures however you want, but you have to put them in the same data path than the original texture.

For example: if in Lönn the path for the texture is `ExampleMap/stuffs/stuff1` and it points to `Graphics/Atlases/Gameplay/decals/ExampleMap/stuffs/stuff1.png`, the data path is `Graphics/Atlases/Gameplay/decals`, so you have to put your localized textures for this texture somewhere in `Graphics/Atlases/Gameplay/decals`

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