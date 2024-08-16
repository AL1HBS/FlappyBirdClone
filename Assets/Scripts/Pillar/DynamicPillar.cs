using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DynamicPillar : Pillar // INHERITANCE
{
    private float randomNumber;

    private float moveSpeed;
    protected override void Init() // POLYMORPHISM
    {
        
    }
    protected override void ObstacleUpdateBehacior() // POLYMORPHISM
    {
        gapPos = Mathf.Sin(Time.time);
    }
}
