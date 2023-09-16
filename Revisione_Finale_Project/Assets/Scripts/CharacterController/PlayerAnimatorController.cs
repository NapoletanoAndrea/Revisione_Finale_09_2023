using AI_Perception.Stimuli.Sources;
using CharacterController;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
	[SerializeField] private CharacterMovement _movement;
	[SerializeField] private Animator _animator;
	[SerializeField] private NoiseSource _noiseSource;

	public float movementThreshold = .1f;
	public string walkString;
	public string crouchString;

	private bool IsMoving => _movement.rigidbody.velocity.magnitude > movementThreshold;
	private float _baseIntensity;

	private void Awake()
	{
		_baseIntensity = _noiseSource.intensity;
	}

	private void Update()
	{
		_animator.SetBool(crouchString, _movement.isCrouched);
		_animator.SetBool(walkString, IsMoving);
		
		_noiseSource.intensity = _baseIntensity * (IsMoving.ToInt() * _movement.isCrouched.ToFloat(1, .5f));
	}

}