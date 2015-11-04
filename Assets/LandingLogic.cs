using UnityEngine;
using System.Collections;

public class LandingLogic : MonoBehaviour {
	public OrbitalCharacterControllerLogic player;
	private Vector3 movement, toPos;
	[SerializeField]
	private float speed;
	private float x,z;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<OrbitalCharacterControllerLogic> ();
		speed = 1f;
		toPos = player.transform.position;
		toPos.y = 0;
	}
	
	// Update is called once per frame
	void Update () {
		x = (Input.GetAxis ("Horizontal") * speed);
		z = (Input.GetAxis ("Vertical") * speed);
		movement = new Vector3(-x, 0, z);
		toPos += movement;
		transform.position = toPos;
	}
}
