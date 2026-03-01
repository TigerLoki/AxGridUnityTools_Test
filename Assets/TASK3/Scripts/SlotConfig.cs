using AxGrid;
using AxGrid.Base;
using AxGrid.Model;
using UnityEngine;

namespace TASK3.Scripts
{
    public class SlotConfig : MonoBehaviourExt
    {
        [Header("Spin")] 
        [SerializeField] private float maxSpeed = 3000f;
        [SerializeField] private float accelerationTime = 3f;
        [SerializeField] private float decelerationTime = 3f;
        [SerializeField] private float bounceTime = 1f;

        [Header("VFX")]
        [SerializeField] private float sparksMaxRate = 50f;

        [Header("UI")]
        [SerializeField] private float frameFadeTime = 0.3f;

        [OnStart]
        private void Init()
        {
            PushToModel();
        }

        private void PushToModel()
        {
            Settings.Model.Set("MaxSpeed", maxSpeed);
            Settings.Model.Set("AccelerationTime", accelerationTime);
            Settings.Model.Set("DecelerationTime", decelerationTime);
            Settings.Model.Set("BounceTime", bounceTime);
            Settings.Model.Set("SparksMaxRate", sparksMaxRate);
            Settings.Model.Set("FrameFadeTime", frameFadeTime);
        }
    }
}