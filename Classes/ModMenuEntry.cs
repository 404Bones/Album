using FMODUnity;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Album
{
    public class ModMenuEntry : MonoBehaviour
    {
        public TextMeshProUGUI textmeshpro;
        public RectTransform rect;
        public Button button;
        public Image image;

        public string modname = "";
        public string author = "";
        public string version = "";
        public string moddesc = "";

        public void Awake()
        {
            if (rect == null) { rect = gameObject.AddComponent<RectTransform>(); }
            else { rect = gameObject.GetComponent<RectTransform>(); }

            if (button == null) { button = gameObject.AddComponent<Button>(); }
            else { button = gameObject.GetComponent<Button>(); }

            if (image == null) { image = gameObject.AddComponent<Image>(); }
            else { image = gameObject.GetComponent<Image>(); }

            if (textmeshpro == null)
            {
                GameObject textgo = Instantiate(new GameObject("EntryText"), transform);
                textmeshpro = textgo.AddComponent<TextMeshProUGUI>();
            }
            else { textmeshpro = GetComponentInChildren<TextMeshProUGUI>(); }


            rect.sizeDelta = new Vector2(423, 70);
            textmeshpro.gameObject.GetComponent<RectTransform>().sizeDelta += new Vector2(50, 0);
            textmeshpro.font = Album.swordfont;
            textmeshpro.text = modname;
            textmeshpro.alignment = TextAlignmentOptions.Center;
            textmeshpro.color = Color.white;
            textmeshpro.fontSize = 32;

            Texture2D tex = EmbeddedResourceLoader.Load("ModsButton") as Texture2D;
            Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            image.sprite = spr;
            button.targetGraphic = image;

            button.colors = Album.colorblock;
            button.onClick.AddListener(OnEntryClick);

        }

        public void OnEntryClick()
        {
            RuntimeManager.PlayOneShot("event:/UI/UI_GEN_Click", default);
            ModMenu.modinfotext.text = string.Format("Mod: {0}\nAuthor: {1}\nVersion: {2}", modname, author, version);
            ModMenu.moddesctext.text = moddesc;
        }
    }
}
