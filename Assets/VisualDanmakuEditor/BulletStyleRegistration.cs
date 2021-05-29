using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

namespace VisualDanmakuEditor
{
    public class BulletStyleRegistration
    {
        private static BulletStyleRegistration instance = null;

        public static BulletStyleRegistration Instance
        {
            get
            {
                if (instance == null) LoadRegistration();
                return instance;
            }
        }

        public static void LoadRegistration()
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(Path.Combine(Application.streamingAssetsPath, "style.json"));
                instance = JsonConvert.DeserializeObject<BulletStyleRegistration>(sr.ReadToEnd());
            }
            finally
            {
                sr?.Close();
            }
        }

        [JsonProperty("StyleNames")]
        public string[] styleNames;
        [JsonProperty("ColorNames")]
        public string[] colorNames;
        [JsonProperty("ResourceSpriteMapping")]
        private Dictionary<string, string[]> resourceSpriteMapping;
        [JsonProperty("ResourceSheetMapping")]
        private Dictionary<string, string> resourceSheeetMapping;

        private Dictionary<string, int> styleName2Index;
        private Dictionary<string, int> colorName2Index;

        private Dictionary<string, Dictionary<string, Texture2D>> textureCache;
        private Dictionary<string, Dictionary<string, Sprite>> spriteCache;

        public int GetStyleIdOfName(string name)
        {
            if (styleName2Index == null)
            {
                styleName2Index = new Dictionary<string, int>();
                for (int i = 0; i < styleNames.Length; i++)
                {
                    styleName2Index.Add(styleNames[i], i);
                }
            }
            return styleName2Index[name];
        }

        public int GetColorIdOfName(string name)
        {
            if (colorName2Index == null)
            {
                colorName2Index = new Dictionary<string, int>();
                for (int i = 0; i < colorNames.Length; i++)
                {
                    colorName2Index.Add(colorNames[i], i);
                }
            }
            return colorName2Index[name];
        }

        public string GetStyleName(int id) => styleNames[id];
        public string GetColorName(int id) => colorNames[id];
        public string GetResourceSpriteName(string style, string color) => resourceSpriteMapping[style][GetColorIdOfName(color)];
        public string GetResourceSheetName(string style) => resourceSheeetMapping[style];

        public Texture2D GetCachedTexture(string style, string color)
        {
            if (textureCache == null)
            {
                CreateSpriteAndTextureCache();
            }

            if (textureCache.ContainsKey(style))
            {
                if (textureCache[style].ContainsKey(color))
                {
                    return textureCache[style][color];
                }
            }
            return null;
        }

        public Sprite GetCachedSprite(string style, string color)
        {
            if (spriteCache == null)
            {
                CreateSpriteAndTextureCache();
            }

            if (spriteCache.ContainsKey(style))
            {
                if (spriteCache[style].ContainsKey(color))
                {
                    return spriteCache[style][color];
                }
            }
            return null;
        }

        private void CreateSpriteAndTextureCache()
        {
            Dictionary<string, Sprite[]> resources = new Dictionary<string, Sprite[]>();
            textureCache = new Dictionary<string, Dictionary<string, Texture2D>>();
            spriteCache = new Dictionary<string, Dictionary<string, Sprite>>();
            for (int i = 0; i < styleNames.Length; i++)
            {
                textureCache.Add(styleNames[i], new Dictionary<string, Texture2D>());
                spriteCache.Add(styleNames[i], new Dictionary<string, Sprite>());
                if (resourceSheeetMapping.ContainsKey(styleNames[i]))
                {
                    string sheetName = resourceSheeetMapping[styleNames[i]];
                    if (!resources.ContainsKey(sheetName))
                    {
                        resources.Add(sheetName, Resources.LoadAll<Sprite>($"Images/{sheetName}"));
                    }
                    for (int j = 0; j < resourceSpriteMapping[styleNames[i]].Length; j++)
                    {
                        Sprite sprite = resources[sheetName].Single((sp) => sp.name == resourceSpriteMapping[styleNames[i]][j]);
                        spriteCache[styleNames[i]].Add(colorNames[j], sprite);
                        Texture2D targetTex = new Texture2D((int)sprite.textureRect.width, (int)sprite.textureRect.height);
                        Color[] pixels = sprite.texture.GetPixels(
                            (int)sprite.textureRect.x,
                            (int)sprite.textureRect.y,
                            (int)sprite.textureRect.width,
                            (int)sprite.textureRect.height);
                        targetTex.SetPixels(pixels);
                        targetTex.Apply();
                        textureCache[styleNames[i]].Add(colorNames[j], targetTex);
                    }
                }
            }
        }
    }
}
