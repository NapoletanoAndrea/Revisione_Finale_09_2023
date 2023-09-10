using System.Collections;
using System.Collections.Generic;
using AI_Perception.Data;
using AI_Perception.Interfaces;
using AI_Perception.Stimuli;
using AI_Perception.Stimuli.Sources;
using AYellowpaper;
using UnityEngine;

namespace AI_Perception.Senses
{

	public class FactionNotifier : Sense, IPerceptionReceiver
	{
		public static Dictionary<Faction, List<FactionNotifier>> notifiers = new();

		[Header("Faction Settings")] [SerializeField]
		private Faction _faction;

		public bool canNotifyToOthers;
		public bool canNotifyFromFactionNotification;

		public float notifySeconds;
		[Min(0)] public float maxDistance;
		
		[RequireInterface(typeof(IStimulusSource))]
		public MonoBehaviour overrideStimulusSource;

		public Sense[] attachedSenses;

		private bool _isEnabled = true;

		private void Awake()
		{
			SwitchFaction(_faction);
		}

		public void SwitchFaction(Faction faction)
		{
			_faction = faction;
			notifiers.TryAdd(_faction, new List<FactionNotifier>());
			notifiers[_faction].Add(this);
		}
		
		public virtual void OnSenseTriggered(IStimulusSource stimulusSource, Sense sense)
		{
			if (!IsStimulusSourceValid(stimulusSource))
				return;
			
			Trigger(stimulusSource);
			Notify(sense);
		}

		private bool IsStimulusSourceValid(IStimulusSource stimulusSource)
		{
			if (stimulusSource != null)
			{
				foreach (var attachedSense in attachedSenses)
				{
					if (attachedSense.GetType() == stimulusSource.StimulatedSenseType)
						return true;
				}
				return false;
			}

			return true;
		}

		private void Notify(Sense sense)
		{
			if (!_isEnabled)
				return;

			StartCoroutine(DisableCoroutine());
			if (canNotifyToOthers)
			{
				if (canNotifyFromFactionNotification || sense.Type != typeof(FactionNotifier))
				{
					StartCoroutine(NotifyCoroutine(overrideStimulusSource != null ? ((IStimulusSource) overrideStimulusSource) : null));
				}
			}
		}

		private IEnumerator NotifyCoroutine(IStimulusSource stimulusSource)
		{
			yield return new WaitForSeconds(notifySeconds);
			var myPosition = transform.position;
			foreach (var notifier in notifiers[_faction])
			{
				if (notifier != this)
				{
					if (maxDistance == 0 || Vector3.Distance(myPosition, notifier.transform.position) < maxDistance)
						notifier.OnSenseTriggered(stimulusSource, this);
				}
			}
		}

		private IEnumerator DisableCoroutine()
		{
			_isEnabled = false;
			yield return new WaitForSeconds(.1f);
			_isEnabled = true;
		}

		protected override List<StimulusType> GetDetectableStimuli()
		{
			return null;
		}
	}

}