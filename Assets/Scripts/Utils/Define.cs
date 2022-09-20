using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum WorldObject
    {
        Unknown,
        Player,
        Monster,
    }
    public enum State
    {
        Die,
        Moving,
        Idle,
        Skill,
    }
    public enum UIEvent
    {
        Click,
        Drag,
    }
    public enum Layer
    {
        Monster = 8,
        Wall = 9,
    }

    public enum MouseEvent
    {
        Press,
        Click,
    }

    public enum CameraMode
    {
        BackView,
        QuarterView
    }
}
