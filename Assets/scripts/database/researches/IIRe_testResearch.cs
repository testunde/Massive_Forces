using UnityEngine;
using System.Collections;
using Scripts;

namespace Database{
	public class IIRe_testResearch : II_Research {
		public static bool done=false;
		
		public IIRe_testResearch() : base(){
			this.type="IIRe_testResearch";
			this.name="IIRe_testResearch";
			this.buildTime=3f;
			this.costs=new long[]{-50,-50,-30,-0};
		}
		
		public override void perform(int frac){
			done=true;
			frCtrl.testUnitHP[frac]+=.2f;
			Debug.Log(frCtrl.testUnitHP[frac]);
		}
		
		public override bool isDone(){
			return done;
		}
	}
}
