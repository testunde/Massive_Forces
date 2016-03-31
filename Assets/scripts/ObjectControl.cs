using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Scripts;

namespace Scripts {
	public class ObjectControl : MonoBehaviour {
		private static InputModul inputMod;
		private static SelectionControl selection;
		private static Res resources;
		private MarkerControl marker;
		private int selectState=0,buildState=0,unitState=0;
		private float interval;
		private int sc=0,dc;	//1-second count; double-click count
		private string targetUnit=null;
		private IO_Building currentBuild=null;
		private IO_Unit currentUnit=null;
		public List<IngameObject> units=new List<IngameObject>();
		public List<IngameObject> buildings=new List<IngameObject>();
		public List<IngameObject> neutrals=new List<IngameObject>();
		
		public void startBuild(IO_Building building){
			if(buildState==0)
				currentBuild=building;
		}
		
		public void placeUnit(string unit){
			if(unitState==0)
				targetUnit=unit;
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
			selection=SelectionControl.getInstance();
			resources=Res.getInstance();
			marker=GameObject.Find("SelectionMarker").GetComponent<MarkerControl>();
			interval=1/Time.fixedDeltaTime;
			dc=(int)(interval*2);
		}
		
		private void initBuilding(){
			IO_Building building=new IO_Building();
			building.loadType("IOb_testBuilding");
			building.initModel();
			building.setCoords(gameObject.GetComponent<CameraControl>().getCoordsAtXZ(new Vector3(5f,-10f,5f)));
			building.build();
			building.finishedBuild();
			building.fraction=1;
		}
		
		void Update(){
			interval=1/Time.fixedDeltaTime;	//returns the set FixedUpdate in Hz [1/0.02s=50Hz]
			
			//for 1-second-interval calls
			if(sc%(int)(interval*1f)==0){
				sc-=(int)(interval*1f);
			}
			sc++;
			
			//abort selection process
			if(selectState>0&&inputMod.cancel){
				marker.abort();
				selectState=0;
			}
			//behavior of selection
			switch(selectState){
				case 0:
					if(inputMod.leftDown&&buildState==0&&unitState==0){
						if(inputMod.shiftHold)
							marker.add=true;
						if(inputMod.ctrlHold)
							marker.remove=true;
						marker.begin(inputMod.pointer);
						selectState=1;
					}
					break;
				case 1:
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
				case 2:
					if(!inputMod.rightUp){
						marker.scaleY(inputMod.pointer);
					}else{
						marker.finish(inputMod.pointer);
						goto case 99;
					}
					break;
				case 99:	//reset
				default:
					selectState=0;
					marker.add=false;
					marker.remove=false;
					break;
			}
			//manage double click on object to select all same objects in camera view
			if(inputMod.leftDown){
				if(dc<interval*.7f){	//you have 0.7 seconds time for the second click
					//deactivated because of failure!
					// selection.selectSameObjectsInView();
				}
				dc=0;
			}
			dc++;
			
			if(inputMod.tDown){
				IngameObject sel=selection.getIfOnlyOne();
				if(sel!=null){
					int k=1;
					//create several units if shift is hold down
					if(inputMod.shiftHold)
						k=5;
					for(int i=0;i<k;i++)
						sel.actions.getAction(1,0).begin();
				}
			}
			if(inputMod.gDown){
				IngameObject sel=selection.getIfOnlyOne();
				if(sel!=null){
					sel.actions.getAction(1,1).begin();
				}
			}
			
			//test building process with IOb_testBuilding
			if(inputMod.fDown)
				initBuilding();
			
			//behavior of build process
			switch(buildState){
				case 0:	//start building process if building string is set
					if(currentBuild!=null&&selectState==0&&unitState==0){
						buildState=1;
						//set here the coords, so if shift and right-click is hold down it spawns at the mouse pointer
						currentBuild.setCoords(inputMod.pointer);
						goto case 1;
					}
					break;
				case 1:
					//rotate if right click, else just follow the mouse pointer
					if(inputMod.rightHold)
						currentBuild.rotateTo(inputMod.pointer);
					else
						currentBuild.setCoords(inputMod.pointer);
					
					if(inputMod.leftDown&&currentBuild.createdBy.create(currentBuild)){
						buildings.Add(currentBuild);
						buildState=0;	//set state to 55 as 'is builded' code
						
						//repeat if shift is hold while clicked
						if(inputMod.shiftHold)
							currentBuild.createdBy.begin();
						else
							currentBuild=null;
					}else if(inputMod.cancel){
						//abort building process when pressed esc or mouse2
						currentBuild.createdBy.abort(currentBuild);
						currentBuild=null;
						buildState=0;	//set state to 99 as cancel code
					}
					break;
				default:	//mostly for state reset
					currentBuild=null;
					buildState=0;
					break;
			}
			
			//test unit placing with IOu_testUnit
			if(inputMod.rDown)
				placeUnit("IOu_testUnit");
			
			//behavior to place unit
			switch(unitState){
				case 0:	//start placing process if unit string is set
					if(targetUnit!=null&&selectState==0&&buildState==0){
						unitState=1;
						goto case 1;
					}
					break;
				case 1:	//seperat case, so its possible to repeat this state immediately
					currentUnit=new IO_Unit();
					currentUnit.loadType(targetUnit);
					currentUnit.initModel();
					currentUnit.createUnit(1);
					unitState=2;
					//set here the coords, so if shift and right-click is hold down it spawns at the mouse pointer
					currentUnit.setCoords(inputMod.pointer);
					break;
				case 2:
					//rotate if right click, else just follow the mouse pointer
					if(inputMod.rightHold)
						currentUnit.rotateTo(inputMod.pointer);
					else
						currentUnit.setCoords(inputMod.pointer);
					
					if(inputMod.leftDown&&resources.costsAvailable(1,currentUnit.costs)){
						units.Add(currentUnit);
						resources.changeBy(1,currentUnit.costs);	//set resources
						//repeat if shift is hold while clicked
						if(inputMod.shiftHold)
							unitState=1;
						else
							unitState=55;	//set state to 55 as 'is builded' code
					}else if(inputMod.cancel){
						//abort building process when pressed esc or mouse2
						currentUnit.deleteModel();
						unitState=99;	//set state to 99 as cancel code
					}
					break;
				default:	//mostly for state reset
					targetUnit=null;
					currentUnit=null;
					unitState=0;
					break;
			}
		}
	}
}
