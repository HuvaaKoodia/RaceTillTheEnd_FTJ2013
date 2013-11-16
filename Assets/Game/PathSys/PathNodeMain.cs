using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathNodeMain : MonoBehaviour {
	
	public delegate void PathNodeDestroyed(List<PathNodeMain> forward_nodes);
	
	public GameObject PathLine_prefab;

	public List<PathNodeMain> ForwardNodes {get{return forward_nodes;}}
	public List<PathNodeMain> BackwardNodes {get{return backward_nodes;}}

	List<PathNodeMain> forward_nodes=new List<PathNodeMain>();
	List<PathNodeMain> backward_nodes=new List<PathNodeMain>();
	List<PathLineMain> path_lines=new List<PathLineMain>();
	
	public GameObject graphics;
	
	public System.Action PathNodeMovedEvent;
	public System.Action PathNodeNewForwardNodesEvent;
	public PathNodeDestroyed OnPathNodeDestroyedEvent;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void AddForwardNode(PathNodeMain node){
		if (forward_nodes.Contains(node)||backward_nodes.Contains(node)) return;
		forward_nodes.Add(node);
		node.backward_nodes.Add(this);
		var o=Instantiate(PathLine_prefab,transform.position,Quaternion.identity) as GameObject;
		var line=o.GetComponent<PathLineMain>();
		line.SetNodes(this,node);
		path_lines.Add(line);
		
		if (PathNodeNewForwardNodesEvent!=null)
			PathNodeNewForwardNodesEvent();
	}
	
	public void RemoveForwardNode(PathNodeMain node){
		/*if (path_lines.ContainsKey(node)){
			Destroy(path_lines[node]);
			path_lines.Remove(node);
			
		}*/
		
		for (int i=0;i<path_lines.Count;i++){
			if (path_lines[i].ForwardNode==node){
				Destroy(path_lines[i].gameObject);
				path_lines.RemoveAt(i);
			}
		}
		forward_nodes.Remove(node);
	}
	
	public void RemoveBackwardNode(PathNodeMain node){

		backward_nodes.Remove(node);
	}

	public bool HasForwardNodes()
	{
		return forward_nodes.Count>0;
	}

	public bool HasBackwardNodes ()
	{
		return backward_nodes.Count>0;
	}

	public PathNodeMain GetNextNode ()
	{
		return forward_nodes[Subs.GetRandom(forward_nodes.Count)];
	}
	
	public void OnDestroy(){
		//remove forward lines
		/*
		foreach(var line in path_lines){
			if (line.Value!=null)
			Destroy(line.Value.gameObject);
		}
		*/
		path_lines.Clear();
	}

	public void setSelected (bool on)
	{
		if (on){
			graphics.renderer.material.color=Color.green;
		}
		else{
			graphics.renderer.material.color=Color.blue;
		}
	}

	public void Delete ()
	{
		//remove forward lines
		foreach(var line in path_lines){
			Destroy(line.gameObject);
		}
		
		if (OnPathNodeDestroyedEvent!=null){
			OnPathNodeDestroyedEvent(forward_nodes);
		}
		
		foreach (var f in forward_nodes){
			f.RemoveBackwardNode(this);
		}
		
		foreach (var b in backward_nodes){
			//remove backward link
			b.RemoveForwardNode(this);
			//link all forward nodes to backward node
			foreach (var f in forward_nodes){
				b.AddForwardNode(f);
			}
		}
		Destroy(gameObject);
	}
	
	public void Reposition(Vector3 ground_pos){
		transform.position=ground_pos+Vector3.up*0.1f;
		if (PathNodeMovedEvent!=null){
			PathNodeMovedEvent();
		}
	}
}
