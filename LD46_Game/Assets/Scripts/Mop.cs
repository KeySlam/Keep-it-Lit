using UnityEngine;

public class Mop : Interactable
{
	[SerializeField]
	private InteractableIcon mopTakeIcon = null;

	[SerializeField]
	private InteractableIcon mopReturnIcon = null;

	[SerializeField]
	private Sprite mopDefault = null;

	[SerializeField]
	private Sprite mopEmpty = null;

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
			GetComponent<SpriteRenderer>().sprite = mopDefault;
		}
		else
		{
			GetComponent<SpriteRenderer>().sprite = mopEmpty;
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
			mop.HasMop = false;
			playerController.carrying = new CarryableGlass();
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
			mop.HasMop = true;
			playerController.carrying = null;
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
}
