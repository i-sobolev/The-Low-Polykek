using UnityEngine;

public class BringItemQuest : Quest
{
    [SerializeField] private CollectedItem item = null;
    private string _questItemName;

    private void Awake() => _questItemName = item.Data.Name;

    public override bool CheckQuestProgress(Player player)
    {
        bool result = false;

        foreach (CollectedItemData item in player.Inventory.InventoryList)
            if (item.Name == _questItemName)
                result = true;

        return result;
    }

    public override void CompleteTheQuest(Player player)
    {
        player.Inventory.RemoveItem(_questItemName);
        Destroy(this);
    }
}
