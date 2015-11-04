using UnityEngine;
using System.Collections;

public class PivotalObject : MonoBehaviour {
	public OrbitalCharacterControllerLogic player;
	private Renderer rend;
	public bool pivotal;
	public BezierSpline path;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<OrbitalCharacterControllerLogic> ();
		rend = GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter (Collider collision){
		if(collision.gameObject.tag=="Player"){
			rend.material.color = Color.green;	
			pivotal = true;
		}
		
	}
	void OnTriggerExit (Collider collision){
		if(collision.gameObject.tag=="Player"){
			rend.material.color = Color.white;	
			pivotal = false;
		}
		
	}
}
