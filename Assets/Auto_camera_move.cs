using UnityEngine;
using System.Collections;

public class Auto_camera_move : MonoBehaviour {


	Vector3 start_pos;

	public float speed=10,length=2;
	// Use this for initialization
	void Start () {
		start_pos=transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		transform.position=start_pos+Quaternion.AngleAxis(Time.time*speed,transform.TransformDirection(Vector3.forward))*Vector3.forward*length;
	}
}
