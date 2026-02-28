using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;

namespace TASK3.Scripts.States
{
    [State("SlotSpinning")]
    public class SlotSpinningState : FSMState
    {
        [Bind("OnBtn")]
        private void OnBtn(string btn)
        {
            if (btn == "Stop")
            {
                Settings.Model.Set("BtnStopEnable", false);
                Parent.Change("SlotStopping");
            }
        }
    }
}