using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
	[SerializeField]
	protected InteractableIcon notEligibleIcon = null;

	public abstract class Interaction
	{
		public InteractableIcon icon = null;
		public InteractableIcon iconNotEligible = null;
		public bool active = false;

		private InteractableIcon activeIcon = null;

		public Interaction(InteractableIcon icon, InteractableIcon iconNotEligible, bool active = true)
		{
			this.icon = icon;
			this.iconNotEligible = iconNotEligible;
			this.active = active;
		}

		public void ShowIcon(bool eligible)
		{
			if (eligible)
				activeIcon = icon;
			else
				activeIcon = iconNotEligible;

			activeIcon.Show();
		}

		public void HideIcon()
		{
			activeIcon.Hide();
			activeIcon = null;
		}

		public virtual void Check(PlayerController playerController) { }
		public abstract void Execute(PlayerController playerController);
		public abstract bool IsEligible(PlayerController playerController);
	}

	public List<Interaction> interactions = new List<Interaction>();
}
