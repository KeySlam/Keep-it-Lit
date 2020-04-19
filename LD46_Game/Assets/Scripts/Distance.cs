using UnityEngine;

public class Distance : MonoBehaviour
{
	[SerializeField]
	private float min = 0;

	[SerializeField]
	private float max = 0;

	[SerializeField]
	private float speed = 0;

	void FixedUpdate()
	{
		float v = Mathf.Sin(Time.time * speed) * max - min;

		transform.localPosition = transform.up * v;
	}
}
