using UnityEngine;
using System.Collections;
using Scripts;

namespace Database{
	public class Acr_TrainUnit : A_Create {
		private Database.IO_Database unitClass;
		
		public Acr_TrainUnit(string targetUnit) : base(){
			this.unitClass=(IO_Database)System.Activator.CreateInstance(System.Type.GetType("Database."+targetUnit));
			this.name="Train "+unitClass.name;
			this.costs=unitClass.costs;
			//set icon
		}
		
		public override void setObject(IngameObject obj){
			base.setObject(obj);
			this.time=unitClass.buildTime/(float)obj.workerUnits;
		}
		
		public override void begin(){
			if(resources.costsAvailable(obj.fraction,unitClass.costs)){
				IO_Unit unit=new IO_Unit();
				unit.loadType(unitClass);
				unit.timeRemaining=unitClass.buildTime/(float)obj.workerUnits;
				unit.createdBy=this;
				production.addItem(unit);
				resources.changeBy(obj.fraction,unitClass.costs);
			}else{
				Debug.Log("Not enough resources!");
			}
		}
		
		public override void abort(IngameItem item){
			if(item is IO_Unit){
				IO_Unit unit=(IO_Unit)item;
				resources.changeBy(obj.fraction,unit.costs,true);
				unit.deleteModel();
			}else{
				Debug.Log("Called "+this.name+".abort() with wrong IO_ class!");
			}
		}
		
		public override void finish(IngameItem item){
			if(item is IO_Unit){
				IO_Unit unit=(IO_Unit)item;
				unit.initModel();
				unit.createUnit(obj.fraction);
				//set unit in front of its building
				Vector3 offset=new Vector3(Mathf.Sin(Mathf.Deg2Rad*obj.model.transform.localEulerAngles.y)*obj.markerSize*.64f,0f,
											Mathf.Cos(Mathf.Deg2Rad*obj.model.transform.localEulerAngles.y)*obj.markerSize*.64f);
				unit.setCoords(obj.model.transform.position+offset);
			}else{
				Debug.Log("Called "+this.name+".finish() with wrong IO_ class!");
			}
		}
	}
}
