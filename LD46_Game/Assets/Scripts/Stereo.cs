using UnityEngine;

public partial class Stereo : Interactable
{
	[SerializeField]
	private InteractableIcon testIcon = null;

	[SerializeField]
	private InteractableIcon testIcon2 = null;

	public class InteractionStereo : Interaction
	{
		public InteractionStereo(InteractableIcon icon, InteractableIcon iconNotEligible, bool active = true) : base(icon, iconNotEligible, active) { }

		public override void Execute(PlayerController playerController)
		{
			Debug.Log("Daberino");
			this.active = false;
		}

		public override bool IsEligible(PlayerController playerController)
		{
			return true;
		}
	}

	public void Awake()
	{
		interactions.Add(new InteractionStereo(testIcon, testIcon2));
	}
}
