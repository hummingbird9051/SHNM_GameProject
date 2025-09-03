using UnityEngine;

public class FrameSetter : SingletonBase<FrameSetter>
{
    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
    }
}
