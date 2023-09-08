using System;
using System.Collections.Generic;
using AYellowpaper;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AI_Perception.Senses
{

	public class Sense : MonoBehaviour
	{
		[RequireInterface(typeof(ISenseNotifier))] 
		public List<MonoBehaviour> notifiers;
		[Min(0), DisableIf(nameof(persistIndefinitely))] public float persistenceSeconds;
		public bool persistIndefinitely;

		[Min(.1f)] public float triggerIntensity;
		
		public event Action<float, Sense> triggered;

		private float _persistenceCount;
		public float PersistenceCount => _persistenceCount;

		private bool _isAlerted;
		public bool IsAlerted;

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

		protected void Trigger(float intensity)
		{
			triggered?.Invoke(intensity, this);
			foreach (var notifier in notifiers)
			{
				((ISenseNotifier) notifier).OnSenseTriggered(intensity, this);
			}
			
			_persistenceCount = 0;
			_isAlerted = true;
		}
		
	}

}