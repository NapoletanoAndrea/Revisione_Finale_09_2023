using AI_Perception.Senses;
using UnityEngine;

namespace AI_Perception.Stimuli.Sources
{

	public class NoiseSource : MonoBehaviour, IStimulusSource<Hearing>
	{
		public float intensity;
		public StimulusType stimulusType;

		public GameObject GameObject => gameObject;
		public float Intensity => intensity;
		public StimulusType StimulusType => stimulusType;
	}

}