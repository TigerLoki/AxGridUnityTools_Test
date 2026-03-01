using AxGrid;
using AxGrid.Base;
using AxGrid.Model;
using AxGrid.Path;
using UnityEngine;
using UnityEngine.UI;

namespace TASK3.Scripts
{
    public class SlotFrameView : MonoBehaviourExtBind
    {
        [SerializeField] private Image frameImage;
        
        private int reelIndex;
        
        private float FrameFadeTime => Settings.Model.GetFloat("FrameFadeTime");

        [OnStart]
        private void Init()
        {
            if (frameImage == null) return;
            
            reelIndex = GetComponentInParent<SlotReelView>().ReelIndex;

            Color c = frameImage.color;
            c.a = 0f;
            frameImage.color = c;
        }

        [Bind("SetReelFrame")]
        private void SetReelFrame(int index, bool enabled)
        {
            if (index != reelIndex) return;
            if (frameImage == null) return;

            Path = new CPath();

            float target = enabled ? 1f : 0f;
            float start = frameImage.color.a;

            Path.EasingQuadEaseOut(
                FrameFadeTime,
                start,
                target,
                v =>
                {
                    Color col = frameImage.color;
                    col.a = v;
                    frameImage.color = col;
                });
        }

        [Bind("ClearAllFrames")]
        private void ClearAllFrames()
        {
            if (frameImage == null) return;

            Color c = frameImage.color;
            c.a = 0f;
            frameImage.color = c;
        }
    }
}