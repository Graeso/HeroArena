using UnityEngine;
using System.Collections;
using InControl;

public class SinglePlayerScript : MonoBehaviour {

	// Public Adjustable Variables (EDIT IN INSPECTOR)
	[Header ("Adjustable Variables")]
	public float speed = 0f;
	public float rotateChar = 0f;

	// Private Adjustable Variables

	// Public Static Variables (MUST BE SET BUT NOT EDITED)
	[Header ("Settable Variables")]
	public Animation playerAnim;
	public GameObject playerBody;
	public GameObject playerParent;
	public GameObject xanderBomb;
	[HideInInspector]
	public bool canMove = true;

	// Private Static Variables
	private Vector3 moveDirection = Vector3.zero;
	private Vector3 headDirection = Vector3.zero;

	void Awake () {
		
	}

	void Start () {
		
	}

	void Update () {

		// Moving and rotating the character with the left stick
		var inputDevice = InputManager.ActiveDevice;
		CharacterController Controller = GetComponent<CharacterController> ();

		if (canMove) {
			moveDirection = new Vector3 (inputDevice.LeftStickX, 0, inputDevice.LeftStickY);
			moveDirection = transform.TransformDirection (moveDirection);
			moveDirection *= speed;

			if (moveDirection != Vector3.zero) {
				playerBody.transform.rotation = Quaternion.Slerp (
					playerBody.transform.rotation,
					Quaternion.LookRotation (moveDirection),
					Time.deltaTime * rotateChar);
			}
		}

		Controller.Move (moveDirection * Time.deltaTime);

		// Button presses including Basic Attack, Special 1, Special 2, and Ultimate
		if (inputDevice.RightTrigger.IsPressed) {
			
		}

		// Playing animations
		if (moveDirection == Vector3.zero) { 
			playerAnim.Play ("Idle");
		}
		if (moveDirection != Vector3.zero) {
			playerAnim.Play ("Run");
		}
	}

}
