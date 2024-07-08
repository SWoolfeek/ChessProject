using UnityEngine;


namespace Settings
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Settings/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        public bool DrawIn50Turns;
    }
}

