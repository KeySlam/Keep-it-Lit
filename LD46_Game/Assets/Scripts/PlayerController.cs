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
	private InteractableIcon openIcon = null;

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

	private void UpdateAnimation(bool moving, bool facing)
	{
		if (moving)
		{
			if (facing)
			{
				if (carrying != null)
				{
					if (carrying.GetType() == typeof(CarryableMop))
					{
						animator.Play("Player_WalkingMop");
						return;
					}
					else if (carrying.GetType() == typeof(CarryableCD))
					{
						animator.Play("Player_WalkingCD");
						return;
					}
					else if (carrying.GetType() == typeof(CarryableGlass))
					{
						animator.Play("Player_WalkingFullBottle");
						return;
					}
					else if (carrying.GetType() == typeof(CarryableBowl))
					{
						if (((CarryableBowl)carrying).empty)
							animator.Play("Player_WalkingEmptyBowl");
						else
							animator.Play("Player_WalkingFullBowl");
					}
				}
				else
				{
					animator.Play("Player_Walking");
					return;
				}
			}
			else
			{
				if (carrying != null)
				{
					if (carrying.GetType() == typeof(CarryableMop))
					{
						animator.Play("Player_WalkingMop");
						return;
					}
					else if (carrying.GetType() == typeof(CarryableCD))
					{
						animator.Play("Player_WalkingCD");
						return;
					}
					else if (carrying.GetType() == typeof(CarryableGlass))
					{
						animator.Play("Player_WalkingFullBottle");
						return;
					}
					else if (carrying.GetType() == typeof(CarryableBowl))
					{
						if (((CarryableBowl)carrying).empty)
							animator.Play("Player_WalkingEmptyBowl");
						else
							animator.Play("Player_WalkingFullBowl");
					}
				}
				else
				{
					animator.Play("Player_Walking");
					return;
				}
			}
		}
		else
		{
			if (facing)
			{
				if (carrying != null)
				{
					if (carrying.GetType() == typeof(CarryableMop))
					{
						animator.Play("Player_IdleMop");
						return;
					}
					else if (carrying.GetType() == typeof(CarryableCD))
					{
						animator.Play("Player_IdleCD");
						return;
					}
					else if (carrying.GetType() == typeof(CarryableGlass))
					{
						animator.Play("Player_IdleFullBottle");
						return;
					}
					else if (carrying.GetType() == typeof(CarryableBowl))
					{
						if (((CarryableBowl)carrying).empty)
							animator.Play("Player_IdleEmptyBowl");
						else
							animator.Play("Player_IdleFullBowl");
					}
				}
				else
				{
					animator.Play("Player_Idle");
					return;
				}
			}
			else
			{
				if (carrying != null)
				{
					if (carrying.GetType() == typeof(CarryableMop))
					{
						animator.Play("Player_IdleMop");
						return;
					}
					else if (carrying.GetType() == typeof(CarryableCD))
					{
						animator.Play("Player_IdleCD");
						return;
					}
					else if (carrying.GetType() == typeof(CarryableGlass))
					{
						animator.Play("Player_IdleFullBottle");
						return;
					}
					else if (carrying.GetType() == typeof(CarryableBowl))
					{
						if (((CarryableBowl)carrying).empty)
							animator.Play("Player_IdleEmptyBowl");
						else
							animator.Play("Player_IdleFullBowl");
					}
				}
				else
				{
					animator.Play("Player_Idle");
					return;
				}
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

		UpdateAnimation(horizontal != 0 || vertical != 0, vertical > 0);

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

		Interactable foundInteractable = null;
		foreach (RaycastHit2D hit in hits.OrderBy(hit => Vector2.Distance(hit.transform.position, transform.position)))
		{
			Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();

			if (interactable == null)
				continue;

			foreach (Interactable.Interaction interaction in interactable.interactions)
			{
				if (interaction.active)
				{
					foundInteractable = interactable;

					bool eligible = interaction.IsEligible(this);

					if (eligible)
					{
						InteractableIcon icon = interaction.icon;

						if (openIcon != icon)
						{
							if (openIcon != null)
								openIcon.Hide();

							openIcon = icon;
							openIcon.Show();
						}

						if (Input.GetKeyDown(KeyCode.E))
						{
							interaction.Execute(this);

							//openIcon.Show();
							//openIcon = null;

							break;
						}
					}
				}
			}
		}

		if (foundInteractable != null && openIcon == null)
		{
			openIcon = foundInteractable.notEligibleIcon;
			openIcon.Show();
		}

		if (foundInteractable == null && openIcon != null)
		{
			openIcon.Hide();
			openIcon = null;
		}

		/*
		GuestController[] guestControllers = FindObjectsOfType<GuestController>();
		float weight = 0;
		foreach (GuestController guestController in guestControllers)
		{
			float dist = Vector2.Distance(guestController.transform.position, transform.position) / 10.0f;
			weight += dist;
		}

		audioController.UpdateCrowd(weight);
		*/
	}
}
