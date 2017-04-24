using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class JeremiahScript : MonoBehaviour
{
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
	public GameObject crimsonReachSpawnPoint;
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
	[Range (0, 100)] public float jeremiahInnerBloodDamage;
	[Range (0, 10)] public float jeremiahInnerBloodCD;
	[Range (0, 1)] public float jeremiahInnerBloodRange;
	public float jeremiahInnerBloodSpeed;
	private bool jeremiahInnerBloodCooling;
	public bool innerBlood = false;

	[Header ("Jeremiah Crimson Reach Variables")]
	[Range (0, 10)] public float jeremiahCrimsonReachCD;
	[Range (0, 10)] public float jeremiahCrimsonReachRange;
	[Range (0, 1)] public float jeremiahCrimsonReachSlow;
	[Range (0, 5)] public float jeremiahCrimsonReachSlowLength;
	public float jeremiahCrimsonReachSpeed;
	private bool jeremiahCrimsonReachCooling;

	[Header ("Jeremiah Miscellaneous Variables")]
	public float slimConeAngle;
	public float wideConeAngle;
	HealthBarScript healthBarScript;
	[HideInInspector] public bool jeremiahSlowed = false;
	[HideInInspector] public float jeremiahSlowedAmount;
	[HideInInspector] public float jeremiahSlowedLength;

	#endregion

	void Awake ()
	{
		S = this;
		this.gameObject.name = "Jeremiah";
	}

	void Update ()
	{
		
		this.transform.position = new Vector3 (transform.position.x, 0, transform.position.z);

		if (Device != null) {

			#region *** JEREMIAH SLOW DURATION ***
			if (jeremiahSlowed) {
				jeremiahSlowedLength -= Time.deltaTime;

				if (jeremiahSlowedLength <= 0f) {
					jeremiahSlowed = false;
					canMove = true;
					jeremiahSlowedAmount = 0f;
					jeremiahSlowedLength = 0f;
				}
			}
			#endregion

			if (canMove) {

				#region *** JEREMIAH BASIC ATTACK ***
				if (jeremiahBasicCooling) {
					jeremiahBasicCD -= Time.deltaTime;

					if (jeremiahBasicCD <= 0f) {
						jeremiahBasicCooling = false;
						jeremiahBasicCD = jeremiahBasicSpeed;
					}
				}

				if ((Device.RightBumper.IsPressed)) {
					if ((jeremiahBasicCooling == false) && (innerBlood == false)) {
						jeremiahBasic (jeremiahBasicRange);
					} else if ((jeremiahInnerBloodCooling == false) && (innerBlood == true)) {
						jeremiahInnerBlood (jeremiahInnerBloodRange);
					}
				}
				#endregion

				#region *** JEREMIAH INNER BLOOD ***
				if (jeremiahInnerBloodCooling) {
					jeremiahInnerBloodCD -= Time.deltaTime;

					if (jeremiahInnerBloodCD <= 0f) {
						jeremiahInnerBloodCooling = false;
						jeremiahInnerBloodCD = jeremiahInnerBloodSpeed;
					}
				}

				if (Device.Action1.WasPressed) {
					if (innerBlood == false)
						innerBlood = true;
					else
						innerBlood = false;
				}
				#endregion

				#region *** JEREMIAH CRIMSON REACH ***
				if (jeremiahCrimsonReachCooling) {
					jeremiahCrimsonReachCD -= Time.deltaTime;

					if (jeremiahCrimsonReachCD <= 0f) {
						jeremiahCrimsonReachCooling = false;
						jeremiahCrimsonReachCD = jeremiahCrimsonReachSpeed;
					}
				}

				if ((Device.Action2.WasPressed)) {
					if (jeremiahCrimsonReachCooling == false) {
						jeremiahCrimsonReach (jeremiahCrimsonReachRange);
					}
				}
				#endregion

				// Moving and rotating the character with the left stick
				CharacterController Controller = GetComponent<CharacterController> ();

				if (jeremiahSlowed == false) {
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

				if (jeremiahSlowed == true) {
					moveDirection = new Vector3 (Device.LeftStickX, 0, Device.LeftStickY);
					moveDirection = transform.TransformDirection (moveDirection);
					moveDirection *= speed - jeremiahSlowedAmount;

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
				playerAnim.Play ("Move Idle");
			}
			if (moveDirection != Vector3.zero) {
				playerAnim.Play ("Run");
			}
		}
	}

	public void jeremiahBasic (float jeremiahBasicRange)
	{
		jeremiahBasicCooling = true;
		Vector3 explosionPos = basicSpawnPoint.transform.position;
		Collider[] hitColliders = Physics.OverlapSphere (explosionPos, jeremiahBasicRange);

		foreach (Collider hit in hitColliders) {
			if ((hit.GetComponent<Collider> ().tag == "Player") && (hit.gameObject != playerParent.gameObject) && (Vector3.Angle (basicSpawnPoint.transform.forward, hit.transform.position - explosionPos) <= slimConeAngle)) {
				healthBarScript = hit.transform.FindChild ("HealthBarCanvas").GetComponent<HealthBarScript> ();
				healthBarScript.GetHit (jeremiahBasicDamage);
			}
		}
	}

	public void jeremiahInnerBlood (float jeremiahInnerBloodRange)
	{
		jeremiahInnerBloodCooling = true;
		Vector3 explosionPos = basicSpawnPoint.transform.position;
		Collider[] hitColliders = Physics.OverlapSphere (explosionPos, jeremiahInnerBloodRange);

		foreach (Collider hit in hitColliders) {
			if ((hit.GetComponent<Collider> ().tag == "Player") && (hit.gameObject != playerParent.gameObject) && (Vector3.Angle (basicSpawnPoint.transform.forward, hit.transform.position - explosionPos) <= wideConeAngle)) {
				healthBarScript = hit.transform.FindChild ("HealthBarCanvas").GetComponent<HealthBarScript> ();
				healthBarScript.GetHit (jeremiahInnerBloodDamage);
			}
		}
	}

	public void jeremiahCrimsonReach (float jeremiahCrimsonReachRange)
	{
		jeremiahCrimsonReachCooling = true;
		Vector3 explosionPos = basicSpawnPoint.transform.position;
		Collider[] hitColliders = Physics.OverlapSphere (explosionPos, jeremiahCrimsonReachRange);

		foreach (Collider hit in hitColliders) {
			if ((hit.GetComponent<Collider> ().tag == "Player") && (hit.gameObject != playerParent.gameObject) && (Vector3.Angle (basicSpawnPoint.transform.forward, hit.transform.position - explosionPos) <= slimConeAngle)) {
				if (hit.name == "Xander") {
					hit.transform.GetComponent<XanderScript> ().canMove = false;
					hit.transform.GetComponent<XanderScript> ().xanderSlowed = true;
					hit.transform.GetComponent<XanderScript> ().xanderSlowedLength += jeremiahCrimsonReachSlowLength;
					hit.transform.position = crimsonReachSpawnPoint.transform.position;
				}
				if (hit.name == "Shera") {
					hit.transform.GetComponent<SheraScript> ().canMove = false;
					hit.transform.GetComponent<SheraScript> ().sheraSlowed = true;
					hit.transform.GetComponent<SheraScript> ().sheraSlowedLength += jeremiahCrimsonReachSlowLength;
					hit.transform.position = crimsonReachSpawnPoint.transform.position;
				}
				if (hit.name == "Croak") {
					hit.transform.GetComponent<CroakScript> ().canMove = false;
					hit.transform.GetComponent<CroakScript> ().croakSlowed = true;
					hit.transform.GetComponent<CroakScript> ().croakSlowedLength += jeremiahCrimsonReachSlowLength;
					hit.transform.position = crimsonReachSpawnPoint.transform.position;
				}
				if (hit.name == "Jeremiah") {
					hit.transform.GetComponent<JeremiahScript> ().canMove = false;
					hit.transform.GetComponent<JeremiahScript> ().jeremiahSlowed = true;
					hit.transform.GetComponent<JeremiahScript> ().jeremiahSlowedLength += jeremiahCrimsonReachSlowLength;
					hit.transform.position = crimsonReachSpawnPoint.transform.position;
				}
			}
		}
	}
}