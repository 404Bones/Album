using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Tools;
using FMODUnity;
using UnityEngine.UI;
using BepInEx.Bootstrap;
using System.IO;
using TMPro;
using BepInEx;
using Newtonsoft.Json;

namespace Album
{
    public class ModMenu : MonoBehaviour, IMenuControlable
    {
        public Image bg;
        public static TextMeshProUGUI modinfotext;
        public static TextMeshProUGUI moddesctext;

        public void InitializeModMenu()
        {
            //Background
            if (gameObject.GetComponent<Image>() != null)
            {
                bg = gameObject.GetComponent<Image>();
            }
            else
            {
                bg = gameObject.AddComponent<Image>();
            }

            if (bg != null)
            {
                Texture2D tex = ResourceLoader.LoadTexture("ModsMenuBackground");
                Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                bg.sprite = spr;
                bg.rectTransform.sizeDelta = new Vector2(tex.width, tex.height);
            }

            //Left panel
            GameObject scrollcontainer = Instantiate(new GameObject("ScrollContainer"), transform);
            RectTransform scrollrecttransform = scrollcontainer.AddComponent<RectTransform>();
            scrollrecttransform.sizeDelta = new Vector2(464, 680);
            scrollrecttransform.position = new Vector2(510, 490) / Album.resMod;

            GameObject modscontainer = Instantiate(scrollcontainer, scrollcontainer.transform);
            modscontainer.name = "ModsContainer";

            VerticalLayoutGroup vertlayout = modscontainer.AddComponent<VerticalLayoutGroup>();
            vertlayout.childAlignment = TextAnchor.UpperLeft;
            vertlayout.childForceExpandHeight = false;
            vertlayout.childControlHeight = false;
            vertlayout.spacing = 12;
            vertlayout.padding = new RectOffset(12, 12, 12, 12);

            ScrollRect scrollrect = scrollcontainer.AddComponent<ScrollRect>();
            scrollcontainer.AddComponent<RectMask2D>();
            scrollrect.content = modscontainer.GetComponent<RectTransform>();
            scrollrect.vertical = true;
            scrollrect.horizontal = false;
            scrollrect.elasticity = 0.1f;
            scrollrect.scrollSensitivity = 2;

            //Mod entries
            foreach (PluginInfo plugin in Chainloader.PluginInfos.Values)
            {
                GameObject go = new GameObject("ModEntry");
                var entry = go.AddComponent<ModMenuEntry>();
                var metadata = plugin.Metadata;
                entry.modname = metadata.Name;
                entry.author = metadata.GUID.Split('.')[0];
                entry.version = metadata.Version.ToString();

                var descpath = Paths.BepInExRootPath + "/plugins/album/moddesc.json";
                var readdesc = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(descpath));

                if (readdesc.ContainsKey(metadata.GUID.ToLower()))
                {
                    entry.moddesc = readdesc[metadata.GUID.ToLower()];
                }
                else
                {
                    entry.moddesc = "This mod has no description.";
                }

                Instantiate(go, modscontainer.transform);
            }

            //Right Panel
            GameObject modinfo = Instantiate(new GameObject("ModInfo"), transform);
            modinfotext = modinfo.AddComponent<TextMeshProUGUI>();
            modinfotext.font = Album.swordfont;
            modinfotext.fontSize = 32;
            RectTransform modinforect = modinfo.GetComponent<RectTransform>();
            modinforect.position = new Vector2(1230, 560) / Album.resMod;
            modinforect.sizeDelta = new Vector2(1000, 500);

            GameObject moddesc = Instantiate(new GameObject("ModDesc"), transform);
            moddesctext = moddesc.AddComponent<TextMeshProUGUI>();
            moddesctext.font = Album.swordfont;
            moddesctext.fontSize = 32;
            RectTransform moddescrect = moddesc.GetComponent<RectTransform>();
            moddescrect.position = new Vector2(1230, 200) / Album.resMod;
            moddescrect.sizeDelta = new Vector2(1000, 1000);

            //Back Button
            GameObject backbuttongo = new GameObject("BackButton");
            backbuttongo.transform.SetParent(transform);
            RectTransform backrect = backbuttongo.AddComponent<RectTransform>();
            Button backbutton = backbuttongo.AddComponent<Button>();
            Image backimage = backbuttongo.AddComponent<Image>();
            GameObject backtextgo = Instantiate(new GameObject("EntryText"), backbuttongo.transform);
            TextMeshProUGUI backtext = backtextgo.AddComponent<TextMeshProUGUI>();

            backtext.text = "Back";
            backtext.font = Album.swordfont;
            backtext.fontSize = 32;
            backtext.alignment = TextAlignmentOptions.Center;
            backbutton.targetGraphic = backimage;
            backbutton.colors = Album.colorblock;
            backbutton.onClick.AddListener(CloseModMenu);

            Texture2D backtex = ResourceLoader.LoadTexture("ModsButton");
            Sprite backspr = Sprite.Create(backtex, new Rect(0, 0, backtex.width, backtex.height), new Vector2(0.5f, 0.5f));
            backimage.sprite = backspr;
            backrect.position = new Vector2(512, 860) / Album.resMod;
            backrect.sizeDelta = new Vector2(400, 60);


            gameObject.SetActive(false);
        }

        public void OpenModMenu()
        {
            RuntimeManager.PlayOneShot("event:/UI/UI_GEN_Click", default);
            gameObject.SetActive(true);
            SetMenuControl(true);
        }

        public void CloseModMenu()
        {
            RuntimeManager.PlayOneShot("event:/UI/UI_GEN_Click", default);
            gameObject.SetActive(false);
            SetMenuControl(false);
        }

        public void OnEscapePressed(bool close = true)
        {
            if (gameObject.activeInHierarchy)
            {
                CloseModMenu();
            }
        }

        public void SetMenuControl(bool enabled)
        {
            if (enabled)
            {
                UIMenuController menuController = MiscUtils.menuController;
                if (menuController == null)
                {
                    return;
                }
                menuController.AddOpenedMenu(this);
                return;
            }
            else
            {
                UIMenuController menuController2 = MiscUtils.menuController;
                if (menuController2 == null)
                {
                    return;
                }
                menuController2.RemoveOpenedMenu(this);
                return;
            }
        }
    }
}
