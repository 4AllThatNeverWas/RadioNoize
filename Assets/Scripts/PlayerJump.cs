using UnityEngine;
using System.Collections;

public class PlayerJump : MonoBehaviour {
	[SerializeField]
	private float jumpForceX;
	[SerializeField]
	private float jumpForceY;
	[SerializeField]
	private float jumpConstX;

	[SerializeField]
	private Animator animator;
	public OrbitalCharacterControllerLogic player;
	private Quaternion baseRot;
//	public bool grounded;

	// Use this for initialization
	void Start () {
		baseRot = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		if(!player.grounded){
			animator.SetBool("grounded", false);
		} else {
			animator.SetBool("grounded", true);
		}
		jumpForceX = player.speed * jumpConstX;
		if(Input.GetButtonDown("AButton")&& (player.grounded)){
			Debug.Log("JUMP");
			player.playerMode = PlayerMode.Jump;
			GetComponent<Rigidbody>().AddForce(this.transform.forward*(jumpForceX));
			GetComponent<Rigidbody>().AddForce(Vector3.up*jumpForceY);	

			animator.SetBool("jump", true);
		}
		if (Input.GetButtonDown ("AButton") && player.playerMode == PlayerMode.Grind) {
			player.playerMode = PlayerMode.Jump;
			GetComponent<Rigidbody>().AddForce(this.transform.forward*(jumpForceY));
			GetComponent<Rigidbody>().AddForce(Vector3.up*jumpForceY);	
		}

	}
	void OnCollisionExit (Collision collision) {
		if (collision.transform.tag == ("World")) {
			player.grounded = false;
			animator.SetBool("jump", false);
		}
	}
	void OnCollisionEnter (Collision collision) {
		if (collision.transform.tag == ("World")) {
			player.grounded = true;
			player.playerMode = PlayerMode.Skate;

			baseRot = transform.localRotation;
			baseRot.x = 0;
			animator.SetBool("jump", false);
			transform.localRotation = Quaternion.Slerp (transform.localRotation, baseRot, Time.time * 2.0f);
			Debug.Log("Should be working");
		}
//		if (collision.transform.tag == ("Rail")) {
//			animator.SetBool("jump", false);
//			if(Input.GetButton("XButton")){
//				GetComponent<RailEngager>().spline = 
//				collision.gameObject.GetComponent<RailComponent>().rail;
//				player.playerMode = PlayerMode.Grind;
//				grounded = true;
//			}
//		}
	}
}
