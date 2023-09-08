using UnityEngine;

namespace AI_Perception.Stimuli
{

	[CreateAssetMenu(fileName = "Stimulus Type", menuName = "Data/Stimulus Type")]
	public class StimulusType : ScriptableObject
	{
		[SerializeField] private string _tag;
		public string Tag => _tag;

		[SerializeField] private string _description;
	}

}