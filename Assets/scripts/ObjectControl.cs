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
			//behavior of selection
			if(selectState>0&&inputMod.rightDown){
				marker.abort();
				selectState=0;
			}
			switch(selectState){
				case 0:{
					if(inputMod.leftDown&&buildState==0){
						marker.begin(inputMod.pointer);
						selectState=1;
					}
					break;
				}case 1:{
					if(!inputMod.leftUp){
						marker.scaleX(inputMod.pointer);
					}else{
						marker.setX(inputMod.pointer);
						selectState=2;
					}
					break;
				}case 2:{
					if(!inputMod.leftDown){
						marker.scaleY(inputMod.pointer);
					}else{
						marker.finish(inputMod.pointer);
						selectState=0;
					}
					break;
				}default:{
					selectState=0;
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
					currentBuild.setPreview();
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
					
					if(inputMod.leftDown){
						currentBuild.build(1);
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
		}
	}
}
