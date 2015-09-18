using UnityEngine;
using System.Collections;

public class BoneGrindLerp : MonoBehaviour {
	public Transform next, last, target;
	private float distance;
	public float speed;
	private float startTime;
	private bool fairGame;
	private float lerpPosition = 0.0f;
	private float lerpTime = 5.0f; 
	public bool goingForward;
	public bool lookForward;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
