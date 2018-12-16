using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour {

    public TextMeshProUGUI ipAddressBox;
    public TextMeshProUGUI timeBox;
    private NetworkManagerExtension networkManager;
    private DungeonController dungeonController;

    private int totalTime = 900;
    private float currentTime;
    private float timeLeft;
    private int minuteLeft;
    private int secondLeft;

    // Use this for initialization
    void Start () {
        networkManager = GameObject.Find("NetworkManagerV2").GetComponent<NetworkManagerExtension>();
        dungeonController = GameObject.Find("EventSystem").GetComponent<DungeonController>();

        ipAddressBox.SetText(networkManager.getNetworkAddress());
    }
	
	// Update is called once per frame
	void Update () {

        currentTime = dungeonController.GetTime();
        timeLeft = totalTime - currentTime;

        minuteLeft = Mathf.FloorToInt(timeLeft / 60F);
        secondLeft = Mathf.FloorToInt(timeLeft - minuteLeft * 60);

        if (secondLeft >= 10)
        {
            timeBox.SetText(minuteLeft.ToString() + " : " + secondLeft.ToString());
        }
        else
        {
            timeBox.SetText(minuteLeft.ToString() + " : 0" + secondLeft.ToString());
        }
    }
}
