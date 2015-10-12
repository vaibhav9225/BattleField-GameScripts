using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

	public Transform physicalCrosshair;
	public Camera mainCamera;
	public Texture crosshairWhite;
	public Texture crosshairRed;
	public Texture crosshairAim;
	public Texture radar;

	private Rect crossHairRect;
	private Texture crossHairTexture;
	private Rect crossHairAimRect;
	private Vector3 screenPos;
	private Vector3 tempScreenPos;
	private Vector3 aimScreenPos;
	private GameObject[] players;
	private float crossHairRadius;
	private float hitDistance;

	void Start () {
		OffRadar ();
		GetPlayers ();
		screenPos = mainCamera.WorldToScreenPoint (transform.position);
		tempScreenPos = Vector3.one;
		crossHairRadius = Screen.height / 4;
		Bullets bullets = mainCamera.GetComponent<Bullets> ();
		hitDistance = bullets.hitDistance;
		float crossHairSize = Screen.width * 0.04f;
		crossHairAimRect = new Rect (-1000, -1000, crossHairSize, crossHairSize);
	}

	void OnGUI () {
		GUI.DrawTexture (new Rect(Screen.width/2 - Screen.height/4, Screen.height/5, Screen.height/2, Screen.height/2), radar);
		GUI.DrawTexture (crossHairRect, crossHairTexture);
		GUI.DrawTexture (crossHairAimRect, crosshairAim);
	}

	public void OffRadar(){
		crossHairTexture = crosshairWhite;
		DrawCrosshair (0.04f, crossHairTexture);
	}

	public void OnRadar(){
		crossHairTexture = crosshairRed;
		DrawCrosshair (0.05f, crossHairTexture);
	}

	void DrawCrosshair(float sizeMultiplier, Texture crossHairTexture){
		float crossHairSize = Screen.width * sizeMultiplier;
		screenPos = mainCamera.WorldToScreenPoint (physicalCrosshair.position);
		float posX = screenPos.x - crossHairSize / 2;
		float posY = Screen.height - screenPos.y - crossHairSize / 2;
		RaycastHit hit;
		if (Physics.Raycast (mainCamera.ScreenPointToRay (screenPos), out hit, hitDistance)) {
			FindTarget (true);
		} else {
			FindTarget(false);
		}
		if (!aimScreenPos.Equals (Vector3.one)) {
			float aimX = aimScreenPos.x - crossHairSize / 2;
			float aimY = Screen.height - aimScreenPos.y - crossHairSize / 2;
			crossHairAimRect = new Rect (aimX, aimY, crossHairSize, crossHairSize);
		} else {
			crossHairAimRect = new Rect (-1000, -1000, crossHairSize, crossHairSize);
		}
		crossHairRect = new Rect (posX, posY, crossHairSize, crossHairSize);
	}

	public Vector3 getScreenPos(){
		return screenPos;
	}

	void GetPlayers(){
		players = GameObject.FindGameObjectsWithTag ("Player");
	}

	void FindTarget(bool isHitting) {
		bool found = false;
		if (players != null) {
			for (int i=0; i<players.Length; i++) {
				if (players [i].GetComponent<Renderer> ().isVisible) {
					Vector3 position = players [i].transform.position;
					Vector3 objScreenPos = mainCamera.WorldToScreenPoint (position);
					if (objScreenPos.y > screenPos.y - Screen.height/20) {
						float distance = Vector2.Distance (new Vector2 (screenPos.x, screenPos.y), new Vector2 (objScreenPos.x, objScreenPos.y));
						if (distance < crossHairRadius) {
							aimScreenPos = objScreenPos;
							if(!isHitting){
								tempScreenPos = screenPos;
								screenPos = objScreenPos;
							}
							found = true;
							break;
						}
					}
				}
			}
			if (!found && players.Length > 0) {
				if(!tempScreenPos.Equals(Vector3.one)) screenPos = tempScreenPos;
				tempScreenPos = Vector3.one;
				aimScreenPos = Vector3.one;
			}
		}
	}
}