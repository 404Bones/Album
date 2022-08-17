using System.IO;
using System.Reflection;
using UnityEngine;
using System.Linq;

namespace Album
{
    public static class EmbeddedResourceLoader
    {
        public static Object Load(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames().First(r => r.Contains(name));
            var resource = assembly.GetManifestResourceStream(resourceName);
            using var memoryStream = new MemoryStream();
            var buffer = new byte[16384];
            int count;
            while ((count = resource!.Read(buffer, 0, buffer.Length)) > 0)
                memoryStream.Write(buffer, 0, count);
            var spriteTexture = new Texture2D(0, 0, TextureFormat.ARGB32, false)
            {
                anisoLevel = 1,
                filterMode = 0
            };

            ImageConversion.LoadImage(spriteTexture, memoryStream.ToArray());
            return spriteTexture;
        }
    }
}