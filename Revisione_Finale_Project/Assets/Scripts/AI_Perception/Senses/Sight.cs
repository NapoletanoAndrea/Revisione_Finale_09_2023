using System;
using System.Collections;
using System.Collections.Generic;
using AI_Perception.Stimuli;
using AI_Perception.Stimuli.Sources;
using UnityEngine;

namespace AI_Perception.Senses
{

	[Serializable]
	public class SightSettings
	{
		[Header("Sight")]
		public float radius;
		public AnimationCurve intensityRadiusCurve = AnimationCurve.Linear(0, 1, 1, 1);
		public float fovAngle;
		public float fovAngleOffset;
		public LayerMask obstacleMask;
		public float tickInterval;

		[Header("Stimuli")] public LayerMask stimuliMask;
		public bool checkTriggers;
		public bool useStimulusSource;
		public List<StimulusType> detectableStimuli;

		[Header("Gizmos")] public bool displayGizmos = true;
		public Color radiusColor = Color.red;
		public Color fovColor = Color.yellow;
	}
	
	public class Sight : Sense
	{
		public Transform origin;
		public Vector3 originOffset;
		public Preset<SightSettings> settingsPresets;
		private Coroutine _checkCor;
		
		public Vector3 Origin => origin.position + originOffset;

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
		
		protected override List<StimulusType> GetDetectableStimuli()
		{
			return settingsPresets.Value.detectableStimuli;
		}

		private IEnumerator CheckSources()
		{
			while (true)
			{
				var sightSettings = settingsPresets.Value;
				var results = Physics.OverlapSphere(Origin, sightSettings.radius, sightSettings.stimuliMask, sightSettings.checkTriggers.ToQueryTriggerInteraction());
				foreach (var result in results)
				{
					#region fov calc

					IStimulusSource stimulusSource = null;
					Vector3 sourcePos = result.transform.position;

					if (sightSettings.useStimulusSource)
					{
						stimulusSource = result.GetComponent<IStimulusSource<Sight>>();
						if (stimulusSource != null)
						{
							sourcePos = stimulusSource.Center;
						}
					}
					
					Vector3 directionToTarget = (sourcePos - Origin).normalized;
					var horizontalDirection = directionToTarget;
					horizontalDirection.y = 0;
					horizontalDirection = horizontalDirection.normalized;
					
					if (Vector3.Angle(transform.forward, horizontalDirection) >= sightSettings.fovAngle / 2)
					{
						continue;
					}

					float distanceToTarget = Vector3.Distance(Origin, sourcePos);
					if (Physics.Raycast(Origin, directionToTarget, distanceToTarget, sightSettings.obstacleMask, QueryTriggerInteraction.Ignore))
					{
						continue;
					}

					#endregion

					if (sightSettings.useStimulusSource)
					{
						if (stimulusSource != null)
						{
							if (stimulusSource.Intensity < triggerIntensity * sightSettings.intensityRadiusCurve.Evaluate(distanceToTarget))
								continue;

							Trigger(stimulusSource);
						}
						continue;
					}

					Trigger(null);
				}

				yield return new WaitForSeconds(settingsPresets.Value.tickInterval);
			}
		}

		private void OnDrawGizmosSelected()
		{
			if(settingsPresets == null)
				return;
			
			if(settingsPresets.EditorValue == null)
				return;
			
			var sightSettings = settingsPresets.EditorValue;

			if (!sightSettings.displayGizmos)
				return;

			Gizmos.color = sightSettings.radiusColor;
			Gizmos.DrawWireSphere(Origin, sightSettings.radius);

			Gizmos.color = sightSettings.fovColor;
			Gizmos.DrawRay(Origin, Quaternion.Euler(origin.up * (sightSettings.fovAngle / 2 + sightSettings.fovAngleOffset)) * origin.forward * sightSettings.radius);
			Gizmos.DrawRay(Origin, Quaternion.Euler(-origin.up * (sightSettings.fovAngle / 2 - sightSettings.fovAngleOffset)) * origin.forward * sightSettings.radius);
		}
		
	}

}