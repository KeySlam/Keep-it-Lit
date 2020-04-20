using System.Collections;
using UnityEngine;

public class CDRack : Interactable
{
	[SerializeField]
	private InteractableIcon cdTakeIcon = null;

	[SerializeField]
	private InteractableIcon cdReturnIcon = null;

	[SerializeField]
	private SpriteRenderer sprite = null;

	public class InteractionTakeCD : Interaction
	{
		CDRack cdRack = null;

		public InteractionTakeCD(CDRack cdRack, InteractableIcon icon, bool active = true) : base(icon, active)
		{
			this.cdRack = cdRack;
		}

		public override void Execute(PlayerController playerController)
		{
			this.cdRack.StartCoroutine(ExecuteRoutine(playerController));
		}

		public IEnumerator ExecuteRoutine(PlayerController playerController)
		{
			playerController.carrying = new CarryableCD();

			cdRack.transform.localScale = new Vector3(1, 1, 1);
			Vector3 start = cdRack.sprite.transform.localPosition;

			float progress = 0;
			float time = 0;

			while (progress < 1)
			{
				time += Time.deltaTime;

				progress = time / 0.4f;

				float s = cdRack.Boing(progress) / 6;

				//mop.transform.localScale = new Vector3(1, 1 - s, 1);
				cdRack.sprite.transform.localPosition = start + new Vector3(0, s, 0);

				yield return null;
			}

			cdRack.sprite.transform.localScale = new Vector3(1, 1, 1);
			cdRack.sprite.transform.localPosition = start;
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

	public class InteractionReturnCD : Interaction
	{
		CDRack cdRack = null;

		public InteractionReturnCD(CDRack cdRack, InteractableIcon icon, bool active = true) : base(icon, active)
		{
			this.cdRack = cdRack;
		}

		public override void Execute(PlayerController playerController)
		{
			cdRack.StartCoroutine(ExecuteRoutine(playerController));
		}

		public IEnumerator ExecuteRoutine(PlayerController playerController)
		{
			playerController.carrying = null;

			cdRack.transform.localScale = new Vector3(1, 1, 1);
			Vector3 start = cdRack.sprite.transform.localPosition;

			float progress = 0;
			float time = 0;

			while (progress < 1)
			{
				time += Time.deltaTime;
				progress = time / 0.4f;

				float s = cdRack.Boing(progress) / 6;

				cdRack.sprite.transform.localScale = new Vector3(1, 1 - s, 1);
				//mop.transform.localPosition = start + new Vector3(0, s, 0);

				yield return null;
			}

			cdRack.sprite.transform.localScale = new Vector3(1, 1, 1);
			cdRack.sprite.transform.localPosition = start;
		}

		public override void Check(PlayerController playerController)
		{
			this.active = IsEligible(playerController);
		}

		public override bool IsEligible(PlayerController playerController)
		{
			return (playerController.carrying != null && playerController.carrying.GetType() == typeof(CarryableCD));
		}
	}

	private InteractionTakeCD interactionTakeCD = null;
	private InteractionReturnCD interactionReturnCD = null;

	public void Awake()
	{
		interactionTakeCD = new InteractionTakeCD(this, cdTakeIcon);
		interactionReturnCD = new InteractionReturnCD(this, cdReturnIcon);

		interactions.Add(interactionTakeCD);
		interactions.Add(interactionReturnCD);
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
