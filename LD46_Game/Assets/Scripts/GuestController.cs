using System;
using System.Collections;
using UnityEngine;

public class GuestController : MonoBehaviour
{
	[SerializeField]
	private float maxVelocity = 0;

	[SerializeField]
	private float maxSteeringForce = 0;

	[SerializeField]
	private float seekingForce = 0;

	[SerializeField]
	private float personalSpace = 0;

	[SerializeField]
	private float avoidanceForce = 0;

	[SerializeField]
	private new Collider2D collider = null;

	[SerializeField]
	private new Rigidbody2D rigidbody = null;

	private PlaceSelector placeSelector = null;

	private Vector2 velocity = default;
	private Vector2 steering = default;

	private Plac place = null;
	private Vector2 seekTarget = default;

	[SerializeField]
	private InteractableIcon confusion = null;

	[SerializeField]
	private InteractableIcon hungry = null;

	[SerializeField]
	private InteractableIcon thirsty = null;

	[SerializeField]
	private InteractableIcon tooLoud = null;

	[SerializeField]
	private InteractableIcon tooSoft = null;

	[SerializeField]
	private Animator animator = null;

	[SerializeField]
	private Transform spriteWrapper = null;

	private bool alive = false;

	private bool dancing = false;

	private Stereo stereo = null;

	bool facing = false;

	private void Awake()
	{
		placeSelector = FindObjectOfType<PlaceSelector>();
		stereo = FindObjectOfType<Stereo>();
	}

	private void UpdateAnimations()
	{
		if (facing && velocity.x > 0.03)
		{
			spriteWrapper.localScale = new Vector3(1, 1, 1);
			facing = false;
		}
		else if (!facing && velocity.x < -0.03)
		{
			spriteWrapper.localScale = new Vector3(-1, 1, 1);
			facing = true;
		}

		if (dancing)
			animator.Play("MaleCharacter_Dancing");
		else if (velocity.magnitude > 0.03)
			animator.Play("MaleCharacter_Walk");
		else
			animator.Play("MaleCharacter_Idle");
	}

	public bool emoting = false;

	public void Confusion()
	{
		StartCoroutine(ConfusionRoutine());
	}

	public void Hungry()
	{
		StartCoroutine(HungryRoutine());
	}

	public void Thirsty()
	{
		StartCoroutine(ThirstyRoutine());
	}

	public void TooLoud()
	{
		StartCoroutine(TooLoudRoutine());
	}

	public void TooSoft()
	{
		StartCoroutine(TooSoftRoutine());
	}

	private IEnumerator ConfusionRoutine()
	{
		confusion.Show();
		emoting = true;

		yield return new WaitForSeconds(2.5f);

		confusion.Hide();
		emoting = false;
	}

	private IEnumerator HungryRoutine()
	{
		hungry.Show();
		emoting = true;

		yield return new WaitForSeconds(2.5f);

		hungry.Hide();
		emoting = false;
	}

	private IEnumerator ThirstyRoutine()
	{
		thirsty.Show();
		emoting = true;

		yield return new WaitForSeconds(2.5f);

		thirsty.Hide();
		emoting = false;
	}

	private IEnumerator TooLoudRoutine()
	{
		tooLoud.Show();
		emoting = true;

		yield return new WaitForSeconds(4.0f);

		tooLoud.Hide();
		emoting = false;
	}

	private IEnumerator TooSoftRoutine()
	{
		tooSoft.Show();
		emoting = true;

		yield return new WaitForSeconds(4.0f);

		tooSoft.Hide();
		emoting = false;
	}

	private IEnumerator AI()
	{
		FindTarget();
		yield return new WaitForSeconds(UnityEngine.Random.Range(2, 5));

		while (true)
		{
			int choice = UnityEngine.Random.Range(0, 3);

			if (choice == 0)
			{
				// Idle
				yield return new WaitForSeconds(UnityEngine.Random.Range(4, 15));
			}
			else if (choice == 1)
			{
				if (stereo.playing)
				{
					dancing = true;

					float t = 0;
					float targetT = UnityEngine.Random.Range(6, 14);
					while (t < targetT)
					{
						if (stereo.playing == false)
						{
							break;
						}

						yield return null;
					}

					dancing = false;
				}
			}
			else if (choice == 2)
			{
				FindTarget();
				yield return new WaitForSeconds(UnityEngine.Random.Range(4, 7));
			}

			yield return null;
		}	
	}

	private void FindTarget()
	{
		place = placeSelector.GetPlace();

		seekTarget = (Vector2)place.transform.position + (Vector2)UnityEngine.Random.insideUnitSphere * place.radius;
	}

	public IEnumerator Spawn()
	{
		StartCoroutine(AI());

		transform.localScale = new Vector3(0, 0, 0);

		float progress = 0;
		float time = 0;

		while (progress < 1)
		{
			time += Time.deltaTime;
			progress = time / 1.0f;

			float s = ElasticOut(progress);

			transform.localScale = new Vector3(s, s, 1);

			yield return null;
		}

		transform.localScale = new Vector3(1, 1, 1);

		alive = true;
	}

	public IEnumerator Leave()
	{
		transform.localScale = new Vector3(1, 1, 1);

		float progress = 0;
		float time = 0;

		while (progress < 1)
		{
			time += Time.deltaTime;
			progress = time / 0.5f;

			float s = 1 - ExponentialOut(progress);

			transform.localScale = new Vector3(s, s, 1);

			yield return null;
		}

		transform.localScale = new Vector3(0, 0, 0);

		Destroy(this.gameObject);
	}

	private void FixedUpdate()
	{
		if (alive == false)
			return;

		steering = Vector2.zero;

		Seek();
		Avoid();

		Vector2.ClampMagnitude(steering, maxSteeringForce);
		steering *= Time.fixedDeltaTime;

		velocity += steering;
		Vector2.ClampMagnitude(velocity, maxVelocity);
		velocity *= Time.fixedDeltaTime;

		rigidbody.position += velocity;
	}

	private void Update()
	{
		UpdateAnimations();
	}
	
	private void Seek()
	{
		float dist = Vector2.Distance(seekTarget, transform.position);
		if (dist < 2)
		{
			return;
		}

		RaycastHit2D[] hits = Physics2D.CircleCastAll(seekTarget, 0.35f, Vector2.zero);
		if (hits.Length > 0)
			return;

		Vector2 desiredVelocity = (seekTarget - (Vector2)transform.position).normalized * maxVelocity;
		Vector2 steering = desiredVelocity - velocity;

		Vector2.ClampMagnitude(steering, seekingForce);

		this.steering += steering;
	}

	private void Avoid()
	{
		float dynamicLength = velocity.magnitude / maxVelocity;

		Vector2 ahead = (Vector2)transform.position + velocity.normalized * personalSpace;

		Debug.DrawLine(transform.position, ahead);

		collider.enabled = false;
		RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, personalSpace, Vector2.zero);
		collider.enabled = true;

		foreach (RaycastHit2D hit in hits)
		{
			Vector2 steering = hit.normal * avoidanceForce;

			this.steering += steering;
		}

		/*
		RaycastHit2D hit = Physics2D.Linecast(transform.position, ahead);
		collider.enabled = true;

		if (hit.collider != null)
		{
			Vector2 steering = hit.normal * avoidanceForce;

			this.steering += steering;
		}
		*/
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
