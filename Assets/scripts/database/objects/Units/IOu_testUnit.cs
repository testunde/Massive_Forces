using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class IOu_testUnit : IO_Unit {
		
		public IOu_testUnit() : base(){
			name="testUnit";
			type="IOu_testUnit";
			maxHP=100;
			buildTime=1;
			markerSize=1f;
			costs=new long[]{-2,-2,-0,-0};
			//set ActionMatrix
			//change MinimapProjection
			model=(GameObject)Resources.Load("objects/Prefabs/Units/IOu_testUnit",typeof(GameObject));
			initAfterModel();
		}
	}
}
