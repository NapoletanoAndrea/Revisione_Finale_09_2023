using AI_Perception.Senses;
using UnityEngine;

namespace AI_Perception.Stimuli.Sources
{

	public class VisibleBody : MonoBehaviour, IStimulusSource<Sight>
	{
		public Transform center;
		public Vector3 centerOffset;
		public float intensity;
		public StimulusType stimulusType;

		public GameObject GameObject => gameObject;
		public Vector3 Center => center.position + centerOffset;
		public float Intensity => intensity;
		public StimulusType StimulusType => stimulusType;

		private void OnValidate()
		{
			if (center == null)
				center = transform;
		}
	}

}