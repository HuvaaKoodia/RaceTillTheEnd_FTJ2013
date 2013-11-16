using UnityEngine;
using System.Collections;

public class PathLineMain : MonoBehaviour {
	
	public LineRenderer Line;
	PathNodeMain n1,n2;
	Transform start,end;
	public Color start_color,selected_color;

	public float LineWidth=2;

	CapsuleCollider capsule;
	
	public PathNodeMain ForwardNode{get{return n2;}}
	
	void Start () {
		capsule=GetComponent<CapsuleCollider>();
		//capsule = gameObject.AddComponent(typeof(CapsuleCollider)) as CapsuleCollider;
		capsule.radius = LineWidth *0.5f;
		capsule.center = Vector3.zero;
		capsule.direction = 2;

		SetSelected(false);
	}
	void Update () {
		if (capsule!=null){
			Line.SetPosition(0,n1.transform.position);
			Line.SetPosition(1,n2.transform.position);

			capsule.transform.position = start.position + (end.position - start.position) *0.5f;
			capsule.transform.LookAt(start.position);
			capsule.height = (end.position - start.position).magnitude;
		}
	}
	
	public void SetNodes(PathNodeMain node1,PathNodeMain node2){
		n1=node1;
		n2=node2;
		start=n1.transform;
		end=n2.transform;
		Update();
	}

	public bool CheckNode (PathNodeMain node)
	{
		return n1==node||n2==node;
	}

	public void SetSelected (bool on)
	{
		if (on){
			Line.SetColors(selected_color,selected_color);
		}
		else{
			Line.SetColors(start_color,start_color);
		}
	}

	public void Delete ()
	{
		n1.RemoveForwardNode(n2);
		n2.RemoveBackwardNode(n1);
	}
}
