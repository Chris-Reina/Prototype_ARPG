using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnergyReceiver
{
    Transform FeedbackPosition { get; set; }
    bool IsTarget { get; set; }
    EnergyType EnergyType { get; set; }
}
