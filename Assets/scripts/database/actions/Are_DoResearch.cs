using UnityEngine;
using System.Collections;
using Scripts;

namespace Database{
	public class Are_DoResearch : A_Research {
		private II_Research researchClass;
		
		public Are_DoResearch(string targetResearch) : base(){
			this.researchClass=(II_Research)System.Activator.CreateInstance(System.Type.GetType("Database."+targetResearch));
			this.name="research "+researchClass.name;
			this.costs=researchClass.costs;
			//set icon
		}
		
		public override void begin(){
			if(isDoable() && frCtrl.RSC_costsAvailable(obj.fraction,researchClass.costs)){
				researchClass.timeRemaining=researchClass.buildTime;
				researchClass.createdBy=this;
				production.addItem(researchClass);
				frCtrl.RSC_changeBy(obj.fraction,researchClass.costs);
			}else{
				if(!isDoable())
					Debug.Log("Research "+name+" already done!");
				else
					Debug.Log("Not enough resources!");
			}
		}
		
		public override void abort(IngameItem item){
			if(item is II_Research){
				II_Research research=(II_Research)item;
				frCtrl.RSC_changeBy(obj.fraction,research.costs,true);
			}else{
				Debug.Log("Called "+this.name+".abort() with wrong IO_ class!");
			}
		}
		
		public override void finish(IngameItem item){
			if(item is II_Research){
				II_Research research=(II_Research)item;
				research.perform(obj.fraction);
			}else{
				Debug.Log("Called "+this.name+".finish() with wrong IO_ class!");
			}
		}
		
		public override bool isDoable(){
			return !researchClass.isDone();
		}
	}
}
