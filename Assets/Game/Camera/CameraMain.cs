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

		if (Input.GetAxisRaw("Horizontal")<0){
			transform.Translate(Vector3.left*Speed);
		}
		if (Input.GetAxisRaw("Horizontal")>0){
			transform.Translate(Vector3.right*Speed);
		}
		if (Input.GetAxisRaw("Vertical")>0){
			transform.Translate(Vector3.forward*Speed);
		}
		if (Input.GetAxisRaw("Vertical")<0){
			transform.Translate(Vector3.back*Speed);
		}

		if (Input.GetButtonDown("Move Camera")){
			start_move_pos=Input.mousePosition;
			moving_cam=true;
		}
		if (Input.GetButton("Move Camera")){
			if (moving_cam){
				var v=new Vector3(start_move_pos.x-Input.mousePosition.x,0,start_move_pos.y-Input.mousePosition.y);
				v*=-MouseSpeedMultiplier;
				transform.Translate(v);
			}
		}

		if (Input.GetButtonUp("Move Camera")){
			moving_cam=false;
		}
	}


}
