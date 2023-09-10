using System;
using System.Collections.Generic;
using AI_Perception.Interfaces;
using AI_Perception.Stimuli;
using AI_Perception.Stimuli.Sources;
using AYellowpaper;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AI_Perception.Senses
{

	public abstract class Sense : MonoBehaviour
	{
		[RequireInterface(typeof(IPerceptionReceiver))]
		public List<MonoBehaviour> receivers;
		[Min(0), DisableIf(nameof(persistIndefinitely))]
		public float persistenceSeconds;
		public bool persistIndefinitely;

		[Min(.1f)] public float triggerIntensity;

		public event Action<IStimulusSource, Sense> triggered;

		private float _persistenceCount;
		public float PersistenceCount => _persistenceCount;

		private bool _isAlerted;
		public bool IsAlerted => _isAlerted;

		public Type Type => GetType();

		protected virtual void Update()
		{
			if (!persistIndefinitely && _isAlerted)
			{
				_persistenceCount += Time.deltaTime;
				if (_persistenceCount > persistenceSeconds)
				{
					_isAlerted = false;
				}
			}
		}

		protected void Trigger(IStimulusSource stimulusSource)
		{
			if (!IsValid(GetDetectableStimuli(), stimulusSource.StimulusType))
				return;
			
			triggered?.Invoke(stimulusSource, this);
			foreach (var notifier in receivers)
			{
				((IPerceptionReceiver) notifier).OnSenseTriggered(stimulusSource, this);
			}

			_persistenceCount = 0;
			_isAlerted = true;
		}

		private bool IsValid(List<StimulusType> types, StimulusType type)
		{
			if (types == null)
				return true;

			return types.Count == 0 || types.Contains(type);
		}

		protected abstract List<StimulusType> GetDetectableStimuli();

	}

}