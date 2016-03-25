using UnityEngine;
using System.Collections;
using Scripts;

namespace Database{
	public class Acr_TrainUnit : A_Create {
		string unit;
		
		public Acr_TrainUnit(string targetUnit){
			this.unit=targetUnit;
		}
		
		//unit=(IO_Unit)Activator.CreateInstance(Type.GetType("Database."+targetUnit));
	}
}
