using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RaceController : MonoBehaviour {
	
	public GameController GameCont;
	
	
	public CheckPointMain StartPos;
	public List<CheckPointMain> CheckPoints=new List<CheckPointMain>();
	
	// Use this for initialization
	void Start () {
		//create cars in star pos

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool CheckLap (List<CheckPointMain> lap)
	{
		if (lap.Count==0) return false;
		foreach (var l in CheckPoints){
			if (l.IsGoal) continue;
			if (!lap.Contains(l))
			{
				return false;
			}
		}
		return true;
	}

	public void CreateCars(){
		for (int i=0;i<4;i++){
			var Corner=StartPos.transform.position;
			var pos=new Vector3(0.25f+0.5f*i,0.2f,0.75f+(i%2)*0.5f)*5;
			var car=Instantiate(GameCont.Unit_prefab,Corner+pos,Quaternion.AngleAxis(-90,Vector3.up)) as GameObject;
			var unit=car.GetComponent<UnitMain>();
			
			unit.RaceCont=this;
			
			GameCont.hud_controller.CarHuds[i].Car=unit;
			
			GameCont.AddCar(unit);
		}

	}
}
