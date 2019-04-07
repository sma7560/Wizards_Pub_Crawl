using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsSetting : MonoBehaviour
{
    // All control buttons
    private Button forwardButton;
    private Button backButton;
    private Button leftButton;
    private Button rightButton;
    private Button basicAttackButton;
    private Button skill1Button;
    private Button skill2Button;
    private Button skill3Button;
    private Button skill4Button;
    private Button characterWindowButton;
    private Button scoreboardButton;
    private Button inGameMenuButton;

    private bool isListening;       // if we're currently listening for a key input to bind
    private KeyCode keyToBind;      // the key player has inputted for key binding
    private KeyCode oldKey;         // the previously set key before the new key binding

    /// <summary>
    /// Initialization
    /// </summary>
    void Start()
    {
        // Get buttons
        forwardButton = GameObject.Find("ForwardButton").GetComponent<Button>();
        backButton = GameObject.Find("BackButton").GetComponent<Button>();
        leftButton = GameObject.Find("LeftButton").GetComponent<Button>();
        rightButton = GameObject.Find("RightButton").GetComponent<Button>();
        basicAttackButton = GameObject.Find("BasicAttackButton").GetComponent<Button>();
        skill1Button = GameObject.Find("Skill1Button").GetComponent<Button>();
        skill2Button = GameObject.Find("Skill2Button").GetComponent<Button>();
        skill3Button = GameObject.Find("Skill3Button").GetComponent<Button>();
        skill4Button = GameObject.Find("Skill4Button").GetComponent<Button>();
        characterWindowButton = GameObject.Find("CharacterWindowButton").GetComponent<Button>();
        scoreboardButton = GameObject.Find("ScoreboardButton").GetComponent<Button>();
        inGameMenuButton = GameObject.Find("InGameMenuButton").GetComponent<Button>();

        // Setup button text
        CustomKeyBinding.SetupCustomKeyBindings();
        SetupButtonText();

        // Setup onClick functions
        forwardButton.onClick.AddListener(ListenForwardButtonWrapper);
        backButton.onClick.AddListener(ListenBackButtonWrapper);
        leftButton.onClick.AddListener(ListenLeftButtonWrapper);
        rightButton.onClick.AddListener(ListenRightButtonWrapper);
        basicAttackButton.onClick.AddListener(ListenBasicAttackButtonWrapper);
        skill1Button.onClick.AddListener(ListenSkill1ButtonWrapper);
        skill2Button.onClick.AddListener(ListenSkill2ButtonWrapper);
        skill3Button.onClick.AddListener(ListenSkill3ButtonWrapper);
        skill4Button.onClick.AddListener(ListenSkill4ButtonWrapper);
        characterWindowButton.onClick.AddListener(ListenCharacterWindowButtonWrapper);
        scoreboardButton.onClick.AddListener(ListenScoreboardButtonWrapper);
        inGameMenuButton.onClick.AddListener(ListenInGameMenuButtonWrapper);
    }

    /// <summary>
    /// Listens for the next player's key input.
    /// </summary>
    void OnGUI()
    {
        // Do not listen for key input if we're not currently trying to bind a key
        if (!isListening) return;

        // Listen for next key input
        Event e = Event.current;

        if (e.isKey)
        {
            // Do not bind if key is already used
            if (oldKey != e.keyCode && (CustomKeyBinding.KeyAlreadyBound(e.keyCode) || e.keyCode == KeyCode.None)) return;

            keyToBind = e.keyCode;
            isListening = false;
        }
        else if (e.isMouse && e.button >= 0 && e.button <= 6)
        {
            switch (e.button)
            {
                case 0:
                    if (oldKey != KeyCode.Mouse0 && CustomKeyBinding.KeyAlreadyBound(KeyCode.Mouse0)) return;
                    keyToBind = KeyCode.Mouse0;
                    break;
                case 1:
                    if (oldKey != KeyCode.Mouse1 && CustomKeyBinding.KeyAlreadyBound(KeyCode.Mouse1)) return;
                    keyToBind = KeyCode.Mouse1;
                    break;
                case 2:
                    if (oldKey != KeyCode.Mouse2 && CustomKeyBinding.KeyAlreadyBound(KeyCode.Mouse2)) return;
                    keyToBind = KeyCode.Mouse2;
                    break;
                case 3:
                    if (oldKey != KeyCode.Mouse3 && CustomKeyBinding.KeyAlreadyBound(KeyCode.Mouse3)) return;
                    keyToBind = KeyCode.Mouse3;
                    break;
                case 4:
                    if (oldKey != KeyCode.Mouse4 && CustomKeyBinding.KeyAlreadyBound(KeyCode.Mouse4)) return;
                    keyToBind = KeyCode.Mouse4;
                    break;
                case 5:
                    if (oldKey != KeyCode.Mouse5 && CustomKeyBinding.KeyAlreadyBound(KeyCode.Mouse5)) return;
                    keyToBind = KeyCode.Mouse5;
                    break;
                case 6:
                    if (oldKey != KeyCode.Mouse6 && CustomKeyBinding.KeyAlreadyBound(KeyCode.Mouse6)) return;
                    keyToBind = KeyCode.Mouse6;
                    break;
            }

            isListening = false;
        }
    }

    /// <summary>
    /// Sets the button text based on current key bindings.
    /// </summary>
    private void SetupButtonText()
    {
        forwardButton.transform.Find("Text").GetComponent<Text>().text = CustomKeyBinding.GetForwardKey().ToString();
        backButton.transform.Find("Text").GetComponent<Text>().text = CustomKeyBinding.GetBackKey().ToString();
        leftButton.transform.Find("Text").GetComponent<Text>().text = CustomKeyBinding.GetLeftKey().ToString();
        rightButton.transform.Find("Text").GetComponent<Text>().text = CustomKeyBinding.GetRightKey().ToString();
        basicAttackButton.transform.Find("Text").GetComponent<Text>().text = CustomKeyBinding.GetBasicAttackKey().ToString();
        skill1Button.transform.Find("Text").GetComponent<Text>().text = CustomKeyBinding.GetSkill1Key().ToString();
        skill2Button.transform.Find("Text").GetComponent<Text>().text = CustomKeyBinding.GetSkill2Key().ToString();
        skill3Button.transform.Find("Text").GetComponent<Text>().text = CustomKeyBinding.GetSkill3Key().ToString();
        skill4Button.transform.Find("Text").GetComponent<Text>().text = CustomKeyBinding.GetSkill4Key().ToString();
        characterWindowButton.transform.Find("Text").GetComponent<Text>().text = CustomKeyBinding.GetCharacterWindowKey().ToString();
        scoreboardButton.transform.Find("Text").GetComponent<Text>().text = CustomKeyBinding.GetScoreboardKey().ToString();
        inGameMenuButton.transform.Find("Text").GetComponent<Text>().text = CustomKeyBinding.GetInGameMenuKey().ToString();
    }

    /// <summary>
    /// Change the background colour of the button.
    /// If highlighting, colour will be set to yellow.
    /// If un-highlighting, colour will be set to white.
    /// </summary>
    /// <param name="button">Button to change the background colour of.</param>
    /// <param name="highlight">Whether or not we are highlighting the button, or unhighlighting the button.</param>
    private void ChangeButtonColour(Button button, bool highlight)
    {
        ColorBlock cb = button.colors;

        if (highlight)
        {
            // Set button background colour to yellow
            cb.normalColor = new Color(1.0f, 0.8f, 0.4f);
            cb.highlightedColor = new Color(1.0f, 0.8f, 0.4f);
            cb.pressedColor = new Color(1.0f, 0.8f, 0.4f);
        }
        else
        {
            // Set button background colour back to default
            cb.normalColor = Color.white;
            cb.highlightedColor = new Color(0.95f, 0.95f, 0.95f);
            cb.pressedColor = new Color(0.8f, 0.8f, 0.8f);
        }

        button.colors = cb;
    }

    /// <summary>
    /// Actions to perform when forward button is clicked.
    /// Highlights button while waiting for player input.
    /// Once player key input is detected, binds forward button to that key.
    /// </summary>
    private void ListenForwardButtonWrapper()
    {
        if (isListening) return;    // only listen to one button at a time

        // Last key binding was left click; do not perform any actions
        if (keyToBind == KeyCode.Mouse0)
        {
            keyToBind = KeyCode.None;
            return;
        }

        StartCoroutine(ListenForwardButton());
    }
    private IEnumerator ListenForwardButton()
    {
        isListening = true;
        oldKey = CustomKeyBinding.GetForwardKey();
        ChangeButtonColour(forwardButton, true);

        // Wait for player input
        while (isListening)
        {
            yield return null;
        }

        // Set key bind
        ChangeButtonColour(forwardButton, false);
        PlayerPrefs.SetString("forward", keyToBind.ToString());
        CustomKeyBinding.SetupCustomKeyBindings();
        SetupButtonText();
    }

    /// <summary>
    /// Actions to perform when back button is clicked.
    /// Highlights button while waiting for player input.
    /// Once player key input is detected, binds back button to that key.
    /// </summary>
    private void ListenBackButtonWrapper()
    {
        if (isListening) return;    // only listen to one button at a time

        // Last key binding was left click; do not perform any actions
        if (keyToBind == KeyCode.Mouse0)
        {
            keyToBind = KeyCode.None;
            return;
        }

        StartCoroutine(ListenBackButton());
    }
    private IEnumerator ListenBackButton()
    {
        isListening = true;
        oldKey = CustomKeyBinding.GetBackKey();
        ChangeButtonColour(backButton, true);

        // Wait for player input
        while (isListening)
        {
            yield return null;
        }

        // Set key bind
        ChangeButtonColour(backButton, false);
        PlayerPrefs.SetString("back", keyToBind.ToString());
        CustomKeyBinding.SetupCustomKeyBindings();
        SetupButtonText();
    }

    /// <summary>
    /// Actions to perform when left button is clicked.
    /// Highlights button while waiting for player input.
    /// Once player key input is detected, binds left button to that key.
    /// </summary>
    private void ListenLeftButtonWrapper()
    {
        if (isListening) return;    // only listen to one button at a time

        // Last key binding was left click; do not perform any actions
        if (keyToBind == KeyCode.Mouse0)
        {
            keyToBind = KeyCode.None;
            return;
        }

        StartCoroutine(ListenLeftButton());
    }
    private IEnumerator ListenLeftButton()
    {
        isListening = true;
        oldKey = CustomKeyBinding.GetLeftKey();
        ChangeButtonColour(leftButton, true);

        // Wait for player input
        while (isListening)
        {
            yield return null;
        }

        // Set key bind
        ChangeButtonColour(leftButton, false);
        PlayerPrefs.SetString("left", keyToBind.ToString());
        CustomKeyBinding.SetupCustomKeyBindings();
        SetupButtonText();
    }

    /// <summary>
    /// Actions to perform when right button is clicked.
    /// Highlights button while waiting for player input.
    /// Once player key input is detected, binds right button to that key.
    /// </summary>
    private void ListenRightButtonWrapper()
    {
        if (isListening) return;    // only listen to one button at a time

        // Last key binding was left click; do not perform any actions
        if (keyToBind == KeyCode.Mouse0)
        {
            keyToBind = KeyCode.None;
            return;
        }

        StartCoroutine(ListenRightButton());
    }
    private IEnumerator ListenRightButton()
    {
        isListening = true;
        oldKey = CustomKeyBinding.GetRightKey();
        ChangeButtonColour(rightButton, true);

        // Wait for player input
        while (isListening)
        {
            yield return null;
        }

        // Set key bind
        ChangeButtonColour(rightButton, false);
        PlayerPrefs.SetString("right", keyToBind.ToString());
        CustomKeyBinding.SetupCustomKeyBindings();
        SetupButtonText();
    }

    /// <summary>
    /// Actions to perform when basic attack button is clicked.
    /// Highlights button while waiting for player input.
    /// Once player key input is detected, binds basic attack button to that key.
    /// </summary>
    private void ListenBasicAttackButtonWrapper()
    {
        if (isListening) return;    // only listen to one button at a time

        // Last key binding was left click; do not perform any actions
        if (keyToBind == KeyCode.Mouse0)
        {
            keyToBind = KeyCode.None;
            return;
        }

        StartCoroutine(ListenBasicAttackButton());
    }
    private IEnumerator ListenBasicAttackButton()
    {
        isListening = true;
        oldKey = CustomKeyBinding.GetBasicAttackKey();
        ChangeButtonColour(basicAttackButton, true);

        // Wait for player input
        while (isListening)
        {
            yield return null;
        }

        // Set key bind
        ChangeButtonColour(basicAttackButton, false);
        PlayerPrefs.SetString("basicAttack", keyToBind.ToString());
        CustomKeyBinding.SetupCustomKeyBindings();
        SetupButtonText();
    }

    /// <summary>
    /// Actions to perform when Skill1 button is clicked.
    /// Highlights button while waiting for player input.
    /// Once player key input is detected, binds Skill1 button to that key.
    /// </summary>
    private void ListenSkill1ButtonWrapper()
    {
        if (isListening) return;    // only listen to one button at a time

        // Last key binding was left click; do not perform any actions
        if (keyToBind == KeyCode.Mouse0)
        {
            keyToBind = KeyCode.None;
            return;
        }

        StartCoroutine(ListenSkill1Button());
    }
    private IEnumerator ListenSkill1Button()
    {
        isListening = true;
        oldKey = CustomKeyBinding.GetSkill1Key();
        ChangeButtonColour(skill1Button, true);

        // Wait for player input
        while (isListening)
        {
            yield return null;
        }

        // Set key bind
        ChangeButtonColour(skill1Button, false);
        PlayerPrefs.SetString("skill1", keyToBind.ToString());
        CustomKeyBinding.SetupCustomKeyBindings();
        SetupButtonText();
    }

    /// <summary>
    /// Actions to perform when Skill2 button is clicked.
    /// Highlights button while waiting for player input.
    /// Once player key input is detected, binds Skill2 button to that key.
    /// </summary>
    private void ListenSkill2ButtonWrapper()
    {
        if (isListening) return;    // only listen to one button at a time

        // Last key binding was left click; do not perform any actions
        if (keyToBind == KeyCode.Mouse0)
        {
            keyToBind = KeyCode.None;
            return;
        }

        StartCoroutine(ListenSkill2Button());
    }
    private IEnumerator ListenSkill2Button()
    {
        isListening = true;
        oldKey = CustomKeyBinding.GetSkill2Key();
        ChangeButtonColour(skill2Button, true);

        // Wait for player input
        while (isListening)
        {
            yield return null;
        }

        // Set key bind
        ChangeButtonColour(skill2Button, false);
        PlayerPrefs.SetString("skill2", keyToBind.ToString());
        CustomKeyBinding.SetupCustomKeyBindings();
        SetupButtonText();
    }

    /// <summary>
    /// Actions to perform when Skill3 button is clicked.
    /// Highlights button while waiting for player input.
    /// Once player key input is detected, binds Skill3 button to that key.
    /// </summary>
    private void ListenSkill3ButtonWrapper()
    {
        if (isListening) return;    // only listen to one button at a time

        // Last key binding was left click; do not perform any actions
        if (keyToBind == KeyCode.Mouse0)
        {
            keyToBind = KeyCode.None;
            return;
        }

        StartCoroutine(ListenSkill3Button());
    }
    private IEnumerator ListenSkill3Button()
    {
        isListening = true;
        oldKey = CustomKeyBinding.GetSkill3Key();
        ChangeButtonColour(skill3Button, true);

        // Wait for player input
        while (isListening)
        {
            yield return null;
        }

        // Set key bind
        ChangeButtonColour(skill3Button, false);
        PlayerPrefs.SetString("skill3", keyToBind.ToString());
        CustomKeyBinding.SetupCustomKeyBindings();
        SetupButtonText();
    }

    /// <summary>
    /// Actions to perform when Skill4 button is clicked.
    /// Highlights button while waiting for player input.
    /// Once player key input is detected, binds Skill4 button to that key.
    /// </summary>
    private void ListenSkill4ButtonWrapper()
    {
        if (isListening) return;    // only listen to one button at a time

        // Last key binding was left click; do not perform any actions
        if (keyToBind == KeyCode.Mouse0)
        {
            keyToBind = KeyCode.None;
            return;
        }

        StartCoroutine(ListenSkill4Button());
    }
    private IEnumerator ListenSkill4Button()
    {
        isListening = true;
        oldKey = CustomKeyBinding.GetSkill4Key();
        ChangeButtonColour(skill4Button, true);

        // Wait for player input
        while (isListening)
        {
            yield return null;
        }

        // Set key bind
        ChangeButtonColour(skill4Button, false);
        PlayerPrefs.SetString("skill4", keyToBind.ToString());
        CustomKeyBinding.SetupCustomKeyBindings();
        SetupButtonText();
    }

    /// <summary>
    /// Actions to perform when Character Window button is clicked.
    /// Highlights button while waiting for player input.
    /// Once player key input is detected, binds Character Window button to that key.
    /// </summary>
    private void ListenCharacterWindowButtonWrapper()
    {
        if (isListening) return;    // only listen to one button at a time

        // Last key binding was left click; do not perform any actions
        if (keyToBind == KeyCode.Mouse0)
        {
            keyToBind = KeyCode.None;
            return;
        }

        StartCoroutine(ListenCharacterWindowButton());
    }
    private IEnumerator ListenCharacterWindowButton()
    {
        isListening = true;
        oldKey = CustomKeyBinding.GetCharacterWindowKey();
        ChangeButtonColour(characterWindowButton, true);

        // Wait for player input
        while (isListening)
        {
            yield return null;
        }

        // Set key bind
        ChangeButtonColour(characterWindowButton, false);
        PlayerPrefs.SetString("characterWindow", keyToBind.ToString());
        CustomKeyBinding.SetupCustomKeyBindings();
        SetupButtonText();
    }

    /// <summary>
    /// Actions to perform when Scoreboard button is clicked.
    /// Highlights button while waiting for player input.
    /// Once player key input is detected, binds Scoreboard button to that key.
    /// </summary>
    private void ListenScoreboardButtonWrapper()
    {
        if (isListening) return;    // only listen to one button at a time

        // Last key binding was left click; do not perform any actions
        if (keyToBind == KeyCode.Mouse0)
        {
            keyToBind = KeyCode.None;
            return;
        }

        StartCoroutine(ListenScoreboardButton());
    }
    private IEnumerator ListenScoreboardButton()
    {
        isListening = true;
        oldKey = CustomKeyBinding.GetScoreboardKey();
        ChangeButtonColour(scoreboardButton, true);

        // Wait for player input
        while (isListening)
        {
            yield return null;
        }

        // Set key bind
        ChangeButtonColour(scoreboardButton, false);
        PlayerPrefs.SetString("scoreboard", keyToBind.ToString());
        CustomKeyBinding.SetupCustomKeyBindings();
        SetupButtonText();
    }

    /// <summary>
    /// Actions to perform when In-game Menu button is clicked.
    /// Highlights button while waiting for player input.
    /// Once player key input is detected, binds In-game Menu button to that key.
    /// </summary>
    private void ListenInGameMenuButtonWrapper()
    {
        if (isListening) return;    // only listen to one button at a time

        // Last key binding was left click; do not perform any actions
        if (keyToBind == KeyCode.Mouse0)
        {
            keyToBind = KeyCode.None;
            return;
        }

        StartCoroutine(ListenInGameMenuButton());
    }
    private IEnumerator ListenInGameMenuButton()
    {
        isListening = true;
        oldKey = CustomKeyBinding.GetInGameMenuKey();
        ChangeButtonColour(inGameMenuButton, true);

        // Wait for player input
        while (isListening)
        {
            yield return null;
        }

        // Set key bind
        ChangeButtonColour(inGameMenuButton, false);
        PlayerPrefs.SetString("inGameMenu", keyToBind.ToString());
        CustomKeyBinding.SetupCustomKeyBindings();
        SetupButtonText();
    }
}
