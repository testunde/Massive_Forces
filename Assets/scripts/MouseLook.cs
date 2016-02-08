using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Scripts.Control
{
    [Serializable]
    public class MouseLook
    {
        public float XSensitivity = 2f;
        public float YSensitivity = 2f;
        public bool clampVerticalRotation = false;	//I don't know what this is used for
        public float MinimumX = -90F;
        public float MaximumX = 90F;
        public float smoothTime = 5f;


        private Vector3 m_CameraTargetRot;


        public void Init(Transform camera)
        {
            m_CameraTargetRot = camera.eulerAngles;
        }


        public void LookRotation(Transform camera)
        {
            float yRot = CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
            float xRot = 0f;//CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;
			
			float yCam = m_CameraTargetRot.y+yRot;
            //m_CameraTargetRot = new Vector3(m_CameraTargetRot.x-xRot, yCam, m_CameraTargetRot.z);
            camera.eulerAngles += new Vector3(0f,yRot,0f);
			Debug.Log(yRot);

			//camera.RotateAround(camera.position,Vector3.up,m_CameraTargetRot.y);
            if(clampVerticalRotation){
				//m_CameraTargetRot = ClampRotationAroundXAxis (m_CameraTargetRot);
			}
        }

    }
}
