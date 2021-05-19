using UnityEngine;
using UnityEngine.UI;

public class CrossHair : MonoBehaviour
{
    private Image _image;
    private RectTransform _rectTransform;
    private InteractionEntity _lastEntity;

    private bool _isHidden;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
        _isHidden = false;
    }

    private void Start()
    {
        Player.Singletone.OnRayCatchEntity += Highlight;

        Player.Singletone.OnDialogStart += Hide;
        Player.Singletone.OnDialogEnd += Show;

        Player.Singletone.OnItemCathed += Hide;
        Player.Singletone.OnItemThrew += Show;
    }

    private void Highlight(InteractionEntity interactionEntity)
    {
        if (_isHidden)
            return;

        if (_lastEntity != interactionEntity && interactionEntity != null)
        {
            ChangeScaleAndAlpha(1, 1.3f);
            _lastEntity = interactionEntity;
            return;
        }

        else if (_lastEntity != null && interactionEntity == null)
        {
            ChangeScaleAndAlpha(0.25f, 1f);
            _lastEntity = null;
        }
    }

    private void ChangeScaleAndAlpha(float newAlpha, float newScale)
    {
        StopAllCoroutines();
        StartCoroutine(UIAnimator.Animator.ImageAlphaSmoothChange(_image, _image.color.a, newAlpha));
        StartCoroutine(UIAnimator.Animator.RectTransformSmoothScale(_rectTransform, _rectTransform.localScale.x, newScale));
    }

    private void Hide()
    {
        _isHidden = true;
        StopAllCoroutines();
        StartCoroutine(UIAnimator.Animator.ImageAlphaSmoothChange(_image, _image.color.a, 0));
    }

    private void Hide(Stranger stranger) => Hide();

    private void Show()
    {
        _isHidden = false;
        StopAllCoroutines();
        StartCoroutine(UIAnimator.Animator.ImageAlphaSmoothChange(_image, _image.color.a, 1));
    }

    private void Show(Stranger stranger) => Show();
}