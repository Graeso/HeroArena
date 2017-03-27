using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour {

	private float lifeTime = 5f;

	void Update () {
		this.transform.Translate (0f, 0f, 5f * Time.deltaTime);
		lifeTime -= Time.deltaTime;

		if (lifeTime <= 0f) {
			Destroy (this.gameObject);
		}
	}

	void OnCollisionEnter (Collision col) {
		Explosion (Vector3.zero, 10, col);
		Destroy (this.gameObject);
	}

	void Explosion (Vector3 center, float radius, Collision col) {
		Collider[] hitColliders = Physics.OverlapSphere (center, radius);
		int i = 0;
		while (i < hitColliders.Length) {
			if (col.gameObject.tag == "Player") {
				// Damage the object
			}
			i++;
		}
	}
}
