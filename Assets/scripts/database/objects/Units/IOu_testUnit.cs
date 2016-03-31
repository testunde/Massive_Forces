using UnityEngine;
using System.Collections;
using Scripts;

namespace Database {
	public class IOu_testUnit : IO_Database {
		
		public IOu_testUnit() : base(){
			name="testUnit";
			type="IOu_testUnit";
			maxHP=100;
			buildTime=2f;
			markerSize=1.9f;
			costs=new long[]{-2,-2,-0,-0};
			//set ActionMatrix
		}
	}
}
