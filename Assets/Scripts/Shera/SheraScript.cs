using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class SheraScript : MonoBehaviour {
	public static SheraScript S;

	public InputDevice Device { get; set; }

	#region Variables
	[Header ("Adjustable Variables")]
	[Range (0, 10)] public float speed = 0f;
	private float rotateChar = 12f;

	[Header ("Settable Variables")]
	public Animation playerAnim;
	public GameObject playerBody;
	public GameObject playerParent;
	public GameObject basicSpawnPoint;
	[HideInInspector] public bool canMove = true;

	// Private Static Variables
	private Vector3 moveDirection = Vector3.zero;
	private Vector3 headDirection = Vector3.zero;

	[Header ("Shera Statistics")]
	public float maxHealth;
	public float maxStamina;
	private float curHealth;
	private float curStamina;

	[Header ("Shera Basic Attack Variables")]
	[Range (0, 100)] public float sheraBasicDamage;
	[Range (0, 10)] public float sheraBasicCD;
	[Range (0, 1)] public float sheraBasicRange;
	public float sheraBasicSpeed;
	private bool sheraBasicCooling;

	[Header ("Shera Miscellaneous Variables")]
	HealthBarScript healthBarScript; 
	#endregion

	void Awake () {
		S = this;
	}

	void Update () {

		this.transform.position = new Vector3 (transform.position.x, 0, transform.position.z);

		if (Device != null) {

			#region *** SHERA BASIC ATTACK ***
			if (sheraBasicCooling) {
				sheraBasicCD -= Time.deltaTime;

				if (sheraBasicCD <= 0f) {
					sheraBasicCooling = false;
					sheraBasicCD = sheraBasicSpeed;
				}
			}

			if (Device.RightBumper.IsPressed) {
				if (sheraBasicCooling == false)
					sheraBasic (sheraBasicRange);
				//playerAnim.Play ("Attack");
			}
			#endregion

			#region  *** SHERA DANCING LEAP ***
			#endregion

			#region *** SHERA DOUBLE KICK ***
			#endregion

			if (canMove) {
				// Moving and rotating the character with the left stick
				CharacterController Controller = GetComponent<CharacterController> ();

				moveDirection = new Vector3 (Device.LeftStickX, 0, Device.LeftStickY);
				moveDirection = transform.TransformDirection (moveDirection);
				moveDirection *= speed;

				if (moveDirection != Vector3.zero && headDirection == Vector3.zero) {
					playerBody.transform.rotation = Quaternion.Slerp (
						playerBody.transform.rotation,
						Quaternion.LookRotation (moveDirection),
						Time.deltaTime * rotateChar);
				}

				// Rotating the character with the right stick
				headDirection = new Vector3 (Device.RightStick.X, 0, Device.RightStick.Y);
				if (headDirection != Vector3.zero) {
					playerBody.transform.parent = playerParent.transform;
					playerBody.transform.rotation = Quaternion.Slerp (
						playerBody.transform.rotation,
						Quaternion.LookRotation (headDirection),
						Time.deltaTime * rotateChar);
				}

				Controller.Move (moveDirection * Time.deltaTime);
			}

			// Playing animations
			if (moveDirection == Vector3.zero) { 
				playerAnim.Play ("Nothing Idle");
			}
			if (moveDirection != Vector3.zero) {
				playerAnim.Play ("Run");
			}
		}
	}

	public void sheraBasic (float sheraBasicRange) {
		sheraBasicCooling = true;
		RaycastHit hit;
		Debug.DrawRay (basicSpawnPoint.transform.position, basicSpawnPoint.transform.forward * sheraBasicRange);
		if (Physics.Raycast (basicSpawnPoint.transform.position, basicSpawnPoint.transform.forward, out hit, sheraBasicRange)) {
			if (hit.collider.tag == "Player") {
				healthBarScript = hit.transform.FindChild ("HealthBarCanvas").GetComponent<HealthBarScript> ();
				healthBarScript.GetHit (sheraBasicDamage);
			}
		}
	}

}