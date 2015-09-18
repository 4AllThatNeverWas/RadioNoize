using UnityEngine;
using System.Collections;

public class RailInstantiator : MonoBehaviour {
	
	public GameObject[] splines;

	public float progress;
	public float frequency;
	
	public bool lookForward;
	
	public Transform[] items;

	public BezierSpline[] rails;

	public int railAmount;
	
	private void Awake() {
		splines = GameObject.FindGameObjectsWithTag("RailPath");
		for (int s = 0; s < splines.Length ; s ++){
			rails[s] = splines[s].GetComponent<BezierSpline>();
			railAmount = s;


		}

		for(int r = 0; r < rails.Length ; r ++) {
//			Debug.Log("s: "+s);
			progress = 0;
			if (frequency <= 0 || items == null || items.Length == 0) {
				return;
			}
			float stepSize = frequency * items.Length;
			if (rails[r].Loop || stepSize == 1) {
				stepSize = 1f / stepSize;
			}
			else {
				stepSize = 1f / (stepSize - 1);
			}
			for (int p = 0, f = 0; f < frequency; f++) {
//				Debug.Log("f: "+f);
			
				for (int i = 0; i < items.Length; i++, p++) {
//					Debug.Log("i: "+i);
//					Debug.Log("p: "+p);
					progress = (progress + 1/frequency);
//					Debug.Log (progress);
					Transform item = Instantiate(items[i]) as Transform;
					Vector3 position = rails[r].GetPoint(p * stepSize);
					item.transform.localPosition = position;
					item.tag = ("Rail");
					item.GetComponent<RailComponent>().rail = rails[r];
					if (lookForward) {
						item.transform.LookAt(position + rails[r].GetDirection(p * stepSize));
					}
					item.transform.parent = transform;
				}
			}
		}
	}
}