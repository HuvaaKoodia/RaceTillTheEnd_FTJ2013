using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	
	public HudMain hud_controller;
	public FollowTarget selection_circle;
	public GameObject PathNode_prefab;
	public GameObject Unit_prefab;
	
	
	PathNodeMain selected_node;
	UnitMain selected_unit;
	bool time_state=true;
	
	enum ControlMode{None,CreatePaths}
	
	ControlMode _controlMode=ControlMode.None;
	
	Vector3 temp_ground_pos,temp_selection_offset;
	
	// Use this for initialization
	void Start () {
		DeselectUnit();
		hud_controller.SetPathMode(false);
	}
	
	
	
	void DrawQuad(Rect position, Color color){
		Texture2D texture = new Texture2D(1, 1);
	    texture.SetPixel(0,0,color);
	    texture.Apply();
	    GUI.skin.box.normal.background = texture;
	    GUI.Box(position, GUIContent.none);
	}

	void OnGUI(){
		if (selection_rect_on){
			float x1=selection_rect_start_pos.x,y1=MouseYToHudY(selection_rect_start_pos.y);
			float w=Input.mousePosition.x-x1,h=MouseYToHudY(Input.mousePosition.y)-y1;
			DrawQuad(new Rect(x1,y1,
				w,h),
				new Color(0f,0f,0f,0.5f));
		}
	}
	
	private float MouseYToHudY(float mouse_y){
		return Screen.height-mouse_y;
	}
	
	bool mode_node=false,node_pressed=false,selection_rect_on=false;
	Vector3 selection_rect_start_pos;
	// Update is called once per frame
	void Update () {
		//select
		if (Input.GetMouseButtonUp(0)){
			mode_node=false;
			if (node_pressed)
				DeselectNode();
			node_pressed=false;
			selection_rect_on=false;
		}
		
		if (Input.GetMouseButton(0)){
			if (mode_node){
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
		}
		
		if (Input.GetMouseButtonDown(0)){
			//check for units
			var unit=RaycastUnit();
			
			if (unit!=null){
				SelectUnit(unit);
				return;
			}
			
			if (_controlMode==ControlMode.None){
				selection_rect_on=true;
				selection_rect_start_pos=Input.mousePosition;
			}
			else
			if (_controlMode==ControlMode.CreatePaths){
				var node=RaycastPathNode();
				
				if (node!=null){
					mode_node=true;
					if (selected_node==node){
						node_pressed=true;
					}
					else{
						SelectNode(node);
					}
					
					if (GroundPosition(out temp_ground_pos))
						temp_selection_offset=node.transform.position-temp_ground_pos;
					return;
				}
			}
			
			DeselectUnit();
			DeselectNode();
		}
		
		//control actions
		if (Input.GetMouseButtonDown(1)){
			
			if (_controlMode==ControlMode.None){
				if (selected_unit!=null){
					
					var node=RaycastPathNode();
					if (node!=null){
						selected_unit.Move(node);
					}
					else{
						if (GroundPosition(out temp_ground_pos))
							selected_unit.Move(temp_ground_pos);
					}
					
				}
			}
			else
			if (_controlMode==ControlMode.CreatePaths){
				//check for ground.
				if (GroundPosition(out temp_ground_pos)){
					var node=RaycastPathNode();
					if (node!=null){
						//link to this node
						selected_node.AddForwardNode(node);
					}
					else
						CreatePathNode(temp_ground_pos+Vector3.up*0.1f);
				}
			}
		}
		
		//other
		
		if (Input.GetKeyDown(KeyCode.Alpha1)){
			SetMode(ControlMode.None);
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha2)){
			SetPathNode(!(_controlMode==ControlMode.CreatePaths));
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha3)){
			if (GroundPosition(out temp_ground_pos)){
				Instantiate(Unit_prefab,temp_ground_pos+Vector3.up*2,Quaternion.identity);
			}
		}
		
		if (Input.GetKeyDown(KeyCode.Delete)){
			if (selected_node!=null)
				selected_node.Delete();
		}
		
		if (Input.GetKeyDown(KeyCode.Space)){
			ToggleTimeState();
		}
	}
	
	UnitMain RaycastUnit(){
		int mask=1<<LayerMask.NameToLayer("Unit");
		RaycastHit info;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out info,500f,mask))
			return info.collider.gameObject.GetComponent<UnitMain>();
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
			hud_controller.SetPathMode(true);
			DeselectUnit();
		}
	}
	
	void SetPathNode(bool on){
		if (on){
			SetMode(ControlMode.CreatePaths);
		}
		else
			SetMode(ControlMode.None);
	}
	
	void SelectUnit (UnitMain unit)
	{
		DeselectNode();
		SetMode(ControlMode.None);
		selected_unit=unit;
		selection_circle.gameObject.SetActive(true);
		selection_circle.SetTarget(unit.gameObject);
	}
	
	void SelectNode (PathNodeMain node)
	{
		DeselectNode();
		selected_node=node;
		selected_node.setSelected(true);
	}

	void DeselectUnit()
	{
		selected_unit=null;
		selection_circle.gameObject.SetActive(false);
	}
	
	void DeselectNode()
	{
		if (!selected_node) return;
		selected_node.setSelected(false);
		selected_node=null;
	}

	void ToggleTimeState ()
	{
		time_state=!time_state;
		if (time_state){
			Time.timeScale=1;
		}
		else{
			Time.timeScale=0.0000001f;
		}
	}

	void CreatePathNode(Vector3 position)
	{
		var o=Instantiate(PathNode_prefab,position,Quaternion.identity) as GameObject;
		var n=o.GetComponent<PathNodeMain>();
		if (selected_node!=null){
			selected_node.AddForwardNode(n);
		}
		
		SelectNode(n);
	}
}
