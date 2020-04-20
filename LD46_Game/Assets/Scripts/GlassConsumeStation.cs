using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassConsumeStation : Interactable
{
	[SerializeField]
	private InteractableIcon glassPutIcon = null;

	[SerializeField]
	private Sprite glassFull = null;

	[SerializeField]
	private Sprite glassEmpty = null;

	[SerializeField]
	private bool isGlassFull = false;
	private bool IsGlassFull
	{
		get { return isGlassFull; }
		set
		{
			isGlassFull = value;

			interactionPutGlass.active = !isGlassFull;

			UpdateView();
		}
	}

	private void UpdateView()
	{
		if (IsGlassFull)
		{
			GetComponent<SpriteRenderer>().sprite = glassFull;
		}
		else
		{
			GetComponent<SpriteRenderer>().sprite = glassEmpty;
			StartCoroutine(Empty());
		}
	}

	public class InteractionPutGlass : Interaction
	{
		GlassConsumeStation glassConsumeStation = null;

		public InteractionPutGlass(GlassConsumeStation glassConsumeStation, InteractableIcon icon, InteractableIcon iconNotEligible, bool active = true) : base(icon, iconNotEligible, active)
		{
			this.glassConsumeStation = glassConsumeStation;
		}

		public override void Execute(PlayerController playerController)
		{
			glassConsumeStation.IsGlassFull = true;
			playerController.carrying = null;
		}

		public override bool IsEligible(PlayerController playerController)
		{
			return (playerController.carrying != null && playerController.carrying.GetType() == typeof(CarryableGlass));
		}
	}
;
	public IEnumerator Empty()
	{
		yield return new WaitForSeconds(3.0f);

		IsGlassFull = false;
	}

	private InteractionPutGlass interactionPutGlass = null;

	public void Awake()
	{
		interactionPutGlass = new InteractionPutGlass(this, glassPutIcon, notEligibleIcon, true);

		IsGlassFull = isGlassFull;

		interactions.Add(interactionPutGlass);
	}
}
