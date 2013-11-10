using UnityEngine;
using System.Collections;

public class CameraMain : MonoBehaviour {
	
	public float MoveSpeed,MoveSpeedMultiplier;	
	
	void Start () 
	{

	}

	void Update () 
	{
		var Speed=MoveSpeed*MoveSpeedMultiplier;
		if (Input.GetKey(KeyCode.A)){
			transform.Translate(Vector3.left*Speed);
		}
		if (Input.GetKey(KeyCode.D)){
			transform.Translate(Vector3.right*Speed);
		}
		if (Input.GetKey(KeyCode.W)){
			transform.Translate(Vector3.forward*Speed);
		}
		if (Input.GetKey(KeyCode.S)){
			transform.Translate(Vector3.back*Speed);
		}
		
		//zoom
		

	}


}
