using System.Collections;
using AI_Perception.Stimuli.Sources;
using UnityEngine;

namespace AI_Perception.Senses
{

	public class Sight : Sense
	{
		[Header("Sight Settings")] [HideInInspector]
		public Transform origin;
		public Vector3 originOffset;
		public float radius;
		public AnimationCurve intensityRadiusCurve = AnimationCurve.Linear(0, 1, 1, 1);
		public float fovAngle;
		public float fovAngleOffset;
		public LayerMask obstacleMask;
		public float tickInterval;

		private Vector3 Origin => origin.position + originOffset;

		[Header("Stimuli Settings")] public LayerMask stimuliMask;
		public bool checkTriggers;
		public bool useStimulusSource;

		[Header("Gizmos")] public bool displayGizmos = true;
		public Color radiusColor = Color.red;
		public Color fovColor = Color.yellow;

		private Coroutine _checkCor;

		private void OnValidate()
		{
			if (origin == null)
				origin = transform;
		}

		private void OnEnable()
		{
			_checkCor = StartCoroutine(CheckSources());
		}

		private void OnDisable()
		{
			StopCoroutine(_checkCor);
			_checkCor = null;
		}

		private IEnumerator CheckSources()
		{
			var waitForSeconds = new WaitForSeconds(tickInterval);
			while (true)
			{
				var results = Physics.OverlapSphere(Origin, radius, stimuliMask, checkTriggers.ToQueryTriggerInteraction());
				foreach (var result in results)
				{
					#region fov calc

					Vector3 directionToTarget = (result.transform.position - Origin).normalized;
					if (Vector3.Angle(transform.forward, directionToTarget) >= fovAngle / 2)
					{
						continue;
					}

					float distanceToTarget = Vector3.Distance(Origin, result.transform.position);
					if (Physics.Raycast(Origin, directionToTarget, distanceToTarget, obstacleMask, QueryTriggerInteraction.Ignore))
					{
						continue;
					}

					#endregion

					if (useStimulusSource)
					{
						var stimulusSource = result.GetComponent<IStimulusSource<Sight>>();
						if (stimulusSource != null)
						{
							if (stimulusSource.Intensity < triggerIntensity * intensityRadiusCurve.Evaluate(distanceToTarget))
								continue;

							Trigger(stimulusSource.Intensity);
						}
						continue;
					}

					Trigger(Mathf.Infinity);
				}

				yield return waitForSeconds;
			}
		}

		private void OnDrawGizmosSelected()
		{
			if (!displayGizmos)
				return;

			Gizmos.color = radiusColor;
			Gizmos.DrawWireSphere(Origin, radius);

			Gizmos.color = fovColor;
			Gizmos.DrawRay(Origin, Quaternion.Euler(origin.up * (fovAngle / 2 + fovAngleOffset)) * origin.forward * radius);
			Gizmos.DrawRay(Origin, Quaternion.Euler(-origin.up * (fovAngle / 2 - fovAngleOffset)) * origin.forward * radius);
		}
	}

}