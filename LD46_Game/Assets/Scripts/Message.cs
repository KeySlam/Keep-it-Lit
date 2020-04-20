using TMPro;
using UnityEngine;

public class Message : MonoBehaviour
{
	[SerializeField]
	private TMP_Text title = null;

	[SerializeField]
	private TMP_Text content = null;

	[SerializeField]
	private CanvasGroup canvasGroup = null;

	[SerializeField]
	private RectTransform rectTransform = null;

	public void Setup(string title, string content)
	{
		this.title.text = title;
		this.content.text = content;
	}

	public void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	public void Update()
	{
		if (rectTransform.anchoredPosition.y <= -500)
		{
			Destroy(this.gameObject);
		}
	}

	public void Hide()
	{
		canvasGroup.alpha = 0;
	}

	public void Show()
	{
		canvasGroup.alpha = 1;
	}
}
