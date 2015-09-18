using UnityEngine;
using System.Collections;

struct CameraPosition{
	private Vector3 position;
	private Transform xForm;
	public Vector3 Position { get{return position;} set {position = value;}}
	public Transform XForm{ get{return xForm;} set {xForm = value;}}

	public void Init(string camName, Vector3 pos, Transform transform, Transform parent){
		position = pos;
		xForm = transform;
		xForm.name = camName;
		xForm.parent = parent;
		xForm.localPosition = Vector3.zero;
		xForm.localPosition = position;
	}
}

//[RequireComponent(typeof(BarsEffect))]
public class ThirdPersonOrbitalCamera : MonoBehaviour {
	#region Variables (private)
	
	[SerializeField]
	private float distance;
	[SerializeField]
	private float height;
	[SerializeField]
	private float speed;
	[SerializeField]
	private Transform followTarget;
	[SerializeField]
	private OrbitalCharacterControllerLogic follow;
	[SerializeField]
	private Vector3 offset = new Vector3(0f, 1.5f, 0f);
	[SerializeField]
	public float widescreen = 0.2f;
	[SerializeField]
	public float targetingTime = 0.5f;
	[SerializeField]
	private float firstPersonThreshold = 0.5f;
	[SerializeField]
	private Vector2 firstPersonXAxisClamp = new Vector2(-60.0f, 75.0f);
	[SerializeField]
	private float firstPersonLookSpeed = 1.5f;
	[SerializeField]
	private float fPSRotationDegreePerSecond = 120f;

	
	private Vector3 velocityCamSmooth = Vector3.zero;
	[SerializeField]
	private float camSmoothDampTime = 0.1f;
	private Vector3 velocityLookDir = Vector3.zero;
	[SerializeField]
	private float lookDirDampTime = 0.1f;

	private Vector3 lookDir;
	private Vector3 curLookDir;
	private Vector3 targetPosition;
	//private BarsEffect barEffect;
	[SerializeField]
	private Texture wSBars;
	private GameObject fade;
	public CamStates camState = CamStates.Behind;
	private float xAxisRot = 0.0f;
	private CameraPosition firstPersonCamPos;
	private float lookWeight;
	private const float targettingThreshold = 0.01f;
	[SerializeField]
//	private Vector2 hideBars;
//	private Texture2D fadeTexture = new Texture2D(1,1);

    #endregion
	
	#region Properties (public)
	public enum CamStates{
		Behind,
		FirstPerson,
		Target,
		Free
	}
	#endregion
	
	#region Unity event functions
	// Use this for initialization
	/*void Awake(){
		var fadeTexture = new Texture2D(1,1);
		fadeTexture.Resize(400,400);
		fadeTexture.SetPixel(0,0,Color.black);
		fadeTexture.Apply();

		fade = new GameObject("Effect");
		fade.AddComponent<GUITexture>();
		fade.transform.position = new Vector3(0.5f, 0.5f, 1000f);
		fade.guiTexture.texture = wSBars;
		fade.guiTexture.texture.height=200;
		//wSBars.enabled = false;
	}*/

	void Start () {
		follow = GameObject.FindWithTag ("Player").GetComponent<OrbitalCharacterControllerLogic>();
		followTarget = GameObject.FindWithTag ("camTarget").transform;
		lookDir = followTarget.forward;
		curLookDir = followTarget.forward;

	/*	barEffect = GetComponent<BarsEffect>();
		if(barEffect == null){
			Debug.LogError("Attach a widescreen BarsEffect script to the camera.", this);
		}*/
		firstPersonCamPos = new CameraPosition();
		firstPersonCamPos.Init(
			"First Person Camera",
			new Vector3(0.0f, 0.004f, 0.005f),
			new GameObject().transform,
			followTarget
		);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnDrawGizmos () {
	}
	void LateUpdate () {
		//Debug.Log(follow.Speed);
		float rightX = Input.GetAxis("RightStickX");
		float rightY = Input.GetAxis("RightStickY");
		float leftX = Input.GetAxis("Horizontal");
		float leftY = Input.GetAxis("Vertical");

		//Debug.Log(Input.GetAxis("Triggers"));
		//Vector3 characterOffset = followTarget.position + offset;
		Vector3 characterOffset = followTarget.position + new Vector3(0f,height, 0f);
		Vector3 lookAt = characterOffset;

		if(Input.GetAxis("Triggers")>targettingThreshold){
			//barEffect.coverage = Mathf.SmoothStep(barEffect.coverage, widescreen, targetingTime);
			camState = CamStates.Target;
		}else{
		//barEffect.coverage = Mathf.SmoothStep(barEffect.coverage, 0.0f, targetingTime);
			if(rightY > firstPersonThreshold && /*camState != CamStates.Free &&*/ !follow.IsInLocomotion()){
				xAxisRot = 0;
				lookWeight = 0f;
				camState = CamStates.FirstPerson;
			}
			if((camState == CamStates.FirstPerson && Input.GetButton("BButton")) ||
			   (camState == CamStates.Target && (Input.GetAxis("Triggers")<=targettingThreshold))){

			camState = CamStates.Behind;
			}
		}
		follow.Animator.SetLookAtWeight(lookWeight);
		switch(camState){
		case CamStates.Behind:
			ResetCamera();
			if(follow.Speed >follow.LocomotionThreshold && follow.IsInLocomotion() && !follow.IsInPivot()){
				lookDir = Vector3.Lerp(followTarget.right * (leftX < 0 ? 1f : -1f), followTarget.forward * (leftY < 0 ? -1f : 1f), Mathf.Abs(Vector3.Dot(this.transform.forward,followTarget.forward)));
				Debug.DrawRay(this.transform.position, lookDir, Color.white);
				
				//Calculate direction from camera to player, kill Y, and normalize to give a valid direction with unit magnitude
				curLookDir = Vector3.Normalize(characterOffset - this.transform.position);
				curLookDir.y = 0;
				Debug.DrawRay(this.transform.position, curLookDir, Color.green);
				//Damping makes it so we don't update target position while pivoting; camera shouldn't rotate around player
				curLookDir = Vector3.SmoothDamp(curLookDir, lookDir, ref velocityLookDir, lookDirDampTime);
				
			}
//			targetPosition = characterOffset + followTarget.up * height - lookDir * distance;
			targetPosition = characterOffset + followTarget.up * height - Vector3.Normalize(curLookDir) * distance;
		//	Debug.DrawRay(followTarget.position, followTarget.up *height, Color.red);
		//	Debug.DrawRay(followTarget.position, -1f * followTarget.forward * distance, Color.blue);
			Debug.DrawLine(followTarget.position, targetPosition, Color.magenta);

			break;
		case CamStates.Target:

			ResetCamera();
//			fadeTexture.Resize(1,1);
			lookDir = followTarget.forward;
			curLookDir = lookDir;
			targetPosition = characterOffset + followTarget.up * height - lookDir *distance;
			//this.transform.position = targetPosition;
			break;
		case CamStates.FirstPerson:
			//Calculate the amount of Rotation to firstPersonCamPos
			xAxisRot += (leftY * firstPersonLookSpeed);
			xAxisRot = Mathf.Clamp(xAxisRot, firstPersonXAxisClamp.x, firstPersonXAxisClamp.y);
			firstPersonCamPos.XForm.localRotation = Quaternion.Euler(xAxisRot, 0, 0);

			//Superimpose firstPersonCamPos rotation onto camera
			Quaternion rotationShift = Quaternion.FromToRotation(this.transform.forward, firstPersonCamPos.XForm.forward);
			this.transform.rotation = rotationShift * this.transform.rotation;

			//Choose lookAt target based on distance

			follow.Animator.SetLookAtPosition(firstPersonCamPos.XForm.position + firstPersonCamPos.XForm.forward);
			lookWeight = Mathf.Lerp (lookWeight, 1.0f, Time.deltaTime * firstPersonLookSpeed);

			Vector3 rotationAmount = Vector3.Lerp(Vector3.zero, new Vector3(0f, fPSRotationDegreePerSecond * (leftX <0f ? -1f : 1f), 0f), Mathf.Abs(leftX));
			Quaternion deltaRotation = Quaternion.Euler(rotationAmount *Time.deltaTime);
			follow.transform.rotation = (follow.transform.rotation * deltaRotation);

			targetPosition = firstPersonCamPos.XForm.position;

			lookAt = Vector3.Lerp(targetPosition + followTarget.forward, this.transform.forward, camSmoothDampTime * Time.deltaTime);


			lookAt = (Vector3.Lerp(this.transform.position + this.transform.forward, lookAt, Vector3.Distance(this.transform.position, firstPersonCamPos.XForm.position)));
			break;
		}

		WallCollision(characterOffset, ref targetPosition);
		smoothPosition(this.transform.position, targetPosition);
		transform.LookAt(lookAt);
	}
	private void smoothPosition(Vector3 fromPos, Vector3 toPos){
	
		this.transform.position = Vector3.SmoothDamp(fromPos, toPos, ref velocityCamSmooth, camSmoothDampTime);
	}
	private void WallCollision(Vector3 fromObject, ref Vector3 toTarget){
		Debug.DrawRay(fromObject, toTarget, Color.cyan);
		RaycastHit wallHit = new RaycastHit();
		if(Physics.Linecast(fromObject, toTarget, out wallHit)){
			Debug.DrawRay(wallHit.point, Vector3.left, Color.red);
			toTarget = new Vector3(wallHit.point.x, fromObject.y, wallHit.point.z);
		}
	}
	private void ResetCamera(){
//		Debug.Log("RESET");
		lookWeight = Mathf.Lerp (lookWeight, 0.0f, Time.deltaTime * firstPersonLookSpeed);
		transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, Time.deltaTime);
	}

	#endregion
}
