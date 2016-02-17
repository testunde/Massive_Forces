using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class IOb_testBuilding : IO_Building {
		
		public IOb_testBuilding() : base(){
			name="testBuilding";
			type="IOb_testBuilding";
			maxHP=1000;
			buildTime=3;
			markerSize=6.2f;
			costs=new long[]{-60,-60,-15,-0};
			//set ActionMatrix
			//change MinimapProjection
			model=GameObject.Instantiate((GameObject)Resources.Load("objects/Prefabs/Buildings/IOb_testBuilding",typeof(GameObject)));
			initAfterModel();
		}
	}
}
