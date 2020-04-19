using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
	[SerializeField]
	private AudioSource crowd = null;

	void Awake()
	{
		crowd.Play();
	}

	public void UpdateCrowd(float weight)
	{
		crowd.volume = Mathf.Min(0.25f, weight / 10.0f);
	}
}
