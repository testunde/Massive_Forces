using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class IOb_testBuilding : IO_Building {
		
		public IOb_testBuilding() : base(){
			name="testBuilding";
			type="IOb_testBuilding";
			maxHP=1000;
			buildTime=10;
			markerSize=9.8f;
			costs=new long[]{-40,-40,-15,-0};
			//set ActionMatrix
			//change MinimapProjection
			model=GameObject.Instantiate((GameObject)Resources.Load("blender/IOb_testBuilding",typeof(GameObject)));
			initAfterModel();
		}
	}
}
