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
        switch(col.collider.tag) {
		case "Projectile":
			if (++curHits == maxHits)
				Destroy (gameObject);
			break;
		case "Safezone":
			Destroy (gameObject);
			break;
        } 
        
    }
}
