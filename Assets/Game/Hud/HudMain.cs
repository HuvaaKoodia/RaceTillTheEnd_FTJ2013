using UnityEngine;
using System.Collections;

public class HudMain : MonoBehaviour {

	public GameObject PathMode_label;
	
	void Start () 
	{

	}

	void Update () 
	{

	}
	
	public void SetPathMode(bool on){
		PathMode_label.SetActive(on);	
	}

}
