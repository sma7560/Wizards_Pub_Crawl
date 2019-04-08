using System;
using UnityEngine;

/// <summary>
/// Stores the custom key bindings set by the user.
/// </summary>
public static class CustomKeyBinding
{
    // Default key bindings
    private static KeyCode forward = KeyCode.W;
    private static KeyCode back = KeyCode.S;
    private static KeyCode left = KeyCode.A;
    private static KeyCode right = KeyCode.D;
    private static KeyCode skill1 = KeyCode.Alpha1;
    private static KeyCode skill2 = KeyCode.Alpha2;
    private static KeyCode skill3 = KeyCode.Alpha3;
    private static KeyCode skill4 = KeyCode.Alpha4;
    private static KeyCode statWindow = KeyCode.K;
    private static KeyCode scoreboard = KeyCode.Tab;
    private static KeyCode inGameMenu = KeyCode.Escape;
    private static KeyCode basicAttack = KeyCode.Mouse0;

    /// <summary>
    /// Sets the key bindings based on player's custom preferences.
    /// </summary>
    public static void SetupCustomKeyBindings()
    {
        // Setup custom key bindings if they exist
        if (PlayerPrefs.HasKey("forward"))
        {
            KeyCode keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("forward"));
            SetForwardKey(keyCode);
        }

        if (PlayerPrefs.HasKey("back"))
        {
            KeyCode keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("back"));
            SetBackKey(keyCode);
        }

        if (PlayerPrefs.HasKey("left"))
        {
            KeyCode keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("left"));
            SetLeftKey(keyCode);
        }

        if (PlayerPrefs.HasKey("right"))
        {
            KeyCode keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("right"));
            SetRightKey(keyCode);
        }

        if (PlayerPrefs.HasKey("basicAttack"))
        {
            KeyCode keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("basicAttack"));
            SetBasicAttackKey(keyCode);
        }

        if (PlayerPrefs.HasKey("skill1"))
        {
            KeyCode keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("skill1"));
            SetSkill1Key(keyCode);
        }

        if (PlayerPrefs.HasKey("skill2"))
        {
            KeyCode keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("skill2"));
            SetSkill2Key(keyCode);
        }

        if (PlayerPrefs.HasKey("skill3"))
        {
            KeyCode keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("skill3"));
            SetSkill3Key(keyCode);
        }

        if (PlayerPrefs.HasKey("skill4"))
        {
            KeyCode keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("skill4"));
            SetSkill4Key(keyCode);
        }

        if (PlayerPrefs.HasKey("statWindow"))
        {
            KeyCode keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("statWindow"));
            SetStatWindowKey(keyCode);
        }

        if (PlayerPrefs.HasKey("scoreboard"))
        {
            KeyCode keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("scoreboard"));
            SetScoreboardKey(keyCode);
        }

        if (PlayerPrefs.HasKey("inGameMenu"))
        {
            KeyCode keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("inGameMenu"));
            SetInGameMenuKey(keyCode);
        }
    }

    /// <returns>
    /// True if key is already bound to another button; false otherwise.
    /// </returns>
    /// <param name="key">Key to check if it is already bound.</param>
    public static bool KeyAlreadyBound(KeyCode key)
    {
        if (forward == key || back == key || left == key || right == key ||
            basicAttack == key || skill1 == key || skill2 == key || skill3 == key || skill4 == key ||
            statWindow == key || scoreboard == key || inGameMenu == key)
        {
            return true;
        }
        
        return false;
    }

    /// --------------------------------------
    /// SETTERS
    /// --------------------------------------
    private static void SetForwardKey(KeyCode key)
    {
        forward = key;
    }

    private static void SetBackKey(KeyCode key)
    {
        back = key;
    }

    private static void SetLeftKey(KeyCode key)
    {
        left = key;
    }

    private static void SetRightKey(KeyCode key)
    {
        right = key;
    }

    private static void SetBasicAttackKey(KeyCode key)
    {
        basicAttack = key;
    }

    private static void SetSkill1Key(KeyCode key)
    {
        skill1 = key;
    }

    private static void SetSkill2Key(KeyCode key)
    {
        skill2 = key;
    }

    private static void SetSkill3Key(KeyCode key)
    {
        skill3 = key;
    }

    private static void SetSkill4Key(KeyCode key)
    {
        skill4 = key;
    }

    private static void SetStatWindowKey(KeyCode key)
    {
        statWindow = key;
    }

    private static void SetScoreboardKey(KeyCode key)
    {
        scoreboard = key;
    }

    private static void SetInGameMenuKey(KeyCode key)
    {
        inGameMenu = key;
    }

    /// --------------------------------------
    /// GETTERS
    /// --------------------------------------
    public static KeyCode GetForwardKey()
    {
        return forward;
    }

    public static KeyCode GetBackKey()
    {
        return back;
    }

    public static KeyCode GetLeftKey()
    {
        return left;
    }

    public static KeyCode GetRightKey()
    {
        return right;
    }

    public static KeyCode GetSkill1Key()
    {
        return skill1;
    }

    public static KeyCode GetSkill2Key()
    {
        return skill2;
    }

    public static KeyCode GetSkill3Key()
    {
        return skill3;
    }

    public static KeyCode GetSkill4Key()
    {
        return skill4;
    }

    public static KeyCode GetStatWindowKey()
    {
        return statWindow;
    }

    public static KeyCode GetScoreboardKey()
    {
        return scoreboard;
    }

    public static KeyCode GetInGameMenuKey()
    {
        return inGameMenu;
    }

    public static KeyCode GetBasicAttackKey()
    {
        return basicAttack;
    }

    /// <summary>
    /// Get the stylized name of the key.
    /// </summary>
    /// <param name="key">Key which we want the stylized name of.</param>
    /// <returns></returns>
    public static string GetKeyName(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Alpha0:
                return "0";
            case KeyCode.Alpha1:
                return "1";
            case KeyCode.Alpha2:
                return "2";
            case KeyCode.Alpha3:
                return "3";
            case KeyCode.Alpha4:
                return "4";
            case KeyCode.Alpha5:
                return "5";
            case KeyCode.Alpha6:
                return "6";
            case KeyCode.Alpha7:
                return "7";
            case KeyCode.Alpha8:
                return "8";
            case KeyCode.Alpha9:
                return "9";
            case KeyCode.Mouse0:
                return "LeftClick";
            case KeyCode.Mouse1:
                return "RightClick";
            case KeyCode.Mouse2:
                return "ScrollWheel";
            case KeyCode.LeftControl:
                return "LeftCtrl";
            case KeyCode.RightControl:
                return "RightCtrl";
            default:
                return key.ToString();
        }
    }
}
