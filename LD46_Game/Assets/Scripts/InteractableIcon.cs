using System.Collections;
using UnityEngine;

public class InteractableIcon : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer spriteRenderer = null;

	private float showDuration = 0.6f;
	private float hideDuration = 0.3f;

	private Coroutine current = null;

	private void Awake()
	{
		spriteRenderer.enabled = false;
	}

	public void Show()
	{
		if (current != null)
			StopCoroutine(current);

		current = StartCoroutine(ShowRoutine());
	}

	private IEnumerator ShowRoutine()
	{
		spriteRenderer.enabled = true;


		transform.localScale = new Vector3(0, 0, 1);

		float progress = 0;
		float time = 0;

		while (progress < 1)
		{
			time += Time.deltaTime;
			progress = time / showDuration;

			float s = ElasticOut(progress);

			transform.localScale = new Vector3(s, s, 1);

			yield return null;
		}

		transform.localScale = new Vector3(1, 1, 1);
	}

	public void Hide()
	{
		if (current != null)
			StopCoroutine(current);

		current = StartCoroutine(HideRoutine());
	}

	private IEnumerator HideRoutine()
	{
		transform.localScale = new Vector3(1, 1, 1);

		float progress = 0;
		float time = 0;

		while (progress < 1)
		{
			time += Time.deltaTime;
			progress = time / hideDuration;

			float s = 1 - ExponentialOut(progress);

			transform.localScale = new Vector3(s, s, 1);

			yield return null;
		}

		spriteRenderer.enabled = false;

		transform.localScale = new Vector3(0, 0, 0);
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
