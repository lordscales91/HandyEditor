using System.IO;
using UnityEngine;
using UnityEditor;

namespace HandyEditor.Editor
{

    
    public class HandyScriptAssistant : ScriptableWizard
    {
        public ScriptTemplate template;
        public string namespaceName = "MyProject";
        public string className = "MyClass";
        public string parentClass = "MonoBehaviour";
        public bool isEditor = false;

        [MenuItem("Assets/Create/HandyEditor/Custom C# Script", priority = 10)]
        static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard<HandyScriptAssistant>("Customize C# Script", "Create");
        }

        void InitProps()
        {
            if(template != null)
            {
                namespaceName = template.namespaceName;
                parentClass = template.parentClass;
                isEditor = template.isEditor;
            }
        }

        private string CreateASMDEFIFNeeded(string selectedFolder)
        {
            string createdFile = null;
            if(!selectedFolder.StartsWith("Assets"))
            {
                // If the folder doesn't start with "Assets", it means that it's outside the project and require an ASMDEF
                string[] found = Directory.GetFiles(selectedFolder, "*.asmdef");
                if (found.Length <= 0)
                {
                    // No ASMDEF found. Generating one...
                    string asmdefName = namespaceName.ToLowerInvariant();
                    createdFile = selectedFolder + "/" + asmdefName + ".asmdef";
                    using(StreamWriter sw = File.CreateText(createdFile))
                    {
                        sw.WriteLine("{");
                        sw.Write("\t\"name\": \"{0}\"", asmdefName);
                        if(isEditor)
                        {
                            sw.WriteLine(',');
                            sw.WriteLine("\t\"includePlatforms\": [");
                            sw.WriteLine("\t\t\"Editor\"");
                            sw.WriteLine("\t]");
                        } else
                        {
                            sw.WriteLine("");
                        }
                        sw.WriteLine("}");
                    }
                }
            }
            
            return createdFile;
        }

        private string Indent(int level)
        {
            string indent = "";
            if(level > 0)
            {
                char[] spaces = new char[level * 4];
                for(int i=0;i<spaces.Length;i++)
                {
                    spaces[i] = ' ';
                }
                indent = new string(spaces);
            }
            return indent;
        }

        void CreateMonobehaviourBody(StreamWriter sw, int level)
        {
            sw.WriteLine("{0}// Start is called before the first frame update", Indent(level));
            sw.WriteLine("{0}void Start()", Indent(level));
            sw.WriteLine("{0}{{", Indent(level)); // Using double curly braces to output a single one. See: https://stackoverflow.com/a/91375/3107765
            sw.WriteLine("");
            sw.WriteLine("{0}}}", Indent(level));
            sw.WriteLine("");
            sw.WriteLine("{0}// Update is called once per frame", Indent(level));
            sw.WriteLine("{0}void Update()", Indent(level));
            sw.WriteLine("{0}{{", Indent(level));
            sw.WriteLine("");
            sw.WriteLine("{0}}}", Indent(level));
        }

        void CreateCustomEditorBody(StreamWriter sw, int level)
        {
            sw.WriteLine("{0}public override void OnInspectorGUI()", Indent(level));
            sw.WriteLine("{0}{{", Indent(level));
            sw.WriteLine("{0}// Probably you want to override this method to customize the GUI", Indent(level+1));
            sw.WriteLine("{0}}}", Indent(level));
        }


        void OnWizardCreate()
        {
            InitProps();
            try
            {
                AssetDatabase.StartAssetEditing();
                string selectedFolder = HandyEditorUtils.GetSelectedFolder();
                string scriptFile = selectedFolder + "/" + className + ".cs";
                if(File.Exists(scriptFile))
                {
                    Debug.LogError("There is already a file with the same name in the selected folder!");
                    throw new System.InvalidOperationException();
                }
                using (StreamWriter sw = File.CreateText(scriptFile))
                {
                    // Start with the default system imports
                    sw.WriteLine("using System.Collections;");
                    sw.WriteLine("using System.Collections.Generic;");
                    sw.WriteLine("using UnityEngine;");
                    int level = 0;
                    if (isEditor)
                    {
                        // If it's an editor script add the corresponding using
                        sw.WriteLine("using UnityEditor;");
                    }
                    sw.WriteLine("");
                    if(namespaceName != null && namespaceName.Length > 0)
                    {
                        // Open the namespace
                        sw.WriteLine("namespace {0}", namespaceName);
                        sw.WriteLine("{");
                        sw.WriteLine("");
                        level++;
                    }
                    if(parentClass != null && parentClass.Length > 0)
                    {
                        // Open class
                        sw.WriteLine("{0}public class {1} : {2}", Indent(level), className, parentClass);
                        sw.WriteLine("{0}{{", Indent(level));
                        level++;
                        sw.WriteLine("{0}// TODO: Class auto-generated by HandyEditor", Indent(level));
                        if("MonoBehaviour".Equals(parentClass))
                        {
                            sw.WriteLine("");
                            CreateMonobehaviourBody(sw, level);
                        } else if(isEditor && parentClass.EndsWith("Editor"))
                        {
                            sw.WriteLine("");
                            CreateCustomEditorBody(sw, level);
                        }
                        level--;
                    } else
                    {
                        // Open class
                        sw.WriteLine("{0}public class {1} ", Indent(level), className);
                        sw.WriteLine("{0}{{", Indent(level));
                        level++;
                        sw.WriteLine("{0}// TODO: Class auto-generated by HandyEditor", Indent(level));
                        level--;
                    }
                    // Close class
                    sw.WriteLine("{0}}}", Indent(level));
                    if (namespaceName != null && namespaceName.Length > 0)
                    {
                        // Close namespace
                        sw.WriteLine("}");
                    }
                }
                string asmdef = CreateASMDEFIFNeeded(selectedFolder);
                Debug.Log("Script file created at: "+scriptFile);
                AssetDatabase.ImportAsset(scriptFile, ImportAssetOptions.ForceUpdate);
                if (asmdef != null)
                {
                    Debug.Log("ASMDEF file created at: " + asmdef);
                    AssetDatabase.ImportAsset(asmdef, ImportAssetOptions.ForceUpdate);
                }
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                AssetDatabase.SaveAssets();
            }
        }
    }
}