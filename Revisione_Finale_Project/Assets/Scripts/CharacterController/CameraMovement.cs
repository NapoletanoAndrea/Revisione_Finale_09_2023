using UnityEngine;

namespace CharacterController
{

    public class CameraMovement : MonoBehaviour
    {
        public new Camera camera;
        public Transform followTransform;
        public Transform playerTransform;
        public bool followPlayer;

        public float cameraSpeedX;
        public float cameraSpeedY;
        public float cameraSpeedMultiplier;
        public bool invertX;
        public bool invertY;

        private float _mouseX;
        private float _mouseY;

        private Vector3 _startOffset;

        private void InitCamera()
        {
            if (!camera)
                camera = Camera.main;

            if (!camera)
                camera = FindObjectOfType<Camera>();
        }

        private void OnValidate()
        {
            InitCamera();
            playerTransform ??= GameObject.FindWithTag("Player").transform;
        }

        private void Awake()
        {
            InitCamera();
            _startOffset = followTransform.position - playerTransform.position;
        }

        private void Update()
        {
            _mouseX = Input.GetAxis("Mouse X");
            _mouseY = Input.GetAxis("Mouse Y");

            float xRotation = _mouseY * invertY.ToInt(-1, 1) * cameraSpeedX;
            float yRotation = _mouseX * invertX.ToInt(-1, 1) * cameraSpeedY;
            followTransform.eulerAngles += new Vector3(xRotation, yRotation) * cameraSpeedMultiplier * Time.deltaTime;
        }

        private void LateUpdate()
        {
            if (followPlayer)
            {
                followTransform.position = playerTransform.position + _startOffset;
            }
        }

    }

}
