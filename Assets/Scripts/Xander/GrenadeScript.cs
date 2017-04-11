using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour {

	HealthBarScript healthBarScript; 
	private float lifeTime = 3f;
	private Rigidbody rb;
	[Range (0, 100)] public float thrust = 50f;
	public GameObject originalXander;
	public GameObject teammate;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		rb.AddForce (transform.forward * thrust);
	}

	void Update () {
		lifeTime -= Time.deltaTime;

		if (lifeTime <= 0f) {
			Destroy (this.gameObject);
		}
	}

	void OnCollisionEnter (Collision col) {
		Explosion (Vector3.zero, 25, col);
		Destroy (this.gameObject);
	}

	void Explosion (Vector3 center, float radius, Collision col) {
		Collider[] hitColliders = Physics.OverlapSphere (center, radius);
		int i = 0;
		while (i < hitColliders.Length) {
			if (hitColliders [i].gameObject.tag == "Player") {
				healthBarScript = col.transform.FindChild ("HealthBarCanvas").GetComponent<HealthBarScript> ();
				healthBarScript.GetHit (15f);
			}
			i++;
		}
	}
}