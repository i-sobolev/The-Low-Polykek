using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject UIInventoryItemsLayoutGroup = null;

    [Header("List")]
    public List<CollectedItemData> InventoryList;
    [SerializeField] private GameObject LayoutElementFab = null;

    [Header("Sounds")]
    [SerializeField] private AudioSource _pickUpSound;

    public void AddItem(CollectedItem item)
    {
        CollectedItemData itemData = item.Data;
        itemData.UiElementRef = Instantiate(LayoutElementFab, UIInventoryItemsLayoutGroup.transform);
        itemData.UiElementRef.GetComponent<Image>().sprite = itemData.Icon;

        InventoryList.Add(itemData);

        _pickUpSound.PlayOneShot(_pickUpSound.clip);
    }

    public void RemoveItem(string itemNameToRemove)
    {
        foreach(CollectedItemData item in InventoryList)
        {
            if (item.Name == itemNameToRemove)
            {
                Destroy(item.UiElementRef);
                InventoryList.Remove(item);
                return;
            }
        }
    }
}