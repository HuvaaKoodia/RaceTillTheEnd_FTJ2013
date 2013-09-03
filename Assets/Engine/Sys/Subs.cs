using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Subs{
	

	#region Random Subs
	
	/// <summary>
	/// Random int from 0 to max (ex).
	/// </summary>
	public static int GetRandom(int max){
		return Random.Range(0,max);
	}
	
	/// <summary>
	/// Random float from 0 to max (ex).
	/// </summary>
	public static float GetRandom(float max){
		return Random.Range(0f,max);
	}
	
	/// <summary>
	/// Random float from 0f to 1f(ex).
	/// </summary>
	public static float RandomFloat(){
		return Random.Range(0f,1f);
	}
	/// <summary>
	/// Random int from 0 to 100(ex)
	/// </summary>
	public static float RandomPercent(){
		return Random.Range(0,100);
	}
	
	public static bool RandomBool(){
		if (RandomPercent()<50)
			return true;
		return false;
	}
	/// <summary>
	/// Random vector3. All values from -1f to 1f.
	/// </summary>

	public static Vector3 RandomVector3(){
		return new Vector3(Random.Range(-1f,1),Random.Range(-1f,1),Random.Range(-1f,1));
	}
	
	public static Color RandomColor(){
		return new Color(Random.Range(0,1f),Random.Range(0,1f),Random.Range(0,1f));
	}
	public static Color RandomColor(float alpha){
		return new Color(Random.Range(0,1f),Random.Range(0,1f),Random.Range(0,1f),alpha);
	}
	
	#endregion

	public static void SendMessageRecursive(Transform tform,string message){
		
		foreach(Transform t in tform){
			t.SendMessage(message,SendMessageOptions.DontRequireReceiver);
			SendMessageRecursive(t,message);
		}
	}

	public static Vector3 LengthDir(Transform transform,Vector3 dir)
	{
		return transform.position+transform.TransformDirection(dir);
	}
	
	/// <summary>
	/// Wraps the specified number according to min and max.
	/// Max exclusive.
	/// </summary>
	public static int Wrap(int number, int min, int max)
	{
		var b=number%(max-min);
		if (b>=0)
			return min+b;
		return max+b;
	}
	
	/// <summary>
	/// Adds and wraps the specified number according to min and max.
	/// Max exclusive.
	/// </summary>
	public static int Add(int number, int min, int max)
	{
		return Add(number,1,min,max);
	}
	
	/// <summary>
	/// Adds and wraps the specified number according to min and max.
	/// Max exclusive.
	/// </summary>
	public static int Add(int number,int amount, int min, int max)
	{
		return Wrap (number+amount,min,max);
	}

	public static Vector3 ClampVector3 (Vector3 vector, Vector3 max)
	{
		var v=vector;
		if (v.x>max.x)
			v.x=max.x;
		if (v.y>max.y)
			v.y=max.y;
		if (v.z>max.z)
			v.z=max.z;
		return v;
	}

	public static Vector3 Vector3Multi (Vector3 v1, Vector3 v2)
	{
		return new Vector3(v1.x*v2.x,v1.y*v2.y,v1.z*v2.z);
	}
	
	public static IEnumerable<T> EnumValues<T>() {
    	return System.Enum.GetValues(typeof(T)).Cast<T>();
  	}
	
	public static T GetRandom<T>(IEnumerable<T> enumerable){
		return enumerable.ElementAt(Random.Range(0,enumerable.Count()));
	}
	
	//area
    public static bool insideArea(Vector2 Position, Rect area)
    {
        return (Position.x >= area.x && Position.x < area.x + area.width && Position.y >= area.y && Position.y < area.y + area.height);
    }

    public static bool outsideArea(Vector2 Position, Rect area)
    {
        return !insideArea(Position,area);
    }
	
	public static void ChangeColor(Transform t, Color color){
		if (t.renderer!=null){
			t.renderer.material.color=color;
		}
		foreach (Transform tr in t){
			ChangeColor(tr,color);
		}
	}
	
	
	//POINT
	/*public struct Point
	{
	    public int x, y;
	 
	    public Point(int px, int py)
	    {
	        x = px;
	        y = py;
	    }
	}*/
	
	/*
    public static bool insideArea(Point Position, Rect area)
    {
        return (Position.X >= area.X && Position.X < area.X + area.Width && Position.Y >= area.Y && Position.Y < area.Y + area.Height);
    }

    public static bool outsideArea(Point Position, Rect area)
    {
        return !insideArea(Position,area);
    }
    */
}
