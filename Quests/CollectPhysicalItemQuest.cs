using System.Collections.Generic;
using UnityEngine;

public class CollectPhysicalItemQuest : Quest
{
    [SerializeField] private List<GameObject> ItemList = null;
    [Space] [SerializeField] private List<Collider> Triggers = null;

    private void Awake()
    {
        foreach (GameObject item in ItemList)
            item.AddComponent<PhysicalQuestItem>().SetParentQuest(this);
    }

    public void RеmoveFromQuestItemsList(PhysicalQuestItem i) => ItemList.Remove(i.gameObject);
    
    public bool IsColliderOneOf(Collider other) => Triggers.Contains(other);
    
    public override bool CheckQuestProgress(Player player) => ItemList.Count == 0;

    public override void CompleteTheQuest(Player player) => Destroy(this);
}

public class PhysicalQuestItem : MonoBehaviour
{
    private CollectPhysicalItemQuest _parentQuest;

    public void SetParentQuest(CollectPhysicalItemQuest quest) => _parentQuest = quest;

    private void OnTriggerEnter(Collider other)
    {
        if (_parentQuest.IsColliderOneOf(other))
        {
            _parentQuest.RеmoveFromQuestItemsList(this);
            Destroy(this);
        }
    }
}
