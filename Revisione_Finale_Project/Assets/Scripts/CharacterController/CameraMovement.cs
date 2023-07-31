using System;
using UnityEngine;

namespace CharacterController
{

    public class CameraMovement : MonoBehaviour
    {
        public new Camera camera;

        public bool invertX;
        public bool invertY;

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
        }

        private void Awake()
        {
            InitCamera();
        }

        private void Update()
        {
            
        }
    }

}
