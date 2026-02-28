using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;

namespace TASK3.Scripts.States
{
    [State("SlotIdle")]
    public class SlotIdleState : FSMState
    {
        [Enter]
        private void Enter()
        {
            Settings.Model.Set("BtnStartEnable", true);
            Settings.Model.Set("BtnStopEnable", false);
        }

        [Bind("OnBtn")]
        private void OnBtn(string btn)
        {
            if (btn == "Start")
                Parent.Change("SlotStarting");
        }
    }
}