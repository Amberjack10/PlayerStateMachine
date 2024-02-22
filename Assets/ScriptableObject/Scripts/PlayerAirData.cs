using System;
using UnityEngine;

[Serializable]
public class PlayerAirData
{
    // 점프할 때의 힘
    [field: Header("JumpData")]
    [field: SerializeField][field: Range(0f, 25f)] public float JumpForce { get; private set; } = 4f;
}
