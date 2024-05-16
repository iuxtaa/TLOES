using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[CreateAssetMenu]
public class VectorValue : ScriptableObject
{
    public const float INITIAL_POSITION_X = -17f;
    public const float INITIAL_POSITION_Y = -2f;
    public Vector2 initialValue = new Vector2(INITIAL_POSITION_X, INITIAL_POSITION_Y);
    public Vector2 changingValue;

}
