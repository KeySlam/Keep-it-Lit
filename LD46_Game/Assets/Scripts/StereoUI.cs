using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StereoUI : InteractableIcon
{
	public Stereo stereo = null;

	public SpriteRenderer bgSprite = null;
	public SpriteRenderer volumeSprite = null;
	public SpriteRenderer progressBar = null;

	public Animator bg = null;
	public Animator volume = null;

	public void UpdateView()
	{
		if (stereo.volume == 0)
			volume.Play("LevelButtons_Level1");
		else if (stereo.volume == 1)
			volume.Play("LevelButtons_Level2");
		else
			volume.Play("LevelButtons_Level3");

		if (stereo.tracksLeft == 0)
		{
			bg.Play("Stereo UI_CD1");
		}
		else
		{
			bg.Play("Stereo UI_CD2");
		}

		progressBar.transform.localScale = new Vector3(stereo.progress * 30, 2, 1);
	}

	private void Awake()
	{
		bgSprite.enabled = false;
		volumeSprite.enabled = false;
		progressBar.enabled = false;
	}


	private void Update()
	{
		UpdateView();
	}

	public override void Show()
	{
		if (current != null)
			StopCoroutine(current);

		current = StartCoroutine(ShowRoutine());
	}

	public override IEnumerator ShowRoutine()
	{
		bgSprite.enabled = true;
		volumeSprite.enabled = true;
		progressBar.enabled = true;

		transform.localScale = new Vector3(0, 0, 1);

		float progress = 0;
		float time = 0;

		while (progress < 1)
		{
			time += Time.deltaTime;
			progress = time / showDuration;

			float s = ElasticOut(progress);

			transform.localScale = new Vector3(s, s, 1);

			yield return null;
		}

		transform.localScale = new Vector3(1, 1, 1);
	}

	public override void Hide()
	{
		if (current != null)
			StopCoroutine(current);

		current = StartCoroutine(HideRoutine());
	}

	public override IEnumerator HideRoutine()
	{
		transform.localScale = new Vector3(1, 1, 1);

		float progress = 0;
		float time = 0;

		while (progress < 1)
		{
			time += Time.deltaTime;
			progress = time / hideDuration;

			float s = 1 - ExponentialOut(progress);

			transform.localScale = new Vector3(s, s, 1);

			yield return null;
		}

		bgSprite.enabled = false;
		volumeSprite.enabled = false;

		transform.localScale = new Vector3(0, 0, 0);
	}

	private float ElasticOut(float k)
	{
		return Mathf.Pow(2.0f, -10.0f * k) * Mathf.Sin((k - 0.1f) * 5.0f * Mathf.PI) + 1.0f;
	}

	private float ExponentialOut(float k)
	{
		return k == 1 ? 1 : 1 - Mathf.Pow(2.0f, -10.0f * k);
	}
}
