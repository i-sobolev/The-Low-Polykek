using UnityEngine;

public class CollectedItem : InteractionEntity
{
    public CollectedItemData Data;

    private void Awake() => gameObject.name = Data.Name;

    public override void Interact(Player player)
    {
        player.Inventory.AddItem(this);
        Destroy(gameObject);
    }
}

[System.Serializable]
public struct CollectedItemData
{
    public string Name;
    public Sprite Icon;
    public GameObject UiElementRef;
}