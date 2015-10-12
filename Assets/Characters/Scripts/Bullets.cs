using UnityEngine;
using System.Collections;

public class Bullets : MonoBehaviour {

	public float bulletDelay = 0.5f;
	public float damage = 5;
	public float hitDistance = 10000;
	public AudioClip bulletShot = null;
	public GameObject bulletHole = null;
	public GameObject fire = null;

	private bool readyToFire = true;
	private float timeElapsed = 0;
	private Vector3 screenPos;
	private Camera mainCamera;

	void Start () {
		mainCamera = this.gameObject.GetComponent<Camera> ();
	}

	void Update () {
		UpdateCrosshair ();
		Crosshair crosshair = this.gameObject.GetComponentInParent<Crosshair> ();
		screenPos = crosshair.getScreenPos ();
		if (Input.GetKey (KeyCode.Space) && readyToFire) {
			RaycastHit hit;
			if(Physics.Raycast(mainCamera.ScreenPointToRay(screenPos), out hit, hitDistance)){
				GunHit gunHit = new GunHit();
				gunHit.damage = damage;
				gunHit.raycastHit = hit;
				if(hit.collider.gameObject.GetComponent<Target>() != null){
					GameObject hole = Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
					hole.transform.parent = hit.collider.gameObject.transform;
					hit.collider.SendMessage("Damage", gunHit, SendMessageOptions.DontRequireReceiver);
				}
				else if(hit.collider.gameObject.GetComponentInParent<Target>() != null){
					Target target = hit.collider.gameObject.GetComponentInParent<Target>();
					target.Damage(gunHit);
				}
				else{
					GameObject hole = Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
					hole.transform.parent = hit.collider.gameObject.transform;
					Destroy(hole,5);
				}
			}
			AudioSource shot = gameObject.GetComponent<AudioSource>();
			shot.PlayOneShot(bulletShot, 1);
			fire.SetActive(true);
			readyToFire = false;
		}
		if (!readyToFire) {
			timeElapsed += Time.deltaTime;
			if(timeElapsed > bulletDelay){
				fire.SetActive(false);
				timeElapsed = 0;
				readyToFire = true;
			}
		}
	}

	void UpdateCrosshair(){
		RaycastHit hit;
		Crosshair crossHair = this.gameObject.GetComponentInParent<Crosshair>();
		if (Physics.Raycast (mainCamera.ScreenPointToRay (screenPos), out hit, hitDistance)) {
			if (hit.collider.gameObject.GetComponent<Target> () != null || hit.collider.gameObject.GetComponentInParent<Target> () != null) {
				crossHair.OnRadar ();
			} else {
				crossHair.OffRadar ();
			}
		} 
		else crossHair.OffRadar ();
	}
}

public class GunHit{
	public float damage;
	public RaycastHit raycastHit;
}