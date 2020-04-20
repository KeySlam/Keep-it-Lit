using System.Collections;
using UnityEngine;

public partial class Stereo : Interactable
{
	[SerializeField]
	private InteractableIcon stereoUI = null;

	[SerializeField]
	private Animator animator = null;

	public AudioController audioController = null;

	private AudioSource audioSource = null;

	[HideInInspector]
	public int tracksLeft = 0;

	[HideInInspector]
	public int volume = 0;

	[HideInInspector]
	public float progress = 2;


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
			if (playerController.carrying == null)
				return;

			if (playerController.carrying.GetType() != typeof(CarryableCD))
				return;

			if (stereo.tracksLeft == 1)
				return;

			this.stereo.StartCoroutine(ExecuteRoutine(playerController));
		}

		public IEnumerator ExecuteRoutine(PlayerController playerController)
		{
			playerController.carrying = null;
			stereo.tracksLeft++;

			stereo.animator.transform.localScale = new Vector3(1, 1, 1);
			Vector3 start = stereo.animator.transform.localPosition;

			float progress = 0;
			float time = 0;

			while (progress < 1)
			{
				time += Time.deltaTime;

				progress = time / 0.4f;

				float s = stereo.Boing(progress) / 6;

				//mop.transform.localScale = new Vector3(1, 1 - s, 1);
				stereo.animator.transform.localPosition = start + new Vector3(0, s, 0);

				yield return null;
			}

			stereo.animator.transform.localScale = new Vector3(1, 1, 1);
			stereo.animator.transform.localPosition = start;
		}

		public override bool IsEligible(PlayerController playerController)
		{
			return true;
		}
	}

	private InteractionStereo interaction = null;

	public void Awake()
	{
		progress = 1;
		tracksLeft = 0;
		volume = 1;

		interaction = new InteractionStereo(this, stereoUI, stereoUI, true);

		interactions.Add(interaction);
	}

	public void Update()
	{
		if (audioSource != null)
		{
			progress = audioSource.time / audioSource.clip.length;
		}

		if (progress >= 1)
		{
			if (tracksLeft != 0)
			{
				audioSource = audioController.PlayTrack();
				tracksLeft--;
				progress = 0;
			}
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
