using UnityEngine;
using System.Collections;
using Scripts;

namespace Database {
	public class IOb_testBuilding : IO_Database {
		
		public IOb_testBuilding() : base(){
			name="testBuilding";
			type="IOb_testBuilding";
			maxHP=1000;
			buildTime=10f;
			markerSize=9.8f;
			costs=new long[]{-40,-40,-15,-0};
			//set ActionMatrix
			actions[0,2]="Aup_DoUpgrade,IIUp_testUpgrade";
			actions[1,0]="Acr_TrainUnit,IOu_testUnit";
			actions[1,1]="Acr_BuildBuilding,IOb_testBuilding";
			actions[2,1]="Acr_BuildBuilding,IOb_testRoundBuilding";
			workerUnits=2;
		}
	}
}
