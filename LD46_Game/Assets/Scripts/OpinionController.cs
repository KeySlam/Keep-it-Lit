using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OpinionController : MonoBehaviour
{
	private PopcornConsumeStation[] popcornConsumeStations;
	private GlassConsumeStation[] glassConsumeStation;
	private Toilet[] toilets;
	private Stereo stereo;
	private Puke[] pukes;
	private MessageBox messageBox;


	private List<string> names = new List<string>()
	{
		"AizahFeeney		",
		"JaredParkes		",
		"AislingWatts		",
		"Sammy-JoDrummond	",
		"BernardDonnelly	",
		"Ella-GraceBush	",
		"ManavRitter		",
		"ReeseRobin		",
		"TomiVillegas		",
		"HaroonBonilla		",
		"MillaPerry		",
		"EliasThorpe		",
		"Sarah-JaneChandler",
		"ElizeHagan		",
		"SeanEllis			",
		"KingaMccarty		",
		"RalphyJenkins		",
		"KieranConroy		",
		"EmmaWerner		",
		"AlenaEaston		",
		"TrentMontgomery	",
		"AsmaaMathews		",
		"CalvinMccallum	",
		"JocelynBoone		",
		"SanaZavala		",
		"MirunaBrandt		",
		"AarushOsborn		",
		"ElliArnold		",
		"CarlDyer			",
		"LayanClemons		",
		"SullivanSouthern	",
		"MaximusSweet		",
		"SameeraAtkinson	",
		"TroyMellor		",
		"NikolaCrawford	",
		"NinaHaas			",
		"KirstinTownsend	",
		"KerryBell			",
		"AleeshaPratt		",
		"ShahidCarlson		",
		"AdemMoody			",
		"MeredithKendall	",
		"RitaBrowne		",
		"Lexi-MaeHill		",
		"DarrellMacias		",
		"LaviniaWeber		",
		"LindsayMill		",
		"WillaKumar		",
		"KristinHuff		",
		"Emily-JaneMooney	",
		"OrianaWhyte		",
		"RaheelHuang		",
		"EricBranch		",
		"MichalinaBenton	",
		"BrandonHebert		",
		"KylieVillarreal	",
		"ElinaPeterson		",
		"KashifHardy		",
		"AbdullahiNichols	",
		"HettyGeorge		",
		"VishalRodriguez	",
		"EesaYang			",
		"MamieCorrigan		",
		"ZishanMac			",
		"BradleeRedmond	",
		"InesJacobson		",
		"EnochGuerrero		",
		"KsaweryHowarth	",
		"KirkWood			",
		"KirstenArmstrong	",
		"AoifeQuintana		",
		"FabioMora			",
		"SannahBernard		",
		"MariyaCornish		",
		"MiahDavidson		",
		"SaeedButt			",
		"MasonFrey			",
		"AarizHolt			",
		"KiaraPaine		",
		"IanNavarro		",
		"MichaelWeston		",
		"RoscoeWoodcock	",
		"OisinGuthrie		",
		"KasonLittle		",
		"MargaretBean		",
		"IshanForeman		",
		"HuseyinMckee		",
		"KeiranCorbett		",
		"ChayMccartney		",
		"MalcolmAtkins		",
		"MaverickBains		",
		"LilyWoolley		",
		"ElissaRiley		",
		"RayanBenson		",
		"SofiaKnowles		",
		"SamiyaSalas		",
		"ShaniaAdams		",
		"ZunairaWilkins	",
		"KareemKaye		",
		"JacquelineNoel	",
	};


	bool wasPlaying = false;

	private Coroutine hungryRoutine = null;
	private Coroutine glassRoutine = null;
	private Coroutine volumeRoutine = null;
	private Coroutine floodRoutine = null;
	private Coroutine pukeRoutine = null;

	public int foodCount;
	public int glassCount;
	public int floodCount;
	public int pukeCount;

	public bool playing;

	private LitController litController;

	public int preferredVolume = 1;

	public List<string> quacksLit = null;
	public List<string> quacksNotLit = null;
	public List<string> quacksStandard = null;

	public void Start()
	{
		popcornConsumeStations = FindObjectsOfType<PopcornConsumeStation>();
		glassConsumeStation = FindObjectsOfType<GlassConsumeStation>();
		toilets = FindObjectsOfType<Toilet>();
		pukes = FindObjectsOfType<Puke>();
		stereo = FindObjectOfType<Stereo>();
		litController = FindObjectOfType<LitController>();
		messageBox = FindObjectOfType<MessageBox>();

		StartCoroutine(VolumeRoutine());

		StartCoroutine(TweetRoutine());
	}

	public void Update()
	{
		foodCount = popcornConsumeStations.Sum(item => item.BowlCount);
		glassCount = glassConsumeStation.Sum(item => item.BottleCount);
		floodCount = toilets.Count(item => item.Flooded);
		pukeCount = pukes.Count(item => item.Flooded);
		playing = stereo.playing;

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

		if (stereo.volume != preferredVolume)
		{
			if (volumeRoutine == null)
				volumeRoutine = StartCoroutine(VolumeTimeout());
		}

		if (floodCount > 0)
		{
			if (floodRoutine == null)
				floodRoutine = StartCoroutine(FloodTimeout());
		}

		if (pukeCount > 0)
		{
			if (pukeRoutine == null)
				pukeRoutine = StartCoroutine(PukeTimeout());
		}
		


		wasPlaying = playing;
	}

	private IEnumerator TweetRoutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(1, 2));

			if (litController.Litness > 0.3)
			{
				messageBox.NewMessage(RandomName(), quacksLit[Random.Range(0, quacksLit.Count)]);
			}
			else
			{
				messageBox.NewMessage(RandomName(), quacksNotLit[Random.Range(0, quacksLit.Count)]);
			}
		}
	}

	private string RandomName()
	{
		string name = "@" + names[Random.Range(0, names.Count)].Trim();
		return name;
	}

	private IEnumerator VolumeRoutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(40, 80));
			preferredVolume = Random.Range(0, 2);
		}
	}

	private IEnumerator HungryTimeout()
	{
		yield return new WaitForSeconds(0.5f);

		if (foodCount == 0)
		{
			StartCoroutine(Hungry());
		}

		yield return new WaitForSeconds(3.0f);

		while (foodCount == 0)
		{
			litController.Litness -= 0.5f * Time.deltaTime;

			yield return null;
		}

		hungryRoutine = null;
	}

	private IEnumerator ThirstyTimeout()
	{
		yield return new WaitForSeconds(0.5f);

		if (glassCount == 0)
		{
			StartCoroutine(Thirsty());
		}

		yield return new WaitForSeconds(3.0f);

		while (glassCount == 0)
		{
			litController.Litness -= 0.5f * Time.deltaTime;

			yield return null;
		}

		glassRoutine = null;
	}

	private IEnumerator VolumeTimeout()
	{
		yield return new WaitForSeconds(0.5f);

		if (stereo.volume > preferredVolume)
		{
			StartCoroutine(TooLoud());
		}
		else if (stereo.volume < preferredVolume)
		{
			StartCoroutine(TooSoft());
		}

		yield return new WaitForSeconds(10.0f);

		while (stereo.volume != preferredVolume)
		{
			litController.Litness -= 0.5f * Time.deltaTime;

			yield return null;
		}

		volumeRoutine = null;
	}

	private IEnumerator FloodTimeout()
	{
		yield return new WaitForSeconds(10.0f);

		while (floodCount > 0)
		{
			litController.Litness -= 0.5f * Time.deltaTime;

			yield return null;
		}

		floodRoutine = null;
	}

	private IEnumerator PukeTimeout()
	{
		yield return new WaitForSeconds(10.0f);

		while (floodCount > 0)
		{
			litController.Litness -= 0.5f * Time.deltaTime;

			yield return null;
		}

		pukeRoutine = null;
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

	private IEnumerator TooLoud()
	{
		List<GuestController> openGuests = new List<GuestController>(FindObjectsOfType<GuestController>());


		int count = Mathf.Max(2, Mathf.CeilToInt(openGuests.Count * 0.2f));

		while (count > 0)
		{
			int batch = Random.Range(1, Mathf.Min(count, 1));

			for (int i = 0; i < batch; i++)
			{
				int index = Random.Range(0, openGuests.Count);

				if (!openGuests[index].emoting)
					openGuests[index].TooLoud();

				openGuests.RemoveAt(index);
			}

			count -= batch;

			yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
		}
	}

	private IEnumerator TooSoft()
	{
		List<GuestController> openGuests = new List<GuestController>(FindObjectsOfType<GuestController>());


		int count = Mathf.Max(2, Mathf.CeilToInt(openGuests.Count * 0.2f));

		while (count > 0)
		{
			int batch = Random.Range(1, Mathf.Min(count, 1));

			for (int i = 0; i < batch; i++)
			{
				int index = Random.Range(0, openGuests.Count);

				if (!openGuests[index].emoting)
					openGuests[index].TooSoft();

				openGuests.RemoveAt(index);
			}

			count -= batch;

			yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
		}
	}
}
