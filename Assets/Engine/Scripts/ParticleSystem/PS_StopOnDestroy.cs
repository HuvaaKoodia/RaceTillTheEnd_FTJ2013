using UnityEngine;
using System.Collections;

public class PS_StopOnDestroy : MonoBehaviour {
	public enum Stop{System,Loop}
	public ParticleSystem PS;
	
	public Stop stop=Stop.System;
	
	void OnDestroy(){
		if (stop==Stop.Loop)
			PS.loop=false;
		else if (stop==Stop.System)
			PS.Stop();
	}
}
