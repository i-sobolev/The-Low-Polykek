using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player settings")]
    public float InteractionDistance;
    public float PlayerSpeed;
    public float MouseSensetivity;

    private Camera _camera;

    private Transform _parentTransform;
    private float _rotationX = 0f;
    private float _heightDelta;

    public InteractionEntity InteractionEntityInRay { get; private set; }

    public static Player Singletone { get; private set; }

    public delegate void InteractionAction(InteractionEntity interactionEntity);
    public event InteractionAction OnRayCatchEntity;

    public delegate void DialogAction(Stranger stranger);
    public event DialogAction OnDialogStart;
    public event DialogAction OnDialogEnd;

    public delegate void Action();
    public event Action OnItemCathed;
    public event Action OnItemThrew;

    public delegate void AltGrabAction();
    public event AltGrabAction OnAltGrabStart;
    public event AltGrabAction OnAltGrabEnd;

    public Inventory Inventory { get; private set; }

    private CharacterController _characterController;

    private bool _isMovementBlocked;

    private void Awake()
    {
        Singletone = this;

        Cursor.lockState = CursorLockMode.Locked;

        _parentTransform = GetComponentsInParent<Transform>()[1];
        _characterController = GetComponentInParent<CharacterController>();

        Inventory = GetComponent<Inventory>();
    }

    private void Start() => _camera = PlayerCamera.CameraComponent;

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.LeftShift))
            PlayerSpeed *= 3;

        if (Input.GetKeyUp(KeyCode.LeftShift))
            PlayerSpeed /= 3;

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;

            else
                Cursor.lockState = CursorLockMode.Locked;
        }
#endif

        RotateCamera();
        Move();

        RaycastInstance();

        if (Input.GetButtonDown("Interaction"))
            Interaction();
    }

    #region Movement
    private void Move()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        if (_isMovementBlocked)
        {
            verticalInput = 0f;
            horizontalInput = 0f;
        }

        if (!_characterController.isGrounded)
            _heightDelta += 6 * Time.deltaTime;

        else
            _heightDelta = 0;

        Vector3 moveDirectionVector = _parentTransform.forward * verticalInput + _parentTransform.right * horizontalInput * 0.9f + new Vector3(0, -_heightDelta, 0);

        _characterController.Move(moveDirectionVector * Time.deltaTime * PlayerSpeed);
    }

    private void RotateCamera()
    {
        float multiplier = Time.deltaTime * MouseSensetivity;

        var mousePositonX = Input.GetAxis("Mouse X") * multiplier;
        var mousePositonY = Input.GetAxis("Mouse Y") * multiplier;

        if (_isMovementBlocked || Cursor.lockState == CursorLockMode.None)
        {
            mousePositonY = 0f;
            mousePositonX = 0f;
            //return;
        }

        _rotationX -= mousePositonY;
        _rotationX = Mathf.Clamp(_rotationX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);

        _parentTransform.Rotate(Vector3.up * mousePositonX);
    }
    #endregion

    #region Interactions
    private void RaycastInstance()
    {
        Ray cameraRay = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(cameraRay, out RaycastHit cameraRayHit, InteractionDistance) && cameraRayHit.collider.GetComponent<InteractionEntity>())
            InteractionEntityInRay = cameraRayHit.collider.GetComponent<InteractionEntity>();

        else
            InteractionEntityInRay = null;

        OnRayCatchEntity?.Invoke(InteractionEntityInRay);
    }

    private void Interaction()
    {
        if (InteractionEntityInRay)
        {
            InteractionEntityInRay.Interact(this);
            InteractionEntityInRay = null;
            OnRayCatchEntity?.Invoke(InteractionEntityInRay);
        }
    }

    public void CatchPhysicalItem(bool altGrab)
    {
        Ray cameraRay = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(cameraRay, out RaycastHit cameraRayHit))
        {
            var itemRigidBody = cameraRayHit.collider.gameObject.GetComponent<Rigidbody>();
            var hitPosition = cameraRayHit.point;
            var pointOnCollider = cameraRayHit.collider.ClosestPoint(hitPosition);

            float touchDistance = Vector3.Distance(_camera.transform.position, pointOnCollider);

            var jointGameObject = Instantiate(new GameObject("Joint"), hitPosition, Quaternion.identity);
            var itemCatcher = jointGameObject.AddComponent<PhysicalItemCatcher>();

            jointGameObject.transform.LookAt(_camera.transform.position);

            itemCatcher.Set(itemRigidBody, pointOnCollider);

            if (!altGrab)
                StartCoroutine(GrabPhysicalItem(jointGameObject, touchDistance));

            else
                StartCoroutine(AltGrabPhysicalItem(jointGameObject));
        }
    }

    private IEnumerator GrabPhysicalItem(GameObject jointGameObject, float distance)
    {
        OnItemCathed?.Invoke();

        FixedJoint joint = jointGameObject.GetComponent<FixedJoint>();

        while (true)
        {
            Ray cameraRay = _camera.ScreenPointToRay(Input.mousePosition);

            jointGameObject.transform.position = Vector3.Lerp(jointGameObject.transform.position, cameraRay.GetPoint(distance), 0.1f);
            jointGameObject.transform.LookAt(_camera.transform.position);

            if (Input.GetButtonUp("Interaction") || joint == null)
            {
                OnItemThrew?.Invoke();
                Destroy(jointGameObject);
                break;
            }

            yield return null;
        }
    }

    private IEnumerator AltGrabPhysicalItem(GameObject jointGameObject)
    {
        OnItemCathed?.Invoke();
        OnAltGrabStart?.Invoke();

        _isMovementBlocked = true;

        while (true)
        {
            float deltaPositionX = Input.GetAxis("Mouse X") * Time.deltaTime;
            float deltaPositionY = Input.GetAxis("Mouse Y") * Time.deltaTime;

            jointGameObject.transform.Translate(new Vector3(-deltaPositionX, 0, -deltaPositionY));

            if (Input.GetButtonUp("Interaction"))
            {
                OnItemThrew?.Invoke();
                OnAltGrabEnd?.Invoke();

                Destroy(jointGameObject);

                _isMovementBlocked = false;

                break;
            }

            yield return null;
        }
    }
    #endregion

    public void StartDialog(Stranger stranger)
    {
        OnDialogStart?.Invoke(stranger);
        _isMovementBlocked = true;
    }

    public void EndDialog()
    {
        OnDialogEnd?.Invoke(null);
        _isMovementBlocked = false;
    }

    public void SetPosition(Vector3 newPosition)
    {
        _characterController.enabled = false;
        _parentTransform.position = newPosition;
        _characterController.enabled = true;
    }
}












//    ⡴⠑⡄⠀⠀⠀⠀⠀⠀⠀ ⣀⣀⣤⣤⣤⣀⡀
//   ⠸⡇⠀⠿⡀⠀⠀⠀⣀⡴⢿⣿⣿⣿⣿⣿⣿⣿⣷⣦⡀
//  ⠀⠀⠀⠀⠑⢄⣠⠾⠁⣀⣄⡈⠙⣿⣿⣿⣿⣿⣿⣿⣿⣆
//  ⠀⠀⠀⠀⢀⡀⠁   ⠀⠙⠛⠂⠈⣿⣿⣿⣿⣿⠿⡿⢿⣆
//   ⠀⠀⠀⢀⡾⣁⣀⠀⠴⠂⠙⣗⡀⠀⢻⣿⣿⠭⢤⣴⣦⣤⣹⠀⠀⠀⢀⢴⣶⣆
//  ⠀⠀⢀⣾⣿⣿⣿⣷⣮⣽⣾⣿⣥⣴⣿⣿⡿⢂⠔⢚⡿⢿⣿⣦⣴⣾⠸⣼⡿
//  ⠀⢀⡞⠁⠙⠻⠿⠟⠉⠀⠛⢹⣿⣿⣿⣿⣿⣌⢤⣼⣿⣾⣿⡟⠉
//  ⠀⣾⣷⣶⠇⠀⠀⣤⣄⣀⡀⠈⠻⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇
//  ⠀⠉⠈⠉⠀⠀⢦⡈⢻⣿⣿⣿⣶⣶⣶⣶⣤⣽⡹⣿⣿⣿⣿⡇
//  ⠀⠀⠀⠀⠀⠀⠀⠉⠲⣽⡻⢿⣿⣿⣿⣿⣿⣿⣷⣜⣿⣿⣿⡇  
//  ⠀⠀ ⠀⠀⠀⠀⠀⢸⣿⣿⣷⣶⣮⣭⣽⣿⣿⣿⣿⣿⣿⣿⠇
//  ⠀⠀⠀⠀⠀⠀⣀⣀⣈⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠇
//  ⠀⠀⠀⠀⠀⠀⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠃