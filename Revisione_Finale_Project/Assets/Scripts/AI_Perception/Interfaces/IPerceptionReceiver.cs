using AI_Perception.Senses;
using AI_Perception.Stimuli;
using AI_Perception.Stimuli.Sources;

namespace AI_Perception.Interfaces
{

	public interface IPerceptionReceiver
	{
		public void OnSenseTriggered(IStimulusSource stimulusSource, Sense sense);
	}

}