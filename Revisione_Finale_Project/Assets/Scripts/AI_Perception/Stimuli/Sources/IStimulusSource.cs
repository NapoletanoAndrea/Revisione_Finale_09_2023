﻿using System;
using AI_Perception.Senses;
using UnityEngine;

namespace AI_Perception.Stimuli.Sources
{

	public interface IStimulusSource
	{
		public GameObject GameObject { get; }
		public float Intensity { get; }
		public StimulusType StimulusType { get; }

		public Type StimulatedSenseType { get; }
	}
	public interface IStimulusSource<T> : IStimulusSource where T : Sense
	{
		Type IStimulusSource.StimulatedSenseType => typeof(T);
	}

}