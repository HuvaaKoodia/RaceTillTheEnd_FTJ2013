using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HudMain : MonoBehaviour {

	public GameObject PathMode_label;
	
	public List<CarHudMain> CarHuds=new List<CarHudMain>();
	
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
