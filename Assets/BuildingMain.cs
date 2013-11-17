using UnityEngine;
using System.Collections;
using NotificationSys;

public class BuildingMain : MonoBehaviour {

	public Transform Gibs,GraphicsT;

	void Update()
	{
	}

	public void Destroy(){

		while (Gibs.childCount>0)
		{
			Gibs.GetChild(0).parent=null;
		}

		Destroy(gameObject);

		//NotificationSys.NotificationCenter.Instance.sendNotification(
		//	new Explosion_note(GraphicsT.position,1000000,10));
	}
}
