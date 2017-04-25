using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class SheraScript : MonoBehaviour
{
	public static SheraScript S;

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

	[Header ("----- Shera Statistics -----")]
	public float maxHealth;
	public float maxStamina;
	private float curHealth;
	private float curStamina;

	[Header ("----- Shera Basic Attack Variables -----")]
	[Range (0, 100)] public float sheraBasicDamage;
	[Range (0, 10)] public float sheraBasicCD;
	[Range (0, 1)] public float sheraBasicRange;
	public float sheraBasicSpeed;
	private bool sheraBasicCooling;

	[Header ("----- Shera Double Kick Variables -----")]
	[Range (0, 100)] public float sheraDoubleKickDamage;
	[Range (0, 1)] public float sheraDoubleKickSlow;
	[Range (0, 5)] public float sheraDoubleKickSlowLength;
	[Range (0, 10)] public float sheraDoubleKickCD;
	[Range (0, 1)] public float sheraDoubleKickRange;
	public float sheraDoubleKickSpeed;
	private bool sheraDoubleKickCooling;

	[Header ("----- Shera Dancing Leap Variables -----")]
	[Range (0, 100)] public float sheraDancingLeapDamage;
	[Range (0, 10)] public float sheraDancingLeapDistance;
	[Range (0, 10)] public float sheraDancingLeapCD;
	[Range (0, 1)] public float sheraDancingLeapRange;
	public float sheraDancingLeapSpeed;
	private bool sheraDancingLeapCooling;

	[Header ("----- Shera Miscellaneous Variables -----")]
	HealthBarScript healthBarScript;
	public float coneAngle = 45f;
	[HideInInspector] public bool sheraSlowed = false;
	[HideInInspector] public float sheraSlowedAmount;
	[HideInInspector] public float sheraSlowedLength;

	#endregion

	void Awake ()
	{
		S = this;
		this.gameObject.name = "Shera";
	}

	void Update ()
	{

		this.transform.position = new Vector3 (transform.position.x, 0, transform.position.z);

		if (Device != null) {

			#region *** SHERA SLOW DURATION ***
			if (sheraSlowed) {
				sheraSlowedLength -= Time.deltaTime;

				if (sheraSlowedLength <= 0f) {
					sheraSlowed = false;
					canMove = true;
					sheraSlowedAmount = 0f;
					sheraSlowedLength = 0f;
				}
			}
			#endregion

			if (canMove) {
				
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
				}
				#endregion

				#region  *** SHERA DANCING LEAP ***
				if (sheraDancingLeapCooling) {
					sheraDancingLeapCD -= Time.deltaTime;

					if (sheraDancingLeapCD <= 0f) {
						sheraDancingLeapCooling = false;
						sheraDancingLeapCD = sheraDancingLeapSpeed;
					}
				}

				if (Device.Action2.WasPressed) {
					if (sheraDancingLeapCooling == false)
						sheraDancingLeap (sheraDancingLeapRange);
				}
				#endregion

				#region *** SHERA DOUBLE KICK ***
				if (sheraDoubleKickCooling) {
					sheraDoubleKickCD -= Time.deltaTime;

					if (sheraDoubleKickCD <= 0f) {
						sheraDoubleKickCooling = false;
						sheraDoubleKickCD = sheraDoubleKickSpeed;
					}
				}

				if (Device.Action1.IsPressed) {
					if (sheraDoubleKickCooling == false)
						sheraDoubleKick (sheraDoubleKickRange);
				}
				#endregion

				// Moving and rotating the character with the left stick
				CharacterController Controller = GetComponent<CharacterController> ();

				if (sheraSlowed == false) {
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

				if (sheraSlowed == true) {
					moveDirection = new Vector3 (Device.LeftStickX, 0, Device.LeftStickY);
					moveDirection = transform.TransformDirection (moveDirection);
					moveDirection *= speed - sheraSlowedAmount;

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
				playerAnim.Play ("Run");
			}
		}
	}

	public void sheraBasic (float sheraBasicRange)
	{
		sheraBasicCooling = true;
		Vector3 explosionPos = basicSpawnPoint.transform.position;
		Collider[] hitColliders = Physics.OverlapSphere (explosionPos, sheraBasicRange);

		foreach (Collider hit in hitColliders) {
			if ((hit.GetComponent<Collider> ().tag == "Player") && (hit.gameObject != playerParent.gameObject) && (Vector3.Angle (basicSpawnPoint.transform.forward, hit.transform.position - explosionPos) <= coneAngle)) {
				healthBarScript = hit.transform.FindChild ("HealthBarCanvas").GetComponent<HealthBarScript> ();
				healthBarScript.GetHit (sheraBasicDamage);
			}
		}
	}

	public void sheraDoubleKick (float sheraBasicRange)
	{
		sheraDoubleKickCooling = true;
		Vector3 explosionPos = basicSpawnPoint.transform.position;
		Collider[] hitColliders = Physics.OverlapSphere (explosionPos, sheraDoubleKickRange);

		foreach (Collider hit in hitColliders) {
			if ((hit.GetComponent<Collider> ().tag == "Player") && (hit.gameObject != playerParent.gameObject)) {
				healthBarScript = hit.transform.FindChild ("HealthBarCanvas").GetComponent<HealthBarScript> ();
				healthBarScript.GetHit (sheraDoubleKickDamage);
				if (hit.name == "Xander") {
					hit.transform.GetComponent<XanderScript> ().xanderSlowed = true;
					hit.transform.GetComponent<XanderScript> ().xanderSlowedAmount += sheraDoubleKickSlow;
					hit.transform.GetComponent<XanderScript> ().xanderSlowedLength += sheraDoubleKickSlowLength;
				}
				if (hit.name == "Shera") {
					hit.transform.GetComponent<SheraScript> ().sheraSlowed = true;
					hit.transform.GetComponent<SheraScript> ().sheraSlowedAmount += sheraDoubleKickSlow;
					hit.transform.GetComponent<SheraScript> ().sheraSlowedLength += sheraDoubleKickSlowLength;
				}
				if (hit.name == "Croak") {
					hit.transform.GetComponent<CroakScript> ().croakSlowed = true;
					hit.transform.GetComponent<CroakScript> ().croakSlowedAmount += sheraDoubleKickSlow;
					hit.transform.GetComponent<CroakScript> ().croakSlowedLength += sheraDoubleKickSlowLength;
				}
				if (hit.name == "Jeremiah") {
					hit.transform.GetComponent<JeremiahScript> ().jeremiahSlowed = true;
					hit.transform.GetComponent<JeremiahScript> ().jeremiahSlowedAmount += sheraDoubleKickSlow;
					hit.transform.GetComponent<JeremiahScript> ().jeremiahSlowedLength += sheraDoubleKickSlowLength;
				}
			}
		}
	}

	public void sheraDancingLeap (float sheraDancingLeapRange)
	{
		sheraDancingLeapCooling = true;
		this.transform.position	+= playerBody.transform.forward * sheraDancingLeapDistance;

		Vector3 explosionPos = basicSpawnPoint.transform.position;
		Collider[] hitColliders = Physics.OverlapSphere (explosionPos, sheraDancingLeapRange);

		foreach (Collider hit in hitColliders) {
			if ((hit.GetComponent<Collider> ().tag == "Player") && (hit.gameObject != playerParent.gameObject)) {
				healthBarScript = hit.transform.FindChild ("HealthBarCanvas").GetComponent<HealthBarScript> ();
				healthBarScript.GetHit (sheraDancingLeapDamage);
			}
		}
	}
} 