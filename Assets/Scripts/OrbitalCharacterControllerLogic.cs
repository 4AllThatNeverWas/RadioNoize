using UnityEngine;
using System.Collections;

public class OrbitalCharacterControllerLogic : MonoBehaviour {
	#region variables (private)
	
	[SerializeField]
	private Animator animator;
	[SerializeField]
	private float directionDampTime = 0.25f;
	[SerializeField]
	private ThirdPersonOrbitalCamera gameCam;
	[SerializeField]
	private float directionSpeed = 3.0f;
	[SerializeField]
	private float rotationDegreesPerSecond = 120.0f;
	[SerializeField]
	private float speedDampTime = 0.05f;

	[SerializeField]
	private float colIdle, colRun, colJump;
	

	public bool grounded = false;
	public float speed = 0.0f;
	private float direction = 0.0f;
	private float charAngle = 0.0f;
	private float horizontal = 0.0f;
	private float vertical = 0.0f;

	public float velocity;

	private AnimatorStateInfo stateInfo;
	private AnimatorTransitionInfo transInfo;

	private int m_LocomotionID = 0;
	private int m_LocomotionPivotLID = 0;
	private int m_LocomotionPivotRID = 0;
	private int m_LocomotionTransRID = 0;
	private int m_LocomotionTransLID = 0;

	public Vector3 forwardVector;
	
	public Animator Animator{
		get{
			return this.animator;
		}
	}

	public float Speed {
		get {
			return speed;
		}
	}
	public float LocomotionThreshold{
		get{
			return 0.2f;
		}
	}

	#endregion
	public float runSpeed = 0.0f;
	public bool applyRootMotion;
	public float maxVelocity;

	public bool goingForward;
	
	public PlayerMode playerMode;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		if(animator.layerCount >=2){
			animator.SetLayerWeight(1,1);
		}
		m_LocomotionID = Animator.StringToHash("Base Layer.Locomotion");
		m_LocomotionPivotLID = Animator.StringToHash("Base Layer.LocomotionPivotL");
		m_LocomotionPivotRID = Animator.StringToHash("Base Layer.LocomotionPivotR");
		m_LocomotionTransRID = Animator.StringToHash("Base Layer.Locomotion -> LocomotionPivotR");
		m_LocomotionTransLID = Animator.StringToHash("Base Layer.Locomotion -> LocomotionPivotL");
	}
	
	// Update is called once per frame
	void Update () {
		velocity = this.GetComponent<Rigidbody> ().velocity.magnitude;
//		Debug.Log("H:"+horizontal);
//		Debug.Log("V:"+vertical);

		if (playerMode == PlayerMode.Jump) {
			animator.applyRootMotion = false;
			GetComponent<CapsuleCollider> ().height = colJump;
		} else if(playerMode == PlayerMode.Grind){
			animator.applyRootMotion = false;
		} else {
			animator.applyRootMotion = true;
			GetComponent<CapsuleCollider>().height = colIdle;
		}
		if (playerMode == PlayerMode.Grind) {
			animator.SetBool ("grind", true);

		} else {

			animator.SetBool ("grind", false);
		}
		if (playerMode == PlayerMode.Skate) {
			if (animator && gameCam.camState != ThirdPersonOrbitalCamera.CamStates.FirstPerson/* && grounded*/) {
				stateInfo = animator.GetCurrentAnimatorStateInfo (0);
				transInfo = animator.GetAnimatorTransitionInfo (0);

				horizontal = Input.GetAxis ("Horizontal");
				vertical = Input.GetAxis ("Vertical");

//				Debug.Log ("H: " + horizontal);
//				Debug.Log ("V: " + vertical);

				charAngle = 0f;
				direction = 0f;

				StickToWorldSpace (this.transform, gameCam.transform, ref direction, ref speed, ref charAngle, IsInPivot ());
				//			if(grounded){
				animator.SetFloat ("speed", speed, speedDampTime, Time.deltaTime);
				//			}
				animator.SetFloat ("direction", direction, directionDampTime, Time.deltaTime);
				animator.SetFloat ("tilt", vertical, directionDampTime, Time.deltaTime);
				/*if((horizontal<= 0.5 && horizontal>= -0.5)&&(vertical<= 0.5 && vertical>= -0.5)){
					animator.applyRootMotion=false;
					applyRootMotion=false;
				}else*/
				if (horizontal > 0.5 || horizontal < -0.5 || vertical > 0.5 || vertical < -0.5 && grounded/* && IsInLocomotion()*/) {
					//				animator.applyRootMotion=true; 
					if (IsInLocomotion () && Input.GetButton ("AButton")) {
						//animator.applyRootMotion=false;
					}
					//applyRootMotion=true;
				} else {
					//				animator.applyRootMotion=false;
				}
				if (IsInLocomotion () && Input.GetButton ("AButton")) {
					//				animator.applyRootMotion=false;
				}
				/*if(!grounded){
					animator.applyRootMotion=false;
				}*/
				if (speed > LocomotionThreshold) {
					if (!IsInPivot ()) {
						animator.SetFloat ("angle", charAngle);
					}
				}
				if (speed < LocomotionThreshold && Mathf.Abs (horizontal) < 0.05f) {
					animator.SetFloat ("direction", 0f);
					animator.SetFloat ("angle", charAngle);

				}
				//			Debug.Log(IsInPivot());
				if (Input.GetButtonDown ("AButton")) {
					animator.SetBool ("jump", true);
					//				animator.applyRootMotion=true;
					grounded = false;
					//				rigidbody.AddForce(new Vector3(0f,60000f,0f));
					//				Debug.Log("The Button Works.");
				}
				//			Debug.Log(rigidbody.velocity.y);
				/*if(Input.GetButtonUp("AButton")){
					animator.SetBool("jump", false);
					animator.applyRootMotion=false;
				}*/

			}
		}
		//runSpeed = new Vector3(horizontal*speed,0,vertical*speed);

		/*rigidbody.AddForce(horizontal*runSpeed,0,-1*vertical*runSpeed);
		if(rigidbody.velocity.magnitude>maxVelocity){
			rigidbody.velocity = rigidbody.velocity.normalized*maxVelocity;

		}*/
	}
	void FixedUpdate(){
	
		if(IsInLocomotion() && ((direction >= 0 && horizontal >=0) || (direction <0 && horizontal <0))){
			Vector3 rotationAmount = Vector3.Lerp(Vector3.zero, new Vector3(0f, rotationDegreesPerSecond *(horizontal < 0f ? -1f : 1f), 0f), Mathf.Abs(horizontal));
			Quaternion deltaRotation = Quaternion.Euler(rotationAmount *Time.deltaTime);
			this.transform.rotation = (this.transform.rotation * deltaRotation);

		}
		/*if(IsInJump()){
			float oldY = transform.position.y;
			transform.Translate(Vector3.up*jumpMultiplyer*animator.GetFloat("JumpCurve"));
			if(IsInLocomotionJump()){
				transform.Translate(Vector3.forward *Time.deltaTime*jumpDist);
			}
			capCollider.height = capSuleHeight + (animator.GetFloat("CapsuleCurve")*0.5f);
			if(gameCam.camState != ThirdPersonOrbitalCamera.CamStates.Free){
				gameCam.transform.Translate(Vector3.up*(transform.position.y - oldY));
			}
		}*/
	}

	public void StickToWorldSpace(Transform root, Transform camera, ref float directionOut, ref float speedOut, ref float angleOut, bool isPivoting)
	{
		Vector3 rootDirection = root.forward;
		Vector3 stickDirection = new Vector3(horizontal, 0, vertical*-1);
//		Debug.Log ("SD: " + stickDirection);
		speedOut = stickDirection.sqrMagnitude;

		//GetCamRotation
		Vector3 CameraDirection = camera.forward;
		CameraDirection.y =0.0f; //kill Y
		Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, CameraDirection);
		
		//Convert jostick input in Worldspace coordinates
		Vector3 moveDirection = referentialShift * stickDirection;
		Vector3 axisSign = Vector3.Cross(moveDirection, rootDirection);

	 	Debug.DrawRay(new Vector3(root.position.x, root.position.y +2f, root.position.z), moveDirection, Color.green);
		Debug.DrawRay(new Vector3(root.position.x, root.position.y +2f, root.position.z), axisSign, Color.black);
		Debug.DrawRay(new Vector3(root.position.x, root.position.y +2f, root.position.z), rootDirection, Color.magenta);
		Debug.DrawRay(new Vector3(root.position.x, root.position.y +2f, root.position.z), stickDirection, Color.blue);

		float angleRootToMove = Vector3.Angle(rootDirection, moveDirection) * (axisSign.y>=0 ? -1f : 1f);

		forwardVector = rootDirection;


		if(!isPivoting){
			angleOut = angleRootToMove;
		}

		angleRootToMove /= 180f;
		/*if(moveDirection == (0,0,0) || rootDirection == (0,0,0)){
			angleRootToMove = 0f;
		}*/
		directionOut = angleRootToMove * directionSpeed;
		//Debug.Log(rootDirection.magnitude+" working");
	}
	public bool IsInPivot(){
		return (stateInfo.nameHash == m_LocomotionPivotLID || 
		        stateInfo.nameHash == m_LocomotionPivotRID ||
		        transInfo.nameHash == m_LocomotionTransRID ||
		        transInfo.nameHash == m_LocomotionTransLID);
	}
	public bool IsInLocomotion(){
		return stateInfo.nameHash == m_LocomotionID;
	}
//	void OnCollisionStay(Collision collision) {
//		if(Physics.Raycast(transform.position,Vector3.down,5) && collision.gameObject.tag=="level" ){
////			Debug.Log("It's Down Here");
////			if(collision.gameObject.tag=="level"){
////				Debug.Log("We're touching");
//				grounded = true;
//				animator.SetBool("jump", false);
//			
////			}
//		} else{
//			grounded = false;
//		}
//	}
/*	void OnCollisionExit(Collision collision) {
		if(Physics.Raycast(transform.position,Vector3.down,5)){
			//Debug.Log("It's Down Here");
			if(collision.gameObject.tag=="level"){
				Debug.Log("Amy Can Flyy");
				grounded = false;
			}
		} /*else{
			grounded = false;
		}
	}*/
}
	