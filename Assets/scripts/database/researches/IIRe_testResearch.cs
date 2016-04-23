using UnityEngine;
using System.Collections;
using Scripts;

namespace Database{
	public class IIRe_testResearch : II_Research {
		
		public IIRe_testResearch() : base(){
			this.name="IIRe_testResearch";
			this.buildTime=3f;
			this.costs=new long[]{-50,-50,-30,-0};
		}
		
		public override void perform(){
			//set all factors:
			frCtrl.testUnitHP+=.2f;
			// int deltaHP=obj.
		}
	}
}
