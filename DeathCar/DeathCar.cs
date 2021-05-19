using System.Collections;
using UnityEngine;

public class DeathCar : MonoBehaviour
{
    private Vector3 _parkingPosition;
    private Vector3 _playerSpawnPosition;
    private bool _isRiding = false;
    public static DeathCar Singltone { get; private set; }

    private AudioSource _carKillSound;

    private void Awake()
    {
        Singltone = this;
        _parkingPosition = transform.position;
        _carKillSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<CharacterController>())
        {
            StopAllCoroutines();
            Player.Singletone.SetPosition(_playerSpawnPosition);
            transform.position = _parkingPosition;

            BlackScreen.Singletone.Show().Hide(1, 2);


            _isRiding = false;
        }
    }

    public void Ride(GameObject target, Vector3 PlayerSpawnPosition, Vector3 rideStartPosition)
    {
        if (_isRiding)
            return;

        _isRiding = true;
        transform.position = rideStartPosition;
        _playerSpawnPosition = PlayerSpawnPosition;

        _carKillSound.PlayOneShot(_carKillSound.clip);
       
        StartCoroutine(RideTo(target));
        StartCoroutine(CarScale());
    }

    private IEnumerator RideTo(GameObject target)
    {
        float lerp = 1;

        while (true)
        {
            transform.position = Vector3.Lerp(target.transform.position, transform.position, lerp);
            transform.LookAt(target.transform.position);
            transform.Rotate(new Vector3(0, 1, 0), -90);
            transform.Rotate(new Vector3(1, 0, 0), -90);

            lerp -= 0.04f * Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator CarScale()
    {
        transform.localScale = Vector3.zero;
        Vector3 target = new Vector3(400, 130, 100);
        float lerp = 0;

        while(lerp < 1)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, target, lerp);
            lerp += 0.05f;

            yield return null;
        }
    }
}
