using UnityEngine;
using System.Collections;

public class LeadFoot : MonoBehaviour {

	[SerializeField]
	private Animator animator;
	public OrbitalCharacterControllerLogic player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionExit (Collision collision) {
		if (collision.transform.tag == ("World")) {
			player.grounded = false;
			animator.SetBool("jump", false);
		}
	}
	void OnCollisionEnter (Collision collision) {
		if (collision.transform.tag == ("World")) {
			Debug.Log ("Found the Floor");
			//			Debug.Log("The floor's right here....");
			player.grounded = true;
			player.playerMode = PlayerMode.Skate;
			

			//			Debug.Log("Should be working");
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
