using UnityEngine;

public class PhysicalItem : InteractionEntity
{
    [Header("Object == Door || Arm etc.")]
    [SerializeField] private bool _alt = false;

    public override void Interact(Player player) => player.CatchPhysicalItem(_alt);
}
