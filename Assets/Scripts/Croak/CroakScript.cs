using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class CroakScript : MonoBehaviour
{
	public static CroakScript S;

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

	[Header ("Croak Statistics")]
	public float maxHealth;
	public float maxStamina;
	private float curHealth;
	private float curStamina;

	[Header ("Croak Basic Attack Variables")]
	[Range (0, 100)] public float croakBasicDamage;
	[Range (0, 10)] public float croakBasicCD;
	[Range (0, 1)] public float croakBasicRange;
	public float croakBasicSpeed;
	private bool croakBasicCooling;

	[Header ("Croak Miscellaneous Variables")]
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
					//playerAnim.Play ("Attack V1");
				}
				#endregion

				#region *** CROAK MANGLE ***
				#endregion

				#region *** CROAK FIRE BREATH ***
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
}