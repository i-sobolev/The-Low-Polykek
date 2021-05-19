using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionEntitiesInfo : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private RectTransform _rectTransform;
    private InteractionEntity _lastEntity;

    private Camera _playerCamera;

    private Vector3 _offset;

    private bool _isHidden;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        
        _text.alpha = 0;
        _rectTransform = GetComponent<RectTransform>();
        //_rectTransform.localScale = Vector2.zero;
    }

    private void Start()
    {
        Player.Singletone.OnRayCatchEntity += ShowInfo;

        Player.Singletone.OnDialogStart += Hide;
        Player.Singletone.OnDialogEnd += Show;

        Player.Singletone.OnItemCathed += Hide;
        Player.Singletone.OnItemThrew += Show;

        _playerCamera = PlayerCamera.CameraComponent;
    }

    private void ShowInfo(InteractionEntity newEntity)
    {
        if (_isHidden)
            return;

        if (newEntity != null)
        {
            if (_lastEntity != newEntity)
            {
                ChangeTextAlpha(1f);
                _lastEntity = newEntity;
                _text.text = newEntity.name;
                _offset = newEntity.InfoPositionOffset;
            }

            _rectTransform.anchoredPosition = _playerCamera.WorldToScreenPoint(newEntity.transform.position + _offset);

            return;
        }

        else if (_lastEntity != null && newEntity == null)
        {
            ChangeTextAlpha(0f);
            StartCoroutine(PositionDelay(_lastEntity.transform));
            _lastEntity = null;
        }
    }

    private void ChangeTextAlpha(float newAlpha)
    {
        StopAllCoroutines();
        StartCoroutine(UIAnimator.Animator.TmpAlphaSmoothChange(_text, _text.color.a, newAlpha));
        //StartCoroutine(UIAnimator.Animator.RectTransformSmoothScale(_rectTransform, _rectTransform.localScale.x, newScale));
    }

    private void Hide()
    {
        _isHidden = true;
        ChangeTextAlpha(0f);

    }
    private void Hide(Stranger stranger) => Hide();

    private void Show()
    {
        _isHidden = false;
        ChangeTextAlpha(1f);
    }

    private void Show(Stranger stranger) => Show();

    private IEnumerator PositionDelay(Transform transform)
    {
        Vector3 positionInWorld = transform.position;

        while (_text.color.a > 0)
        {
            _rectTransform.anchoredPosition = _playerCamera.WorldToScreenPoint(positionInWorld + _offset);
            yield return null;
        }
    }
}