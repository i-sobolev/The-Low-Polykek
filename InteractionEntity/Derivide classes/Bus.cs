using UnityEngine;
using UnityEngine.Events;

public class Bus : InteractionEntity
{
    public UnityEvent OnEngineStart;
    private BringItemQuest BringKeyQuest;

    private void Awake()
    {
        BringKeyQuest = GetComponent<BringItemQuest>();
    }

    public override void Interact(Player player)
    {
        if (BringKeyQuest.CheckQuestProgress(player))
        {
            BringKeyQuest.CompleteTheQuest(player);
            OnEngineStart.Invoke();
        }
    }
}