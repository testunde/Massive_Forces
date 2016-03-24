using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
 //copied from:
 //http://wiki.unity3d.com/index.php?title=FramesPerSecond
public class FPSDisplay : MonoBehaviour
{
	float deltaTime = 0.0f;
	Queue<float> times;
	
	void Start(){
		times=new Queue<float>();
	}
 
	void Update()
	{
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		if(times.Count>90)
			times.Dequeue();
		times.Enqueue(deltaTime);
	}
 
	void OnGUI()
	{
		int w = Screen.width, h = Screen.height;
 
		GUIStyle style = new GUIStyle();
 
		Rect rect = new Rect(0, 0, w, h * 2 / 100);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 100;
		style.normal.textColor = new Color (0.0f, 1.0f, 0.0f, 1.0f);
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		float fpsSmooth=0f;
		foreach(float f in times){
			fpsSmooth+=f;
		}
		fpsSmooth=1f/(fpsSmooth/times.Count);
		string text = string.Format("{0:0.0} ms ({1:0.} fps) >> {2:0.0}", msec, fps, fpsSmooth);
		GUI.Label(rect, text, style);
	}
}
