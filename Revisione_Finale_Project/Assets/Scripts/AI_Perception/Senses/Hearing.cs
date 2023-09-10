using System;
using System.Collections;
using System.Collections.Generic;
using AI_Perception.Stimuli;
using AI_Perception.Stimuli.Sources;
using UnityEngine;

namespace AI_Perception.Senses
{

	[Serializable]
	public class HearingSettings
	{
		[Header("Hearing Settings")]
		public float radius;
		public AnimationCurve intensityRadiusCurve = AnimationCurve.Linear(0, 1, 1, 1);
		public float tickInterval;
		
		[Header("Stimuli Settings")] public LayerMask stimuliMask;
		public bool checkTriggers;
		public bool useStimulusSource;
		public List<StimulusType> detectableStimuli;

		[Header("Gizmos")] public bool displayGizmos = true;
		public Color radiusColor = new(1, 0, 0, .5f);
	}

	public class Hearing : Sense
	{
		public Transform origin;
		public Vector3 originOffset;
		private Vector3 Origin => origin.position + originOffset;

		public Preset<HearingSettings> settingsPresets;

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
		
		protected override List<StimulusType> GetDetectableStimuli()
		{
			return settingsPresets.Value.detectableStimuli;
		}

		private IEnumerator CheckSources()
		{
			while (true)
			{
				var hearingSettings = settingsPresets.Value;
				var results = Physics.OverlapSphere(Origin, hearingSettings.radius, hearingSettings.stimuliMask, hearingSettings.checkTriggers.ToQueryTriggerInteraction());
				foreach (var result in results)
				{
					if (hearingSettings.useStimulusSource)
					{
						var stimulusSource = result.GetComponent<IStimulusSource<Sight>>();
						if (stimulusSource != null)
						{
							float distanceToTarget = Vector3.Distance(Origin, result.transform.position);
							if (stimulusSource.Intensity < triggerIntensity * hearingSettings.intensityRadiusCurve.Evaluate(distanceToTarget))
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
			
			var hearingSettings = settingsPresets.EditorValue;
			
			if (!hearingSettings.displayGizmos)
				return;

			Gizmos.color = hearingSettings.radiusColor;
			Gizmos.DrawSphere(Origin, hearingSettings.radius);
		}
		
	}

}