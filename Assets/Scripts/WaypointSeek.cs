using UnityEngine;
using System.Collections;

public class WaypointSeek : MonoBehaviour {
	private Transform player;
	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
//		Physics.Raycast(this.transform.position, player.position - this.transform.position, Color.green);
		Debug.DrawRay(this.transform.position, player.position - this.transform.position, Color.green);
	}
}
