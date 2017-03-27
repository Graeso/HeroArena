using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarScript : MonoBehaviour {
	private GameObject parent;
	private float maxHealth;
	private float curHealth;
	XanderScript xanderScript;
	SheraScript sheraScript;

	void Start () {
		parent = this.transform.root.parent.gameObject;
		if (parent.name == "Xander") {
			xanderScript = parent.GetComponent<XanderScript> ();
			maxHealth = xanderScript.maxHealth;
		}
		if (parent.name == "Shera") {
			sheraScript = parent.GetComponent<SheraScript> ();
			// maxHealth = sheraScript.maxHealth;
		} else {
			maxHealth = 100f;
		}
	}

	void Update () {
		if (curHealth <= 0f) {
			Destroy (parent.gameObject);
			Destroy (this.gameObject);
		}
	}
}
