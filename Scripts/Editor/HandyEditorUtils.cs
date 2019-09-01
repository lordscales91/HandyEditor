using System.IO;
using UnityEditor;

namespace HandyEditor.Editor
{

    public class HandyEditorUtils
    {
        public static string GetSelectedFolder()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }
            return path;
        }
    }
}
