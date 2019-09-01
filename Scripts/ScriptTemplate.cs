using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandyEditor
{
    [CreateAssetMenu(menuName = "HandyEditor/Script Template", fileName = "BehaviourTemplate", order = 15)]
    public class ScriptTemplate : ScriptableObject
    {
        public string namespaceName = "MyProject";
        public string parentClass = "MonoBehaviour";
        public bool isEditor = false;
    }
}
