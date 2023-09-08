namespace AI_Perception.Senses
{

	public interface ISenseNotifier
	{
		public void OnSenseTriggered(float intensity, Sense sense);
	}

}