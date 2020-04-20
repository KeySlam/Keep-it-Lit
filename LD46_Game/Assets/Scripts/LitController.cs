using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LitController : MonoBehaviour
{
	[SerializeField]
	private GuestController guestPrefab = null;

	List<GuestController> allGuests = new List<GuestController>();

	[SerializeField]
	private Transform spawnPoint = null;

	private float litness = 0;
	public float Litness {
		get { return litness; }
		set
		{
			litness = Mathf.Clamp(value, 0, 1);
		}
	}
	public float litnessIncrease = 0;

	private int peopleIn = 0;

	private bool isLeaving = false;
	private bool isEntering = false;

	public void Update()
	{
		Litness += litnessIncrease * Time.deltaTime;
	}

	public void LateUpdate()
	{
		int targetPeople = (int)(Litness * 45) + 5;

		if (targetPeople > peopleIn)
		{
			if (isEntering == false)
			{
				peopleIn++;
				StartCoroutine(GuestEnter());
			}
		}
		else if (targetPeople < peopleIn)
		{
			if (isLeaving == false)
			{
				peopleIn--;
				StartCoroutine(GuestLeave());
			}
		}
	}

	public IEnumerator GuestLeave()
	{
		isLeaving = true;

		int index = Random.Range(0, allGuests.Count);
		GuestController guest = allGuests[index];
		allGuests.RemoveAt(index);

		Coroutine co = StartCoroutine(guest.Leave());

		yield return co;

		isLeaving = false;
	}

	public IEnumerator GuestEnter()
	{
		isEntering = true;

		GuestController guest = Instantiate(guestPrefab, spawnPoint.position, Quaternion.identity);
		yield return StartCoroutine(guest.Spawn());

		yield return new WaitForSeconds(0.5f);

		allGuests.Add(guest);

		isEntering = false;
	}
}
