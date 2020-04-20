using System.Collections;
using UnityEngine;

public partial class Stereo : Interactable
{
	[SerializeField]
	private InteractableIcon stereoUI = null;

	[SerializeField]
	private Animator animator = null;



	private void UpdateView()
	{
		/*
		if (HasMop)
		{
			animator.Play("Mop_Leave");
		}
		else
		{
			animator.Play("Mop_Take");
		}
		*/
	}

	public class InteractionStereo : Interaction
	{
		Stereo stereo= null;

		public InteractionStereo(Stereo stereo, InteractableIcon icon, InteractableIcon iconNotEligible, bool active = true) : base(icon, iconNotEligible, active)
		{
			this.stereo = stereo;
		}

		public override void Execute(PlayerController playerController)
		{
			//this.mop.StartCoroutine(ExecuteRoutine(playerController));
		}

		public IEnumerator ExecuteRoutine(PlayerController playerController)
		{
			yield return null;
			/*
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
			*/
		}

		public override bool IsEligible(PlayerController playerController)
		{
			return true;
		}
	}

	private InteractionStereo interaction = null;

	public void Awake()
	{
		interaction = new InteractionStereo(this, stereoUI, stereoUI, true);

		//HasMop = hasMop;

		interactions.Add(interaction);
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
