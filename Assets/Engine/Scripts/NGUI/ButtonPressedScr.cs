using UnityEngine;
using System.Collections;

public class ButtonPressedScr : MonoBehaviour {
	
	public GameObject controller;
	public string EventName;
	
	void OnClick(){
		controller.SendMessage(EventName);
	}
}
