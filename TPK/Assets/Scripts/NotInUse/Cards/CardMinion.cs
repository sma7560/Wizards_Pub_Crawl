using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Minion Card", menuName = "Defender Card/New Minion")]
public class CardMinion : DefenderCard  {

	public int movementSpeed;
	public int health;
	public int damage;

	public GameObject prefab;
}
