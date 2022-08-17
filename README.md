# Album
![GithubAllDownloads](https://img.shields.io/github/downloads/404Bones/Album/total?style=flat-square)

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
2. Add `using Album;` to the top of your main class
3. In the `Awake()` or `OnEnable()` function of your main class (which inherits from `BaseUnityPlugin`) add this line of code,
 changing the parameters as necessary: 

```C#
new ModDescription("Mod's ID as it is in the GUID", "Description of your mod")
```
Your mod's description should now be displayed in Album. Remember to **not** change your mod's ID as it will cause problems in Album and other mods.
