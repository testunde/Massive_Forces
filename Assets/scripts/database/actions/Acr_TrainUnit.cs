using UnityEngine;
using System.Collections;
using Scripts;

namespace Database{
	public class Acr_TrainUnit : A_Create {
		private Database.IO_Database unitClass;
		
		public Acr_TrainUnit(string targetUnit) : base(){
			this.unitClass=(IO_Database)System.Activator.CreateInstance(System.Type.GetType("Database."+targetUnit));
			this.name="train "+unitClass.name;
			this.costs=unitClass.costs;
			//set icon
		}
		
		public override void setObject(IngameObject obj){
			base.setObject(obj);
			this.time=unitClass.buildTime/(float)obj.workerUnits;
		}
		
		public override void begin(){
			if(frCtrl.RSC_costsAvailable(obj.fraction,unitClass.costs)){
				IO_Unit unit=new IO_Unit();
				unit.loadType(unitClass,obj.fraction);
				unit.timeRemaining=unitClass.buildTime/(float)obj.workerUnits;
				unit.createdBy=this;
				production.addItem(unit);
				frCtrl.RSC_changeBy(obj.fraction,unitClass.costs);
			}else{
				Debug.Log("Not enough resources!");
			}
		}
		
		public override void abort(IngameItem item){
			if(item is IO_Unit){
				IO_Unit unit=(IO_Unit)item;
				frCtrl.RSC_changeBy(obj.fraction,unit.costs,true);
				unit.deleteModel();
			}else{
				Debug.Log("Called "+this.name+".abort() with wrong IO_ class!");
			}
		}
		
		public override void finish(IngameItem item){
			if(item is IO_Unit){
				IO_Unit unit=(IO_Unit)item;
				unit.initModel();
				unit.createUnit();
				//set unit in front of its building
				Vector3 nonY=Vector3.right+Vector3.forward;
				float factor=(obj.markerSize*.64f)/Vector3.Distance(Vector3.Scale(obj.model.transform.position,nonY),Vector3.Scale(obj.meetingPoint,nonY));
				Vector3 offset=new Vector3((obj.meetingPoint.x-obj.model.transform.position.x)*factor,0f,
								(obj.meetingPoint.z-obj.model.transform.position.z)*factor);
				unit.setCoords(obj.model.transform.position+offset);
				unit.setTargetPos(obj.meetingPoint);
			}else{
				Debug.Log("Called "+this.name+".finish() with wrong IO_ class!");
			}
		}
	}
}
