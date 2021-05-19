using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BusRide : MonoBehaviour
{
    public Vector3 BusStopPosition;
    public Vector3 RideAwayTargetPosition;
    public UnityEvent OnBusStop;

    private Vector3 _startPosition;

    public List<Animation> doorAnimation;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(BusStopPosition, 1);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(RideAwayTargetPosition, 1);
    }

    private void Start()
    {
        _startPosition = transform.position;
        StartCoroutine(RideTo(BusStopPosition));
    }

    public void RideAway()
    {
        StartCoroutine(RideTo(RideAwayTargetPosition));
    }

    public void PlayDoorAnimation() => doorAnimation.ForEach(anim => anim.Play());

    private IEnumerator RideTo(Vector3 target)
    {
        float lerp = 1;

        while (lerp >= 0.01f)
        {
            transform.position = Vector3.Lerp(target, _startPosition, lerp.EaseInOutQuad());
            lerp -= 0.035f * Time.deltaTime;

            yield return null;
        }

        OnBusStop?.Invoke();
        PlayDoorAnimation();
        _startPosition = transform.position;
        yield return null;
    }
}
