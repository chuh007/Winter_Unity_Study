using UnityEngine;

namespace Code.Items
{
    [CreateAssetMenu(fileName = "ScrapItemData", menuName = "SO/Items/ScrapItem", order = 0)]
    public class ScrapItemDataSO : ItemDataSO
    {
        [TextArea]  public string description;
        public override string GetDescription() => description;
    }
}