using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OpinionController : MonoBehaviour
{
	private PopcornConsumeStation[] popcornConsumeStations;
	private GlassConsumeStation[] glassConsumeStation;
	private Toilet[] toilets;
	private Stereo[] stereos;



	bool wasPlaying = false;

	private Coroutine hungryRoutine = null;
	private Coroutine glassRoutine = null;

	public int foodCount;
	public int glassCount;
	public int floodCount;
	public bool playing;

	private LitController litController;

	public void Start()
	{
		popcornConsumeStations = FindObjectsOfType<PopcornConsumeStation>();
		glassConsumeStation = FindObjectsOfType<GlassConsumeStation>();
		toilets = FindObjectsOfType<Toilet>();
		stereos = FindObjectsOfType<Stereo>();
		litController = FindObjectOfType<LitController>();
	}

	public void Update()
	{
		foodCount = popcornConsumeStations.Sum(item => item.BowlCount);
		glassCount = glassConsumeStation.Sum(item => item.BottleCount);
		floodCount = toilets.Count(item => item.Flooded);
		playing = stereos.Any(item => item.playing);

		if (playing == false && wasPlaying == true)
		{			
			StartCoroutine(Confuse());
		}

		if (foodCount == 0)
		{
			if (hungryRoutine == null)
				hungryRoutine = StartCoroutine(HungryTimeout());
		}

		if (glassCount == 0)
		{
			if (glassRoutine == null)
				glassRoutine = StartCoroutine(ThirstyTimeout());
		}


		wasPlaying = playing;
	}

	private IEnumerator HungryTimeout()
	{
		yield return new WaitForSeconds(3.0f);

		if (foodCount == 0)
		{
			StartCoroutine(Hungry());
		}

		while (foodCount == 0)
		{
			litController.Litness -= 0.5f * Time.deltaTime;

			yield return null;
		}

		hungryRoutine = null;
	}

	private IEnumerator ThirstyTimeout()
	{
		yield return new WaitForSeconds(3.0f);

		if (glassCount == 0)
		{
			StartCoroutine(Thirsty());
		}

		while (glassCount == 0)
		{
			litController.Litness -= 0.5f * Time.deltaTime;

			yield return null;
		}

		glassRoutine = null;
	}


	private IEnumerator Confuse()
	{
		List<GuestController> openGuests = new List<GuestController>(FindObjectsOfType<GuestController>());

		Debug.Log(openGuests.Count);

		int count = Mathf.Min(2, Mathf.CeilToInt(openGuests.Count * 0.6f));

		while (count > 0)
		{
			int batch = Random.Range(1, Mathf.Min(count, 4));

			for (int i = 0; i < batch; i++)
			{
				int index = Random.Range(0, openGuests.Count);
				openGuests[index].Confusion();
				openGuests.RemoveAt(index);
			}

			count -= batch;

			yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
		}
	}

	private IEnumerator Hungry()
	{
		List<GuestController> openGuests = new List<GuestController>(FindObjectsOfType<GuestController>());


		int count = Mathf.Max(2, Mathf.CeilToInt(openGuests.Count * 0.3f));

		Debug.Log(count);

		while (count > 0)
		{
			int batch = Random.Range(1, Mathf.Min(count, 2));

			for (int i = 0; i < batch; i++)
			{
				int index = Random.Range(0, openGuests.Count);

				if (!openGuests[index].emoting)
					openGuests[index].Hungry();

				openGuests.RemoveAt(index);
			}

			count -= batch;

			yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
		}
	}

	private IEnumerator Thirsty()
	{
		List<GuestController> openGuests = new List<GuestController>(FindObjectsOfType<GuestController>());


		int count = Mathf.Max(2, Mathf.CeilToInt(openGuests.Count * 0.3f));

		while (count > 0)
		{
			int batch = Random.Range(1, Mathf.Min(count, 2));

			for (int i = 0; i < batch; i++)
			{
				int index = Random.Range(0, openGuests.Count);

				if (!openGuests[index].emoting)
					openGuests[index].Thirsty();

				openGuests.RemoveAt(index);
			}

			count -= batch;

			yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
		}
	}
}
