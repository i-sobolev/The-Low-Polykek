using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCamera : MonoBehaviour
{
    public static Camera CameraComponent;

    [Header("Walk animation parameters")]
    [SerializeField] [Range(0, 20f)] private float _animationSpeed = 0;
    [SerializeField] [Range(0f, 1f)] private float _deltaPositionY = 0;
    [SerializeField] [Range(0f, 1f)] private float _deltaPositionX = 0;

    private Vector3 _offset;
    private float mult = 0;

    [Header("Focus animation parameters")]
    [SerializeField] [Range(20f, 100f)] private float _targetFieldOfView = 0;
    private float _baseFieldOfView;
    private bool _isCameraBlocked;

    private void Awake()
    {
        CameraComponent = GetComponent<Camera>();
        _baseFieldOfView = CameraComponent.fieldOfView;
    }

    private void Start()
    {
        Player.Singletone.OnDialogStart += FocusOn;
        Player.Singletone.OnDialogEnd += DeFocus;

        Player.Singletone.OnAltGrabStart += BlockCamera;
        Player.Singletone.OnAltGrabEnd += UnBlockCamera;
    }

    private void Update()
    {
        WalkAnimPlay();
    }

    private void WalkAnimPlay()
    {
        bool isMoveButtonPressed = (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0);

        if (_isCameraBlocked)
            isMoveButtonPressed = false;

        mult += isMoveButtonPressed ? Time.deltaTime * 2 : -Time.deltaTime;
        mult = Mathf.Clamp(mult, 0, 1);

        _offset = new Vector3(Mathf.Sin(Time.time * _animationSpeed / 2) * _deltaPositionX, Mathf.Sin(Time.time * _animationSpeed) * _deltaPositionY);
        transform.localPosition = _offset * mult.EaseInQuint();
    }

    private void FocusOn(Stranger stranger)
    {
        BlockCamera();
        StopAllCoroutines();
        StartCoroutine(FieldOfViewAnimation(_targetFieldOfView));
        StartCoroutine(LookAtAnimation(stranger.transform.position + stranger.CameraLookAtPositionOffset));
    }

    private void DeFocus(Stranger stranger)
    {
        UnBlockCamera();
        StopAllCoroutines();
        StartCoroutine(FieldOfViewAnimation(_baseFieldOfView));
        StartCoroutine(SmoothResetCameraRotation());
    }

    private void BlockCamera() => _isCameraBlocked = true;
    private void UnBlockCamera() => _isCameraBlocked = false;

    private IEnumerator FieldOfViewAnimation(float target)
    {
        float lerp = 0f;
        float startValue = CameraComponent.fieldOfView;

        while (lerp < 1)
        {
            CameraComponent.fieldOfView = Mathf.Lerp(startValue, target, lerp.EaseInOutQuad());
            lerp += 1f * Time.deltaTime;
            yield return null;
        }

        CameraComponent.fieldOfView = target;
    }

    private IEnumerator LookAtAnimation(Vector3 targetPosition)
    {
        Ray cameraRay = CameraComponent.ScreenPointToRay(Input.mousePosition);

        Vector3 startPosition = Vector3.zero;

        if (Physics.Raycast(cameraRay, out RaycastHit cameraRayHit))
            startPosition = cameraRayHit.point;

        var lerp = 0f;

        while (lerp < 1)
        {
            transform.LookAt(Vector3.Lerp(startPosition, targetPosition, lerp.EaseInOutQuad()));
            lerp += 0.5f * Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator SmoothResetCameraRotation()
    {
        Quaternion startRotation = transform.localRotation;
        Quaternion targetRotation = new Quaternion(0, 0, 0, 1);

        var lerp = 0f;

        while (lerp < 1)
        {
            transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, lerp.EaseInOutQuad());
            lerp += 0.75f * Time.deltaTime;
            yield return null;
        }
    }
}