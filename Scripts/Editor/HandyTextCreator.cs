using System.IO;
using UnityEngine;
using UnityEditor;

namespace HandyEditor.Editor
{

    public class HandyTextCreator : MonoBehaviour
    {
        [MenuItem("Assets/Create/HandyEditor/Text - Markdown", priority = 16)]
        public static void CreateMarkdownText()
        {
            CreateEmptyText(HandyEditorUtils.GetSelectedFolder(), "NewMarkdownFile", "md");
        }

        [MenuItem("Assets/Create/HandyEditor/Text - JSON", priority = 17)]
        public static void CreateJsonText()
        {
            CreateEmptyText(HandyEditorUtils.GetSelectedFolder(), "NewJsonFile", "json");
        }

        [MenuItem("Assets/Create/HandyEditor/Text - Plain", priority = 18)]
        public static void CreatePlainText()
        {
            CreateEmptyText(HandyEditorUtils.GetSelectedFolder(), "NewTextFile", "txt");
        }

        public static void CreateEmptyText(string folder, string name, string ext)
        {
            string fname = folder + "/" + name + "." + ext;
            using (StreamWriter sw = File.CreateText(fname)) { }
            AssetDatabase.ImportAsset(fname, ImportAssetOptions.ForceUpdate);
        }
    }
}
