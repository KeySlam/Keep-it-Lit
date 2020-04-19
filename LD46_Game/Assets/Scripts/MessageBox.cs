using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
	[SerializeField]
	private Message messagePrefab = null;

	[SerializeField]
	private float messagePadding = 5;

	private RectTransform rectTransform = null;

	private Coroutine coroutine = null;
	private Coroutine visCoroutine = null;


	[SerializeField]
	private RectTransform parent = null;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	private string[] messages = new string[]
	{
		"Look at this!",
		"Its working!!",
		"And it even features\nmultiple lines!",
		"Wow",
		"...",
		"cool",
	};
	private int index = 0;

	private void Show()
	{
		if (visCoroutine != null)
		{
			StopCoroutine(visCoroutine);
			visCoroutine = null;
		}

		visCoroutine = StartCoroutine(ShowRoutine());
	}

	private IEnumerator ShowRoutine()
	{
		float progress = 0;
		float time = 0;

		while (progress < 1)
		{
			time += Time.deltaTime;
			progress = time / 1f;

			float s = ElasticOut(progress);

			parent.anchoredPosition = new Vector3(0, s, 0);

			yield return null;
		}

		parent.anchoredPosition = new Vector3(0, 0, 0);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			NewMessage("@DebugPerson", messages[index++]);
		}
	}

	private void NewMessage(string title, string content)
	{
		if (coroutine != null)
		{
			StopCoroutine(coroutine);
			coroutine = null;
		}

		coroutine = StartCoroutine(NewMessageRoutine(title, content));
	}

	private IEnumerator NewMessageRoutine(string title, string content)
	{
		Message message = Instantiate(messagePrefab, transform);
		message.transform.SetAsFirstSibling();
		message.Setup(title, content);
		message.Hide(); 

		yield return null;
		yield return null;
		yield return null;

		float height = LayoutUtility.GetPreferredHeight(message.GetComponent<RectTransform>()) + messagePadding;

		rectTransform.anchoredPosition = new Vector3(0, height, 0);

		message.Show();

		float progress = 0;
		float time = 0;

		while (progress < 1)
		{
			time += Time.deltaTime;
			progress = time / 1f;

			float s = ElasticOut(progress);

			rectTransform.anchoredPosition = new Vector3(0, height * (s - 1), 0);

			yield return null;
		}

		rectTransform.anchoredPosition = new Vector3(0, 0, 0);
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
