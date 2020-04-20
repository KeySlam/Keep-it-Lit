using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopcornMachine : Interactable
{
	[SerializeField]
	private InteractableIcon popcornFillIcon = null;

	[SerializeField]
	private Animator animator = null;

	public class InteractionFillPopcorn : Interaction
	{
		PopcornMachine popcornMachine = null;

		public InteractionFillPopcorn(PopcornMachine popcornMachine, InteractableIcon icon, bool active = true) : base(icon, active)
		{
			this.popcornMachine = popcornMachine;
		}

		public override void Execute(PlayerController playerController)
		{
			this.popcornMachine.StartCoroutine(ExecuteRoutine(playerController));
		}

		public IEnumerator ExecuteRoutine(PlayerController playerController)
		{
			((CarryableBowl)playerController.carrying).empty = false;

			popcornMachine.transform.localScale = new Vector3(1, 1, 1);
			Vector3 start = new Vector3(0, 0, 0);

			float progress = 0;
			float time = 0;

			while (progress < 1)
			{
				time += Time.deltaTime;

				progress = time / 0.4f;

				float s = popcornMachine.Boing(progress) / 6;

				//mop.transform.localScale = new Vector3(1, 1 - s, 1);
				popcornMachine.animator.transform.localPosition = start + new Vector3(0, s, 0);

				yield return null;
			}

			popcornMachine.animator.transform.localScale = new Vector3(1, 1, 1);
			popcornMachine.animator.transform.localPosition = start;
		}

		public override void Check(PlayerController playerController)
		{
			this.active = IsEligible(playerController);
		}

		public override bool IsEligible(PlayerController playerController)
		{
			return (playerController.carrying != null && playerController.carrying.GetType() == typeof(CarryableBowl) && ((CarryableBowl)playerController.carrying).empty);
		}
	}

	public class InteractionPopcornReturn : Interaction
	{
		PopcornMachine popcornMachine = null;

		public InteractionPopcornReturn(PopcornMachine popcornMachine, InteractableIcon icon, bool active = true) : base(icon, active)
		{
			this.popcornMachine = popcornMachine;
		}

		public override void Execute(PlayerController playerController)
		{
			popcornMachine.StartCoroutine(ExecuteRoutine(playerController));
		}

		public IEnumerator ExecuteRoutine(PlayerController playerController)
		{
			((CarryableBowl)playerController.carrying).empty = true;

			popcornMachine.transform.localScale = new Vector3(1, 1, 1);
			Vector3 start = new Vector3(0, 0, 0);

			float progress = 0;
			float time = 0;

			while (progress < 1)
			{
				time += Time.deltaTime;
				progress = time / 0.4f;

				float s = popcornMachine.Boing(progress) / 6;

				popcornMachine.animator.transform.localScale = new Vector3(1, 1 - s, 1);
				//mop.transform.localPosition = start + new Vector3(0, s, 0);

				yield return null;
			}

			popcornMachine.animator.transform.localScale = new Vector3(1, 1, 1);
			popcornMachine.animator.transform.localPosition = start;
		}

		public override void Check(PlayerController playerController)
		{
			this.active = IsEligible(playerController);
		}

		public override bool IsEligible(PlayerController playerController)
		{
			return (playerController.carrying != null && playerController.carrying.GetType() == typeof(CarryableBowl) && !((CarryableBowl)playerController.carrying).empty);
		}
	}

	private InteractionFillPopcorn interactionFillPopcorn = null;
	private InteractionPopcornReturn interactionPopcornReturn = null;

	public void Awake()
	{
		interactionFillPopcorn = new InteractionFillPopcorn(this, popcornFillIcon);
		interactionPopcornReturn = new InteractionPopcornReturn(this, popcornFillIcon);

		interactions.Add(interactionFillPopcorn);
		interactions.Add(interactionPopcornReturn);
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
