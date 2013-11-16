using UnityEngine;
using System.Collections;

//map DB laxy hax
using System.Collections.Generic;

public class MapData{
	public int[,] map_data;

	public MapData(int w,int h){
		map_data=new int[w,h];
	}

}

public class MapController : MonoBehaviour {

	public List<MapData> Maps=new List<MapData>();

	public GameObject CameraGo;

	public AstarPath AStar;
	public RaceController raceController;
	public GameObject Building_prefab,Obstacle_prefab,CheckPoint_prefab;
	public int TileSize =10,GridWidth=10,GridHeight=10;

	GameObject[,] BuildingGrid;
	bool[,] PosGrid;

	Timer obs_timer;

	public bool ActionModeStarted=false;

	Vector2 RotateArrayVector2(Vector2 pos, Vector2 array_size, float angle){
		//array_size-=Vector2.one;
		var m=Matrix4x4.TRS(Vector3.zero,Quaternion.AngleAxis(angle,Vector3.forward),Vector3.one);
		var v=m.MultiplyVector(new Vector3(pos.x,pos.y));

		int xx=Mathf.RoundToInt(v.x),ax=Mathf.RoundToInt(array_size.x);
		int yy=Mathf.RoundToInt(v.y),ay=Mathf.RoundToInt(array_size.y);

		if (angle==90||angle==180) xx--;
		if (angle==180||angle==270) yy--;

		int x=Subs.Wrap(xx,0,ax);
		int y=Subs.Wrap(yy,0,ay);
		return new Vector2(x,y);
	}

	// Use this for initialization
	void Start () {
		BuildingGrid=new GameObject[GridWidth,GridHeight];
		PosGrid=new bool[GridWidth,GridHeight];
		obs_timer=new Timer(5000);
		//loading map

		Vector2 map_pos=Vector2.zero;
		for (int m=0;m<4;m++){
			map_pos=new Vector2((m%2)*5,(m/2)*5);

			int angle=Subs.GetRandom(new int[]{0,90,180,270});

			var map=Subs.GetRandom(Maps);
			int w=map.map_data.GetLength(1);
			int h=map.map_data.GetLength(0);
			Vector2 size=new Vector2(w,h);
			for (int y=0;y<h;y++)
			{
				for (int x=0;x<w;x++)
				{
					Vector2 pos=new Vector2(x,y);

					pos=RotateArrayVector2(pos,size,angle);

					pos.y=h-1-pos.y;

					pos+=map_pos;

					if (map.map_data[y,x]==1){
						AddBuilding((int)pos.x,(int)pos.y);
					}
					if (map.map_data[y,x]==2){
						AddCheckPoint((int)pos.x,(int)pos.y);
					}
				}
			}

		}

		var goal=raceController.CheckPoints[Subs.GetRandom(raceController.CheckPoints.Count)];
		goal.IsGoal=true;
		raceController.StartPos=goal;

		raceController.CreateCars();

		CameraGo.transform.position=
			new Vector3(raceController.StartPos.transform.position.x,
			            CameraGo.transform.position.x,
			            raceController.StartPos.transform.position.z
			            );

		AStar.Scan();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.B)){
			Vector2 pos=Vector3.zero;
			if (GetMouseTilePos(out pos)){
				int x=(int)pos.x,y=(int)pos.y;
				AddBuilding(x,y);
            }
		}

		if (Input.GetKeyDown(KeyCode.V)){
			ActionModeStarted=true;
		}

		if (Input.GetKeyDown(KeyCode.C)){
			for (int i=0;i<35;i++){
				AddBuilding(Subs.GetRandom(GridWidth),Subs.GetRandom(GridHeight));
			}
			AStar.Scan();
		}

		if (Input.GetKeyDown(KeyCode.X)){
			AStar.Scan();
		}

		if (ActionModeStarted){
			obs_timer.Update();

			if (obs_timer.Done){
				CreateRandomObstacle();
				obs_timer.Reset();
			}

		}
	}

	void AddBuilding(int x,int y){
		if (PosGrid[x,y]){
			PosGrid[x,y]=false;
			Destroy(BuildingGrid[x,y]);
			BuildingGrid[x,y]=null;
		}
		else{
			PosGrid[x,y]=true;

			var go=Instantiate(Building_prefab,new Vector3(x*10,0,y*10),Quaternion.identity) as GameObject;
			BuildingGrid[x,y]=go;
		}
	}

	private void CreateRandomObstacle(){
		int rx=Subs.GetRandom(GridWidth);
		int ry=Subs.GetRandom(GridHeight);

		Vector3 rv=new Vector3(1,0,1)*2.5f+new Vector3(Subs.GetRandom(5f),0,Subs.GetRandom(5f));

		var go=Instantiate(Obstacle_prefab,new Vector3(rx*TileSize,170,ry*TileSize)+rv,Quaternion.identity) as GameObject;

		if (obs_timer.Delay>500)
			obs_timer.Delay-=100;

	}

	private bool GetMouseTilePos(out Vector2 tile){
		var ray=Camera.main.ScreenPointToRay(Input.mousePosition);
		int mask=1<<LayerMask.NameToLayer("Ground");
		RaycastHit info;
		if (Physics.Raycast(ray,out info,500,mask)){
			//calculate correct tile
			int x=(int)Mathf.Floor(info.point.x/TileSize);
			int y=(int)Mathf.Floor(info.point.z/TileSize);
			
			tile=new Vector2(x,y);
			return true;
		}
		tile=new Vector2(0,0);
		return false;
	}

	public void AddCheckPoint (int x, int y)
	{
		if (PosGrid[x,y]){

		}
		else{

			var go=Instantiate(CheckPoint_prefab,new Vector3(x*10,0,y*10),Quaternion.identity) as GameObject;
			raceController.CheckPoints.Add(go.GetComponent<CheckPointMain>());
		}
	}
}
