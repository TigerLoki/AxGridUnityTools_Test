using AxGrid;
using AxGrid.FSM;

namespace TASK3.Scripts.States
{
    [State("SlotInit")]
    public class SlotInitState : FSMState
    {
        [Enter]
        private void Enter()
        {
            Settings.Model.Set("BtnStartEnable", true);
            Settings.Model.Set("BtnStopEnable", false);

            Parent.Change("SlotIdle");
        }
    }
}