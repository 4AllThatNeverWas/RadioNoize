using UnityEngine;
using System.Collections;

public class PlayerPivotLogic : MonoBehaviour {
	public OrbitalCharacterControllerLogic player;
	[SerializeField]
	private Animator animator;

	private Transform pivot;
	public GameObject[] pivotalObjects;
	[SerializeField]
	private bool canPivot = false;
	private int current;
	[SerializeField]
	private float rotationSpeed;
	[SerializeField]
	private float radius;
	[SerializeField]
	private float radiusSpeed;
	private Vector3 desiredPosition;

	[SerializeField]
	private Transform leftRef;
	[SerializeField]
	private Transform rightRef;
	private float direction;


	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		pivotalObjects = GameObject.FindGameObjectsWithTag ("Pivotal");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		CheckForPivotal ();
		if (Input.GetButton ("XButton") && player.playerMode == PlayerMode.Skate && canPivot) {
			player.playerMode = PlayerMode.Pivot;
			CheckBias();
		}
		if (player.playerMode == PlayerMode.Pivot) {
			animator.SetBool("Pivot", true);
			this.transform.RotateAround(pivotalObjects[current].transform.position, Vector3.up, rotationSpeed * Time.deltaTime*direction);
			desiredPosition = (transform.position - pivotalObjects[current].transform.position).normalized * radius + pivotalObjects[current].transform.position;
			transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
//			Vector3 desiredRotation = desiredPosition;
//			desiredRotation.y = transform.position.y;
//			desiredRotation += Vector3.forward*5;
//			transform.LookAt(desiredRotation);
		}
	}
	void CheckForPivotal(){
		for (int i = 0; i < pivotalObjects.Length; i++) {
			if(pivotalObjects[i].GetComponent<PivotalObject>().pivotal==true){
				canPivot = true; 
				
				current = i;
				
				break;
			} else {
				canPivot = false;
			}
		}
	}
	void CheckBias(){
		if (Vector3.Distance (rightRef.transform.position, pivotalObjects [current].transform.position) <=
			Vector3.Distance (leftRef.transform.position, pivotalObjects [current].transform.position)) {
			direction = 1;
		} else {
			direction = -1;
		}
	}
}
