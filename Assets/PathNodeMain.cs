using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathNodeMain : MonoBehaviour {
	
	public GameObject PathLine_prefab;
	
	List<PathNodeMain> links=new List<PathNodeMain>();
	List<PathLineMain> path_lines=new List<PathLineMain>();
	
	public GameObject graphics;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void AddLink(PathNodeMain node){
		links.Add(node);
		var o=Instantiate(PathLine_prefab,transform.position,Quaternion.identity) as GameObject;
		var line=o.GetComponent<PathLineMain>();
		line.SetNodes(this,node);
		path_lines.Add(line);
	}

	public bool HasLinks ()
	{
		return links.Count>0;
	}

	public PathNodeMain GetNextNode ()
	{
		return links[0];
	}
	
	public void OnDestroy(){
		for(int i=0;i<path_lines.Count;i++){
			var l=path_lines[i];
			if (l!=null&&l.CheckNode(this)){
				
				Destroy(l.gameObject);
				i--;
			}
			path_lines.Remove(l);
		}
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
}
