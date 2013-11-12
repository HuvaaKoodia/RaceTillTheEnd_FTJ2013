using UnityEngine;
using System.Collections;

public class UnitGraphicsMain : MonoBehaviour {
	
	public ParticleSystem PS;
	public Transform Obj;
	
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
	
	public void StartSmoking(){
		if (!PS.isPlaying)
			PS.Play();
	}
	
	public void DetachObject(Vector3 velocity){
		Obj.parent=null;
		Obj.GetComponent<Collider>().enabled=true;
		var r=Obj.GetComponent<Rigidbody>();
		r.isKinematic=false;
		r.velocity=velocity;
		PS.Play();
	}
	
}
