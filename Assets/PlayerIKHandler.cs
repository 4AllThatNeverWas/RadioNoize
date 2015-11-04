using UnityEngine;
using System.Collections;

public class PlayerIKHandler : MonoBehaviour {

	private Animator animator;

	private float iKWeight = 1;

	public Transform leftIKTarget; 
	public Transform rightIKTarget;

	public Transform leftIKHint; 
	public Transform rightIKHint;

	private Vector3 lFPos; 
	private Vector3 rFPos;

	private Quaternion lFRot; 
	private Quaternion rFRot;

	private float lFWeight; 
	private float rFWeight;

	private Transform leftFoot;
	private Transform rightFoot;

	[SerializeField]
	private float offsetY;

	[SerializeField]
	private float lookIKWeight;
	[SerializeField]
	private float bodyWeight;
	[SerializeField]
	private float headWeight;
	[SerializeField]
	private float eyeWeight;
	[SerializeField]
	private float clampWeight;

	public Transform lookPos;



	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();

		leftFoot = animator.GetBoneTransform (HumanBodyBones.LeftFoot);
		rightFoot = animator.GetBoneTransform (HumanBodyBones.RightFoot);

		lFRot = leftFoot.rotation;
		rFRot = rightFoot.rotation;
	}
	
	// Update is called once per frame
	void Update () {

		Ray ray = new Ray (Camera.main.transform.position, Camera.main.transform.forward);

		Debug.DrawRay (Camera.main.transform.position, Camera.main.transform.forward * 15);

//		lookPos.position = ray.GetPoint (15);

		RaycastHit leftHit;
		RaycastHit rightHit;

		Vector3 lPos = leftFoot.TransformPoint (Vector3.zero);
		Vector3 rPos = rightFoot.TransformPoint (Vector3.zero);	

		if(Physics.Raycast(lPos, -Vector3.up, out leftHit, 2)){
			lFPos = leftHit.point;
			lFRot = Quaternion.FromToRotation(transform.up, leftHit.normal) * transform.rotation;
		}
		if(Physics.Raycast(rPos, -Vector3.up, out rightHit, 2)){
			rFPos = rightHit.point;
			rFRot = Quaternion.FromToRotation(transform.up, rightHit.normal) * transform.rotation;
		}

	}
	void OnAnimatorIK(){

		animator.SetLookAtWeight (lookIKWeight, bodyWeight, headWeight, eyeWeight, clampWeight);
		animator.SetLookAtPosition (lookPos.position);
//		Debug.Log ("Got Here");
		lFWeight = animator.GetFloat ("LeftFoot");
		rFWeight = animator.GetFloat ("RightFoot");

		Debug.Log ("lFWeight: "+lFWeight);
		Debug.Log ("rFWeight: "+rFWeight);
		animator.SetIKPositionWeight (AvatarIKGoal.LeftFoot, lFWeight);
		animator.SetIKPositionWeight (AvatarIKGoal.RightFoot, rFWeight);

		animator.SetIKPosition (AvatarIKGoal.LeftFoot, lFPos + new Vector3(0, offsetY, 0));
		animator.SetIKPosition (AvatarIKGoal.RightFoot, rFPos + new Vector3(0, offsetY, 0));

		animator.SetIKRotationWeight (AvatarIKGoal.LeftFoot, lFWeight);
		animator.SetIKRotationWeight (AvatarIKGoal.RightFoot, rFWeight);

		animator.SetIKRotation (AvatarIKGoal.LeftFoot, lFRot);
		animator.SetIKRotation (AvatarIKGoal.RightFoot, rFRot);
		
//		animator.SetIKPositionWeight (AvatarIKGoal.LeftFoot, iKWeight);
//		animator.SetIKPositionWeight (AvatarIKGoal.RightFoot, iKWeight);
//
//		animator.SetIKPosition (AvatarIKGoal.LeftFoot, leftIKTarget.position);
//		animator.SetIKPosition (AvatarIKGoal.RightFoot, rightIKTarget.position);
//
//		animator.SetIKHintPositionWeight (AvatarIKHint.LeftKnee, iKWeight);
//		animator.SetIKHintPositionWeight (AvatarIKHint.RightKnee, iKWeight);
//		
//		animator.SetIKHintPosition (AvatarIKHint.LeftKnee, leftIKHint.position);
//		animator.SetIKHintPosition (AvatarIKHint.RightKnee, rightIKHint.position);
//
//		animator.SetIKRotationWeight (AvatarIKGoal.LeftFoot, iKWeight);
//		animator.SetIKRotationWeight (AvatarIKGoal.RightFoot, iKWeight);
//		
//		animator.SetIKRotation (AvatarIKGoal.LeftFoot, leftIKTarget.rotation);
//		animator.SetIKRotation (AvatarIKGoal.RightFoot, rightIKTarget.rotation);
	}
}
