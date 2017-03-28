using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour {
	public static HealthBarScript S;
	
	// Private variables
	private GameObject thisParent;
	private float maxHealth;
	[SerializeField] private float curHealth;
	private float calcHealth;
	private Image healthBar;

	// Other script variable access
	XanderScript xanderScript;
	SheraScript sheraScript;

	void Start () {
		S = this;
		thisParent = this.transform.root.gameObject;
		if (thisParent.name == "Xander") {
			xanderScript = thisParent.GetComponent<XanderScript> ();
			maxHealth = xanderScript.maxHealth;
		}
		if (thisParent.name == "Shera") {
			sheraScript = thisParent.GetComponent<SheraScript> ();
			maxHealth = sheraScript.maxHealth;
		} else {
			maxHealth = 100f;
		}
		curHealth = maxHealth;

		healthBar = this.transform.FindChild("HealthBar").GetComponent<Image>();
		healthBar.transform.localScale = new Vector3 (Mathf.Clamp (maxHealth, 0f, 1f), healthBar.transform.localScale.y, healthBar.transform.localScale.z);
	}

	public void GetHit (float healthLost) {
		curHealth -= healthLost;

		calcHealth = curHealth / maxHealth;

		if (curHealth > maxHealth) {
			curHealth = maxHealth;
		}

		healthBar.transform.localScale = new Vector3 (Mathf.Clamp (calcHealth, 0f, 1f), healthBar.transform.localScale.y, healthBar.transform.localScale.z);
	}

	void Update () {
		if (curHealth <= 0f) {
			Destroy (thisParent);
		}
	}
}
