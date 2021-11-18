
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputKeyOptions", menuName = "Data/Options/InputKey")]
public class InputKeys : ScriptableObject
{
    [Header("UI")]
    public KeyCode openCloseInventory = KeyCode.C;
    public KeyCode closeAllPanels = KeyCode.Escape;
    [Header("Combat")]
    public KeyCode forceAction = KeyCode.LeftShift;
    [Space(10)]
    public KeyCode[] Abilities = new KeyCode[8];
    
}
