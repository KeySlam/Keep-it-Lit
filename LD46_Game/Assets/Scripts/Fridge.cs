using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fridge : Interactable
{
	[SerializeField]
	private InteractableIcon bottleTakeIcon = null;

	[SerializeField]
	private InteractableIcon bottleReturnIcon = null;

	[SerializeField]
	private Animator animator = null;

	public class InteractionTakeBottle : Interaction
	{
		Fridge fridge = null;

		public InteractionTakeBottle(Fridge fridge, InteractableIcon icon, bool active = true) : base(icon, active)
		{
			this.fridge = fridge;
		}

		public override void Execute(PlayerController playerController)
		{
			this.fridge.StartCoroutine(ExecuteRoutine(playerController));
		}

		public IEnumerator ExecuteRoutine(PlayerController playerController)
		{
			playerController.carrying = new CarryableGlass();

			fridge.transform.localScale = new Vector3(1, 1, 1);
			Vector3 start = fridge.animator.transform.localPosition;

			float progress = 0;
			float time = 0;

			while (progress < 1)
			{
				time += Time.deltaTime;

				progress = time / 0.4f;

				float s = fridge.Boing(progress) / 6;

				//mop.transform.localScale = new Vector3(1, 1 - s, 1);
				fridge.animator.transform.localPosition = start + new Vector3(0, s, 0);

				yield return null;
			}

			fridge.animator.transform.localScale = new Vector3(1, 1, 1);
			fridge.animator.transform.localPosition = start;
		}

		public override void Check(PlayerController playerController)
		{
			this.active = IsEligible(playerController);
		}

		public override bool IsEligible(PlayerController playerController)
		{
			return (playerController.carrying == null);
		}
	}

	public class InteractionReturnBottle : Interaction
	{
		Fridge fridge = null;

		public InteractionReturnBottle(Fridge cdRack, InteractableIcon icon, bool active = true) : base(icon, active)
		{
			this.fridge = cdRack;
		}

		public override void Execute(PlayerController playerController)
		{
			fridge.StartCoroutine(ExecuteRoutine(playerController));
		}

		public IEnumerator ExecuteRoutine(PlayerController playerController)
		{
			playerController.carrying = null;

			fridge.transform.localScale = new Vector3(1, 1, 1);
			Vector3 start = fridge.animator.transform.localPosition;

			float progress = 0;
			float time = 0;

			while (progress < 1)
			{
				time += Time.deltaTime;
				progress = time / 0.4f;

				float s = fridge.Boing(progress) / 6;

				fridge.animator.transform.localScale = new Vector3(1, 1 - s, 1);
				//mop.transform.localPosition = start + new Vector3(0, s, 0);

				yield return null;
			}

			fridge.animator.transform.localScale = new Vector3(1, 1, 1);
			fridge.animator.transform.localPosition = start;
		}

		public override void Check(PlayerController playerController)
		{
			this.active = IsEligible(playerController);
		}

		public override bool IsEligible(PlayerController playerController)
		{
			return (playerController.carrying != null && playerController.carrying.GetType() == typeof(CarryableGlass));
		}
	}

	private InteractionTakeBottle interactionTakeBottle = null;
	private InteractionReturnBottle interactionReturnBottl = null;

	public void Awake()
	{
		interactionTakeBottle = new InteractionTakeBottle(this, bottleTakeIcon);
		interactionReturnBottl = new InteractionReturnBottle(this, bottleReturnIcon);

		interactions.Add(interactionTakeBottle);
		interactions.Add(interactionReturnBottl);
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
