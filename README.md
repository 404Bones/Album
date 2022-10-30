# Album
![GithubAllDownloads](https://img.shields.io/github/downloads/404Bones/Album/total)

A mod manager for Brutal Orchestra.

## How to use:
1. Download BepInEx 5.4 from https://github.com/BepInEx/BepInEx/releases
2. Extract the contents of the ZIP file into Brutal Orchestra's game directory
3. Download the latest release of Album from https://github.com/404Bones/Album/releases and drop the file into the `plugins` folder of the `BepInEx` folder
4. Start the game, you should now see a "Mods" button on the bottom right of the main menu

## How to add your mod's description to Album
By default, your mod will show up in Album once it's installed, displaying the name, author, and version of the mod.
Use this format in your mod so Album will display it properly: ``[BepInPlugin("Author.ModID", "ModName", "ModVersion")]``

1. In your IDE of choice, add Album.dll as a dependency
2. Add `using Album;` and `using BepInEx.Bootstrap;` to the top of your main class
3. Add [BepInDependency("Bones404.Album", BepInDependency.DependencyFlags.SoftDependency)] below your BepInPlugin definition so that your mod loads after Album.
3. In the `Awake()` or `OnEnable()` function of your main class (which inherits from `BaseUnityPlugin`) add this line of code,
 changing the parameters as necessary: 

```C#
//Add description if Album is installed
            foreach (var plugin in Chainloader.PluginInfos)
            {
                var metadata = plugin.Value.Metadata;
                if (metadata.GUID == "Bones404.Album")
                {
                    new ModDescription("Author.ModID", "ModName", "ModVersion")
                    break;
                }
            }

```
Your mod's description should now be displayed in Album. Remember to **not** change your mod's GUID as it will cause problems in Album and other mods.
