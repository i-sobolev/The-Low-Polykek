using UnityEngine;

public class Quest : MonoBehaviour
{
    public virtual void CompleteTheQuest(Player player = null)
    {
        return;
    }

    public virtual bool CheckQuestProgress(Player player = null)
    {
        return false;
    }
}
