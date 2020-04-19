using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private AudioController audioController;

	[SerializeField]
	private float movementSpeed = 0;

	[SerializeField]
	private float interactionRange = 0;

	[SerializeField]
	private new Collider2D collider = null;

	[SerializeField]
	private Transform spriteWrapper = null;

	[SerializeField]
	private Animator animator = null;

	[SerializeField]
	private new Rigidbody2D rigidbody = null;

	public Carryable carrying = null;
	private Interactable.Interaction openInteraction = null;
	private Interactable.Interaction prefInteraction = null;

	private bool facing = false;

	private void UpdateFacing()
	{
		if (facing)
		{
			spriteWrapper.localScale = new Vector3(1, 1, 1);
		}
		else
		{
			spriteWrapper.localScale = new Vector3(-1, 1, 1);
		}
	}

	private void UpdateAnimation(bool facing)
	{
		if (facing)
		{
			if (carrying != null)
			{
				if (carrying.GetType() == typeof(CarryableMop))
				{
					animator.Play("Player_WalkingMop");
				}
			}
			else
			{
				animator.Play("Player_WalkingBackward");
			}
		}
		else
		{
			if (carrying != null)
			{
				if (carrying.GetType() == typeof(CarryableMop))
				{
					animator.Play("Player_WalkingMop");
				}
			}
			else
			{
				animator.Play("Player_Walking");
			}
		}
	}

	void FixedUpdate()
	{
		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");

		if (horizontal != 0)
		{
			facing = horizontal > 0;
			UpdateFacing();
		}

		UpdateAnimation(vertical > 0);

		bool interacting = Input.GetKeyDown(KeyCode.E);

		Vector2 movementVector = new Vector2(horizontal, vertical).normalized;
		movementVector *= movementSpeed;

		rigidbody.position += movementVector * Time.fixedDeltaTime;
	}

	private void Update()
	{
		collider.enabled = false;
		RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, interactionRange, Vector2.zero);
		collider.enabled = true;

		bool found = false;
		foreach (RaycastHit2D hit in hits.OrderBy(hit => Vector2.Distance(hit.transform.position, transform.position)))
		{
			Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();

			if (interactable == null)
				continue;

			foreach (Interactable.Interaction interaction in interactable.interactions)
			{
				if (!interaction.active)
					continue;

				found = true;

				bool eligible = interaction.IsEligible(this);

				if (openInteraction != interaction)
				{
					if (openInteraction != null)
						openInteraction.HideIcon();

					openInteraction = interaction;
					openInteraction.ShowIcon(eligible);
				}

				Debug.Log(eligible && Input.GetKeyDown(KeyCode.E));
				if (eligible && Input.GetKeyDown(KeyCode.E))
				{
					interaction.Execute(this);
				}

				break;
			}

			if (found) break;
		}

		if (!found && openInteraction != null)
		{
			openInteraction.HideIcon();
			openInteraction = null;
		}

		GuestController[] guestControllers = FindObjectsOfType<GuestController>();
		float weight = 0;
		foreach (GuestController guestController in guestControllers)
		{
			float dist = Vector2.Distance(guestController.transform.position, transform.position) / 10.0f;
			weight += dist;
		}

		audioController.UpdateCrowd(weight);
	}
}
