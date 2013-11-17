using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	bool STARTUP,GAMEOVER;

	public HudMain hud_controller;
	public MapController map_controller;
	public FollowTarget selection_circle;
	public GameObject PathNode_prefab;
	public GameObject Unit_prefab;

	PathNodeMain selected_node;
	PathLineMain selected_line;
	List<UnitMain> selected_units;
	bool time_state=true;
	
	enum ControlMode{None,CreatePaths}
	
	ControlMode _controlMode=ControlMode.None;
	
	Vector3 temp_ground_pos,temp_selection_offset;
	
	// Use this for initialization
	void Start () {
		hud_controller.SetPathMode(false);
		selected_units=new List<UnitMain>();
		DeselectUnits();

		STARTUP=true;
		Time.timeScale=0;
		SetPathMode(true);

		GAMEOVER=false;
	}
	
	private void SelectUnits(Vector3 p1,Vector3 p2){
		//List<UnitMain> list=new List<UnitMain>();
		//p1.y=MouseYToHudY(p1.y);
		//p2.y=MouseYToHudY(p2.y);
		
		foreach (GameObject u in GameObject.FindGameObjectsWithTag("Unit")){
			var obj_pos=Camera.main.WorldToScreenPoint(u.transform.position);
			
			if (obj_pos.z<0) continue;
			
			if (p1.y>p2.y){
				var t=p1.y;
				p1.y=p2.y;
				p2.y=t;
			}
			
			if (p1.x>p2.x){
				var t=p1.x;
				p1.x=p2.x;
				p2.x=t;
			}
			
			//obj_pos.y=MouseYToHudY(obj_pos.y);
			//var r=new Rect(p1.x,p1.y,Mathf.Abs(p2.x-p1.x),Mathf.Abs(p2.y-p1.y));
			var r=new Rect(p1.x,p1.y,Mathf.Abs(p2.x-p1.x),Mathf.Abs(p2.y-p1.y));
			if (Subs.insideArea(new Vector2(obj_pos.x,obj_pos.y),r)){
				//list.Add(u.GetComponent<UnitMain>());
				AddUnit(u.GetComponent<UnitMain>());
				SetMode(ControlMode.None);
			}
		}
		//return list;
	}
	
	private float MouseYToHudY(float mouse_y){
		return Screen.height-mouse_y;
	}
	
	bool moving_node=false,node_pressed=false,selection_rect_on=false,selection_rect_on_legit,can_create_path;
	Vector3 selection_rect_start_pos;
	// Update is called once per frame
	void Update () {

		if (_controlMode==ControlMode.CreatePaths){
			if (GroundPosition(out temp_ground_pos)){
				selection_circle.transform.position=temp_ground_pos+Vector3.up*0.1f;
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape)){
			Application.LoadLevel("MenuScene");
		}

		if (GAMEOVER){



			if (Input.GetKeyDown(KeyCode.R)){
				Application.LoadLevel("GameScene");
			}

			return;
		}
		//select
		if (Input.GetButtonUp("Select")){

			if (moving_node){
				moving_node=false;
				if (_controlMode==ControlMode.CreatePaths)
					selection_circle.gameObject.SetActive(true);
			}
			if (node_pressed)
				DeselectNode();
			node_pressed=false;
			
			selection_rect_on=false;
			if (selection_rect_on_legit){
				SelectUnits(selection_rect_start_pos,Input.mousePosition);
				selection_rect_on_legit=false;
				if (_controlMode==ControlMode.CreatePaths)
					selection_circle.gameObject.SetActive(true);
			}
			else
			if (_controlMode==ControlMode.CreatePaths){
				if (can_create_path){
					//CreateNodeInMousePos();
				}
			}
			can_create_path=false;

		}
		
		if (Input.GetButton("Select")){
			if (moving_node){//moving node
				if (GroundPosition(out temp_ground_pos)){
					if (node_pressed){
						if (Vector3.Distance(
							temp_ground_pos,selected_node.transform.position
							-new Vector3(temp_selection_offset.x,0,temp_selection_offset.z)
						)>1){
							selected_node.Reposition(temp_ground_pos+temp_selection_offset);
							node_pressed=false;
						}
					}
					else{
						selected_node.Reposition(temp_ground_pos+temp_selection_offset);
					}
				}
			}
			else{
				//start selection rectangle
				if (!selection_rect_on_legit&&selection_rect_on){
					if (Vector3.Distance(Input.mousePosition,selection_rect_start_pos)>10){
						selection_rect_on_legit=true;
						selection_circle.gameObject.SetActive(false);
					}
				}
			}
		}
		
		if (Input.GetButtonDown("Select")){
			//check for units
			var unit=RaycastUnit();
			
			if (unit!=null){
				SelectUnit(unit);
#if UNITY_EDITOR
				unit.HP-=10;
#endif
				return;
			}
			

				selection_rect_on=true;
				selection_rect_start_pos=Input.mousePosition;

			if (_controlMode==ControlMode.None){
				var node=RaycastPathNode();
				
				if (node!=null){
					SetPathMode(true);
					SetSelectedNode(node);
					return;
				}
			}

			if (_controlMode==ControlMode.CreatePaths){
				var node=RaycastPathNode();
				
				if (node!=null){
					moving_node=true;
					selection_circle.gameObject.SetActive(false);

					if (selected_node==node){
						node_pressed=true;
					}
					else{
						SetSelectedNode(node);
						DeselectLine();
					}
					
					if (GroundPosition(out temp_ground_pos))
						temp_selection_offset=node.transform.position-temp_ground_pos;
					return;
				}
			
				var line=RaycastPathLine();
				
				if (line!=null){
					SetSelectedLine(line);
					DeselectNode();
					return;
				}

				can_create_path=true;
			}
			
			DeselectUnits();

			DeselectLine();
		}
		
		//control actions
		if (Input.GetButtonDown("Command")){
			if (_controlMode==ControlMode.None){
				if (HasSelectedUnits()){
					var node=RaycastPathNode();
					if (node!=null){
						foreach (var s in selected_units)
							s.Move(node);
					}
					else{
						if (GroundPosition(out temp_ground_pos)){
							foreach (var s in selected_units)
								s.Move(temp_ground_pos);
						}
					}	
				}
			}
			else
			if (_controlMode==ControlMode.CreatePaths){
				CreateNodeInMousePos();
			}
		}
		
		//other
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.Alpha1)){
			SetMode(ControlMode.None);
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha3)){
			if (GroundPosition(out temp_ground_pos)){
				var go=Instantiate(Unit_prefab,temp_ground_pos+Vector3.up*2,Quaternion.identity) as GameObject;
				AddCar(go.GetComponent<UnitMain>());
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha4)){
			SetGameover();
		}
#endif

		if (Input.GetButtonDown("PathMode")){
			TogglePathMode();
		}

		if (Input.GetButtonDown("Remove")){
			if (selected_node!=null){
				PathNodeMain node=null;
				if (selected_node.HasForwardNodes()){
					node=Subs.GetRandom(selected_node.ForwardNodes);
				}
				else{
					if (selected_node.HasBackwardNodes()){
						node=Subs.GetRandom(selected_node.BackwardNodes);
					}
				}
				selected_node.Delete();
				if (node!=null)
					SetSelectedNode(node);
			}
			if (selected_line!=null)
				selected_line.Delete();
		}
		
		if (Input.GetKeyDown(KeyCode.Space)){
			ToggleTimeState();
		}
	}

	bool CreateNodeInMousePos(){
		//check for ground.
		if (GroundPosition(out temp_ground_pos)){
			var node=RaycastPathNode();
			if (node!=null){
				//link to this node
				selected_node.AddForwardNode(node);
				SetSelectedNode(node);
				return true;
			}
			else{
				CreatePathNode(temp_ground_pos+Vector3.up*0.1f);
				return true;
			}
		}
		return false;
	}

	UnitMain RaycastUnit(){
		int mask=1<<LayerMask.NameToLayer("Unit");
		RaycastHit info;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out info,500f,mask))
			return info.collider.gameObject.GetComponent<UnitMain>();
		return null;
	}
	
	PathLineMain RaycastPathLine(){
		int mask=1<<LayerMask.NameToLayer("PathLine");
		RaycastHit info;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out info,500f,mask))
			return info.collider.gameObject.GetComponent<PathLineMain>();
		return null;
	}

	PathNodeMain RaycastPathNode(){
		int mask=1<<LayerMask.NameToLayer("PathNode");
		RaycastHit info;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out info,500f,mask))
			return info.collider.gameObject.GetComponent<PathNodeMain>();
		return null;
	}
	
	bool GroundPosition(out Vector3 point){
		int mask=1<<LayerMask.NameToLayer("Ground");
		RaycastHit info;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out info,500f,mask)){
			point=info.point;
			return true;
		}
		point=Vector3.zero;
		return false;
	}
	
	void SetMode(ControlMode mode){
		if (_controlMode==ControlMode.CreatePaths){
			hud_controller.SetPathMode(false);
		}
		
		_controlMode=mode;
		
		if (_controlMode==ControlMode.CreatePaths){
			selection_circle.gameObject.SetActive(true);
			hud_controller.SetPathMode(true);
			DeselectUnits();
		}
		else{
			selection_circle.gameObject.SetActive(false);
		}
	}
	
	void SetPathMode(bool on){
		if (on){ 
			SetMode(ControlMode.CreatePaths);
		}
		else{
			SetMode(ControlMode.None);
		}
	}

	void TogglePathMode(){
		SetPathMode(!(_controlMode==ControlMode.CreatePaths));
	}

	void SetSelectedLine(PathLineMain line){
		if (selected_line!=line){
			DeselectLine();
			line.SetSelected(true);
			selected_line=line;
		}
		else
			DeselectLine();
	}

	void DeselectLine(){
		if (selected_line!=null)
			selected_line.SetSelected(false);
		selected_line=null;
	}
	
	void SelectUnit (UnitMain unit)
	{
		DeselectUnits();
		AddUnit(unit);
	}
	
	void AddUnit (UnitMain unit)
	{
		DeselectNode();
		SetMode(ControlMode.None);
		selected_units.Add(unit);
		unit.SetSelected(true);
	}
	
	void SetSelectedNode (PathNodeMain node)
	{
		DeselectNode();
		selected_node=node;
		selected_node.setSelected(true);
	}

	void DeselectUnits()
	{
		foreach(var s in selected_units){
			if (!s.Dead)
				s.SetSelected(false);
		}
		selected_units.Clear();
	}
	
	void DeselectNode()
	{
		if (!selected_node) return;
		selected_node.setSelected(false);
		selected_node=null;
	}
	
	bool HasSelectedUnits(){
		return selected_units.Count>0;
		
	}
	
	void ToggleTimeState ()
	{
		if (STARTUP) return;
		time_state=!time_state;
		if (time_state){
			Time.timeScale=1;
		}
		else{
			Time.timeScale=0;
		}
	}

	void CreatePathNode(Vector3 position)
	{
		var o=Instantiate(PathNode_prefab,position,Quaternion.identity) as GameObject;
		var n=o.GetComponent<PathNodeMain>();
		if (selected_node!=null){
			selected_node.AddForwardNode(n);
		}
		
		SetSelectedNode(n);
	}
	
	public void OnCarDead(UnitMain unit){
		selected_units.Remove(unit);

		if (GameObject.FindGameObjectsWithTag("Unit").Length==1){
			SetGameover();
		}

	}

	void StartGame(){

		STARTUP=false;
		Time.timeScale=1;
		
		hud_controller.HideStartUpMenu();
		map_controller.StartActionMode();

		var cars=GameObject.FindGameObjectsWithTag("Unit");
		var nodes=GameObject.FindGameObjectsWithTag("PathNode");

		if (nodes.Length==0) return;

		foreach (var car in cars){

			GameObject go=null;
			float min=1000000000;
			foreach (var node in nodes){
				var dis=Vector3.Distance(car.transform.position,node.transform.position);
				if (dis<min){
					min=dis;
					go=node;
				}
			}
			var u=car.GetComponent<UnitMain>();
			var p=go.GetComponent<PathNodeMain>();

			Debug.Log("u "+u);
			Debug.Log("p "+p);

			u.Move(p);
		}

	}

	void SetGameover(){
		GAMEOVER=true;
		int points=0;
		foreach (var car in hud_controller.CarHuds)
			points+=car.LAPS;
		hud_controller.SetGameover(points);
	}
	
	public void AddCar(UnitMain unit){
		unit.OnDeadEvent+=OnCarDead;
	}

	//HUD
	void DrawQuad(Rect position, Color color){
		Texture2D texture = new Texture2D(1, 1);
	    texture.SetPixel(0,0,color);
	    texture.Apply();
	    GUI.skin.box.normal.background = texture;
	    GUI.Box(position, GUIContent.none);
	}

	void OnGUI(){
		if (selection_rect_on_legit){
			float x1=selection_rect_start_pos.x,y1=MouseYToHudY(selection_rect_start_pos.y);
			float w=Input.mousePosition.x-x1,h=MouseYToHudY(Input.mousePosition.y)-y1;
			DrawQuad(new Rect(x1,y1,w,h),new Color(0f,0f,0f,0.5f));
		}
	}
}
