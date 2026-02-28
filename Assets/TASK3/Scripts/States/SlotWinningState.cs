using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;

namespace TASK3.Scripts.States
{
    [State("SlotWinning")]
    public class SlotWinningState : FSMState
    {
        [Enter]
        private void Enter()
        {
            int m1 = Settings.Model.GetInt("Win1");
            int m2 = Settings.Model.GetInt("Win2");
            int m3 = Settings.Model.GetInt("Win3");

            bool r1 = (m1 == m2) || (m1 == m3);
            bool r2 = (m2 == m1) || (m2 == m3);
            bool r3 = (m3 == m1) || (m3 == m2);

            if (!r1 && !r2 && !r3)
            {
                Parent.Change("SlotIdle");
                return;
            }

            Settings.Invoke("PlayWinSequence", r1, r2, r3);
        }

        [Bind("WinSequenceDone")]
        private void OnWinSequenceDone()
        {
            Parent.Change("SlotIdle");
        }
    }
}