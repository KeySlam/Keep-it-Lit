using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toilet : Interactable
{
	[SerializeField]
	private InteractableIcon cleanupIcon = null;

	[SerializeField]
	private Animator animator = null;

	private bool flooded = false;
	public bool Flooded
	{
		get { return flooded; }
		set
		{
			flooded = value;

			UpdateAnimations();
			UpdateInteractions();
		}
	}

	public void UpdateAnimations()
	{
		if (flooded)
		{
			animator.Play("ToiletDoor_Flood");
		}
		else
		{
			animator.Play("ToiletDefault");
		}
	}

	public void UpdateInteractions()
	{
		interactionCleanup.active = flooded;
	}

	public class InteractionCleanup : Interaction
	{
		Toilet toilet = null;

		public InteractionCleanup(Toilet toilet, InteractableIcon icon, bool active = true) : base(icon, active)
		{
			this.toilet = toilet;
		}

		public override void Execute(PlayerController playerController)
		{
			this.toilet.StartCoroutine(ExecuteRoutine(playerController));
		}

		public IEnumerator ExecuteRoutine(PlayerController playerController)
		{
			toilet.Flooded = false;
			yield return null;
		}

		public override void Check(PlayerController playerController)
		{
			this.active = IsEligible(playerController);
		}

		public override bool IsEligible(PlayerController playerController)
		{
			return (playerController.carrying != null && playerController.carrying.GetType() == typeof(CarryableMop));
		}
	}

	private InteractionCleanup interactionCleanup = null;

	public void Awake()
	{
		interactionCleanup = new InteractionCleanup(this, cleanupIcon);

		Flooded = flooded;

		interactions.Add(interactionCleanup);
	}

	public float Boing(float k)
	{
		if (k < 0.5)
		{
			return QuadInOut(k * 2.0f);
		}
		else
		{
			return 1 - QuadInOut(k * 2.0f - 1);
		}
	}

	public float QuadInOut(float k)
	{
		if ((k *= 2.0f) < 1.0f)
		{
			return 0.5f * k * k;
		}

		return -0.5f * (--k * (k - 2.0f) - 1.0f);
	}
}
