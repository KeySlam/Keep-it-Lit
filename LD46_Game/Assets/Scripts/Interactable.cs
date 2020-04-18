using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
	public class Interaction
	{
		public bool active;
		public InteractableIcon icon = null;
		public Action<Interaction> execute;
		
		public Interaction(bool active, InteractableIcon icon, Action<Interaction> execute)
		{
			this.active = active;
			this.icon = icon;
			this.execute = execute;
		}

		public void ShowIcon()
		{
			icon.Show();
		}

		public void HideIcon()
		{
			icon.Hide();
		}
	}

	public List<Interaction> interactions = new List<Interaction>();
}
