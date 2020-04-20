using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
	[SerializeField]
	private AudioSource crowd = null;

	[SerializeField]
	private AudioSource musicSource = null;

	public AudioClip[] tracks = null;

	void Awake()
	{
		crowd.Play();
	}

	public void UpdateCrowd(float weight)
	{
		crowd.volume = Mathf.Min(0.25f, weight / 10.0f);
	}

	public AudioSource PlayTrack()
	{
		AudioClip clip = tracks[Random.Range(0, tracks.Length)];

		musicSource.clip = clip;
		musicSource.Play();

		return musicSource;
	}
}
