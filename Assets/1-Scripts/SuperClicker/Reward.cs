using UnityEngine;
using System;

#region Enum
public enum RewardType
{
	Plus,
	Multi,
	Agent
}
#endregion
[Serializable]
public class Reward 
{    

	#region Properties
	[field: SerializeField] public RewardType RewardType { get; set; }
	[field: SerializeField] public float Value { get; set; }
	public SlotButtonUI ObjectReward { get; set; }
	#endregion


	#region Public Methods
	#endregion

	#region Private Methods
	#endregion
}
