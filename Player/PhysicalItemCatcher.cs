using UnityEngine;

public class PhysicalItemCatcher : MonoBehaviour
{
    private FixedJoint joint;

    public void Set(Rigidbody connectedBody, Vector3 connectPosition)
    {
        gameObject.AddComponent<Rigidbody>().isKinematic = true;

        joint = gameObject.AddComponent<FixedJoint>();

        joint.connectedBody = connectedBody;
        joint.connectedAnchor = connectPosition;

        joint.breakForce = 20000f;
    }
}
