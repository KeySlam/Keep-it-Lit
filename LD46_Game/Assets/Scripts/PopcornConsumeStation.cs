using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopcornConsumeStation : Interactable
{
	[SerializeField]
	private InteractableIcon bowlPutIcon = null;

	[SerializeField]
	private InteractableIcon bowlTakeIcon = null;

	[SerializeField]
	private Animator animator = null;

	private int bowlCount = 3;
	public int BowlCount
	{
		get { return bowlCount; }
		set
		{
			bowlCount = value;

			UpdateInteractions();
			UpdateView();
		}
	}

	private void UpdateInteractions()
	{
		if (bowlCount == 3)
		{
			interactionTakeBowl.active = false;
			interactionPutBowl.active = false;
		}
		else
		{
			interactionTakeBowl.active = true;
			interactionPutBowl.active = true;
		}
	}

	private void UpdateView()
	{
		if (bowlCount == 0)
			animator.Play("Table_Bowls0");
		else if (bowlCount == 1)
			animator.Play("Table_Bowls1");
		else if (bowlCount == 2)
			animator.Play("Table_Bowls2");
		else if (bowlCount == 3)
			animator.Play("Table_Bowls3");
	}

	public class InteractionPutBowl : Interaction
	{
		PopcornConsumeStation popcornConsumeStation = null;

		public InteractionPutBowl(PopcornConsumeStation popcornConsumeStation, InteractableIcon icon, bool active = true) : base(icon, active)
		{
			this.popcornConsumeStation = popcornConsumeStation;
		}

		public override void Execute(PlayerController playerController)
		{
			popcornConsumeStation.StartCoroutine(ExecuteRoutine(playerController));
		}

		public IEnumerator ExecuteRoutine(PlayerController playerController)
		{
			if (!((CarryableBowl)playerController.carrying).empty)
				popcornConsumeStation.BowlCount++;
			
			playerController.carrying = null;

			popcornConsumeStation.animator.transform.localScale = new Vector3(1, 1, 1);
			Vector3 start = new Vector3(0, 0, 0);

			float progress = 0;
			float time = 0;

			while (progress < 1)
			{
				time += Time.deltaTime;
				progress = time / 0.4f;

				float s = popcornConsumeStation.Boing(progress) / 6;

				popcornConsumeStation.animator.transform.localScale = new Vector3(1, 1 - s, 1);
				//mop.transform.localPosition = start + new Vector3(0, s, 0);

				yield return null;
			}

			popcornConsumeStation.animator.transform.localScale = new Vector3(1, 1, 1);
			popcornConsumeStation.animator.transform.localPosition = start;
		}

		public override bool IsEligible(PlayerController playerController)
		{
			return (playerController.carrying != null && playerController.carrying.GetType() == typeof(CarryableBowl));
		}
	}

	public class InteractionTakeBowl : Interaction
	{
		PopcornConsumeStation popcornConsumeStation = null;

		public InteractionTakeBowl(PopcornConsumeStation popcornConsumeStation, InteractableIcon icon, bool active = true) : base(icon, active)
		{
			this.popcornConsumeStation = popcornConsumeStation;
		}

		public override void Execute(PlayerController playerController)
		{
			this.popcornConsumeStation.StartCoroutine(ExecuteRoutine(playerController));
		}

		public IEnumerator ExecuteRoutine(PlayerController playerController)
		{
			playerController.carrying = new CarryableBowl();

			popcornConsumeStation.transform.localScale = new Vector3(1, 1, 1);
			Vector3 start = popcornConsumeStation.animator.transform.localPosition;

			float progress = 0;
			float time = 0;

			while (progress < 1)
			{
				time += Time.deltaTime;

				progress = time / 0.4f;

				float s = popcornConsumeStation.Boing(progress) / 6;

				//mop.transform.localScale = new Vector3(1, 1 - s, 1);
				popcornConsumeStation.animator.transform.localPosition = start + new Vector3(0, s, 0);

				yield return null;
			}

			popcornConsumeStation.animator.transform.localScale = new Vector3(1, 1, 1);
			popcornConsumeStation.animator.transform.localPosition = start;
		}

		public override bool IsEligible(PlayerController playerController)
		{
			return (playerController.carrying == null);
		}
	}

	private InteractionPutBowl interactionPutBowl = null;
	private InteractionTakeBowl interactionTakeBowl = null;

	public void Awake()
	{
		interactionPutBowl = new InteractionPutBowl(this, bowlPutIcon, true);
		interactionTakeBowl = new InteractionTakeBowl(this, bowlTakeIcon, true);

		BowlCount = bowlCount;

		interactions.Add(interactionPutBowl);
		interactions.Add(interactionTakeBowl);

		StartCoroutine(DepleteRoutine());
	}
	
	public IEnumerator DepleteRoutine()
	{
		while (true)
		{
			if (BowlCount > 0)
			{
				yield return new WaitForSeconds(Random.Range(5, 10));
				BowlCount--;
			}

			yield return null;
		}
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
