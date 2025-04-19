using UnityEngine;

[CreateAssetMenu(menuName = "SO/Pool/Config")]
public class ToolInfoSO : ScriptableObject
{
    [Header("Pool Manager")]
    public string poolingFolder = "Assets/08SO/Pool";
    public string poolSOAssetName = "PoolManager.asset";
    public string poolUxmlAssetName = "PoolItem.uxml";
    public string typeFolder = "Types";
    public string itemFolder = "Items";
}
