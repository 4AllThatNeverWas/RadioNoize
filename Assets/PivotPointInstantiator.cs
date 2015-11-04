using UnityEngine;
using System.Collections;

public class PivotPointInstantiator : MonoBehaviour {
//	public GameObject[] splines;
//	
//	public float progress;
//	public float frequency;
//	
//	public bool lookForward;
//	
//	public Transform[] items;
//	
//	public BezierSpline[] pivots;
//	
//	public int pivotAmount;
//	
//	private void Awake() {
//		splines = GameObject.FindGameObjectsWithTag("PivotPath");
//		for (int s = 0; s < splines.Length ; s ++){
//			pivots[s] = splines[s].GetComponent<BezierSpline>();
//			pivotAmount = s;
//			
//			
//		}
//		
//		for(int p = 0; p < pivots.Length ; p ++) {
//			//			Debug.Log("s: "+s);
//			progress = 0;
//			if (frequency <= 0 || items == null || items.Length == 0) {
//				return;
//			}
//			float stepSize = frequency * items.Length;
//			if (pivots[p].Loop || stepSize == 1) {
//				stepSize = 1f / stepSize;
//			}
//			else {
//				stepSize = 1f / (stepSize - 1);
//			}
//			for (int s = 0, f = 0; f < frequency; f++) {
//				//				Debug.Log("f: "+f);
//				
//				for (int i = 0; i < items.Length; i++, s++) {
//					//					Debug.Log("i: "+i);
//					//					Debug.Log("p: "+p);
//					progress = (progress + 1/frequency);
//					//					Debug.Log (progress);
//					Transform item = Instantiate(items[i]) as Transform;
//					Vector3 position = pivots[p].GetPoint(s * stepSize);
//					item.transform.localPosition = position;
//					item.tag = ("Pivot");
//					item.GetComponent<PivotalObject>().path = pivots[p];
//					if (lookForward) {
//						item.transform.LookAt(position + pivots[p].GetDirection(s * stepSize));
//					}
//					item.transform.parent = transform;
//				}
//			}
//		}
//	}
}