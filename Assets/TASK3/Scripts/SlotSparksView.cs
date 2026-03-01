using AxGrid;
using AxGrid.Base;
using AxGrid.Model;
using AxGrid.Path;
using UnityEngine;

namespace TASK3.Scripts
{
    public class SlotParticleController : MonoBehaviourExtBind
    {
        private int reelIndex;
        [SerializeField] private ParticleSystem[] sparksSystems;
    
        private float SparksMaxRate => Settings.Model.GetFloat("SparksMaxRate");
        private float AccelerationTime => Settings.Model.GetFloat("AccelerationTime");
        private float DecelerationTime => Settings.Model.GetFloat("DecelerationTime");

        [OnStart]
        private void Init()
        {
            reelIndex = GetComponentInParent<SlotReelView>().ReelIndex;
        }
    
        [Bind("StartSparks")]
        private void StartSparks(int index)
        {
            if (index != reelIndex) return;
            if (sparksSystems == null) return;

            Path = new CPath();

            Path.Wait(AccelerationTime * 0.5f)
                .Action(() =>
                {
                    SetSparksRate(0f);

                    foreach (var ps in sparksSystems)
                        ps.Play(true);
                })
                .EasingQuadEaseOut(
                    AccelerationTime * 0.5f,
                    0f,
                    SparksMaxRate,
                    SetSparksRate
                );
        }
    
        [Bind("StopSparks")]
        private void StopSparks(int index)
        {
            if (index != reelIndex) return;
            if (sparksSystems == null) return;

            float currentRate = 0f;
            if (sparksSystems.Length > 0)
                currentRate = sparksSystems[0].emission.rateOverTime.constant;

            Path = new CPath();

            Path.EasingQuadEaseOut(
                    DecelerationTime * 0.5f,
                    currentRate,
                    0f,
                    SetSparksRate
                )
                .Action(() =>
                {
                    SetSparksRate(0f);

                    foreach (var ps in sparksSystems)
                        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                });
        }
    
        private void SetSparksRate(float value)
        {
            if (sparksSystems == null) return;

            foreach (var ps in sparksSystems)
            {
                var em = ps.emission;
                em.rateOverTime = value;
            }
        }
    }
}
