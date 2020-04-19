using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField]
	private Transform target = null;

	[SerializeField]
	private float smoothTime = 0;

	[SerializeField]
	private float maxSpeed = 0;

	private Vector2 velocity = default;

	void FixedUpdate()
	{
		Vector2 pos = Vector2.SmoothDamp(transform.position, target.position, ref velocity, smoothTime, maxSpeed, Time.fixedDeltaTime);
		transform.position = new Vector3(pos.x, pos.y, -10);
	}
}
