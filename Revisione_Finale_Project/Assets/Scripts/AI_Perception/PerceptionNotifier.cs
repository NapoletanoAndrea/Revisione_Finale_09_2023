using System;
using System.Collections.Generic;
using AI_Perception.Senses;
using AYellowpaper;
using UnityEngine;

namespace AI_Perception
{

	public class PerceptionNotifier : MonoBehaviour, ISenseNotifier
	{
		[RequireInterface(typeof(ISenseNotifier))]
		public List<MonoBehaviour> notifyTo = new();

		public event Action<float, Sense> perceptionTriggered;

		private void TriggerPerception(float intensity, Sense sense)
		{
			perceptionTriggered?.Invoke(intensity, sense);
			foreach (var notifier in notifyTo)
			{
				if(sense.notifiers.Contains(notifier))
					continue;
				
				((ISenseNotifier) notifier).OnSenseTriggered(intensity, sense);
			}
		}

		public void OnSenseTriggered(float intensity, Sense sense)
		{
			TriggerPerception(intensity, sense);
		}
	}

}