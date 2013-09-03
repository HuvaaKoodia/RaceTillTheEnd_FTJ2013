using UnityEngine;
using System.Collections;

public class MaterialDepthStart : MonoBehaviour {
	
	public int queue_depth;

	void Start () {
		renderer.material.renderQueue=queue_depth;
	}

}
