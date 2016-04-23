using UnityEngine;
using System.Collections;
using Scripts;

namespace Database {
	public class IOu_Tank1 : IO_Database {
		
		public IOu_Tank1() : base(){
			name="Tank1";
			type="IOu_Tank1";
			maxHP=250;
			damage=25;
			buildTime=3f;
			markerSize=2.8f;
			costs=new long[]{-10,-8,-4,-0};
			//set ActionMatrix
		}
	}
}
