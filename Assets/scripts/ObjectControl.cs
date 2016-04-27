using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Scripts;

namespace Scripts {
	public class ObjectControl : MonoBehaviour {
		private static InputModul inputMod;
		private static SelectionControl selection;
		private static FractionControl frCtrl;
		private MarkerControl marker;
		public int selectState=0,buildState=0,unitState=0;
		private float interval;
		private int sc=0,dc;	//1-second count; double-click count
		private string targetUnit=null;
		private IO_Building currentBuild=null;
		private IO_Unit currentUnit=null;
		public List<IngameObject> units=new List<IngameObject>();
		public List<IngameObject> buildings=new List<IngameObject>();
		public List<IngameObject> neutrals=new List<IngameObject>();
		
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
		
		public void startBuild(IO_Building building){
			if(buildState==0)
				currentBuild=building;
		}
		
		public void initBuilding(){	//set the frist building
			IO_Building building=new IO_Building();
			building.loadType("IOb_testBuilding",1);
			building.initModel();
			building.setCoords(inputMod.getCoordsAtXZ(new Vector3(5f,-10f,5f)));
			building.build();
			building.finishedBuild();
			buildings.Add(building);
		}
		
		void Start(){
			inputMod=InputModul.getInstance();
			selection=SelectionControl.getInstance();
			frCtrl=FractionControl.getInstance();
			marker=GameObject.Find("SelectionMarker").GetComponent<MarkerControl>();
			//to prevent the first click as a double click
			interval=1/Time.deltaTime;
			dc=(int)(interval*2);
			inputMod.graph.generate(.5f);
		}
		
		void Update(){
			interval=1/Time.deltaTime;//fixedDeltaTime;	//returns the set FixedUpdate in Hz [1/0.02s=50Hz]
			
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
			
			//print current ActionMatrix
			if(inputMod.cDown){
				IngameObject sel=selection.getIfOnlyOne();
				if(sel!=null){
					for(int i=0;i<ActionMatrix.height;i++){
						string temp="";
						for(int j=0;j<ActionMatrix.width;j++){
							Action ac=sel.actions.getAction(j,i);
							ac.setObject(sel);
							string STATE="";
							if(!ac.isAvailable())STATE+="N";
							STATE+="A ";
							if(!ac.isDoable())STATE+="N";
							STATE+="DA";
							temp+=j+"."+i+">"+ac.name+"["+STATE+"] | ";
						}
						Debug.Log(temp);
					}
				}
			}
			
			if(inputMod.tDown){
				if(selection.areOnlyOneType()!=null){
					int k=1;
					//create several units if shift is hold down
					if(inputMod.shiftHold)
						k=5;
					foreach(IngameObject sel in selection.getSelectedObjects()){
						for(int i=0;i<k;i++)
							sel.actions.getAction(1,0).begin();
					}
				}
			}
			if(inputMod.gDown){
				IngameObject sel=selection.getIfOnlyOne();
				if(sel!=null){
					sel.actions.getAction(1,1).begin();
				}
			}
			if(inputMod.hDown){
				IngameObject sel=selection.getIfOnlyOne();
				if(sel!=null){
					sel.actions.getAction(2,1).begin();
				}
			}
			if(inputMod.mDown){
				IngameObject sel=selection.getIfOnlyOne();
				if(sel!=null){
					sel.actions.getAction(3,2).begin();
				}
			}
			if(inputMod.vDown){
				IngameObject sel=selection.getIfOnlyOne();
				if(sel!=null && sel.actions.getAction(0,2).isDoable()){
					sel.actions.getAction(0,2).begin();
				}
			}
			if(inputMod.rightDown&&buildState==0&&selectState==0&&unitState==0){
				if(selection.areOnlyBuildings() || selection.areOnlyUnits()){
					foreach(IngameObject sel in selection.getSelectedObjects()){
						Database.Acom_SetMeetPoint ac=((Database.Acom_SetMeetPoint)sel.actions.getAction(3,2));
						if(inputMod.shiftHold)
							ac.addPoint(inputMod.pointer);
						else
							ac.setPoint(inputMod.pointer);
					}
				}
			}
			
			//test building process with IOb_testBuilding: start-building
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
					
					//check if resources are still available and set its respective preview
					currentBuild.changePreview(frCtrl.RSC_costsAvailable(1,currentBuild.costs) && !currentBuild.actionBeh.areNonUnits());
					
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
						currentBuild.abortBuild();
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
					currentUnit.loadType(targetUnit,1);
					currentUnit.initModel();
					currentUnit.createUnit();
					currentUnit.actionBeh.enabled=false;
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
					
					if(inputMod.leftDown&&frCtrl.RSC_costsAvailable(1,currentUnit.costs)){
						units.Add(currentUnit);
						frCtrl.RSC_changeBy(1,currentUnit.costs);	//set resources
						currentUnit.actionBeh.enabled=true;
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
			
			//shortcut management
			for(int i=0;i<10;i++){
				if(inputMod.numDown[i]){
					if(inputMod.shiftHold){
						selection.setSCSlot(i);
					}else{
						selection.selectSCSlot(i);
					}
				}
			}
		}
	}
}
