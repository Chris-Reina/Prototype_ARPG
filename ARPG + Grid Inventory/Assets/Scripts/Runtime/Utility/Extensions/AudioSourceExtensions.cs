using System.Collections;
using System.Collections.Generic;
using DoaT;
using UnityEngine;

public static class AudioSourceExtensions
{
    public static void Setup(this AudioSource aS, AudioCue cue)
    {
        aS.clip = cue.clip;
        aS.loop = cue.loop;
        aS.pitch = cue.pitch.Random();
        aS.volume = cue.volume;
        aS.panStereo = cue.stereoPan;
        aS.spatialBlend = cue.spatialBlend;
        aS.reverbZoneMix = cue.reverbZoneMix;
    }
}
