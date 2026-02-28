using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using TASK3.Scripts.States;
using UnityEngine;

namespace TASK3.Scripts
{
    public class SlotFsmInit : MonoBehaviourExt
    {
        [OnAwake]
        private void CreateFsm()
        {
            Settings.Fsm = new FSM();

            Settings.Fsm.Add(new SlotInitState());
            Settings.Fsm.Add(new SlotIdleState());
            Settings.Fsm.Add(new SlotStartingState());
            Settings.Fsm.Add(new SlotSpinningState());
            Settings.Fsm.Add(new SlotStoppingState());
            Settings.Fsm.Add(new SlotWinningState());
        }

        [OnStart]
        private void StartFsm()
        {
            Settings.Fsm.Start("SlotInit");
        }

        [OnUpdate]
        private void UpdateFsm()
        {
            Settings.Fsm.Update(Time.deltaTime);
        }
    }
}