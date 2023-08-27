using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    [SerializeField]
    private float moveSpeed = 10.0f;

    [SerializeField]
    private float rotateSpeed = 160.0f;

    [SerializeField]
    private float zoomAmount = 1.0f;

    [SerializeField]
    private float zoomSpeed = 2.0f;

    private const float MinFollowOffsetY = 2.0f;

    private const float MaxFollowOffsetY = 12.0f;

    private CinemachineTransposer _transposer;

    private Vector3 _targetFollowOffset;

    private void Start()
    {
        _transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        _targetFollowOffset = _transposer.m_FollowOffset;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
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

        var moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        transform.position += moveSpeed * Time.deltaTime * moveVector;
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

        transform.eulerAngles += rotateSpeed * Time.deltaTime * rotationVector;
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

        _transposer.m_FollowOffset = Vector3.Lerp(_transposer.m_FollowOffset, _targetFollowOffset, zoomSpeed * Time.deltaTime);
    }
}
