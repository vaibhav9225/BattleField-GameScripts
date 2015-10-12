using UnityEngine;
using System.Collections;

public class Jet : MonoBehaviour {
	
	public int forwardVelocity = 5;
	public int upwardVelocity = 5;
	public int rotationSpeed = 5;
	public GameObject horizontal = null;
	public GameObject vertical = null;
	
	private Rigidbody rb;
	private float forward = 0.5f;
	private float left = 0;
	private float right = 0;
	private float up = 0;
	private float down = 0;
	
	void Start(){
		rb = gameObject.GetComponent<Rigidbody>();
	}
	
	void FixedUpdate () {
		rb.velocity = transform.TransformDirection (new Vector3 (0, 0, -forwardVelocity * forward * Time.deltaTime));
		
		if (Input.GetKey (KeyCode.A)) {
			if(forward < 2) forward += Time.deltaTime;
		}
		if (Input.GetKey (KeyCode.D)) {
			if(forward > 0.1) forward -= Time.deltaTime;
		}
		
		if (Input.GetKey (KeyCode.LeftArrow)) {
			if (left < 1){
				left += Time.deltaTime;
				horizontal.transform.Rotate (new Vector3 (0, 0, -rotationSpeed * left * Time.deltaTime));
			}
			transform.Rotate (new Vector3 (0, -rotationSpeed * left * Time.deltaTime, 0));
		} else if (left > 0) {
			transform.Rotate (new Vector3 (0, -rotationSpeed * left * Time.deltaTime, 0));
			horizontal.transform.Rotate (new Vector3 (0, 0, rotationSpeed * left * Time.deltaTime));
			left -= Time.deltaTime;
		} else {
			horizontal.transform.Rotate (new Vector3 (0, 0, rotationSpeed * left * Time.deltaTime));
			left = 0;
		}
		
		if (Input.GetKey (KeyCode.RightArrow)) {
			if (right < 1){
				right += Time.deltaTime;
				horizontal.transform.Rotate (new Vector3 (0, 0, rotationSpeed * right * Time.deltaTime));
			}
			transform.Rotate (new Vector3 (0, rotationSpeed * right * Time.deltaTime, 0));
		} else if (right > 0) {
			transform.Rotate (new Vector3 (0, rotationSpeed * right * Time.deltaTime, 0));
			horizontal.transform.Rotate (new Vector3 (0, 0, -rotationSpeed * right * Time.deltaTime));
			right -= Time.deltaTime;
		} else {
			horizontal.transform.Rotate (new Vector3 (0, 0, -rotationSpeed * right * Time.deltaTime));
			right = 0;
		}
		
		if (Input.GetKey (KeyCode.UpArrow)) {
			if (up < 1){
				up += Time.deltaTime;
				vertical.transform.Rotate (new Vector3 (rotationSpeed * up/4 * Time.deltaTime, 0, 0));
			}
			rb.velocity = transform.TransformDirection (new Vector3 (0, upwardVelocity * Time.deltaTime * up, -forwardVelocity * Time.deltaTime * forward));
		} else if (up > 0) {
			vertical.transform.Rotate (new Vector3 (-rotationSpeed * up/4 * Time.deltaTime, 0, 0));
			rb.velocity = transform.TransformDirection (new Vector3 (0, upwardVelocity * Time.deltaTime * up, -forwardVelocity * Time.deltaTime * forward));
			up -= Time.deltaTime;
		} else {
			vertical.transform.Rotate (new Vector3 (-rotationSpeed * up/4 * Time.deltaTime, 0, 0));
			up = 0;
		}
		
		if (Input.GetKey (KeyCode.DownArrow)) {
			if (down < 1){
				down += Time.deltaTime;
				vertical.transform.Rotate (new Vector3 (-rotationSpeed * down/4 * Time.deltaTime, 0, 0));
			}
			rb.velocity = transform.TransformDirection (new Vector3 (0, -upwardVelocity * Time.deltaTime * down, -forwardVelocity * Time.deltaTime * forward));
		} else if (down > 0) {
			vertical.transform.Rotate (new Vector3 (rotationSpeed * down/4 * Time.deltaTime, 0, 0));
			rb.velocity = transform.TransformDirection (new Vector3 (0, -upwardVelocity * Time.deltaTime * down, -forwardVelocity * Time.deltaTime * forward));
			down -= Time.deltaTime;
		} else {
			vertical.transform.Rotate (new Vector3 (rotationSpeed * down/4 * Time.deltaTime, 0, 0));
			down = 0;
		}
	}
}