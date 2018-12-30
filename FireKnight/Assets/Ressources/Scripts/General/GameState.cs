using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dim
{
    /////////////////////////////////////////////////
    /// [UNIMPLEMENTED] Singleton, deals with GameState changes. 
    /////////////////////////////////////////////////
    public class GameState
    {

        public static GameState instance;

        public delegate void StateChangedDelegate(State newState);

        public event StateChangedDelegate StateChange;

        private State state;


        private GameState(State startingState)
        {
            state = startingState;
        }

        public static void Initiate(State startingState)
        {
            instance = new GameState(startingState);
        }

        public static State GetState()
        {
            return instance.state;
        }

        public static void SetState(State newState)
        {
            if (newState != instance.state)
            {
                instance.state = newState;
                instance.StateChange?.Invoke(newState);
            }

        }


        public enum State
        {
            MainMenu,
            LevelSelection,
            SettingsMenu,
            Ingame,
            Pause,
            InGameMenu,
            Quit
        } 
    }
}
