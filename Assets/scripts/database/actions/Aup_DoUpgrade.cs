using UnityEngine;
using System.Collections;
using Scripts;

namespace Database{
	public class Aup_DoUpgrade : A_Upgrade {
		private II_Upgrade upgradeClass;
		
		public Aup_DoUpgrade(string targetUpgrade) : base(){
			this.upgradeClass=(II_Upgrade)System.Activator.CreateInstance(System.Type.GetType("Database."+targetUpgrade));
			this.name="upgrade "+upgradeClass.name;
			this.costs=upgradeClass.costs;
			//set icon
		}
		
		public override void begin(){
			this.upgradeClass.obj=obj;
			if(frCtrl.RSC_costsAvailable(obj.fraction,upgradeClass.costs)){
				upgradeClass.timeRemaining=upgradeClass.buildTime;
				upgradeClass.createdBy=this;
				production.addItem(upgradeClass);
				frCtrl.RSC_changeBy(obj.fraction,upgradeClass.costs);
			}else{
				Debug.Log("Not enough resources!");
			}
		}
		
		public override void abort(IngameItem item){
			if(item is II_Upgrade){
				II_Upgrade upgrade=(II_Upgrade)item;
				frCtrl.RSC_changeBy(obj.fraction,upgrade.costs,true);
			}else{
				Debug.Log("Called "+this.name+".abort() with wrong IO_ class!");
			}
		}
		
		public override void finish(IngameItem item){
			if(item is II_Upgrade){
				II_Upgrade upgrade=(II_Upgrade)item;
				upgrade.perform();
			}else{
				Debug.Log("Called "+this.name+".finish() with wrong IO_ class!");
			}
		}
	}
}
