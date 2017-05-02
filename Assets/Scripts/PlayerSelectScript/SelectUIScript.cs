using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.UI;

public class SelectUIScript : MonoBehaviour {
	public static SelectUIScript S;

	public GameObject xanderPlayerSelect;
	public GameObject bloodhunterPlayerSelect;
	public GameObject croakPlayerSelect;
	public Image playerJoin;
	public Image informationBox;

	public InputDevice Device { get; set; }

	void Awake () {
		S = this;
	}

	void Start () {
		xanderPlayerSelect = this.transform.FindChild ("XanderPlayerSelect").gameObject;
		bloodhunterPlayerSelect = this.transform.FindChild ("BloodhunterPlayerSelect").gameObject;
		croakPlayerSelect = this.transform.FindChild ("CroakPlayerSelect").gameObject;
		playerJoin = this.transform.FindChild ("PlayerJoin").GetComponent<Image>();
		informationBox = this.transform.FindChild ("InformationBox").GetComponent<Image>();
	}

	void Update () {
		if (Device != null) {
			print ("Player found!");
			playerJoin.enabled = false;	
			xanderPlayerSelect.SetActive (true);
		}
	}
}
