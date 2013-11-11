using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitMain : MonoBehaviour {
	
	PathNodeMain move_node;
	Vector3 move_p,move_d;
	bool moving=false;
	public float move_speed=10;
	
	public AIPath AIPathFinder;
	
	public float TARGET_REACHED_DISTANCE=2.2f;
	
	public bool Moving{
		get{return moving;}
	}
	
	public GameObject SelectionCircle;
	public UnitGraphicsMain GraphicsMain; 
	
	Timer stuck_timer;
	
	// Use this for initialization
	void Start () {
		AIPathFinder.canMove=true;
		AIPathFinder.OnTargetReachedEvent+=OnTargetReached;
		SetSelected(false);
		
		GraphicsMain.SetColor(Subs.RandomColor());
		
		stuck_timer=new Timer(4000);
		stuck_timer.Active=false;
	}
	
	bool stuck=false,reverse_on=false;
	Vector3 old_pos;
	
	// Update is called once per frame
	void Update (){
		
		if (moving){			
			stuck_timer.Update();
			
			if (reverse_on){
				if (stuck_timer.Done){
					stuck_timer.Reset(false);
					ResetStuckSys();
				}
			}
			else if (!stuck){
				if (Vector3.Distance(transform.position,old_pos)<0.01f){
					stuck_timer.Delay=Random.Range(3000,5000);
					stuck_timer.Reset(true);
					stuck=true;
				}
			}
			else{
				if (Vector3.Distance(transform.position,old_pos)>0.1){
					ResetStuckSys();
				}
				
				if (stuck_timer.Done){
					stuck_timer.Delay=Random.Range(600,1500);
					stuck_timer.Reset(true);
					reverse_on=true;
					
					AIPathFinder.ReverseDirection=true;
				}
			}

		}
		old_pos=transform.position;
	}
	
	public void OnTargetReached(){
		//get next node
		if (move_node!=null){
			var o_n=move_node;
			if (o_n.HasForwardNodes()){
				Move(o_n.GetNextNode());
				return;
			}
			//Destroy(o_n.gameObject);
		}
		StopMoving();
	}
	
	void ResetStuckSys(){
		stuck=false;
		reverse_on=false;
		AIPathFinder.ReverseDirection=false;
		stuck_timer.Reset(false);
	}
	
	public void Move(Vector3 p)
	{
		DeselectMoveNode();
		AIPathFinder.endReachedDistance=1f;
		UpdateMovePosition(p);
	}
	
	public void Move(PathNodeMain n)
	{
		Move(n.transform.position);
		AIPathFinder.endReachedDistance=TARGET_REACHED_DISTANCE;
		SelectMoveNode(n);
	}
	
	public void StopMoving(){
		moving=false;
		ResetStuckSys();
		
		AIPathFinder.canSearch=false;
		//AIPathFinder.canMove=false;
	}
	
	void UpdateMovePosition(Vector3 p){
		move_p=new Vector3(p.x,transform.position.y,p.z);
		moving=true;
		AIPathFinder.canSearch=true;
		//AIPathFinder.canMove=true;
		AIPathFinder.SetVectorTarget(move_p);
		AIPathFinder.SearchPath();
		
		ResetStuckSys();
	}
	
	void SelectMoveNode(PathNodeMain node){
		DeselectMoveNode();
		move_node=node;
		move_node.PathNodeMovedEvent+=OnPathMoved;
		move_node.PathNodeNewForwardNodesEvent+=OnPathNewNode;
		move_node.OnPathNodeDestroyedEvent+=OnPathDestroyed;
	}
	
	void DeselectMoveNode(){
		if (move_node!=null){
			move_node.PathNodeMovedEvent-=OnPathMoved;
			move_node.PathNodeNewForwardNodesEvent-=OnPathNewNode;
			move_node.OnPathNodeDestroyedEvent-=OnPathDestroyed;
			move_node=null;
		}
	}
	
	public void SetSelected(bool selected){
		SelectionCircle.SetActive(selected);
	}

	/*
	public void OnCollisionStay(Collision other){
		if (other.collider.gameObject.tag=="Unit"){
			stop=true;
		}
	}
	public void OnCollisionExit(Collision other){
		if (other.collider.gameObject.tag=="Unit"){
			stop=false;
		}
	}*/
	
	void OnPathMoved(){
		UpdateMovePosition(move_node.transform.position);
	}
	
	void OnPathNewNode(){
		if (!moving)
			OnTargetReached();
	}
	
	void OnPathDestroyed(List<PathNodeMain> nodes){
		Move(nodes[Subs.GetRandom(nodes.Count)]);
	}
}
