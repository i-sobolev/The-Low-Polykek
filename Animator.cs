using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace UIAnimator
{
    public class Animator : MonoBehaviour
    {
        const float StandartStep = 2.5f;

        public static IEnumerator ImageAlphaSmoothChange(Image image, float startAlpha, float targetAlpha, float step = StandartStep)
        {
            var lerp = 0f;
            
            while (lerp < 1)
            {
                var newAlpha = Mathf.Lerp(startAlpha, targetAlpha, lerp.EaseInOutQuad());

                image.color = new Color(image.color.r, image.color.g, image.color.b, newAlpha);
                lerp += step * Time.deltaTime;

                yield return null;
            }

            image.color = new Color(image.color.r, image.color.g, image.color.b, targetAlpha);
            yield return null;
        }

        public static IEnumerator TmpAlphaSmoothChange(TMPro.TextMeshProUGUI text, float startAlpha, float targetAlpha, float step = StandartStep)
        {
            var lerp = 0f;

            while (lerp < 1)
            {
                var newAlpha = Mathf.Lerp(startAlpha, targetAlpha, lerp.EaseInOutQuad());

                text.alpha = newAlpha;
                lerp += step * Time.deltaTime;

                yield return null;
            }

            text.color = new Color(text.color.r, text.color.g, text.color.b, targetAlpha);
            yield return null;
        }

        public static IEnumerator RectTransformSmoothScale(RectTransform transform, float startScale, float targetScale, float step = StandartStep)
        {
            float lerp = 0;

            while (lerp < 1)
            {
                var newScale = Mathf.Lerp(startScale, targetScale, lerp.EaseInOutQuad());

                transform.localScale = new Vector2(newScale, newScale);
                lerp += step * Time.deltaTime;

                yield return null;
            }

            transform.localScale = new Vector2(targetScale, targetScale);
            yield return null;
        }
    }
}
