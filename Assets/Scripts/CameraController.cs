using Cinemachine;
using UnitAction;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera sceneCamera;

    [SerializeField]
    private float moveSpeed = 10.0f;

    [SerializeField]
    private float rotateSpeed = 160.0f;

    [SerializeField]
    private float zoomAmount = 1.0f;

    [SerializeField]
    private float zoomSpeed = 2.0f;

    [SerializeField]
    private GameObject actionCamera;

    [SerializeField]
    private Vector3 actionLookOffset;

    private const float MinFollowOffsetY = 2.0f;

    private const float MaxFollowOffsetY = 12.0f;

    private CinemachineTransposer _sceneTransposer;

    private Vector3 _targetFollowOffset;

    private void Start()
    {
        _sceneTransposer = sceneCamera.GetCinemachineComponent<CinemachineTransposer>();
        _targetFollowOffset = _sceneTransposer.m_FollowOffset;

        BaseAction.OnAnyActionStarted += OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += OnAnyActionCompleted;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    private void OnAnyActionStarted(BaseAction action)
    {
        switch (action)
        {
            case ShootAction shootAction:
                ShowActionCamera();
                var height = Vector3.up * actionLookOffset.y;
                var dir = (shootAction.TargetUnit.WorldPosition - shootAction.OwnerUnit.WorldPosition).normalized;
                var offset = Quaternion.Euler(0, 90, 0) * dir * actionLookOffset.x;
                actionCamera.transform.position = shootAction.OwnerUnit.WorldPosition + dir * actionLookOffset.z + height + offset;
                actionCamera.transform.LookAt(shootAction.TargetUnit.transform.position + height);
                break;
        }
    }

    private void OnAnyActionCompleted(BaseAction action)
    {
        switch (action)
        {
            case ShootAction shootAction:
                HideActionCamera();
                break;
        }
    }

    private void ShowActionCamera()
    {
        actionCamera.SetActive(true);
    }

    private void HideActionCamera()
    {
        actionCamera.SetActive(false);
    }

    private void HandleMovement()
    {
        var inputMoveDir = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.z += 1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.z -= 1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x -= 1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x += 1f;
        }

        var moveVector = sceneCamera.transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        sceneCamera.transform.position += moveSpeed * Time.deltaTime * moveVector;
    }

    private void HandleRotation()
    {
        var rotationVector = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector.y -= 1f;
        }

        if (Input.GetKey(KeyCode.E))
        {
            rotationVector.y += 1f;
        }

        sceneCamera.transform.eulerAngles += rotateSpeed * Time.deltaTime * rotationVector;
    }

    private void HandleZoom()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            _targetFollowOffset.y -= zoomAmount;
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            _targetFollowOffset.y += zoomAmount;
        }

        _targetFollowOffset.y = Mathf.Clamp(_targetFollowOffset.y, MinFollowOffsetY, MaxFollowOffsetY);

        _sceneTransposer.m_FollowOffset = Vector3.Lerp(_sceneTransposer.m_FollowOffset, _targetFollowOffset, zoomSpeed * Time.deltaTime);
    }
}
