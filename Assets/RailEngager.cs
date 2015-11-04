using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RailEngager : MonoBehaviour {
	public BezierSpline spline;
	public bool lookForward;
	private float side;
	public float duration;
//	public bool reverse;
	private float progress;
	public RailInstantiator rI;
	public SplineWalkerMode grindType;
	public OrbitalCharacterControllerLogic player;
	public bool goingForward;

	[SerializeField]
	private Animator animator;

	public GameObject[] segments;
	public RailComponent[] components;	

	private bool find = true;

	private int lastHead, lastTail, splineNumber;
	private int setState,lastState;
	private Vector3 playerFwd, segFwd;
	private float playerHeadingAngle, segHeadingAngle, difference;
	private Quaternion toRotation;

	private int current;
	public bool canGrind = false;
	[SerializeField]
	private float heightAboveRail;

	private float distance;
	[SerializeField]
	private Transform target;

	[SerializeField]
	private enum GrindState{
		Royal,
		PornStar,
		Soul,
		Unity
	}

	private GrindState grindState;

	private float speed;
	private float startTime;

	private float lerpPosition = 0.0f;
	private float lerpTime = 5.0f; 
	
	private void Start () {
		animator = GetComponent<Animator>();
		startTime = Time.time;

		segments = GameObject.FindGameObjectsWithTag ("Rail");
//		for (int i = 0; i < segments.Length; i++) {
//			components[i] = segments[i].GetComponent<RailComponent>();


//		
//		}
	}

	private void Update () {

		if (find) {
			FindSegments ();
		}
		if (player.playerMode != PlayerMode.Grind) {
			CheckForGrindableSurface ();
		}
//				Debug.Log("Player: "+ transform.localRotation.eulerAngles.magnitude);
//		Debug.Log ("Rail: " + segments [current].transform.localRotation.eulerAngles.magnitude);
//		Debug.Log (Mathf.Abs(Mathf.Abs(transform.localRotation.eulerAngles.magnitude) - Mathf.Abs(segments[current].transform.localRotation.eulerAngles.magnitude)));
//		Debug.Log ("Player: " + this.transform.forward);

//		Debug.Log ("Target: " + segments[current].transform.forward);
		float angleRight = 0.0f;
		float angleLeft = 0.0f;
		Vector3 heading = segments [current].transform.position - this.transform.position;
		Vector3 biasHeading = this.transform.position - segments [current].transform.position;
		angleLeft = Vector3.Angle (transform.right*-1, heading);
//		Debug.Log ("AngleLeft: " + angleLeft);

		angleRight = Vector3.Angle (transform.right, heading);
//		Debug.Log ("AngleRt: " + angleRight);

		side = AngleDir (segments [current].transform.forward, biasHeading, transform.up);

//		Debug.Log ("Side: "+side);

		playerFwd = this.transform.forward;
		playerFwd.y = 0;
		playerHeadingAngle = Quaternion.LookRotation (playerFwd).eulerAngles.y;
//		if (playerHeadingAngle > 180f) {
//			playerHeadingAngle -= 360f;
//		}
//		Debug.Log ("pHA: " + playerHeadingAngle);
		segFwd = segments[current].transform.forward;
		segFwd.y = 0;
		segHeadingAngle = Quaternion.LookRotation (segFwd).eulerAngles.y;
//		if (segHeadingAngle > 180f) {
//			segHeadingAngle -= 360f;
//		}
//		Debug.Log("sHA: "+segHeadingAngle);
		difference = Mathf.Abs (segHeadingAngle - playerHeadingAngle);
//		Debug.Log ("Difference: " + difference);

		if (canGrind && Input.GetButton ("XButton") && player.playerMode != PlayerMode.Grind) {
//			if(side >-1){
//				if(angleLeft>angleRight){
//					goingForward = true;
//				} else {
//					goingForward = false;
//				}
//			} else if(side == -1){
//				if(angleLeft<angleRight){
//					goingForward = true;
//				} else {
//					goingForward = false;
//				}
//			}
			if(difference<=90){
				goingForward = true;
			} else if(difference>90){
				goingForward = false;
			}



			target = segments[current].transform;
			SetGrindState();
			player.playerMode = PlayerMode.Grind;


//			player.grounded = true;
		}
		if (player.playerMode == PlayerMode.Grind) {
			if(Input.GetButtonDown ("XButton")){
				SetGrindState();
			}
			if(Input.GetButtonDown ("YButton")){
				SetGrindState();
				if(goingForward){
					goingForward = false;
				} else {
					goingForward = true;
				}
			}
			if(lookForward){
				toRotation = segments[current].transform.localRotation;

				if(!goingForward){
//				
					toRotation = Quaternion.LookRotation(-segments[current].transform.forward, Vector3.up);

				}
			}

			//
			this.transform.localRotation = Quaternion.Lerp( this.transform.localRotation, toRotation, Time.time* 0.5f);

			LerpGrind();
			SetTarget();
		}
	}

	float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up) {
		Vector3 perp = Vector3.Cross(fwd, targetDir);
		float dir = Vector3.Dot(perp, up);
		
		if (dir > 0f) {
			return 1f;
		} else if (dir < 0f) {
			return -1f;
		} else {
			return 0f;
		}
		
	}
	void SetGrindState(){
//		Debug.Log (System.Enum.GetValues (typeof(grindState)).Length);
//		int grindStates = System.Enum.GetValues (typeof(grindState)).Length - 1;

		setState = Random.Range(0,System.Enum.GetValues(typeof(GrindState)).Length);
		if (setState == lastState) {
			SetGrindState ();
		} else {
			switch (setState) {
			case 0:
				grindState = GrindState.Royal;
				animator.SetInteger ("grindState", setState);
				break;
			case 1:
				grindState = GrindState.PornStar;
				animator.SetInteger ("grindState", setState);
				break;
			case 2:
				grindState = GrindState.Soul;
				animator.SetInteger ("grindState", setState);
				break;
			case 3:
				grindState = GrindState.Unity;
				animator.SetInteger ("grindState", setState);
				break;
			}
			lastState = setState;
		}
	}

	void LerpGrind() {

		distance = Vector3.Distance(transform.position,target.position);


		speed = 1.0f;
		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = (distCovered / distance)*3.0f;
		lerpPosition += Time.deltaTime/lerpTime;
		this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, target.position+Vector3.up*heightAboveRail, fracJourney*lerpPosition);
	}

	void SetTarget() {


		if (goingForward) {
			if (segments [current].GetComponent<RailComponent> ().next != null) {
				Debug.Log(current);
				target = segments [current].GetComponent<RailComponent> ().next;
				spline = segments [current].GetComponent<RailComponent> ().rail;
//				if(spline.Loop && (segments[current].GetComponent<RailComponent>().head)){
//					lastHead = current;
////					Debug.Log("LH: "+lastHead);
//					current ++;
//				}else if(spline.Loop && (segments[current].GetComponent<RailComponent>().tail)){
//					lastTail = current;
//					current = lastHead;
//				}else{
				current = segments[current].GetComponent<RailComponent> ().next.GetComponent<RailComponent>().segNum;
//				}
			} else {
				Debug.Log("FEELS LIKE FALLING");
				player.playerMode = PlayerMode.Jump;
				GetComponent<Rigidbody>().AddForce(segments[current].transform.forward*(8000f));
				GetComponent<Rigidbody>().AddForce(this.transform.up*(9000f));

			}
		} else {
			if (segments [current].GetComponent<RailComponent> ().last != null) {
//				Debug.Log(current);
//				DeDebugbug.Log("LT: "+lastTail);
				target = segments [current].GetComponent<RailComponent> ().last;
				spline = segments [current].GetComponent<RailComponent> ().rail;
//				if(spline.Loop && (segments[current].GetComponent<RailComponent>().head)){
//					lastHead = current;
//					current = lastTail;
//				} else if(spline.Loop && (segments[current].GetComponent<RailComponent>().tail)){
//					Debug.Log("LT: "+lastTail);
//					lastTail = current;
//					current --;
//				} else{
//					Debug.Log("Still Happening");
				current = segments[current].GetComponent<RailComponent> ().last.GetComponent<RailComponent>().segNum;
//				}
			} else {
				Debug.Log("FEELS LIKE FALLING");
				player.playerMode = PlayerMode.Jump;
				GetComponent<Rigidbody>().AddForce(segments[current].transform.forward*(-8000f));
				GetComponent<Rigidbody>().AddForce(this.transform.up*(9000f));

			}
		}
	}


	void FindSegments () {
		segments = GameObject.FindGameObjectsWithTag ("Rail");
		int n = 0;
		foreach (GameObject seg in segments) {
			seg.GetComponent<RailComponent>().segNum = n;
			n++;
		}
//		FindComponents ();
		find = false;
		SetOrder();
	}

	void SetOrder (){
		lastHead = 0;


		splineNumber = 0;
		for (int i = 1; i < segments.Length-1; i++) {
//			spline = segments [current].GetComponent<RailComponent> ().rail;
			spline = segments[i].GetComponent<RailComponent>().rail;
//			Debug.Log (spline);
			segments[i].GetComponent<RailComponent>().railNumber = splineNumber;
			if(segments[i+1].GetComponent<RailComponent>().rail == segments[i].GetComponent<RailComponent>().rail){
				segments[i].GetComponent<RailComponent>().next = segments[i+1].GetComponent<RailComponent>().transform;
			} else {
				segments[i].GetComponent<RailComponent>().tail = true;
				segments[i].GetComponent<RailComponent>().GetComponent<Collider>().enabled = false;
				lastTail = i;
//				tails[splineNumber] = i;
				splineNumber++;
//				Debug.Log(spline.Loop);
				if(spline.Loop){
					Debug.Log("Loop me!");
					segments[i].GetComponent<RailComponent>().next = segments[lastHead].GetComponent<RailComponent>().transform;
					segments[lastHead].GetComponent<RailComponent>().last = segments[i].GetComponent<RailComponent>().transform;
				}
				segments[i+1].GetComponent<RailComponent>().head = true;
				segments[i+1].GetComponent<RailComponent>().GetComponent<Collider>().enabled = false;
				lastHead = i+1;
//				Debug.Log("LH: "+lastHead);
//				heads[splineNumber] = i+1;

			}
			if(segments[i-1].GetComponent<RailComponent>().rail == segments[i].GetComponent<RailComponent>().rail){
				segments[i].GetComponent<RailComponent>().last = segments[i-1].GetComponent<RailComponent>().transform;
			}

		}

		segments[0].GetComponent<RailComponent>().next = segments[1].GetComponent<RailComponent>().transform;
		segments[0].GetComponent<RailComponent>().head = true;
		segments[0].GetComponent<RailComponent>().GetComponent<Collider>().enabled = false;
		segments [0].GetComponent<RailComponent> ().railNumber = 0;
		segments[segments.Length-1].GetComponent<RailComponent>().tail = true;
		segments[segments.Length-1].GetComponent<RailComponent>().GetComponent<Collider>().enabled = false;
		segments[segments.Length-1].GetComponent<RailComponent>().last = segments[segments.Length - 2].GetComponent<RailComponent>().transform;
		segments[segments.Length-1].GetComponent<RailComponent>().railNumber = splineNumber;
		spline = segments [segments.Length - 1].GetComponent<RailComponent> ().rail;
		if (spline.Loop) {
			segments[segments.Length - 1].GetComponent<RailComponent>().next = segments[lastHead].GetComponent<RailComponent>().transform;
			segments[lastHead].GetComponent<RailComponent>().last = segments[segments.Length - 1].GetComponent<RailComponent>().transform;
		}
//		Debug.Log (segments [segments.Length - 1].GetComponent<RailComponent> ().next);
//		segments[0].GetComponent<RailComponent>().last = segments[segments.Length].GetComponent<RailComponent>().transform;
	}
	void CheckForGrindableSurface(){
		for (int i = 0; i < segments.Length; i++) {
			if(segments[i].GetComponent<RailComponent>().grindable==true){
				canGrind = true; 

				current = i;

				break;
			} else {
				canGrind = false;
			}
		}
	}
//	void FindComponents(){
//		for (int i = 0; i < segments.Length; i++) {
//			components[i] = segments[i].GetComponent<RailComponent>();
//		}
////		components[0] = segments[0].GetComponent<RailComponent>();
//		Debug.Log("Length: " + segments[0].GetComponent<RailComponent>());
//		find = false;
//	}
}
//private void BezierGrind () {
//	if(player.playerMode == PlayerMode.Grind){
//		if(spline.Loop){
//			grindType = SplineWalkerMode.Loop;
//		} 
//		else{
//			grindType = SplineWalkerMode.Once;
//		}
//		if (goingForward) {
//			progress += Time.deltaTime / duration;
//			if(progress >1){
//				if (grindType == SplineWalkerMode.Once) {
//					progress = 1f;
//				}
//				else if (grindType == SplineWalkerMode.Loop) {
//					progress -= 1f;
//				}
//				else if (grindType == SplineWalkerMode.Pendulum){
//					goingForward = false;
//				}
//			}
//		}
//		
//		if(!goingForward){
//			progress -= Time.deltaTime / duration;
//			if(progress <= 0){
//				if (grindType == SplineWalkerMode.Once) {
//					progress = 1f;
//				}
//				else if (grindType == SplineWalkerMode.Loop) {
//					progress = 1f;
//				}
//				else if (grindType == SplineWalkerMode.Pendulum){
//					goingForward = true;
//				}
//			}
//		}
//		
//		Vector3 position = spline.GetPoint(progress)+Vector3.up*heightAboveRail;
//		//			Debug.Log(spline.GetPoint(progress));
//		transform.localPosition = position;
//		if(lookForward){
//			if(goingForward){
//				transform.LookAt(position + spline.GetDirection (progress));
//			}
//			if(!goingForward){
//				transform.LookAt(position - spline.GetDirection (progress));
//			}
//		}
//	}
//}
