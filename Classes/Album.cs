namespace Album
{
    using BepInEx;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using System.Collections.Generic;
    using MonoMod.RuntimeDetour;
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.IO;
    using Newtonsoft.Json;

    [BepInPlugin("Bones404.Album", "Album", "1.0.2")]
    public class Album : BaseUnityPlugin
    {
        const BindingFlags AllFlags = (BindingFlags)(-1);

        static public Button optionsbutton;
        static public ModMenu modmenu;
        static public TMP_FontAsset swordfont;
        static public ColorBlock colorblock;
        static public Vector2 resMod;
        static public Dictionary<string, string> moddesc = new Dictionary<string, string>();
        static private int iconIndex = 0;

        static ColorBlock defButton;
        static ColorBlock unselectButton;

        public void OnEnable()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            IDetour mainmenuhook = new Hook(
                    typeof(MainMenuController).GetMethod("Start", AllFlags),
                    typeof(Album).GetMethod("MainMenuStart", AllFlags)
                );

            if (Directory.Exists(Paths.BepInExRootPath + "/albumdesc/"))
            {
                Directory.Delete(Paths.BepInExRootPath + "/albumdesc/", true);
            }

            new ModDescription("Bones404.Album", "A mod manager for Brutal Orchestra.");

            resMod = new Vector2(1920, 1080) / new Vector2(Screen.width, Screen.height);

            Logger.LogInfo("Album successfully loaded!");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void MainMenuStart(Action<MainMenuController> orig, MainMenuController self)
        {
            orig(self);

            //Write mod descriptions file
            Directory.CreateDirectory(Paths.BepInExRootPath + "/plugins/album/");
            var descfile = File.CreateText(Paths.BepInExRootPath + "/plugins/album/moddesc.json");
            descfile.Write(JsonConvert.SerializeObject(moddesc));
            descfile.Close();

            GameObject modmenubuttongo = new GameObject();
            GameObject origoptionsbutton = new GameObject();
            Button modmenubutton = null;
            TextMeshProUGUI modmenutext = null;
            foreach (Button button in FindObjectsOfType(typeof(Button)))
            {
                if (button.gameObject.name == "OptionsButton")
                {
                    origoptionsbutton = button.gameObject;
                    optionsbutton = button;
                    button.targetGraphic = origoptionsbutton.GetComponentInChildren<TextMeshProUGUI>();
                    modmenubuttongo = Instantiate(origoptionsbutton, origoptionsbutton.transform.parent);
                    modmenubutton = modmenubuttongo.GetComponent<Button>();
                    modmenutext = modmenubuttongo.GetComponentInChildren<TextMeshProUGUI>();
                    swordfont = modmenutext.font;
                    colorblock = button.colors;
                    break;
                }
            }

            GameObject modmenugo = Instantiate(new GameObject("ModMenuHandler"), FindObjectOfType<Canvas>().transform);
            modmenu = modmenugo.AddComponent<ModMenu>();
            modmenu.InitializeModMenu();

            defButton = optionsbutton.colors;
            unselectButton = optionsbutton.colors;
            unselectButton.selectedColor = defButton.normalColor;

            modmenubuttongo.transform.position += new Vector3(0, 50, 0);
            modmenubuttongo.name = "ModsButton";

            modmenutext.text = "Mods";
            modmenubutton.targetGraphic = modmenutext;
            modmenubutton.transform.SetParent(modmenubuttongo.transform);

            if (modmenubutton != null)
            {
                modmenubutton.onClick = new Button.ButtonClickedEvent();
                modmenubutton.onClick.AddListener(OnModButtonClick);
            }
        }

        public static void OnModButtonClick()
        {
            modmenu.OpenModMenu();
        }
        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Album.EmbeddedAssemblies.Newtonsoft.Json.dll"))
            {
                var assemblyData = new Byte[stream.Length];
                stream.Read(assemblyData, 0, assemblyData.Length);
                return Assembly.Load(assemblyData);
            }
        }

        public static void AddMenuIcon(Sprite sprite)
        {
            GameObject versionIcon;
            foreach (GameObject item in FindObjectsOfType(typeof(GameObject)))
            {
                if (item.name == "Exquisite")
                {
                    versionIcon = item;
                    GameObject modIcon = Instantiate(versionIcon, versionIcon.transform.parent);
                    modIcon.GetComponentInChildren<Image>().sprite = sprite;
                    modIcon.transform.localPosition = new Vector3(120 + iconIndex * 48, 0, 0);
                    iconIndex++;
                    break;
                }
            }
        }
    }
}