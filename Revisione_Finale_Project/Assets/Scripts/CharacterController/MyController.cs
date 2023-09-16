using AI_Perception.Stimuli.Sources;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CharacterController
{

	public class MyController : MonoBehaviour
	{
		private CharacterMovement _movement;
		private VisibleBody _visibleBody;

		[SerializeField] private KeyCode _jumpKey = KeyCode.Space;
		[SerializeField] private KeyCode _crouchKey = KeyCode.C;
		[SerializeField] private KeyCode _runKey = KeyCode.LeftShift;

		[ReadOnly] public Vector2 moveInput;
		public float crouchMultiplier;

		private void Awake()
		{
			_movement = GetComponent<CharacterMovement>();
			_visibleBody = GetComponent<VisibleBody>();
		}

		private void Update()
		{
			if (Input.GetKeyDown(_jumpKey))
			{
				_movement.TryJumping();
			}

			if (Input.GetKeyDown(_crouchKey))
				_movement.isCrouched = !_movement.isCrouched;

			if (Input.GetKeyDown(_runKey))
				_movement.isRunning = !_movement.isRunning;

			moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
			_visibleBody.centerOffset = _movement.isCrouched ? Vector3.down * crouchMultiplier : Vector3.zero;
		}

		private void FixedUpdate()
		{
			_movement.Move(moveInput * Time.fixedDeltaTime);
			
			if (moveInput.magnitude < Mathf.Epsilon)
				return;
			
			_movement.Rotate(moveInput, Time.fixedDeltaTime);
		}
	}

}