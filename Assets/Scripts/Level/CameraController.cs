using Cinemachine;
using UnitAction;
using UnityEngine;

namespace Level
{
    public class CameraController : MonoBehaviour
    {
        private const float MinFollowOffsetY = 2.0f;

        private const float MaxFollowOffsetY = 12.0f;

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
            case ShootAction:
                HideActionCamera();
                break;
            }
        }

        private void ShowActionCamera() { actionCamera.SetActive(true); }

        private void HideActionCamera() { actionCamera.SetActive(false); }

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

            var transform1 = sceneCamera.transform;
            var moveVector = transform1.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
            transform1.position += moveSpeed * Time.deltaTime * moveVector;
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
            switch (Input.mouseScrollDelta.y)
            {
            case > 0:
                _targetFollowOffset.y -= zoomAmount;
                break;
            case < 0:
                _targetFollowOffset.y += zoomAmount;
                break;
            }

            _targetFollowOffset.y = Mathf.Clamp(_targetFollowOffset.y, MinFollowOffsetY, MaxFollowOffsetY);
            _sceneTransposer.m_FollowOffset = Vector3.Lerp(_sceneTransposer.m_FollowOffset, _targetFollowOffset, t: zoomSpeed * Time.deltaTime);
        }
    }
}
