using UnityEngine;

public class LerpMoveTest : MonoBehaviour
{
    public Transform MoveTargetGameObjectTransform;
    [Range(0f, 1f)] public float LerpValue;
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, MoveTargetGameObjectTransform.position, LerpValue);
    }
}
