using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Very simple FSM.

namespace Cuerious.Systems
{
    public class FSMState
    {
        public Action UpdateState;
        public Action InitState;
    }


    public class SimpleFSM
    {
        public Dictionary<string, FSMState> possibleStates;
        public FSMState currentState;
        public bool On;

        public SimpleFSM()
        {
            On = false;
            currentState = null;

            possibleStates = new Dictionary<string, FSMState>();
        }

        public void SwitchState(string newState)
        {
            if (possibleStates.ContainsKey(newState))
            {
                currentState = possibleStates[newState];
                currentState.InitState();
            }
            else
            {
                Otter.Util.Log("Couldn't find state \"" + newState + "\"!");
            }
        }

        public void Update()
        {
            if(On && currentState != null)
            {
                currentState.UpdateState();
            }
        }
    }
}
