using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class Res {
		private static Res instance=null;
		public int f=2;	//amount of fractions with player, but without neutral
		public int a=4;	//amount of resource types
		private double[,] res;
		private double[,] maxRes;
		
		private Res(){
			res=new double[f,a];
			maxRes=new double[f,a];
			
			//set start values
			for(int i=0;i<f;i++){
				res[i,0]=100;
				maxRes[i,0]=2000;
				
				res[i,1]=100;
				maxRes[i,1]=2000;
				
				res[i,2]=50;
				maxRes[i,2]=2000;
				
				res[i,3]=0;
				maxRes[i,3]=500;
			}
		}
		
		public static Res getInstance(){
			if(instance==null)
				instance=new Res();
			
			return instance;
		}
		
		private void limit(int fraction){
			for(int i=0;i<a;i++){
				if(res[fraction,i]>maxRes[fraction,i])
					res[fraction,i]=maxRes[fraction,i];
				//else if(res[fraction,i]<0d)
				//	res[fraction,i]=0d;
			}
		}
		
		public bool isAvailable(int fraction,int r,double amount){
			bool result=false;
			if(r<a){
				result=(res[fraction,r]>=amount);
			}
			return result;
		}
		
		//CURRENT VALUES
		public void changeBy(int fraction,double[] amount){
			if(amount.Length==a){
				for(int i=0;i<a;i++)
					res[fraction,i]+=amount[i];
			}
			limit(fraction);
		}
		public void set(int fraction,double[] amount){
			if(amount.Length==a){
				for(int i=0;i<a;i++)
					res[fraction,i]=amount[i];
			}
			limit(fraction);
		}
		public double[] get(int fraction){
			double[] result=new double[a];
			for(int i=0;i<a;i++)
				result[i]=res[fraction,i];
			return result;
		}
		
		//MAX VALUES
		public void changeByMax(int fraction,double[] amount){
			if(amount.Length==a){
				for(int i=0;i<a;i++)
					maxRes[fraction,i]+=amount[i];
			}
		}
		public void setMax(int fraction,double[] amount){
			if(amount.Length==a){
				for(int i=0;i<a;i++)
					maxRes[fraction,i]=amount[i];
			}
		}
		public double[] getMax(int fraction){
			double[] result=new double[a];
			for(int i=0;i<a;i++)
				result[i]=maxRes[fraction,i];
			return result;
		}
	}
}
