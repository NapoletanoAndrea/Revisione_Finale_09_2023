using System;
using System.Collections;
using AI_Perception.Stimuli.Sources;
using UnityEngine;

namespace AI_Perception.Senses
{

	public class Hearing : Sense
	{
		[Header("Hearing Settings")] [HideInInspector]
		public Transform origin;
		public Vector3 originOffset;
		public float radius;
		public AnimationCurve intensityRadiusCurve = AnimationCurve.Linear(0, 1, 1, 1);
		public float tickInterval;

		private Vector3 Origin => origin.position + originOffset;

		[Header("Stimuli Settings")] public LayerMask stimuliMask;
		public bool checkTriggers;
		public bool useStimulusSource;

		[Header("Gizmos")] public bool displayGizmos = true;
		public Color radiusColor = new Color(1, 0, 0, .5f);

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
					if (useStimulusSource)
					{
						var stimulusSource = result.GetComponent<IStimulusSource<Sight>>();
						if (stimulusSource != null)
						{
							float distanceToTarget = Vector3.Distance(Origin, result.transform.position);
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
			Gizmos.DrawSphere(Origin, radius);
		}
	}

}