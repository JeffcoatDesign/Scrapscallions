using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Cinematic
{
    public class CinematicManager : MonoBehaviour
    {
        public static CinematicManager instance;

        [SerializeField] private CinemachineVirtualCamera m_singleTargetVCam;
        [SerializeField] private CinemachineVirtualCamera m_targetGroupVCam;
        [SerializeField] private CinemachineTargetGroup m_targetGroup;
        [SerializeField] private Vector3 m_targetBody;
        private CameraType m_activeCamera;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            if (m_targetGroup.m_Targets.Length < 1)
            {
                SetCamera(CameraType.None);
            }
            else
                SetCamera(CameraType.Group);
        }

        internal void SetCamera(CameraType cameraType)
        {
            m_singleTargetVCam.gameObject.SetActive(false);
            m_targetGroupVCam.gameObject.SetActive(false);
            m_activeCamera = cameraType;
            switch (cameraType)
            {
                case CameraType.SingleTarget:
                    m_singleTargetVCam.gameObject.SetActive(true); break;
                case CameraType.Group:
                    {
                        RandomizeTargetGroupCamera();
                        m_targetGroupVCam.gameObject.SetActive(true); break;
                    }
                case CameraType.None:
                    break;
            }
        }

        internal void AddTarget(Transform target, float weight = 1f, float radius = 3f)
        {
            m_targetGroup.AddMember(target, weight, radius);
        }

        internal void SetSingleTarget(Transform target)
        {
            m_singleTargetVCam.LookAt = target;
            m_singleTargetVCam.Follow = target;
        }

        private void RandomizeTargetGroupCamera()
        {
            Vector3 offset = Vector3.zero;
            offset.x = Random.Range(-20, 20);
            offset.y = Random.Range(-20, -1);
            offset.z = Random.Range(-20, 20);
            m_targetGroupVCam.GetCinemachineComponent<CinemachineGroupComposer>().m_TrackedObjectOffset = offset;
        }

        internal enum CameraType
        {
            None,
            SingleTarget,
            Group
        }
    }
}