using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineScript : MonoBehaviour {

	HealthBarScript healthBarScript;
	private Rigidbody rb;
	[HideInInspector] public GameObject originalXander;
	[HideInInspector] public GameObject teammate;

	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	void OnTriggerEnter (Collider col) {
		Explosion (col);
		Destroy (this.gameObject);
	}

	void Explosion (Collider col) {
		if (col.gameObject.tag == "Player") {
			healthBarScript = col.transform.FindChild ("HealthBarCanvas").GetComponent<HealthBarScript> ();
			healthBarScript.GetHit (50f);
		}
	}

}
