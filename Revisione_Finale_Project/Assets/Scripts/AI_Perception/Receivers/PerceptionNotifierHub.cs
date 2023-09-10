using System.Collections.Generic;
using AI_Perception.Interfaces;
using AI_Perception.Senses;
using AI_Perception.Stimuli.Sources;
using AYellowpaper;
using UnityEngine;

namespace AI_Perception.Receivers
{

	public class PerceptionNotifierHub : MonoBehaviour, IPerceptionReceiver
	{
		[RequireInterface(typeof(IPerceptionReceiver))]
		public List<MonoBehaviour> notifyTo = new();
		
		private void TriggerPerception(IStimulusSource stimulusSource, Sense sense)
		{
			foreach (var notifier in notifyTo)
			{
				if(sense.receivers.Contains(notifier))
					continue;
				
				((IPerceptionReceiver) notifier).OnSenseTriggered(stimulusSource, sense);
			}
		}

		public void OnSenseTriggered(IStimulusSource stimulusSource, Sense sense)
		{
			TriggerPerception(stimulusSource, sense);
		}
	}

}