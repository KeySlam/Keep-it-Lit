using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassConsumeStation : Interactable
{
	[SerializeField]
	private InteractableIcon glassPutIcon = null;

	[SerializeField]
	private Animator animator = null;

	private int bottleCount = 3;
	public int BottleCount
	{
		get { return bottleCount; }
		set
		{
			bottleCount = value;

			UpdateInteractions();
			UpdateView();
		}
	}

	private void UpdateInteractions()
	{
		if (bottleCount < 3)
		{
			interactionPutGlass.active = true;
		}
		else
		{
			interactionPutGlass.active = false;
		}
	}

	private void UpdateView()
	{
		if (bottleCount == 0)
			animator.Play("Table_Bottles0");
		else if (bottleCount == 1)
			animator.Play("Table_Bottles1");
		else if (bottleCount == 2)
			animator.Play("Table_Bottles2");
		else if (bottleCount == 3)
			animator.Play("Table_Bottles3");
	}

	public class InteractionPutGlass : Interaction
	{
		GlassConsumeStation glassConsumeStation = null;

		public InteractionPutGlass(GlassConsumeStation glassConsumeStation, InteractableIcon icon, bool active = true) : base(icon, active)
		{
			this.glassConsumeStation = glassConsumeStation;
		}

		public override void Execute(PlayerController playerController)
		{
			Debug.Log("Add: " + glassConsumeStation.BottleCount);
			glassConsumeStation.BottleCount++;
			playerController.carrying = null;
		}

		public override bool IsEligible(PlayerController playerController)
		{
			return (playerController.carrying != null && playerController.carrying.GetType() == typeof(CarryableGlass));
		}
	}

	private InteractionPutGlass interactionPutGlass = null;

	public void Awake()
	{
		interactionPutGlass = new InteractionPutGlass(this, glassPutIcon, true);

		BottleCount = bottleCount;

		interactions.Add(interactionPutGlass);
		StartCoroutine(DepleteRoutine());
	}

	public IEnumerator DepleteRoutine()
	{
		while (true)
		{
			if (BottleCount > 0)
			{
				yield return new WaitForSeconds(Random.Range(5, 10));
				BottleCount--;
			}

			yield return null;
		}
	}
}
