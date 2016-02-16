using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class IOu_testUnit : IO_Unit {
		
		public IOu_testUnit() : base(){
			name="testUnit";
			type="IOu_testUnit";
			maxHP=100;
			buildTime=.8f;
			costs=new double[]{-2d,-2d,-0d,-0d};
			//set ActionMatrix
			//change MinimapProjection
			model=(GameObject)Resources.Load("objects/Prefabs/Units/IOu_testUnit",typeof(GameObject));
			initAfterModel();
		}
	}
}
