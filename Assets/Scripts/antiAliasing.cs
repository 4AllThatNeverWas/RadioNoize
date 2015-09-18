using UnityEngine;
using System.Collections;

public class antiAliasing : MonoBehaviour {
	void Start() {
		QualitySettings.antiAliasing=3;
	}
}