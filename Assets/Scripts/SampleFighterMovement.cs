using UnityEngine;
using System.Collections;

public class SampleFighterMovement : MonoBehaviour {
	
	private float horizontal = 0.0f;
	private float vertical = 0.0f;
	[SerializeField]
	private float runSpeed;
	[SerializeField]
	private float maxVelocity;
	private Transform opponent;
	private bool grounded = false;
	// Use this for initialization
	void Start () {
		Physics.IgnoreLayerCollision(8,9);
		opponent = GameObject.FindWithTag("Opponent").transform;
	}
	
	// Update is called once per frame
	void Update () {
		Physics.IgnoreLayerCollision(8,9);
		transform.LookAt(opponent);
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");
//		if(grounded){
			if(vertical>=0){
				this.transform.position = Vector3.Lerp(this.transform.position, opponent.position, vertical*(runSpeed/4)*Time.deltaTime);
			}
			if(vertical<0){
				//this.transform.position = Vector3.Lerp(this.transform.position,this.transform.position+(Vector3.back*runSpeed*Time.deltaTime),vertical*(runSpeed/3)*Time.deltaTime);
				this.transform.Translate(Vector3.back*(runSpeed/2*Time.deltaTime));
				//rigidbody.AddForce(0,0,vertical*runSpeed*Time.deltaTime);
			}
			transform.RotateAround(opponent.position, Vector3.up, -1*horizontal*(runSpeed)*Time.deltaTime);
//		}
		//rigidbody.AddForce(0,0,vertical*runSpeed);
		if(GetComponent<Rigidbody>().velocity.magnitude>maxVelocity && grounded){
			GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized*maxVelocity;
		}
	}
	void OnCollisionEnter(Collision collision){
		if(collision.gameObject.tag=="World" && grounded == false){
			grounded = true;
		}
	}
	void OnCollisionExit(Collision collision){
		if(collision.gameObject.tag=="World" && grounded == true){
			grounded = false;
		}
	}
}
