using UnityEngine;
using System.Collections;
using Scripts;

namespace Database {
	public class IOb_testRoundBuilding : IO_Database {
		
		public IOb_testRoundBuilding() : base(){
			name="testBuilding";
			type="IOb_testRoundBuilding";
			maxHP=2000;
			buildTime=7f;
			markerSize=9.8f;
			costs=new long[]{-90,-80,-20,-0};
			//set ActionMatrix
			workerUnits=1;
		}
	}
}
