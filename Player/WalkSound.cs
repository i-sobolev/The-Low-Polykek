using System.Collections;
using UnityEngine;

public class WalkSound : MonoBehaviour
{
    private AudioSource AudioComponent;
    private bool isPlaying = false;

    private float changeVolumeSpeed = 0.1f;

    private void Awake()
    {
        AudioComponent = GetComponent<AudioSource>();
        AudioComponent.volume = 0;
    }

    private void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (!isPlaying)
            {
                isPlaying = true;
                AudioComponent.Play();
                StopAllCoroutines();
                StartCoroutine(SmoothVolumeChange(0.75f, changeVolumeSpeed));
            }
        }

        else
        {
            if (isPlaying)
            {
                isPlaying = false;
                StopAllCoroutines();
                StartCoroutine(SmoothVolumeChange(0, changeVolumeSpeed));
            }
        }
    }

    private IEnumerator SmoothVolumeChange(float endValue, float step)
    {
        float startValue = AudioComponent.volume;
        
        float lerp = 0;

        while (lerp < 1)
        {
            AudioComponent.volume = Mathf.Lerp(startValue, endValue, lerp.EaseInOutQuad());
            lerp += step;
            yield return null;
        }

        AudioComponent.volume = endValue;
        yield return null;
    }
}