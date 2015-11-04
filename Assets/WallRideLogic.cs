using UnityEngine;
using System.Collections;

public class WallRideLogic : MonoBehaviour {
	[SerializeField]
	private bool running;
	[SerializeField]
	private enum RunSide{
		Right,
		Left
	}
	[SerializeField]
	private Transform leftWallCheck;
	[SerializeField]
	private Transform rightWallCheck;
	[SerializeField]
	private Transform checkRef;

	[SerializeField]
	private float wLead, wProx, wHeight;
	private Vector3 toPos;
	[SerializeField]
	private float speed;
	private float startTime;
	
	private float lerpPosition = 0.0f;
	private float lerpTime = 5.0f; 
	private float distance;
	

	// Use this for initialization
	void Start () {
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
//		Ray ray = new Ray (Camera.main.transform.position, Camera.main.transform.forward);

		Vector3 checkPoint = checkRef.position;
		checkPoint.z -= wLead;
		checkPoint.y += wHeight;
		Debug.DrawRay (checkPoint, checkRef.right * wProx, Color.black);
		
		//		lookPos.position = ray.GetPoint (15);
		
		RaycastHit leftHit;
		RaycastHit rightHit;

		if (Physics.Raycast (checkPoint, checkRef.right, out rightHit, wProx)) {
			toPos = rightHit.point;
			Debug.Log(toPos);
		}
		if (Input.GetButton ("XButton")) {
			running = true;
		}
		if (running) {
			transform.rotation = Quaternion.FromToRotation(transform.up, rightHit.normal) * transform.rotation;
			WallRun();
		}
		if (this.transform.position == toPos) {
			running = false;
		}
	}
	void WallRun(){
		distance = Vector3.Distance(transform.position,toPos);


		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = (distCovered / distance)*3.0f;
		lerpPosition += Time.deltaTime/lerpTime;
		this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, toPos, fracJourney*lerpPosition);
	}
}
