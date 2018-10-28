using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderBehaviour : MonoBehaviour
{

    public enum defenderMode { spawnMonster, spawnTrap };

    public GameObject monster;
    public GameObject trap;
    public int energy;
    public int money;

    public defenderMode mode;
    public Camera defenderCamera;

    private Quaternion monsterDefaultRotation = Quaternion.Euler(0, 0, 0);
    private int currentCardCost = 100; //update this cost to whatever currently seelcted card's cost is

    // Use this for initialization
    void Start()
    {
        energy = 1000;
        money = 1000;

        mode = defenderMode.spawnMonster;
    }

    // Update is called once per frame
    void Update()
    {

        //Defender Monster spawn script
        //Uses raycasting to select a tile to place a monster

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("mouseClick");
            RaycastHit hit;
            Ray ray = defenderCamera.ScreenPointToRay(Input.mousePosition);
            if (currentCardCost > energy)
            {
                Debug.Log("No energy!");
            }
            else if (Physics.Raycast(ray, out hit, 200.0f) && hit.transform.tag == "Tile")
            {
                Debug.Log("raycast hit " + hit.point);
                switch (mode)
                {
                    case defenderMode.spawnMonster:
                        {
                            Vector3 monsterSpawn = hit.point;
                            monsterSpawn.y = monsterSpawn.y + 0.5f;    //change to use monster height when that data is stored
                                                                    //monsterSpawn.y = monsterSpawn.y + monster.transform.y;                 
                            Instantiate(monster, monsterSpawn, hit.transform.rotation, hit.transform);      //monster costs 100 energy
                            energy -= 100;
                            break;
                        }
                    case defenderMode.spawnTrap:
                        {
                            Vector3 trapSpawn = hit.point;
                            Instantiate(trap, trapSpawn, hit.transform.rotation, hit.transform);           //trap costs 100 gold
                            energy -= 100;
                            break;
                        }
                }
            }
        }

        //switch spawn modes
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (mode == defenderMode.spawnMonster)
                mode = defenderMode.spawnTrap;
            else if (mode == defenderMode.spawnTrap)
                mode = defenderMode.spawnMonster;
        }
    }
}
