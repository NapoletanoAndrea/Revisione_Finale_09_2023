using System;
using UnityEngine;

namespace AI_Perception.Stimuli.Sources
{

	public class GenericStimulusSource : IStimulusSource
	{

		public GameObject GameObject => null;
		public Vector3 Center => default;
		public float Intensity => Mathf.Infinity;
		public StimulusType StimulusType => null;
		public Type StimulatedSenseType => null;
	}

}