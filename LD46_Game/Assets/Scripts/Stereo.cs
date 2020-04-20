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
	public int tracksLeft = 1;

	[HideInInspector]
	public int volume = 0;

	[HideInInspector]
	public float progress = 0;

	public bool playing = false;

	private void UpdateView()
	{

	}

	public class InteractionStereo : Interaction
	{
		Stereo stereo= null;

		public InteractionStereo(Stereo stereo, InteractableIcon icon, bool active = true) : base(icon, active)
		{
			this.stereo = stereo;
		}

		public override void Execute(PlayerController playerController)
		{
			if (playerController.carrying == null || playerController.carrying.GetType() != typeof(CarryableCD))
			{
				stereo.volume += 1;
				if (stereo.volume == 3)
					stereo.volume = 0;
			}
			else
			{
				this.stereo.StartCoroutine(ExecuteRoutine(playerController));
			}
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
		progress = 0;
		tracksLeft = 0;
		volume = 1;

		audioSource = audioController.PlayTrack();

		interaction = new InteractionStereo(this, stereoUI, true);

		interactions.Add(interaction);
	}

	public void Update()
	{
		progress = audioSource.time / audioSource.clip.length;

		playing = audioSource.isPlaying;

		if (volume == 0)
			audioSource.volume = 0.3f;
		else if (volume == 1)
			audioSource.volume = 0.5f;
		else if (volume == 2)
			audioSource.volume = 0.7f;

		if (!audioSource.isPlaying)
		{
			if (tracksLeft != 0)
			{
				audioSource = audioController.PlayTrack();
				playing = true;
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
