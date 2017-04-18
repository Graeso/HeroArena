using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class JeremiahScript : MonoBehaviour {
	public static JeremiahScript S;

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

	[Header ("Jeremiah Statistics")]
	public float maxHealth;
	public float maxStamina;
	private float curHealth;
	private float curStamina;

	[Header ("Jeremiah Basic Attack Variables")]
	[Range (0, 100)] public float jeremiahBasicDamage;
	[Range (0, 10)] public float jeremiahBasicCD;
	[Range (0, 1)] public float jeremiahBasicRange;
	public float jeremiahBasicSpeed;
	private bool jeremiahBasicCooling;

	[Header ("Jeremiah Inner Blood Variables")]
	public bool innerBlood = false;

	[Header ("Jeremiah Miscellaneous Variables")]
	HealthBarScript healthBarScript;
	public float coneAngle = 45f;

	#endregion

	void Awake () {
		S = this;
	}

	void Update () {
		
		this.transform.position = new Vector3 (transform.position.x, 0, transform.position.z);

		if (Device != null) {

			#region *** JEREMIAH BASIC ATTACK ***
			if (jeremiahBasicCooling) {
				jeremiahBasicCD -= Time.deltaTime;

				if (jeremiahBasicCD <= 0f) {
					jeremiahBasicCooling = false;
					jeremiahBasicCD = jeremiahBasicSpeed;
				}
			}

			if (Device.RightBumper.IsPressed) {
				if (jeremiahBasicCooling == false) {
					jeremiahBasic (jeremiahBasicRange);
				}
				//playerAnim.Play ("Attack V1");
			}
			#endregion

			#region *** JEREMIAH INNER BLOOD ***
			#endregion

			#region *** JEREMIAH CRIMSON REACH ***
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
				playerAnim.Play ("Move Idle");
			}
			if (moveDirection != Vector3.zero) {
				playerAnim.Play ("Run");
			}
		}
	}

	public void jeremiahBasic (float jeremiahBasicRange) {
		jeremiahBasicCooling = true;
		Vector3 explosionPos = basicSpawnPoint.transform.position;
		Collider[] hitColliders = Physics.OverlapSphere (explosionPos, jeremiahBasicRange);

		foreach (Collider hit in hitColliders) {
			if ((hit.GetComponent<Collider>().tag == "Player") && (hit.gameObject != playerParent.gameObject) && (Vector3.Angle (basicSpawnPoint.transform.forward, hit.transform.position - explosionPos) <= coneAngle)) {
				healthBarScript = hit.transform.FindChild ("HealthBarCanvas").GetComponent<HealthBarScript> ();
				healthBarScript.GetHit (jeremiahBasicDamage);
			}
		}
	}

	public void jeremiahBasic2 (float jeremiahBasicRange) {
		jeremiahBasicCooling = true;
		Vector3 explosionPos = basicSpawnPoint.transform.position;
		Collider[] hitColliders = Physics.OverlapSphere (explosionPos, jeremiahBasicRange);

		foreach (Collider hit in hitColliders) {
			if ((hit.GetComponent<Collider> ().tag == "Player") && (hit.gameObject != playerParent.gameObject) && (Vector3.Angle (basicSpawnPoint.transform.forward, hit.transform.position - explosionPos) <= coneAngle)) {
				healthBarScript = hit.transform.FindChild ("HealthBarCanvas").GetComponent<HealthBarScript> ();
				healthBarScript.GetHit (jeremiahBasicDamage);
			}
		}
	}
}