# LocalizationHelper

Helper to localize textures in your mods

## Usage

Put a `LocalizationAssets.json` in your mod root folder to specify the new texture for each language.

It has this format:
```
{
  "ExampleMap/stuffs/stuff1": {
    "french": "ExampleMap/stuffs/french_stuff1",
    "german": "ExampleMap/stuffs/german_stuff1"
  },
  "stuff/ExampleMap/memo_title": {
    "french": "stuff/ExampleMap/french_memo_title"
  }
}
```
You can name the localized textures however you want, but you have to put them in the same folder type than the original texture.

For example: if `ExampleMap/stuffs/stuff1` points to `Graphics/Atlases/Gameplay/decals/ExampleMap/stuffs/stuff1.png`, you have to put your localized textures for this asset somewhere in `Graphics/Atlases/Gameplay/decals`

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