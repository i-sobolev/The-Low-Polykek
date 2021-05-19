using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StrangerReplicasWindow : MonoBehaviour
{
    private TextMeshProUGUI _text;
    public static StrangerReplicasWindow Singletone { get; private set; }

    private AudioSource _soundEffect;

    private void Awake()
    {
        Singletone = this;
        _text = GetComponent<TextMeshProUGUI>();
        _soundEffect = GetComponent<AudioSource>();
    }

    public void ShowNewReplica(string name, string replica)
    {
        StopAllCoroutines();
        Clear();
        StartCoroutine(TextAnimation($"{name}: {replica}"));
    }

    public void Clear()
    {
        StopAllCoroutines();
        _text.SetText(string.Empty);
    }
    private IEnumerator TextAnimation(string text)
    {
        short delay = 2;
        foreach (char s in text)
        {
            _text.text += s;

            if (delay-- < 0)
            {
                _soundEffect.PlayOneShot(_soundEffect.clip);
                delay = 2;
            }

            yield return null;
        }
    }
}