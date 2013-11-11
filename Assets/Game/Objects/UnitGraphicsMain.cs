using UnityEngine;
using System.Collections;

public class UnitGraphicsMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void SetColor(Color color){
		foreach (Renderer r in transform.GetComponentsInChildren<Renderer>()){
			r.material.color=color;
		}
		
	}
}
