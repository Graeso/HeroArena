using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour {

	public Animation anim;
	public bool selected = false;

	void Start () {
		anim = GetComponent<Animation> ();
	}

	void Update () {
		if ((selected == false) && (this.name == "XanderPlayerSelect")) {
			anim.Play ("Idle");
		}
		if ((selected == false) && (this.name == "BloodhunterPlayerSelect")) {
			anim.Play ("Nothing Idle");
		}
	}

}
