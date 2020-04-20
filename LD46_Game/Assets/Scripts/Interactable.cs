using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
	[SerializeField]
	public InteractableIcon notEligibleIcon = null;

	public abstract class Interaction
	{
		public InteractableIcon icon = null;
		public bool active = false;

		public Interaction(InteractableIcon icon, bool active = true)
		{
			this.icon = icon;
			this.active = active;
		}

		public virtual void Check(PlayerController playerController) { }
		public abstract void Execute(PlayerController playerController);
		public abstract bool IsEligible(PlayerController playerController);
	}

	public List<Interaction> interactions = new List<Interaction>();
}
