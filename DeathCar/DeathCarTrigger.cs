using UnityEngine;

public class DeathCarTrigger : MonoBehaviour
{
    public Transform CarSpawnPoint;
    public Transform PlayerSpawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterController>())
        {
            DeathCar.Singltone.Ride(other.gameObject, PlayerSpawnPoint.position, CarSpawnPoint.position);
        }
    }
}