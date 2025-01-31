using System.Text;
using UnityEditor;
using UnityEngine;

namespace Code.Items
{
    public class ItemDataSO : ScriptableObject
    {
        public string itemName;
        public Sprite icon;
        public string itemID;
        public int maxStack;

        [Range(0, 100f)] public float dropChance;

        protected StringBuilder _stringBuilder = new StringBuilder();

        public virtual string GetDescription() => string.Empty;

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            string path = AssetDatabase.GetAssetPath(this);
            itemID = AssetDatabase.AssetPathToGUID(path);
        }
#endif

    }
}

