using UnityEngine;
using System.Collections;

public class AsteroidMain : MonoBehaviour {

	public GameObject Explosion_prefab;
	public float explosion_radius=3;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	bool dead=false;

	void OnCollisionEnter(Collision col){
		if (dead) return;
		dead=true;


		int mask=1<<LayerMask.NameToLayer("Unit");


		var o=Physics.OverlapSphere(transform.position,explosion_radius,mask);

		foreach (var unit in o){
			float dis=Vector3.Distance(transform.position,unit.transform.position);
			float dmg=(explosion_radius-Vector3.Distance(transform.position,unit.transform.position))/explosion_radius*100;
			unit.GetComponent<Rigidbody>().AddExplosionForce(4000,transform.position,explosion_radius);
			unit.GetComponent<UnitMain>().HP-=(int)dmg;

		}

		Destroy(gameObject);

		Instantiate(Explosion_prefab,transform.position,Quaternion.identity);

		if (col.gameObject.tag=="Building"){
			//destroy building!!!
			col.gameObject.GetComponent<BuildingMain>().Destroy();
		}
	}
}
