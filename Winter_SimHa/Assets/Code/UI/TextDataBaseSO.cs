using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.UI
{
    [CreateAssetMenu(fileName = "TextDataBase", menuName = "SO/TextDataBase", order = 0)]
    public class TextDataBaseSO : ScriptableObject
    {
        [Serializable]
        public struct DataRow
        {
            public string key;
            public string value;
        }
        
        public List<DataRow> dataList;
        private Dictionary<string, string> _dataDictionary;
        public string this[string key] => _dataDictionary.GetValueOrDefault(key);

        private void OnEnable()
        {
            if (_dataDictionary == null)
            {
                _dataDictionary = new Dictionary<string, string>();
                foreach (var dataRow in dataList)
                {
                    _dataDictionary.Add(dataRow.key, dataRow.value);
                }
            }
        }

        public string GetString(string key)
            => _dataDictionary.ContainsKey(key) ? _dataDictionary[key] : key;
    }
}