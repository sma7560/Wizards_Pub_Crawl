using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Scriptable object that all cards will extend
[CreateAssetMenu(fileName = "New Card", menuName = "Defender Card")]
public class DefenderCard : ScriptableObject {

	public string cardName;
	public string desc;

	public Sprite artwork;

	public int costEnergy;
	public int costMoney;
}
