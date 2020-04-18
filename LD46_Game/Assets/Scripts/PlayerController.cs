using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private float movementSpeed = 0;

	[SerializeField]
	private float interactionRange = 0;

	[SerializeField]
	private new Collider2D collider = null;

	private HashSet<Interactable.Interaction> openInteractions = new HashSet<Interactable.Interaction>();

	void Update()
	{
		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");

		bool interacting = Input.GetKeyDown(KeyCode.E);

		Vector2 movementVector = new Vector2(horizontal, vertical).normalized;
		movementVector *= movementSpeed;

		transform.position += (Vector3)movementVector * Time.deltaTime;

		collider.enabled = false;
		RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, interactionRange, Vector2.zero);
		collider.enabled = true;


		HashSet<Interactable.Interaction> nearInteractions = new HashSet<Interactable.Interaction>();

		foreach (RaycastHit2D hit in hits.OrderBy(hit => Vector2.Distance(hit.transform.position, transform.position)))
		{
			bool found = false;

			Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();

			if (interactable == null)
				continue;


			foreach (Interactable.Interaction interaction in interactable.interactions)
			{
				if (!interaction.active)
					continue;

				if (!openInteractions.Contains(interaction))
				{
					interaction.ShowIcon();
					openInteractions.Add(interaction);
				}

				nearInteractions.Add(interaction);

				if (Input.GetKeyDown(KeyCode.E))
				{
					interaction.execute(interaction);
				}

				found = true;
				break;
			}

			if (found)
				break;
		}

		List<Interactable.Interaction> toRemove = new List<Interactable.Interaction>();
		foreach (Interactable.Interaction interaction in openInteractions)
		{
			if (!nearInteractions.Contains(interaction))
				toRemove.Add(interaction);
		}

		foreach (Interactable.Interaction interaction in toRemove)
		{
			interaction.HideIcon();
			openInteractions.Remove(interaction);
		}
	}
}
