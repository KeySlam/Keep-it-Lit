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

	private Vector2 velocity = default;
	private Vector2 steering = default;

	private Vector2 seekTarget = default;

	private bool alive = false;

	private void Awake()
	{

	}

	public IEnumerator Spawn()
	{
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

		seekTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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

	private void Seek()
	{
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
