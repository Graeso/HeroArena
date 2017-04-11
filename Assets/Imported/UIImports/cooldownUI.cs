using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cooldownUI : MonoBehaviour {

	public List<Skill> skill;

	public void FixedUpdates(){
		if(Input.GetKeyDown(KeyCode.Alpha1)){

			if (skill[0].currentCoolDown >= skill[0].cooldown) {
				skill [0].currentCoolDown = 0;
				
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)){

			if (skill[1].currentCoolDown >= skill[1].cooldown) {
				skill [1].currentCoolDown = 0;

			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)){

			if (skill[2].currentCoolDown >= skill[2].cooldown) {
				skill [2].currentCoolDown = 0;

			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha4)){

			if (skill[3].currentCoolDown >= skill[3].cooldown) {
				skill [3].currentCoolDown = 0;

			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha5)){

			if (skill[4].currentCoolDown >= skill[4].cooldown) {
				skill [4].currentCoolDown = 0;

			}
		}if(Input.GetKeyDown(KeyCode.Alpha6)){

			if (skill[5].currentCoolDown >= skill[5].cooldown) {
				skill [5].currentCoolDown = 0;

			}
		}if(Input.GetKeyDown(KeyCode.Alpha7)){

			if (skill[6].currentCoolDown >= skill[6].cooldown) {
				skill [6].currentCoolDown = 0;

			}
		}if(Input.GetKeyDown(KeyCode.Alpha8)){

			if (skill[7].currentCoolDown >= skill[7].cooldown) {
				skill [7].currentCoolDown = 0;

			}
		}
			
	}

	void Update(){
		foreach (Skill s in skill) {
			if (s.currentCoolDown < s.cooldown) {
				s.currentCoolDown += Time.deltaTime;
				s.skillIcon.fillAmount = s.currentCoolDown/s.cooldown;
			}
		
		}

		
	
	}
}
[System.Serializable]
public class Skill
{
	public float cooldown;
	public Image skillIcon;
	[HideInInspector]
	public float currentCoolDown;

}