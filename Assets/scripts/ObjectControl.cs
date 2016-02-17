using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Scripts;

namespace Scripts {
	public class ObjectControl : MonoBehaviour {
		private static InputModul inputMod;
		private static Res resources;
		private MarkerControl marker;
		private int selectState=0,buildState=0;
		private float interval;
		private int c=0;
		private string targetBuilding=null;
		private IO_Building currentBuild=null;
		public List<IngameObject> units=new List<IngameObject>();
		public List<IngameObject> buildings=new List<IngameObject>();
		public List<IngameObject> neutrals=new List<IngameObject>();
		public List<IO_Building> buildQueue=new List<IO_Building>();
		
		public void startBuild(string building){
			if(buildState==0)
				targetBuilding=building;
		}
		
		public void buildFinished(int buildingID){
			foreach(IO_Building building in buildQueue){
				if(building.ID==buildingID){
					buildQueue.Remove(building);
					break;
				}
			}
		}
		
		public bool makeUnit(int buildingID,string unit){
			bool result=false;
			foreach(IO_Building building in buildings){
				if(building.ID==buildingID){
					result=true;
					//MAKE UNIT
					break;
				}
			}
			return result;
		}
		
		void Start(){
			inputMod=InputModul.getInstance();
			resources=Res.getInstance();
			marker=GameObject.Find("SelectionMarker").GetComponent<MarkerControl>();
		}
		
		void Update(){
			interval=1/Time.fixedDeltaTime;	//returns the setted FixedUpdate in Hz [1/0.02s=50Hz]
			c++;
			
			//abort selection process
			if(selectState>0&&inputMod.cancel){
				marker.abort();
				selectState=0;
			}
			//behavior of selection
			switch(selectState){
				case 0:{
					if(inputMod.leftDown&&buildState==0){
						if(inputMod.shiftHold)
							marker.add=true;
						if(inputMod.ctrlHold)
							marker.remove=true;
						marker.begin(inputMod.pointer);
						selectState=1;
					}
					break;
				}case 1:{
					if(!inputMod.leftUp){
						marker.scaleX(inputMod.pointer);
					}else{
						if(!inputMod.rightHold){
							marker.finish(inputMod.pointer);
							goto case 99;
						}else{
							marker.setX(inputMod.pointer);
							selectState=2;
						}
					}
					break;
				}case 2:{
					if(!inputMod.rightUp){
						marker.scaleY(inputMod.pointer);
					}else{
						marker.finish(inputMod.pointer);
						goto case 99;
					}
					break;
				}case 99:	//reset
				default:{
					selectState=0;
					marker.add=false;
					marker.remove=false;
					break;
				}
			}
			
			//test building process with IOb_testBuilding
			if(inputMod.fDown)
				startBuild("IOb_testBuilding");
			
			//behavior of build process
			switch(buildState){
				case 0:{	//start building process if building string is set
					if(targetBuilding!=null&&selectState==0){
						buildState=1;
						goto case 1;
					}
					break;
				}case 1:{	//seperat case, so its possible to repeat this state immediately
					currentBuild=(IO_Building)Activator.CreateInstance(Type.GetType("Scripts."+targetBuilding));
					currentBuild.setPreview(1);
					buildState=2;
					//set here the coords, so if shift and right-click is hold down it spawns at the mouse pointer
					currentBuild.setCoords(inputMod.pointer);
					break;
				}case 2:{
					//rotate if right click, else just follow the mouse pointer
					if(inputMod.rightHold)
						currentBuild.rotateTo(inputMod.pointer);
					else
						currentBuild.setCoords(inputMod.pointer);
					
					if(inputMod.leftDown&&resources.costsAvailable(1,currentBuild.costs)){
						currentBuild.build();
						buildings.Add(currentBuild);
						buildQueue.Add(currentBuild);
						resources.changeBy(1,currentBuild.costs);	//set resources
						//repeat if shift is hold while clicked
						if(inputMod.shiftHold)
							buildState=1;
						else
							buildState=55;	//set state to 55 as 'is builded' code
					}else if(inputMod.cancel){
						//abort building process when pressed esc or mouse2
						currentBuild.abortBuild();
						buildState=99;	//set state to 99 as cancel code
					}
					break;
				}default:{	//mostly for state reset
					targetBuilding=null;
					currentBuild=null;
					buildState=0;
					break;
				}
			}
			
			if(c%(int)(interval*1f)==0){
				c-=(int)(interval*1f);
				//foreach(IO_Building building in buildQueue){
				for(int i=0;i<buildQueue.Count;i++){
					IO_Building building=buildQueue[i];
					if(building.timeRemaining<=0){
						building.finishedBuild();
						buildQueue.Remove(building);
					}else{
						building.timeRemaining--;
					}
				}
			}
		}
	}
}
