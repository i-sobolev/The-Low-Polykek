using UnityEngine;

public abstract class InteractionEntity : MonoBehaviour
{
    public Vector3 InfoPositionOffset;
    public abstract void Interact(Player player);

#if UNITY_EDITOR
    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position + InfoPositionOffset, "Assets/Sprites/UI/Gizmos/OffsetPoint.psd");
    }
#endif
}