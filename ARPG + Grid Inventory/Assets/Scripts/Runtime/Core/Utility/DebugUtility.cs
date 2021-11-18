using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugUtility
{
    public static string WIP() { return WorkInProgress(); }

    public static string WIP(string message) { return WorkInProgress(message); }
    public static string WorkInProgress(string message) { return $"This Execution is a Work In Progress: {message}."; }
    public static string WorkInProgress() { return $"This Execution is a Work In Progress."; }
    
    
    public static string NYI() { return NotYetImplemented(); }

    public static string NYI(string message) { return NotYetImplemented(message); }
    public static string NotYetImplemented(string message) { return $"This Execution is Not Yet Implemented: {message}."; }
    public static string NotYetImplemented() { return $"This Execution is Not Yet Implemented."; }

}
