using AxGrid;
using AxGrid.FSM;
using UnityEngine;

namespace TASK3.Scripts.States
{
    [State("SlotStarting")]
    public class SlotStartingState : FSMState
    {
        [Enter]
        private void Enter()
        {
            Settings.Model.Set("BtnStartEnable", false);
            Settings.Model.Set("BtnStopEnable", false);
            
            int sc1 = Settings.Model.GetInt("SymbolsCount1");
            int sc2 = Settings.Model.GetInt("SymbolsCount2");
            int sc3 = Settings.Model.GetInt("SymbolsCount3");

            int w1 = sc1 <= 0 ? 1 : Random.Range(1, sc1 + 1);
            int w2 = sc2 <= 0 ? 1 : Random.Range(1, sc2 + 1);
            int w3 = sc3 <= 0 ? 1 : Random.Range(1, sc3 + 1);

            Settings.Model.Set("Win1", w1);
            Settings.Model.Set("Win2", w2);
            Settings.Model.Set("Win3", w3);

            Settings.Invoke("StartSequence");
        }

        [One(3f)]
        private void EnableStop()
        {
            Settings.Model.Set("BtnStopEnable", true);
            Parent.Change("SlotSpinning");
        }
    }
}