using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : MonoBehaviour
{
    private Image Image;
    public static BlackScreen Singletone { get; private set; }

    private void Awake()
    {
        Singletone = this;
        Image = GetComponent<Image>();
        Hide();
    }

    public BlackScreen Hide(float step = 1f, float delay = 0)
    {
        StartCoroutine(HideC(step, delay));
        return this;
    }

    public BlackScreen Show()
    {
        Image.color = new Color(Image.color.r, Image.color.g, Image.color.b, 1);
        return this;
    }

    public void ShowScreen() => Image.color = new Color(Image.color.r, Image.color.g, Image.color.b, 1);

    private IEnumerator HideC(float step = 1f, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(UIAnimator.Animator.ImageAlphaSmoothChange(Image, 1, 0, step));
    }
}
