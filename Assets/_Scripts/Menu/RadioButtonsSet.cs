using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadioButtonsSet : MonoBehaviour
{
    [SerializeField] private List<Toggle> toggles;
    [SerializeField] private LoadManager loadManager;

    private int _activeToggle = 0;

    public void ChangeValue(int inputIndex)
    {
        if (inputIndex == _activeToggle)
        {
            toggles[_activeToggle].isOn = true;
        }
        else if (toggles[inputIndex].isOn == true)
        {
            int prevIndex = _activeToggle;
            _activeToggle = inputIndex;
            toggles[prevIndex].isOn = false;
            loadManager.FilterLoaded(_activeToggle);
        }
    }
}
