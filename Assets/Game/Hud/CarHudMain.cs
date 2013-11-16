using UnityEngine;
using System.Collections;

public class CarHudMain : MonoBehaviour {
	
	public UnitMain car;
	public UnitMain Car{
		get{return car;}
		set{
			car=value;
			UpdateLabels();
		}
	}
	public UILabel HP_label,NAME_label,LAPS_label;
	public UISprite Sprite;

	public int LAPS{get;private set;}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update (){
		UpdateLabels();
	}
	
	void UpdateLabels(){
		if (car!=null){
			Sprite.color=car.color;
			HP_label.text="HP: "+car.HP;
			LAPS_label.text="LAPS: "+car.LAPS;
			LAPS=car.LAPS;
		}
		else{
			Sprite.color=Color.black;
			HP_label.text="DEAD";
		}
	}
}