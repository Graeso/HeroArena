using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class XanderScript : MonoBehaviour {

	public InputDevice player;

	#region Variables
	[Header ("Xander Statistics")]
	public float maxHealth;
	public float maxStamina;
	private float curHealth;
	private float curStamina;

	[Header ("Xander Basic Attack Variables")]
	[Range (0, 100)]
	public float xanderBasicDamage;
	[Range (0, 10)]
	public float xanderBasicCD;
	public float xanderBasicSpeed = .5f;
	private bool basicCooling;

	[Header ("Xander Mine Variables")]
	[Range (0, 100)]
	public float xanderMineDamage;
	[Range (0, 10)]
	public float xanderMineCD;
	public float xanderMineSpeed;
	private bool mineCooling;

	[Header ("Xander Ultimate Variables")]
	[Range (0, 250)]
	public float xanderUltDamage;
	[Range (0, 120)]
	public float xanderUltCD;
	public float xanderUltSpeed;
	private bool ultCooling;

	[Header ("Xander Dig Variables")]
	[Range (0, 100)]
	public float xanderDigDistance;
	[Range (0, 10)]
	public float xanderDigCD;
	public float xanderDigSpeed;
	private bool digCooling;

	[Header ("Xander Various")]
	public GameObject basicSpawnPoint;
	#endregion

	void Start () {
		
	}

	void Update () {
		
		if (basicCooling) {
			xanderBasicCD -= Time.deltaTime;

			if (xanderBasicCD <= 0f) {
				basicCooling = false;
				xanderBasicCD = xanderBasicSpeed;
			}
		}

		if (player.RightTrigger.IsPressed) {
			if (basicCooling == false)
				xanderBasic (xanderBasicDamage, xanderBasicCD, xanderBasicSpeed);
		}

	}

	public void xanderBasic (float basicDamage, float basicCD, float basicSpeed) {
		basicCooling = true;
		this.GetComponent<AudioSource> ().Play ();
		GameObject creation;
		creation = Instantiate (Resources.Load ("XanderGrenade"), basicSpawnPoint.transform.position, basicSpawnPoint.transform.rotation) as GameObject;
		creation.transform.eulerAngles = new Vector3 (creation.transform.eulerAngles.x, creation.transform.eulerAngles.y + Random.Range (-4f, 4f), creation.transform.eulerAngles.x);
		creation = Instantiate (Resources.Load ("XanderGrenade"), basicSpawnPoint.transform.position, basicSpawnPoint.transform.rotation) as GameObject;
		creation.transform.eulerAngles = new Vector3 (creation.transform.eulerAngles.x, creation.transform.eulerAngles.y + Random.Range (-4f, 4f), creation.transform.eulerAngles.x);
		creation = Instantiate (Resources.Load ("XanderGrenade"), basicSpawnPoint.transform.position, basicSpawnPoint.transform.rotation) as GameObject;
		creation.transform.eulerAngles = new Vector3 (creation.transform.eulerAngles.x, creation.transform.eulerAngles.y + Random.Range (-4f, 4f), creation.transform.eulerAngles.x);
	}

	public void xanderMine (float mineDamage, float mineCD, float mineSpeed) {
		
	}

	public void xanderUlt (float ultDamage, float ultCD, float ultSpeed) {
		
	}

	public void xanderDig (float digDistance, float digCD, float digSpeed) {
		
	}

}
