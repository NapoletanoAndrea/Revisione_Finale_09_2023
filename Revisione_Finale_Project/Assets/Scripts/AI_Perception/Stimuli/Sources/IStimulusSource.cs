using AI_Perception.Senses;

namespace AI_Perception.Stimuli.Sources
{

	public interface IStimulusSource<T> where T : Sense
	{
		public float Intensity { get; }
		public StimulusType StimulusType { get; }
	}

}