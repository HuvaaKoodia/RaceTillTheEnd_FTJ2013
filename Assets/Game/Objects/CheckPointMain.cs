using UnityEngine;
using System.Collections;

public class CheckPointMain : MonoBehaviour {
	
	public GameObject Area,Bottom;
	public bool IsGoal=false;
	
	// Use this for initialization
	void Start () {
		if (IsGoal){
			Area.renderer.material.color=new Color(1f,1f,1f,Area.renderer.material.color.a);
		}
		else
			Bottom.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
