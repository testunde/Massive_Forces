﻿using UnityEngine;
using System.Collections;
using Scripts;

namespace Database{
	public class Acr_BuildBuilding : A_Create {
		private Database.IO_Database buildingClass;
		
		public Acr_BuildBuilding(string targetBuilding) : base(){
			this.buildingClass=(IO_Database)System.Activator.CreateInstance(System.Type.GetType("Database."+targetBuilding));
			this.name="Build "+buildingClass.name;
			this.costs=buildingClass.costs;
			//set icon
		}
		
		public override void setObject(IngameObject obj){
			base.setObject(obj);
			this.time=buildingClass.buildTime/(float)obj.workerUnits;
		}
		
		public override void begin(){
			if(isDoable()){
				IO_Building building=new IO_Building();
				building.loadType(buildingClass,obj.fraction);
				building.timeRemaining=buildingClass.buildTime/(float)obj.workerUnits;
				building.createdBy=this;
				building.initModel();
				if(frCtrl.RSC_costsAvailable(obj.fraction,buildingClass.costs) && !building.actionBeh.areNonUnits())
					building.setPreview(true);
				else
					building.setPreview(false);
				objCtrl.startBuild(building);
			}
		}
		
		public override bool create(IngameItem item){
			if(item is IO_Building){
				IO_Building building=(IO_Building)item;
				if(frCtrl.RSC_costsAvailable(obj.fraction,buildingClass.costs) && !building.actionBeh.areNonUnits()){
					production.addItem(building);
					frCtrl.RSC_changeBy(obj.fraction,buildingClass.costs);
					building.build();
					return true;
				}else{
					Debug.Log("Not enough resources or IngameObject blocks build process!");
				}
			}else{
				Debug.Log("Called "+this.name+".create() with wrong IO_ class!");
			}
			return false;
		}
		
		public override void abort(IngameItem item){
			if(item is IO_Building){
				IO_Building building=(IO_Building)item;
				frCtrl.RSC_changeBy(obj.fraction,building.costs,true);
				building.abortBuild();
			}else{
				Debug.Log("Called "+this.name+".abort() with wrong IO_ class!");
			}
		}
		
		public override void finish(IngameItem item){
			if(item is IO_Building){
				IO_Building building=(IO_Building)item;
				building.finishedBuild();
			}else{
				Debug.Log("Called "+this.name+".finish() with wrong IO_ class!");
			}
		}
		
		public override bool isDoable(){
			return objCtrl.buildState==0;
		}
	}
}
