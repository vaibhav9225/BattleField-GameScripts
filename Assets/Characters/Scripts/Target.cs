using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour {

	public float health = 100;

	public void Damage(GunHit gunHit){
		health -= gunHit.damage;
		if (health <= 0) {
			this.gameObject.SetActive(false);
		}
	}
}