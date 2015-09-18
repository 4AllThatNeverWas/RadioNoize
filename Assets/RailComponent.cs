using UnityEngine;
using System.Collections;

public class RailComponent : MonoBehaviour {
	public OrbitalCharacterControllerLogic player;
	public Vector3 progress;
	public BezierSpline rail;
	public RailInstantiator inst;
	[SerializeField]
	private float dist;
	private Transform foot;
	private Renderer rend;
	public bool grindable;
	public Transform next, last;
	public int railNumber;
	public bool head, tail = false;
	public int segNum;

//	public PlayerMode playerMode;
	// Use this for initialization
	void Awake () {

	}
	void Start () {
		inst = transform.root.GetComponent<RailInstantiator>();
//		progress = rail.GetPoint();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<OrbitalCharacterControllerLogic> ();
		foot = GameObject.FindGameObjectWithTag ("leadFoot").transform;
		rend = GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (player.playerMode == PlayerMode.Grind) {
			GetComponent<Collider>().enabled = false;
		} else {
			GetComponent<Collider>().enabled = true;
		}
		dist = Vector3.Distance (this.transform.position, foot.position);
		if (dist < 3) {
			rend.material.color = Color.green;	
			grindable = true;
		} else {
			rend.material.color = Color.white;
			grindable = false;
		}
	}

	void OnCollisionEnter (Collision collision){
//		if(collision.gameObject.tag=="Player" && playerMode == PlayerMode.Skate){
//			playerMode = PlayerMode.Grind;
//		}
	
	}
}
