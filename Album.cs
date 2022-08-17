namespace Album
{
    using BepInEx;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using MonoMod.RuntimeDetour;
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using FMODUnity;

    [BepInPlugin("Bones404.Album", "Album", "1.0.0")]
    public class Album : BaseUnityPlugin
    {
        const BindingFlags AllFlags = (BindingFlags)(-1);

        static public Button optionsbutton;
        static public ModMenu modmenu;
        static public TMP_FontAsset swordfont;
        static public ColorBlock colorblock;
        static public Vector2 resMod;

        static ColorBlock defButton;
        static ColorBlock unselectButton;

        public void OnEnable()
        {
            IDetour mainmenuhook = new Hook(
                    typeof(MainMenuController).GetMethod("Start", AllFlags),
                    typeof(Album).GetMethod("MainMenuStart", AllFlags)
                );

            new ModDescription("Album", "A mod manager for Brutal Orchestra.");
            resMod = new Vector2(1920, 1080) / new Vector2(Screen.width, Screen.height);

            Logger.LogInfo("Album successfully loaded!");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void MainMenuStart(Action<MainMenuController> orig, MainMenuController self)
        {
            orig(self);

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

    }
}