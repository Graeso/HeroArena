using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class CroakScript : MonoBehaviour
{
	public static CroakScript S;

	public InputDevice Device { get; set; }

	#region Variables

	[Header ("----- Adjustable Variables -----")]
	[Range (0, 10)] public float speed = 0f;
	private float rotateChar = 12f;

	[Header ("----- Settable Variables -----")]
	public Animation playerAnim;
	public GameObject playerBody;
	public GameObject playerParent;
	public GameObject basicSpawnPoint;
	[HideInInspector] public bool canMove = true;

	// Private Static Variables
	private Vector3 moveDirection = Vector3.zero;
	private Vector3 headDirection = Vector3.zero;

	[Header ("----- Croak Statistics -----")]
	public float maxHealth;
	public float maxStamina;
	private float curHealth;
	private float curStamina;

	[Header ("----- Croak Basic Attack Variables -----")]
	[Range (0, 100)] public float croakBasicDamage;
	[Range (0, 10)] public float croakBasicCD;
	[Range (0, 1)] public float croakBasicRange;
	public float croakBasicSpeed;
	private bool croakBasicCooling;

	[Header ("----- Croak Mangle Variables -----")]
	[Range (0, 100)] public float croakMangleDamage;
	[Range (0, 10)] public float croakMangleCD;
	[Range (0, 10)] public float croakMangleRange;
	[Range (0, 1)] public float croakMangleSlow;
	[Range (0, 5)] public float croakMangleSlowLength;
	public float croakMangleSpeed;
	private bool croakMangleCooling;

	[Header ("----- Croak Fireball Variables -----")]
	[Range (0, 100)] public float croakFireballDamage;
	[Range (0, 10)] public float croakFireballCD;
	public float croakFireballSpeed;
	private bool croakFireballCooling;

	[Header ("----- Croak Miscellaneous Variables -----")]
	HealthBarScript healthBarScript;
	public float coneAngle = 45f;
	[HideInInspector] public bool croakSlowed = false;
	[HideInInspector] public float croakSlowedAmount;
	[HideInInspector] public float croakSlowedLength;

	#endregion

	void Awake ()
	{
		S = this;
		this.gameObject.name = "Croak";
	}

	void Update ()
	{

		this.transform.position = new Vector3 (transform.position.x, 0, transform.position.z);

		if (Device != null) {

			#region *** CROAK SLOW DURATION ***
			if (croakSlowed) {
				croakSlowedLength -= Time.deltaTime;

				if (croakSlowedLength <= 0f) {
					croakSlowed = false;
					canMove = true;
					croakSlowedAmount = 0f;
					croakSlowedLength = 0f;
				}
			}
			#endregion

			if (canMove) {
				
				#region *** CROAK BASIC ATTACK ***
				if (croakBasicCooling) {
					croakBasicCD -= Time.deltaTime;

					if (croakBasicCD <= 0f) {
						croakBasicCooling = false;
						croakBasicCD = croakBasicSpeed;
					}
				}

				if (Device.RightBumper.IsPressed) {
					if (croakBasicCooling == false)
						croakBasic (croakBasicRange);
				}
				#endregion

				#region *** CROAK MANGLE ***
				if (croakMangleCooling) {
					croakMangleCD -= Time.deltaTime;

					if (croakMangleCD <= 0f) {
						croakMangleCooling = false;
						croakMangleCD = croakMangleSpeed;
					}
				}

				if ((Device.Action1.WasPressed)) {
					if (croakMangleCooling == false) {
						croakMangle (croakMangleRange);
					}
				}
				#endregion

				#region *** CROAK FIRE BREATH ***
				if (croakFireballCooling) {
					croakFireballCD -= Time.deltaTime;

					if (croakFireballCD <= 0f) {
						croakFireballCooling = false;
						croakFireballCD = croakFireballSpeed;
					}
				}

				if (Device.Action2.WasPressed) {
					if (croakFireballCooling == false)
						croakFireball ();
				}
				#endregion

				// Moving and rotating the character with the left stick
				CharacterController Controller = GetComponent<CharacterController> ();

				if (croakSlowed == false) {
					moveDirection = new Vector3 (Device.LeftStickX, 0, Device.LeftStickY);
					moveDirection = transform.TransformDirection (moveDirection);
					moveDirection *= speed;

					if (moveDirection != Vector3.zero && headDirection == Vector3.zero) {
						playerBody.transform.rotation = Quaternion.Slerp (
							playerBody.transform.rotation,
							Quaternion.LookRotation (moveDirection),
							Time.deltaTime * rotateChar);
					}
				}

				if (croakSlowed == true) {
					moveDirection = new Vector3 (Device.LeftStickX, 0, Device.LeftStickY);
					moveDirection = transform.TransformDirection (moveDirection);
					moveDirection *= speed - croakSlowedAmount;

					if (moveDirection != Vector3.zero && headDirection == Vector3.zero) {
						playerBody.transform.rotation = Quaternion.Slerp (
							playerBody.transform.rotation,
							Quaternion.LookRotation (moveDirection),
							Time.deltaTime * rotateChar);
					}
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
				playerAnim.Play ("Run ");
			}
		}
	}

	public void croakBasic (float croakBasicRange)
	{
		croakBasicCooling = true;
		Vector3 explosionPos = basicSpawnPoint.transform.position;
		Collider[] hitColliders = Physics.OverlapSphere (explosionPos, croakBasicRange);

		foreach (Collider hit in hitColliders) {
			if ((hit.GetComponent<Collider> ().tag == "Player") && (hit.gameObject != playerParent.gameObject) && (Vector3.Angle (basicSpawnPoint.transform.forward, hit.transform.position - explosionPos) <= coneAngle)) {
				healthBarScript = hit.transform.FindChild ("HealthBarCanvas").GetComponent<HealthBarScript> ();
				healthBarScript.GetHit (croakBasicDamage);
			}
		}
	}

	public void croakMangle (float croakMangleRange)
	{
		croakMangleCooling = true;
		Vector3 explosionPos = basicSpawnPoint.transform.position;
		Collider[] hitColliders = Physics.OverlapSphere (explosionPos, croakMangleRange);

		foreach (Collider hit in hitColliders) {
			if ((hit.GetComponent<Collider> ().tag == "Player") && (hit.gameObject != playerParent.gameObject)) {
				healthBarScript = hit.transform.FindChild ("HealthBarCanvas").GetComponent<HealthBarScript> ();
				healthBarScript.GetHit (croakMangleDamage);
				if (hit.name == "Xander") {
					hit.transform.GetComponent<XanderScript> ().xanderSlowed = true;
					hit.transform.GetComponent<XanderScript> ().xanderSlowedAmount += croakMangleSlow;
					hit.transform.GetComponent<XanderScript> ().xanderSlowedLength += croakMangleSlowLength;
				}
				if (hit.name == "Shera") {
					hit.transform.GetComponent<SheraScript> ().sheraSlowed = true;
					hit.transform.GetComponent<SheraScript> ().sheraSlowedAmount += croakMangleSlow;
					hit.transform.GetComponent<SheraScript> ().sheraSlowedLength += croakMangleSlowLength;
				}
				if (hit.name == "Croak") {
					hit.transform.GetComponent<CroakScript> ().croakSlowed = true;
					hit.transform.GetComponent<CroakScript> ().croakSlowedAmount += croakMangleSlow;
					hit.transform.GetComponent<CroakScript> ().croakSlowedLength += croakMangleSlowLength;
				}
				if (hit.name == "Jeremiah") {
					hit.transform.GetComponent<JeremiahScript> ().jeremiahSlowed = true;
					hit.transform.GetComponent<JeremiahScript> ().jeremiahSlowedAmount += croakMangleSlow;
					hit.transform.GetComponent<JeremiahScript> ().jeremiahSlowedLength += croakMangleSlowLength;
				}
			}
		}
	}

	public void croakFireball ()
	{
		croakFireballCooling = true;
		GameObject creation;
		creation = Instantiate (Resources.Load ("croakFireball"), basicSpawnPoint.transform.position, basicSpawnPoint.transform.rotation) as GameObject;
		creation.transform.eulerAngles = new Vector3 (creation.transform.eulerAngles.x, creation.transform.eulerAngles.y - 2, creation.transform.eulerAngles.x);
	}
}