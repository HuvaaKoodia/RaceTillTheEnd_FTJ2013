using UnityEngine;
using System.Collections;

public class MaterialDepthUpdate : MonoBehaviour {
	
	public int queue_depth;

	void Update () {
		renderer.material.renderQueue=queue_depth;
	}
}
