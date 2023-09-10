using AI_Perception.Senses;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AI_Perception.Stimuli.Sources
{

	public class VisibleBody : MonoBehaviour, IStimulusSource<Sight>
	{
		public float baseIntensity;
		[ReadOnly] public float intensity;
		public StimulusType stimulusType;

		public GameObject GameObject => gameObject;
		public float Intensity => intensity;
		public StimulusType StimulusType => stimulusType;
	}

}