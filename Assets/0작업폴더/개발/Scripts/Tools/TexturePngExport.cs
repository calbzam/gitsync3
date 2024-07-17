#nullable enable

using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using File = UnityEngine.Windows.File;
#endif

// https://forum.unity.com/threads/transparent-rendertexture-with-postprocessing.1265873/#post-9188264

namespace My.Common.Scripts.Editor
{
    /// <summary>
    /// Class for exporting PNG from textures.
    /// </summary>
    public static class TexturePngExport
    {
#if UNITY_EDITOR
        /// <summary>
        /// Exports the selected <see cref="RenderTexture" /> to PNG file.
        /// </summary>
        [MenuItem("Tools/Export/Texture To PNG")]
        public static void ExportToPng()
        {
            if (Selection.activeObject is RenderTexture renderTexture)
            {
                var assetPath = AssetDatabase.GetAssetPath(renderTexture);
                var fullPath = Path.Combine(Application.dataPath, "..", assetPath);
                fullPath = Path.ChangeExtension(Path.GetFullPath(fullPath), "png");

                if (!File.Exists(fullPath) || !EditorUtility.DisplayDialog("Confirmation", $"File for export '{fullPath}' already exists. Continue overriding?", "No", "Yes"))
                {
                    var activeRenderTexture = RenderTexture.active;
                    RenderTexture.active = renderTexture;
                    var texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
                    texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
                    texture2D.Apply(); // This line is necessary to make the pixel changes take effect
                    RenderTexture.active = activeRenderTexture;

                    var bytes = texture2D.EncodeToPNG();
                    File.WriteAllBytes(fullPath, bytes);

                    // Refresh AssetDatabase
                    AssetDatabase.Refresh();

                    Debug.Log($"Exported render texture as PNG using path '{fullPath}'");
                }
                else
                {
                    Debug.Log("Use cancelled exported of the render texture as PNG");
                }
            }
            else
            {
                Debug.LogError("Not a render texture selected");
            }
        }
#endif
    }
}
