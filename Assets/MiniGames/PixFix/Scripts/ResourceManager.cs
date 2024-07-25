using UnityEngine;
using System;
using System.Linq;

namespace MiniGames.PixFix.Scripts
{
    public static class ResourceManager
    {
        private static RenderTexture[] tileRenderTextures;

        public static RenderTexture[] TileRenderTextures()
        {
            if (tileRenderTextures == null)
            {
                LoadTextures();
            }
            return tileRenderTextures;
        }

        private static void LoadTextures()
        {
            tileRenderTextures = Resources.LoadAll<RenderTexture>("TileRenderTextures");

            if (tileRenderTextures.Length == 0)
            {
                Debug.LogWarning("No textures found in Resources/Textures.");
            }
            else
            {
                Debug.Log("Loaded " + tileRenderTextures.Length + " textures.");
                
                // Sắp xếp theo thứ tự số ở phần cuối tên
                Array.Sort(tileRenderTextures, (tex1, tex2) =>
                {
                    int index1 = ExtractIndex(tex1.name);
                    int index2 = ExtractIndex(tex2.name);
                    return index1.CompareTo(index2);
                });
            }
        }

        private static int ExtractIndex(string name)
        {
            string[] parts = name.Split(' ');
            if (parts.Length > 1 && int.TryParse(parts[parts.Length - 1], out int index))
            {
                return index;
            }
            else
            {
                Debug.LogWarning($"Failed to extract index from texture name: {name}");
                return -1; // Trả về giá trị không hợp lệ nếu không thể phân tích
            }
        }
    }
}