using UnityEngine;
using System.Collections;

public class AutoRotate : MonoBehaviour {
	
	
	public Vector3 speed;
	public Space space=Space.World;
	public bool UseDeltatime=true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float dt=0.01f;
		if (UseDeltatime)
			dt=Time.deltaTime;
		transform.Rotate(speed*dt,space);
	}
}
