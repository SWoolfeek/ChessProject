using UnityEngine;


namespace Settings
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Settings/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        public bool DrawIn50Turns;

        
        public enum TimersType
        {
            TurnedOff = 0,
            Classic = 1,
            Fast = 2
            
        }
    }
}

