using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;

namespace TASK3.Scripts.States
{
    [State("SlotStopping")]
    public class SlotStoppingState : FSMState
    {
        private int stoppedCount;

        [Enter]
        private void Enter()
        {
            stoppedCount = 0;
            Settings.Invoke("StopSequence");
        }

        [Bind("ReelStopped")]
        private void OnStopped()
        {
            stoppedCount++;

            if (stoppedCount >= 3)
                Parent.Change("SlotWinning");
        }
    }
}