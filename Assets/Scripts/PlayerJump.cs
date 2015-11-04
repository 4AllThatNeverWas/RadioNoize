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
	private float gravity = 9.8f;

//	public bool GetGroundHit;

	private Vector3 toPos;

	public GameObject landing;

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
//		if(!player.grounded){
//			Debug.Log("Still no floor....");
//			toPos.y -= gravity *Time.deltaTime;
//			this.transform.position += toPos;
//			
////			this.transform.position = toPos;
//		}


		if(!player.grounded){
			animator.SetBool("grounded", false);
		} else {
			animator.SetBool("grounded", true);
			baseRot = transform.localRotation;
			baseRot.x = 0;
			animator.SetBool("jump", false);
			transform.localRotation = Quaternion.Slerp (transform.localRotation, baseRot, Time.time * 2.0f);
		}
		jumpForceX = player.speed * jumpConstX;
		if(Input.GetButtonDown("AButton")&& (player.grounded)){
//			Instantiate(landing, this.transform.position, this.transform.rotation);
			Debug.Log("JUMP");
			player.playerMode = PlayerMode.Jump;
//			StartCoroutine(ProjectileJump());
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
	IEnumerator ProjectileJump(){
		// Move projectile to the position of throwing object + add some offset if needed.
//		Projectile.position = myTransform.position + new Vector3(0, 0.0f, 0);
		Vector3 startPos = this.transform.position;
		float firingAngle = 45f;
		Debug.Log(landing.transform.position);
		// Calculate distance to target
		float targetDistance = Vector3.Distance(startPos, landing.transform.position);
		
		// Calculate the velocity needed to throw the object to the target at specified angle.
		float projectile_Velocity = targetDistance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);
		
		// Extract the X  Y componenent of the velocity
		float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
		float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
		
		// Calculate flight time.
		float flightDuration = targetDistance / Vx;
		
		// Rotate projectile to face the target.
		this.transform.rotation = Quaternion.LookRotation(landing.transform.position - this.transform.position);
		
		float elapse_time = 0;
		
		while (elapse_time < flightDuration) {
			transform.Translate (0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
			
			elapse_time += Time.deltaTime;
			
			yield return null;
		}
//		} DestroyImmediate (landing, true);
	}  
	void OnCollisionExit (Collision collision) {
		if (collision.transform.tag == ("World")) {
			player.grounded = false;
			animator.SetBool("jump", false);
		}
	}
	void OnCollisionEnter (Collision collision) {
		if (collision.transform.tag == ("World")) {
//			Debug.Log("The floor's right here....");
			player.grounded = true;
			player.playerMode = PlayerMode.Skate;

//			baseRot = transform.localRotation;
//			baseRot.x = 0;
//			animator.SetBool ("jump", false);
//			transform.localRotation = Quaternion.Slerp (transform.localRotation, baseRot, Time.time * 2.0f);
//			Debug.Log("Should be working");
		}
	}
////		if (collision.transform.tag == ("Rail")) {
////			animator.SetBool("jump", false);
////			if(Input.GetButton("XButton")){
////				GetComponent<RailEngager>().spline = 
////				collision.gameObject.GetComponent<RailComponent>().rail;
////				player.playerMode = PlayerMode.Grind;
////				grounded = true;
////			}
////		}
//	}
}
