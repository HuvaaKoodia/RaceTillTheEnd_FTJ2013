using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Timer{
	
	//class
	public delegate void TimerEvent();
	public static List<Timer> timers=new List<Timer>(),timers_destroyed=new List<Timer>();
	
	public static void UpdateTimers () {
		foreach (var t in timers){
			t.Update();
			if (t.Destroyed)
				timers_destroyed.Add(t);
		}
		if (timers_destroyed.Count>0){
			foreach (var t in timers_destroyed){
				timers.Remove(t);
			}
			timers_destroyed.Clear();
		}
	}
	
	public static void clearTimers(){
		timers.Clear();
	}
	
	//object
	public bool Active=false;
	
	bool done;
	public bool Done{
		get{
			if (done){
				done=false;
				return true;}
			return false;
		}
		private set{done=value;}
	}
	
	public float Delay{get{return delay;} set{delay=value;}}
	public float Percent{get{return tick/delay;}}
	public float Tick{get{return tick;}}
	public bool Destroyed{get;private set;}
	
	float delay,tick;
	public TimerEvent Timer_Event{get;set;}
	
	
	/// <summary>
	/// Creates an inactive timer.
	/// </param>
	public Timer():this(null,false){}
	/// <summary>
	/// Creates an active timer with a certain delay.
	/// Doesn't add this to the global queue.
	/// </param>
	public Timer(int millis):this(millis,null){}
	
	/// <summary>
	/// Creates an inactive timer.
	/// Addthis adds this to to the global queue.
	/// </param>
	public Timer(TimerEvent te,bool addthis){
		if (addthis) timers.Add(this);
		Timer_Event=te;
		Done=false;
	}
	/// <summary>
	/// Creates an active timer with a certain delay.
	/// Addthis adds this to to the global queue.
	/// </param>
	public Timer(int millis,TimerEvent te,bool addthis):this(te,addthis){
		delay=tick=millis;
		Active=true;
	}
	
	/// <summary>
	/// Creates an active timer with a certain delay.
	/// Doesn't add this to the global queue.
	/// </param>
	public Timer(int millis,TimerEvent te):this(millis,te,false){}
	
	public void Update(float delta){
		if (!Active) return;
		tick-=delta*1000f;
		
		if (tick<=0){
			if (Timer_Event!=null)
				Timer_Event();
			Done=true;
			Reset();
		}
	}
	
	public void Update(){
		Update(Time.deltaTime);
	}
	
	//subs
	
	public void Reset(){
		tick=delay;
	}
	public void Reset(bool active){
		Active=active;
		tick=delay;
	}
	public void Reset(int millis,bool active){
		Delay=millis;
		Active=active;
		tick=delay;
	}
	
	public void Destroy(){
		Destroyed=true;
	}
}
