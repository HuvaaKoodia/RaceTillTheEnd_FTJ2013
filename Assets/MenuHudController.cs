using UnityEngine;
using System.Collections;

public class MenuHudController : MonoBehaviour {

	public GameObject CreditsPanel;

	// Use this for initialization
	void Start () {
		Time.timeScale=1;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnPlay(){
		Application.LoadLevel("GameScene");
	}

	void OnExit(){
		Application.Quit();
	} 

	void OnCredits(){
		CreditsPanel.SetActive(!CreditsPanel.activeSelf);
	} 
}
