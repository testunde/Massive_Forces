using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class IOu_Tank1 : IO_Unit {
		
		public IOu_Tank1() : base(){
			name="Tank1";
			type="IOu_Tank1";
			maxHP=250;
			buildTime=2;
			markerSize=2.8f;
			costs=new long[]{-10,-8,-4,-0};
			//set ActionMatrix
			//change MinimapProjection
			model=GameObject.Instantiate((GameObject)Resources.Load("objects/Prefabs/Units/Tank1",typeof(GameObject)));
			initAfterModel();
		}
	}
}
