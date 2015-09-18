using UnityEngine;
using System.Collections;

public class BaseMovement : MonoBehaviour {
	private float horizontal = 0.0f;
	private float vertical = 0.0f;
	[SerializeField]
	private float runSpeed;
	[SerializeField]
	private float maxVelocity;
	public bool grounded = false;
	public bool goingForward;

	public PlayerMode playerMode;

	// Use this for initialization
	void Start () {
		playerMode = PlayerMode.Skate;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(transform.forward);
		if(playerMode == PlayerMode.Skate){
			horizontal = Input.GetAxis("Horizontal");
			vertical = Input.GetAxis("Vertical");

			if(vertical>0){
				//this.transform.Translate(Vector3.forward*(runSpeed/2*Time.deltaTime));
				if(grounded){
					GetComponent<Rigidbody>().AddForce(Vector3.forward*(runSpeed));
				}
				if(!grounded){
					GetComponent<Rigidbody>().AddForce(Vector3.forward*(runSpeed/2));
				}
			}
			if(vertical<0){
				//this.transform.position = Vector3.Lerp(this.transform.position,this.transform.position+(Vector3.back*runSpeed*Time.deltaTime),vertical*(runSpeed/3)*Time.deltaTime);
	//			this.transform.Translate(Vector3.back*(runSpeed/2*Time.deltaTime));
				if(grounded){
					GetComponent<Rigidbody>().AddForce(Vector3.back*(runSpeed));
				}
				if(!grounded){
					GetComponent<Rigidbody>().AddForce(Vector3.back*(runSpeed/2));
				}
				//rigidbody.AddForce(0,0,vertical*runSpeed*Time.deltaTime);
			}
			
			if(horizontal>0){
	//			this.transform.Translate(Vector3.right*(runSpeed/2*Time.deltaTime));
				if(grounded){
					GetComponent<Rigidbody>().AddForce(Vector3.right*(runSpeed));
				}
				if(!grounded){
					GetComponent<Rigidbody>().AddForce(Vector3.right*(runSpeed/2));
				}
			}
			if(horizontal<0){
				//this.transform.position = Vector3.Lerp(this.transform.position,this.transform.position+(Vector3.back*runSpeed*Time.deltaTime),vertical*(runSpeed/3)*Time.deltaTime);
	//			this.transform.Translate(Vector3.left*(runSpeed/2*Time.deltaTime));
				if(grounded){	
					GetComponent<Rigidbody>().AddForce(Vector3.left*(runSpeed));
				}
				if(!grounded){	
					GetComponent<Rigidbody>().AddForce(Vector3.left*(runSpeed/2));
				}
				//rigidbody.AddForce(0,0,vertical*runSpeed*Time.deltaTime);
			}
			if(GetComponent<Rigidbody>().velocity.magnitude>maxVelocity && grounded){
				GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized*maxVelocity;
			}

		}
	}
	void OnCollisionEnter(Collision collision){
		if(collision.gameObject.tag=="World" && grounded == false){
			grounded = true;
		}
		if(collision.gameObject.tag=="Rail" && playerMode == PlayerMode.Skate){
//			if(this.transform.forward
			transform.root.GetComponent<RailEngager>().spline = 
				collision.gameObject.GetComponent<RailComponent>().rail;
			playerMode = PlayerMode.Grind;
			grounded = true;
		}
	}
	void OnCollisionExit(Collision collision){
		if(collision.gameObject.tag=="World" && grounded == true){
			grounded = false;
		}
	}
}
