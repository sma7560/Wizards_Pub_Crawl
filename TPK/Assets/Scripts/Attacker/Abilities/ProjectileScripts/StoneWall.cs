using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneWall : BaseProjectile
{
    private int maxHits = 3;
    private int curHits= 0;
    public override void Behaviour(Collision col)
    {
        base.Behaviour(col);
        if (col.collider.tag == "Projectile") {
            if (++curHits == maxHits) Destroy(gameObject);  
        } 
        
    }
}
