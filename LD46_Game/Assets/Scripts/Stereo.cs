using UnityEngine;

public class Stereo : Interactable
{
	[SerializeField]
	private InteractableIcon testIcon = null;

	public void Awake()
	{
		interactions.Add(new Interaction(true, testIcon, delegate(Interaction interaction) {
			Debug.Log("daberino");
			interaction.active = false;
		}));
	}

}
