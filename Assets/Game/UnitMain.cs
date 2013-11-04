using UnityEngine;
using System.Collections;

public class UnitMain : MonoBehaviour {
	
	PathNodeMain move_node;
	Vector3 move_p,move_d;
	bool stop=false,moving=false;
	public float move_speed=10;
	
	public bool Moving{
		get{return moving;}
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate (){
		if (!stop&&moving){
			move_d=move_p-transform.position;
			move_d.Normalize();
			rigidbody.MovePosition(transform.position+move_d*move_speed*Time.deltaTime);
			if (Vector3.Distance(transform.position,move_p)<0.3f){
				moving=false;
				//get next node
				if (move_node!=null){
					var o_n=move_node;
					move_node=null;
					if (o_n.HasLinks()){
						Move(o_n.GetNextNode());
					}
					//Destroy(o_n.gameObject);
				}
			}
		}
		
	}

	public void Move(Vector3 p)
	{
		move_p=new Vector3(p.x,transform.position.y,p.z);
		moving=true;
	}
	
	public void Move(PathNodeMain n)
	{
		move_node=n;
		Move(n.transform.position);
	}
	
	public void OnCollisionStay(Collision other){
		
		if (other.collider.gameObject.tag=="Unit"){
			stop=true;
			Debug.Log("STOP!");
		}
	}
	public void OnCollisionExit(Collision other){
		
		if (other.collider.gameObject.tag=="Unit"){
			stop=false;
			Debug.Log("STOP off!");
		}
	}
}
