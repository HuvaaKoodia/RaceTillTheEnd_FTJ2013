using UnityEngine;
using System.Collections;

public class CameraMain : MonoBehaviour {
	
	public float MoveSpeed,MouseSpeedMultiplier=0.2f,MoveSpeedMultiplier;	
	
	void Start () 
	{

	}
	bool moving_cam=false;
	Vector3 start_move_pos;
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

		if (Input.GetMouseButtonDown(2)){
			start_move_pos=Input.mousePosition;
			moving_cam=true;
		}
		if (Input.GetMouseButton(2)){
			if (moving_cam){
				var v=new Vector3(start_move_pos.x-Input.mousePosition.x,0,start_move_pos.y-Input.mousePosition.y);
				Debug.Log(start_move_pos);
				v*=-MouseSpeedMultiplier;
				transform.Translate(v);
			}
		}

		if (Input.GetMouseButtonUp(2)){
			moving_cam=false;
		}
	}


}
