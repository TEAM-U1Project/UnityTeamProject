using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Play,
        End,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        End,
    }


    public enum UIEvent
    {
        Click,
        Drag,
        End,
    }

    public enum MouseEvent
    { 
        Press,
        Click,
        End,
    }

    public enum CameraMode
    {
        QuarterView,
        End,
    }
}