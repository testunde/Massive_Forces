using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class IOb_testBuilding : IO_Building {
		
		public IOb_testBuilding() : base(){
			name="testBuilding";
			type="IOb_testBuilding";
			maxHP=1000;
			buildTime=3f;
			costs=new double[]{-10d,-10d,-10d,-10d};
			//set ActionMatrix
			//change MinimapProjection
			model=GameObject.Instantiate((GameObject)Resources.Load("objects/Prefabs/Buildings/IOb_testBuilding",typeof(GameObject)));initAfterModel();
		}
	}
}
