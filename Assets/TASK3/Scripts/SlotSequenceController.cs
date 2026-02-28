using AxGrid;
using AxGrid.Base;
using AxGrid.Model;
using AxGrid.Path;

namespace TASK3.Scripts
{
    public class SlotSequenceController : MonoBehaviourExtBind
    {
        [Bind("StartSequence")]
        private void StartSequence()
        {
            Settings.Invoke("ClearAllFrames");
            
            Path = new CPath();

            Path
                .Action(() => Settings.Invoke("StartReel", 1))
                .Action(() => Settings.Invoke("StartSparks", 1))
                .Wait(0.2f)
                .Action(() => Settings.Invoke("StartReel", 2))
                .Action(() => Settings.Invoke("StartSparks", 2))
                .Wait(0.2f)
                .Action(() => Settings.Invoke("StartReel", 3))
                .Action(() => Settings.Invoke("StartSparks", 3));
        }

        [Bind("StopSequence")]
        private void StopSequence()
        {
            Path = new CPath();

            Path
                .Action(() => Settings.Invoke("StopReel", 1))
                .Action(() => Settings.Invoke("StopSparks", 1))
                .Wait(0.2f)
                .Action(() => Settings.Invoke("StopReel", 2))
                .Action(() => Settings.Invoke("StopSparks", 2))
                .Wait(0.2f)
                .Action(() => Settings.Invoke("StopReel", 3))
                .Action(() => Settings.Invoke("StopSparks", 3));
        }
        
        [Bind("PlayWinSequence")]
        private void PlayWinSequence(bool r1, bool r2, bool r3)
        {
            Path = new CPath();

            Path.Wait(0.2f)
                .Action(() =>
                {
                    if (r1)
                        Settings.Invoke("SetReelFrame", 1, true);
                })
                .Wait(0.2f)
                .Action(() =>
                {
                    if (r2)
                        Settings.Invoke("SetReelFrame", 2, true);
                })
                .Wait(0.2f)
                .Action(() =>
                {
                    if (r3)
                        Settings.Invoke("SetReelFrame", 3, true);
                })
                .Action(() => Settings.Invoke("WinSequenceDone"));
        }
    }
}