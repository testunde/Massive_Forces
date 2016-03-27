using UnityEngine;
using System.Collections;
using Scripts;

namespace Database{
	public class Acr_TrainUnit : A_Create {
		private string unitClass;
		private IO_Unit unit;
		
		public Acr_TrainUnit(IngameObject obj,string targetUnit) : base(obj){
			this.unitClass=targetUnit;
			//unit=(IO_Unit)Activator.CreateInstance(Type.GetType("Database."+targetUnit));
			//set name
			//set time
		}
		
		public override void begin(){
			
		}
		
		public override void abort(){
			
		}
		
		public override void finish(){
			
		}
	}
}
