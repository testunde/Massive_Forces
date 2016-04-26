using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scripts;

namespace Scripts {
	public class ActionBehaviour : MonoBehaviour {
		public SelectReact selectReact;
		private static InputModul inputMod;
		public IngameObject connectedObject;
		public List<CollideReact> colliders=new List<CollideReact>();
		private Dictionary<IngameObject,List<CollideReact>> collisions=new Dictionary<IngameObject,List<CollideReact>>();
		
		//automatically disable/enable all scripts if ActionBehaviour gets disabled/enabled
		void OnDisable(){
			if(selectReact!=null)
				selectReact.enabled=false;
			
			foreach(CollideReact cr in colliders)
				cr.enabled=false;
		}
		void OnEnable(){
			if(selectReact!=null)
				selectReact.enabled=true;
			
			foreach(CollideReact cr in colliders)
				cr.enabled=true;
		}
		
		public void addCollision(IngameObject obj,CollideReact col){
			if(collisions.ContainsKey(obj)){
				List<CollideReact> tempList=null;
				if(collisions.TryGetValue(obj,out tempList))
					tempList.Add(col);
			}else{
				List<CollideReact> tempList=new List<CollideReact>();
				tempList.Add(col);
				collisions.Add(obj,tempList);
			}
		}
		
		public void removeCollision(IngameObject obj,CollideReact col){
			if(collisions.ContainsKey(obj)){
				List<CollideReact> tempList=null;
				if(collisions.TryGetValue(obj,out tempList)){
					tempList.Remove(col);
					if(tempList.Count==0)
						collisions.Remove(obj);
				}
			}
		}
		
		//check-method for collision
		public bool areNonUnits(){
			bool result=false;
			foreach(IngameObject obj in collisions.Keys){
				if(obj is IO_Building || obj is IO_Neutral){
					result=true;
					break;
				}
			}
			return result;
		}
		
		void Start(){
			inputMod=InputModul.getInstance();
		}
		
		void FixedUpdate(){
			connectedObject.heartbeat(inputMod.originalFixedDeltaTime);
			if(connectedObject is IO_Unit){
				IO_Unit unit=(IO_Unit)connectedObject;
				if(unit.meetingPoint.Count>0){
					if(!unit.moveTo(unit.meetingPoint.Peek()))
						unit.meetingPoint.Dequeue();
				}
			}
		}
	}
}
