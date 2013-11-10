using UnityEngine;
using System.Collections;

public class PathLineMain : MonoBehaviour {
	
	public LineRenderer Line;
	PathNodeMain n1,n2;
	
	
	public PathNodeMain ForwardNode{get{return n2;}}
	
	void Start () {
	
	}
	
	
	void Update () {
		Line.SetPosition(0,n1.transform.position);
		Line.SetPosition(1,n2.transform.position);
	}
	
	public void SetNodes(PathNodeMain node1,PathNodeMain node2){
		n1=node1;
		n2=node2;
		Update();
	}

	public bool CheckNode (PathNodeMain node)
	{
		return n1==node||n2==node;
	}
}
