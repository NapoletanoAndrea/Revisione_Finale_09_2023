using UnityEngine;

namespace CharacterController
{

	public class MyController : MonoBehaviour
	{
		private CharacterMovement _movement;

		[SerializeField] private KeyCode _jumpKey = KeyCode.Space;
		[SerializeField] private KeyCode _crouchKey = KeyCode.C;
		[SerializeField] private KeyCode _runKey = KeyCode.LeftShift;

		private Vector2 moveInput;

		private void Awake()
		{
			_movement = GetComponent<CharacterMovement>();
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
		}

		private void FixedUpdate()
		{
			if (moveInput.magnitude < Mathf.Epsilon)
				return;
			
			_movement.Move(moveInput * Time.fixedDeltaTime);
			_movement.Rotate(moveInput, Time.fixedDeltaTime);
		}
	}

}