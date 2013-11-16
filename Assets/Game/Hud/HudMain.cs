using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HudMain : MonoBehaviour {

	public GameObject Gameover_panel,StartUpPanel;
	public UILabel Points_label,PathMode_label;

	public List<CarHudMain> CarHuds=new List<CarHudMain>();
	
	void Start () 
	{
		Gameover_panel.SetActive(false);
	}

	void Update () 
	{

	}

	public void SetGameover(int points){
		Gameover_panel.SetActive(true);
		Points_label.text="You got "+points+" points.";
		StartCoroutine(Fade());

	}

	public void SetPathMode(bool on){
		if (on)
			PathMode_label.text="ON";
		else
			PathMode_label.text="OFF";
	}

	IEnumerator Fade() {
		while (Time.timeScale>0){
			Time.timeScale-=Time.deltaTime;
			yield return null;
		}
	}

	public void HideStartUpMenu ()
	{
		StartUpPanel.SetActive(false);
	}
}
