using System;
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

	private void FixedUpdate()
	{
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
}
