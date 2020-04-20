using System.Collections;
using UnityEngine;

public class Mop : Interactable
{
	[SerializeField]
	private InteractableIcon mopTakeIcon = null;

	[SerializeField]
	private InteractableIcon mopReturnIcon = null;

	[SerializeField]
	private Animator animator = null;

	[SerializeField]
	private bool hasMop = true;
	private bool HasMop {
		get { return hasMop; }
		set
		{
			hasMop = value;

			interactionTakeMop.active = hasMop;
			interactionReturnMop.active = !hasMop;

			UpdateView();
		}
	}

	private void UpdateView()
	{
		if (HasMop)
		{
			animator.Play("Mop_Leave");
		}
		else
		{
			animator.Play("Mop_Take");
		}
	}

	public class InteractionTakeMop : Interaction
	{
		Mop mop = null;

		public InteractionTakeMop(Mop mop, InteractableIcon icon, InteractableIcon iconNotEligible, bool active = true) : base(icon, iconNotEligible, active)
		{
			this.mop = mop;
		}

		public override void Execute(PlayerController playerController)
		{
			this.mop.StartCoroutine(ExecuteRoutine(playerController));	
		}

		public IEnumerator ExecuteRoutine(PlayerController playerController)
		{
			mop.HasMop = false;
			playerController.carrying = new CarryablePlaceholder();

			mop.transform.localScale = new Vector3(1, 1, 1);
			Vector3 start = mop.animator.transform.localPosition;

			float progress = 0;
			float time = 0;

			while (progress < 1)
			{
				time += Time.deltaTime;

				if (playerController.carrying.GetType() != typeof(CarryableMop) && time >= 0.2)
				{
					playerController.carrying = new CarryableMop();
				}

				progress = time / 0.4f;

				float s = mop.Boing(progress) / 6;

				//mop.transform.localScale = new Vector3(1, 1 - s, 1);
				mop.animator.transform.localPosition = start + new Vector3(0, s, 0);

				yield return null;
			}

			mop.animator.transform.localScale = new Vector3(1, 1, 1);
			mop.animator.transform.localPosition = start;
		} 

		public override bool IsEligible(PlayerController playerController)
		{
			return (playerController.carrying == null);
		}
	}

	public class InteractionReturnMop : Interaction
	{
		Mop mop = null;

		public InteractionReturnMop(Mop mop, InteractableIcon icon, InteractableIcon iconNotEligible, bool active = true) : base(icon, iconNotEligible, active)
		{
			this.mop = mop;
		}

		public override void Execute(PlayerController playerController)
		{
			mop.StartCoroutine(ExecuteRoutine(playerController));
		}

		public IEnumerator ExecuteRoutine(PlayerController playerController)
		{
			mop.HasMop = true;
			playerController.carrying = null;

			mop.transform.localScale = new Vector3(1, 1, 1);
			Vector3 start = mop.animator.transform.localPosition;

			float progress = 0;
			float time = 0;

			while (progress < 1)
			{
				time += Time.deltaTime;
				progress = time / 0.4f;

				float s = mop.Boing(progress) / 6;

				mop.animator.transform.localScale = new Vector3(1, 1 - s, 1);
				//mop.transform.localPosition = start + new Vector3(0, s, 0);

				yield return null;
			}

			mop.animator.transform.localScale = new Vector3(1, 1, 1);
			mop.animator.transform.localPosition = start;
		}

		public override bool IsEligible(PlayerController playerController)
		{
			return (playerController.carrying != null && playerController.carrying.GetType() == typeof(CarryableMop));
		}
	}

	private InteractionTakeMop interactionTakeMop = null;
	private InteractionReturnMop interactionReturnMop = null;

	public void Awake()
	{
		interactionTakeMop = new InteractionTakeMop(this, mopTakeIcon, notEligibleIcon);
		interactionReturnMop = new InteractionReturnMop(this, mopReturnIcon, notEligibleIcon);

		HasMop = hasMop;

		interactions.Add(interactionTakeMop);
		interactions.Add(interactionReturnMop);
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
