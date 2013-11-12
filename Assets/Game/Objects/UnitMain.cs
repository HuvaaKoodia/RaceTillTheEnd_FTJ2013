using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitMain : MonoBehaviour {
	
	public delegate void OnUnitDead(UnitMain unit);
	
	PathNodeMain move_node;
	Vector3 move_p,move_d;
	bool moving=false;
	public float move_speed=10;
	
	public AIPath AIPathFinder;
	
	public float TARGET_REACHED_DISTANCE=2.2f;
	
	public GameObject SelectionCircle;
	public UnitGraphicsMain GraphicsMain; 
	
	public Color color{get;private set;}
	
	public int MaxHP=100,DmgThreshold=2,SmokeThreshold=70;
	
	public OnUnitDead OnDeadEvent;
	
	
	
	
	//stats
	int hp;
	
	public bool Dead{get;private set;}
	public void Die(){
		
		GraphicsMain.SetColor(Color.black);
		GraphicsMain.DetachObject(rigidbody.velocity);
		Destroy(gameObject);
		if (OnDeadEvent!=null)
			OnDeadEvent(this);
	}
	public int HP{
		get{return hp;}
		set{
			hp=value;
			if (hp<=0){
				hp=0;
				Die();
			}
			if (hp<=SmokeThreshold){
				GraphicsMain.StartSmoking();
			}
			Debug.Log("HP: "+hp);
		}
	}
	
	public int LAPS{
		get;private set;
	}
	
	public RaceController RaceCont;
	
	List<CheckPointMain> lap_check_points_completed=new List<CheckPointMain>();
	
	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag=="Checkpoint"){
			var cp=other.gameObject.GetComponent<CheckPointMain>();
			
			if (cp.IsGoal){
				if (RaceCont.CheckLap(lap_check_points_completed)){
					LAPS++;
					lap_check_points_completed.Clear();
				}
			}
			else{
				if (!lap_check_points_completed.Contains(cp)){
					lap_check_points_completed.Add(cp);
				}
			}
		}
	}
	
	
	//ai stuck sys
	Timer stuck_timer;
	bool stuck=false,reverse_on=false;
	Vector3 old_pos;
	
	void Start () {
		AIPathFinder.canMove=true;
		AIPathFinder.OnTargetReachedEvent+=OnTargetReached;
		SetSelected(false);
		
		color=Subs.RandomColor();
		GraphicsMain.SetColor(color);
		
		stuck_timer=new Timer(4000);
		stuck_timer.Active=false;
		
		hp=MaxHP;
		LAPS=0;
	}
	
	void Update (){
		//stuck sys
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
	
	void OnCollisionEnter(Collision collision){
		int amount=(int)(collision.relativeVelocity.magnitude*0.5f);
		if (amount>DmgThreshold)
			HP-=amount;
	}
}
