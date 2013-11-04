using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	
	public FollowTarget selection_circle;
	public GameObject PathNode_prefab;
	
	PathNodeMain selected_path_node;
	UnitMain selected_unit;
	PathNodeMain selected_node;
	bool time_state=true;
	
	// Use this for initialization
	void Start () {
		DeselectUnit();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetMouseButtonDown(0)){
			//check for units
			var unit=RaycastUnit();
			
			if (unit!=null){
				SelectUnit(unit);
				return;
			}

			var node=RaycastPathNode();
			
			if (node!=null){
				if (selected_node==node)
					DeselectNode();
				else
					SelectNode(node);
				return;
			}
			
			DeselectUnit();
		}
		if (Input.GetMouseButtonDown(1)){
			if (selected_unit!=null){
				//check for ground.
				var p=Vector3.zero;
				
				if (GroundPosition(out p)){					
					//movement
					CreatePathNode(p+Vector3.up*0.1f);
					if (!selected_unit.Moving){
						selected_unit.Move(selected_path_node);
					}
				}
			}
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

	void SelectUnit (UnitMain unit)
	{
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
		if (selected_path_node!=null){
			selected_path_node.AddLink(n);
		}
		
		selected_path_node=n;
	}
}
