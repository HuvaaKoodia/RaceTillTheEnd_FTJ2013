using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour {
	
	public float decrease_ticks=5,increase_ticks=5,zoom_multiplier,zoom_speed=20;
	
	
	
	
	public CameraMain CamMain;
	public Transform center_position;
	
	
	public float MoveMultiplier{get{return distance;}}
	
	int d=0;
	float distance=1;
	
	void Start () 
	{

	}

	void Update () 
	{
		if (Input.GetAxis("Mouse ScrollWheel")>0){
			
			if (d>-decrease_ticks){
				d--;
				distance*=zoom_multiplier;
				transform.position=center_position.position+transform.TransformDirection(Vector3.forward*zoom_speed*(1-distance));
			
				//CamMain.MoveSpeedMultiplier=distance;
			}
		}
		
		if (Input.GetAxis("Mouse ScrollWheel")<0){
			if (d<increase_ticks){
				d++;
				distance/=zoom_multiplier;
				transform.position=center_position.position+transform.TransformDirection(Vector3.forward*zoom_speed*(1-distance));
			
				//CamMain.MoveSpeedMultiplier=distance;
			}
		}
	}


}
