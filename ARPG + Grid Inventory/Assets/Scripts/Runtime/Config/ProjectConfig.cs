using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProjectConfig
{
    public const int TargetFPS = 60;
    public static float TargetFrameTime => 1f / TargetFPS;
}
