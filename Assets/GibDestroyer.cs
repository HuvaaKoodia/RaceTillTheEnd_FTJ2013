using UnityEngine;
using System.Collections;

public class GibDestroyer : MonoBehaviour {
	
	public int Delay;
	Timer time;
	bool scaledown=false;
	// Use this for initialization
	void Start () {
		time=new Timer(Delay,destroy);
	}
	
	void Update(){

		if (scaledown){
			transform.localScale-=Vector3.one*Time.deltaTime;

			if (transform.localScale.x<0)
				Destroy(gameObject);
		}
		else{
			time.Update();
		}

	}
	
	// Update is called once per frame
	void destroy(){
		scaledown=true;
	}
}
