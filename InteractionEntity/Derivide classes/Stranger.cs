using UnityEngine;
using UnityEngine.Events;

public class Stranger : InteractionEntity
{
    public Vector3 CameraLookAtPositionOffset;
    [Header("Stranger properties")]
    [SerializeField] private string Name = " ";

    public Replicas[] replicas;

    private string[] _currentReplica;
    private int _numberOfCurrentReplicaLine;
    private int _numberOfCurrentReplica;

    [Space]

    public UnityEvent OnFirstTalk;
    private bool _firstTalk = true; 

    private Quest _quest;
    public UnityEvent OnQuestComplete;

#if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawIcon(transform.position + CameraLookAtPositionOffset, "Assets/Sprites/UI/Gizmos/CameraLookAtOffsetPoint.psd");
    }
#endif

    private void Awake()
    {
        if (GetComponent<Quest>() != null)
            _quest = GetComponent<Quest>();

        gameObject.name = Name;
        _currentReplica = replicas[_numberOfCurrentReplica].Main;
    }

    public override void Interact(Player player)
    {
        if (_numberOfCurrentReplicaLine == 0)
        {
            player.StartDialog(this);

            if (_firstTalk)
            {
                OnFirstTalk?.Invoke();
                _firstTalk = false;
            }
        }

        if (_quest != null && _quest.CheckQuestProgress(player))
        {
            _quest.CompleteTheQuest(player);
            OnQuestComplete?.Invoke();

            _currentReplica = replicas[++_numberOfCurrentReplica].Main;
            _numberOfCurrentReplicaLine = 0;
        }
        
        if (IsReplicasEnd())
        {
            EndDialog(player);
            return;
        }

        StrangerReplicasWindow.Singletone.ShowNewReplica(name, _currentReplica[_numberOfCurrentReplicaLine++]);
    }

    private void EndDialog(Player player)
    {
        StrangerReplicasWindow.Singletone.Clear();

        _currentReplica = replicas[_numberOfCurrentReplica].Addition;
        _numberOfCurrentReplicaLine = 0;

        player.EndDialog();
    }

    private bool IsReplicasEnd() => _numberOfCurrentReplicaLine > _currentReplica.Length - 1;
}

[System.Serializable]
public struct Replicas
{
    public string[] Main;
    public string[] Addition;
}