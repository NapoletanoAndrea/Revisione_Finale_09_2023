using System.Collections;
using UnityEngine;

namespace CharacterController
{

	public class CharacterMovement : MonoBehaviour
	{
		public new Transform transform;
		public new Rigidbody rigidbody;
		public Transform cameraTransform;

		[Header("Movement")] public float speedMultiplier;
		public float walkSpeed;
		public float runSpeed;
		public float crouchWalkSpeed;
		public float crouchRunSpeed;
		public float airWalkSpeed;
		public float airRunSpeed;

		[Header("Jumping")] public float jumpForce;
		public ForceMode forceMode;
		[Min(0)] public int maxConsecutiveJumps = 1;
		public AnimationCurve consecutiveJumpForceMultipliersCurve = AnimationCurve.Linear(0, 1, 1, 1);
		[Min(0)] public float disableJumpSeconds = .1f;
		public Transform groundCheck;
		public float checkRadius;
		public LayerMask groundMask;
		public Color gizmosColor = Color.green;

		[Header("Rotation")] public float rotationSpeed;
		public AnimationCurve rotationCurve;
		public bool isInstant;

		[Header("Runtime Values")] public bool isGrounded;
		public bool isCrouched;
		public bool isRunning;

		public int consecutiveJumpCount;
		private Coroutine disableJumpCoroutine = null;

		private void OnValidate()
		{
			if (transform == null)
			{
				transform = GetComponent<Transform>();
			}
			
			if (rigidbody == null)
			{
				rigidbody = GetComponent<Rigidbody>();
			}

			if (cameraTransform == null)
			{
				if (Camera.main != null)
				{
					cameraTransform = Camera.main.transform;
				}
				else
				{
					cameraTransform = FindObjectOfType<Camera>().transform;
				}
			}
		}

		private void Update()
		{
			isGrounded = CheckGround();

			if (isGrounded && disableJumpCoroutine == null)
				consecutiveJumpCount = 0;
		}

		#region Movement

		private float GetSpeed()
		{
			float currSpeed = walkSpeed;

			if (isGrounded)
			{
				currSpeed = airWalkSpeed;
				if (isRunning)
					return airRunSpeed;
			}

			if (isRunning)
			{
				currSpeed = runSpeed;
				if (isCrouched)
					return crouchRunSpeed;
			}
			else if (isCrouched)
			{
				return crouchWalkSpeed;
			}

			return currSpeed;
		}

		public void Move(Vector2 movementInput)
		{
			Move(movementInput.InputToDirection(cameraTransform));
		}

		public void Move(Vector3 direction)
		{
			var movementVector = GetSpeed() * speedMultiplier * direction;
			movementVector.y += rigidbody.velocity.y;
			rigidbody.velocity = movementVector;
		}

		public void Rotate(Vector2 movementInput, float deltaTime)
		{
			Rotate(movementInput.InputToDirection(cameraTransform), deltaTime);
		}

		public void Rotate(Vector3 direction, float deltaTime)
		{
			if (isInstant)
			{
				transform.forward = direction;
				return;
			}
			transform.forward = Vector3.Lerp(transform.forward, direction, rotationSpeed * deltaTime);
		}

		#endregion

		#region Jumping

		public void TryJumping()
		{
			if ((isGrounded || consecutiveJumpCount < maxConsecutiveJumps) && disableJumpCoroutine == null)
			{
				Jump();
			}
		}

		public void ForceJump()
		{
			Jump();
		}

		private void Jump()
		{
			if (maxConsecutiveJumps < 1)
				return;
			
			consecutiveJumpCount++;
			float normalizedJumpCount = maxConsecutiveJumps <= 1 ? 0 : Mathf.InverseLerp(1, maxConsecutiveJumps, consecutiveJumpCount);
			float consecutiveMultiplier = consecutiveJumpForceMultipliersCurve.Evaluate(normalizedJumpCount);
			rigidbody.AddForce(Vector3.up * jumpForce * consecutiveMultiplier, forceMode);

			if (disableJumpCoroutine != null)
			{
				StopCoroutine(disableJumpCoroutine);
			}
			disableJumpCoroutine = StartCoroutine(DisableJumpCoroutine());
		}

		private IEnumerator DisableJumpCoroutine()
		{
			yield return new WaitForSeconds(disableJumpSeconds);
			disableJumpCoroutine = null;
		}

		private bool CheckGround()
		{
			return Physics.CheckSphere(groundCheck.position, checkRadius, groundMask, QueryTriggerInteraction.Ignore);
		}

		#endregion

		private void OnDrawGizmosSelected()
		{
			if (groundCheck == null)
			{
				return;
			}

			Gizmos.color = gizmosColor;
			Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
		}
	}

}