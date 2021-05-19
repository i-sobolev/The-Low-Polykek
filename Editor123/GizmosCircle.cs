using UnityEngine;

public class GizmosCircle : MonoBehaviour
{
    [SerializeField] private Color color = Color.white;

    private void Awake() => Destroy(this);

    private void OnDrawGizmos()
    {
        Gizmos.color = color; 
        Gizmos.DrawSphere(transform.position, 1f);
    }
}
