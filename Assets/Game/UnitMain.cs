using UnityEngine;
using System.Collections;

public class UnitMain : MonoBehaviour {
	
	PathNodeMain move_node;
	Vector3 move_p,move_d;
	bool moving=false;
	public float move_speed=10;
	
	public bool Moving{
		get{return moving;}
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate (){
		if (moving){
			rigidbody.MovePosition(transform.position+move_d*move_speed*Time.deltaTime);
			if (Vector3.Distance(transform.position,move_p)<0.3f){
				moving=false;
				if (move_node!=null){
					var o_n=move_node;
					move_node=null;
					if (o_n.HasLinks()){
						Move(o_n.GetNextNode());
					}
					Destroy(o_n.gameObject);
				}
			}
		}
	}

	public void Move(Vector3 p)
	{
		move_p=new Vector3(p.x,transform.position.y,p.z);
		moving=true;
		move_d=move_p-transform.position;
		move_d.Normalize();
	}
	
	public void Move(PathNodeMain n)
	{
		move_node=n;
		Move(n.transform.position);
	}
}
