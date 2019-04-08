using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Attached to every skill slot to allow for hover descriptions.
/// Also allows for equipping skills by clicking/dragging & dropping during pre-phase.
/// </summary>
public class SkillHoverDescription : EventTrigger
{
    public Skill skill;         // the current skill that this script is attached to

    // Drag and drop
    private GameObject icon;    // image of the skill used during dragging
    private Canvas canvas;      // canvas used for dragged icon to render it on top of everything else

    /// <summary>
    /// Enable skill description when skill is hovered over.
    /// </summary>
    public override void OnPointerEnter(PointerEventData eventData)
    {
        GameObject skillDescription = null;
        TextMeshProUGUI skillDescriptionText = null;
        TextMeshProUGUI skillTitleText = null;

        PrephaseManager prephaseManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<PrephaseManager>();
        HeroManager heroManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<HeroManager>();
        MatchManager matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        AbilityManager abilityManager = heroManager.GetHeroObject(matchManager.GetPlayerId()).GetComponent<AbilityManager>();

        if (prephaseManager.IsCurrentlyInPrephase())
        {
            // Logic for skill hover on prephase screen
            PrephaseUI prephaseUI = GameObject.FindGameObjectWithTag("PrephaseUI").GetComponent<PrephaseUI>();
            skillDescription = prephaseUI.skillDescription;
            skillDescriptionText = prephaseUI.skillDescriptionText;
            skillTitleText = prephaseUI.skillTitleText;

            // Do not display skill description for empty equipped skill slots
            if (transform.name == "EquipSkill1" && abilityManager.equippedSkills[0] == null)
            {
                return;
            }
            else if (transform.name == "EquipSkill2" && abilityManager.equippedSkills[1] == null)
            {
                return;
            }
            else if (transform.name == "EquipSkill3" && abilityManager.equippedSkills[2] == null)
            {
                return;
            }
            else if (transform.name == "EquipSkill4" && abilityManager.equippedSkills[3] == null)
            {
                return;
            }
        }
        else if (skill != null)
        {
            // Show skill description in stat window if skill is not null
            StatWindowUI statWindow = GameObject.FindGameObjectWithTag("StatWindow").GetComponent<StatWindowUI>();
            skillDescription = statWindow.skillDescription;
            skillDescriptionText = statWindow.skillDescriptionText;
            skillTitleText = statWindow.skillDescriptionTitleText;
        }

        // Show skill description
        if (skillTitleText != null && skillDescriptionText != null && skillDescription != null)
        {
            skillTitleText.text = skill.skillName;
            skillDescriptionText.text = skill.skillDescription;
            skillDescription.SetActive(true);
        }
    }

    /// <summary>
    /// Disable skill description when skill is no longer hovered over.
    /// </summary>
    public override void OnPointerExit(PointerEventData eventData)
    {
        GameObject skillDescription = null;
        PrephaseManager prephaseManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<PrephaseManager>();

        if (prephaseManager.IsCurrentlyInPrephase())
        {
            PrephaseUI prephaseUI = GameObject.FindGameObjectWithTag("PrephaseUI").GetComponent<PrephaseUI>();
            skillDescription = prephaseUI.skillDescription;
        }
        else
        {
            StatWindowUI statWindow = GameObject.FindGameObjectWithTag("StatWindow").GetComponent<StatWindowUI>();
            skillDescription = statWindow.skillDescription;
        }

        if (skillDescription != null)
        {
            skillDescription.SetActive(false);
        }
    }

    /// <summary>
    /// When skill in skill bank is clicked, adds it to equipped skills.
    /// When equipped skill is clicked, unequips the skill.
    /// </summary>
    public override void OnPointerClick(PointerEventData eventData)
    {
        // Do nothing if this is not the prephase screen
        PrephaseManager prephaseManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<PrephaseManager>();
        if (!prephaseManager.IsCurrentlyInPrephase()) return;

        HeroManager heroManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<HeroManager>();
        MatchManager matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        AbilityManager abilityManager = heroManager.GetHeroObject(matchManager.GetPlayerId()).GetComponent<AbilityManager>();
        PrephaseUI prephaseUI = GameObject.FindGameObjectWithTag("PrephaseUI").GetComponent<PrephaseUI>();

        if (!transform.name.Contains("Equip"))
        {
            // Skill in skill bank is clicked, attempt to equip the skill
            abilityManager.EquipSkill(skill);
        }
        else
        {
            // Unequip the clicked equipped skill
            abilityManager.UnequipSkill(skill);
        }

        // Reflect the equipped skills change in the UI
        prephaseUI.UpdateEquippedSkills();
    }

    /// <summary>
    /// This skill has begun to be dragged.
    /// Create its icon to be dragged.
    /// </summary>
    public override void OnBeginDrag(PointerEventData eventData)
    {
        // Do nothing if no skill is contained
        if (skill == null) return;

        // Only allow dragging during pre-phase
        PrephaseManager prephaseManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<PrephaseManager>();
        if (!prephaseManager.IsCurrentlyInPrephase()) return;

        // Do not allow dragging from skill bank if skill is already equipped
        HeroManager heroManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<HeroManager>();
        MatchManager matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        AbilityManager abilityManager = heroManager.GetHeroObject(matchManager.GetPlayerId()).GetComponent<AbilityManager>();
        if (!transform.name.Contains("Equip") && abilityManager.IsEquipped(skill)) return;

        // Initialize canvas
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject(transform.name);
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        // Create the icon to be dragged
        icon = new GameObject();
        icon.transform.SetParent(canvas.transform);
        icon.name = skill.name;
        icon.tag = "DraggedSkill";

        // Set the image sprite
        Image iconImage = icon.AddComponent<Image>();
        iconImage.sprite = GetComponent<Image>().sprite;

        // Set the dimensions
        RectTransform iconRect = icon.GetComponent<RectTransform>();
        RectTransform myRect = GetComponent<RectTransform>();
        iconRect.sizeDelta = new Vector2(myRect.rect.width, myRect.rect.height);

        // Disable raycasting for this icon
        iconImage.raycastTarget = false;

        // Check if dragged skill came from an equipped skill, and unequip it
        if (GameObject.FindGameObjectWithTag("DraggedSkill").transform.parent.name.Contains("Equip"))
        {
            // Unequip dragged skill
            Skill draggedSkill = GetSkill(GameObject.FindGameObjectWithTag("DraggedSkill").name);
            abilityManager.UnequipSkill(draggedSkill);

            // Reflect the equipped skills change in the UI
            PrephaseUI prephaseUI = GameObject.FindGameObjectWithTag("PrephaseUI").GetComponent<PrephaseUI>();
            prephaseUI.UpdateEquippedSkills();
        }
    }

    /// <summary>
    /// Called during dragging of the skill.
    /// Allows an image of the skill to be dragged around the screen.
    /// </summary>
    public override void OnDrag(PointerEventData eventData)
    {
        if (icon != null)
        {
            icon.transform.position = Input.mousePosition;
        }
    }

    /// <summary>
    /// Called when dragging of the skill ends.
    /// Remove the image that was being dragged.
    /// </summary>
    public override void OnEndDrag(PointerEventData eventData)
    {
        // Delete canvas
        if (canvas != null)
        {
            Destroy(canvas.gameObject);
            canvas = null;
        }

        // Delete icon
        if (icon != null)
        {
            icon = null;
        }
    }

    /// <summary>
    /// Skill has been dropped on this slot.
    /// </summary>
    public override void OnDrop(PointerEventData eventData)
    {
        // Do not allow dropping skills in slots which are not for equipping skills
        if (!transform.name.Contains("Equip")) return;

        // Get AbilityManager and PrephaseUI
        HeroManager heroManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<HeroManager>();
        MatchManager matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        AbilityManager abilityManager = heroManager.GetHeroObject(matchManager.GetPlayerId()).GetComponent<AbilityManager>();
        PrephaseUI prephaseUI = GameObject.FindGameObjectWithTag("PrephaseUI").GetComponent<PrephaseUI>();
        
        int slotIndex = int.Parse(transform.name.Replace("EquipSkill", "")) - 1;                // get slot index based on slot name
        Skill droppedSkill = GetSkill(GameObject.FindGameObjectWithTag("DraggedSkill").name);   // get the skill currently being dropped

        // Check if dragged skill came from an equipped skill
        if (GameObject.FindGameObjectWithTag("DraggedSkill").transform.parent.name.Contains("Equip"))
        {
            // Check if this is a swap between two equipped skills
            if (skill != null)
            {
                Skill oldSkill = skill;
                abilityManager.UnequipSkill(oldSkill);

                // Equip old skill in the other slot which we dragged from
                int oldIndex = int.Parse(GameObject.FindGameObjectWithTag("DraggedSkill").transform.parent.name.Replace("EquipSkill", "")) - 1;
                abilityManager.EquipSkill(oldSkill, oldIndex);
            }
        }

        // Equip the skill in this slot
        abilityManager.EquipSkill(droppedSkill, slotIndex);

        // Reflect the equipped skills change in the UI
        prephaseUI.UpdateEquippedSkills();
    }

    /// <returns>
    /// Returns the skill with the given name.
    /// </returns>
    /// <param name="name">Name of the wanted skill.</param>
    private Skill GetSkill(string name)
    {
        // Get AbilityManager
        HeroManager heroManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<HeroManager>();
        MatchManager matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        AbilityManager abilityManager = heroManager.GetHeroObject(matchManager.GetPlayerId()).GetComponent<AbilityManager>();

        // Loop through all known abilities, and check if one of them matches the given name
        foreach (Skill s in abilityManager.knownSkills)
        {
            if (s.name.Equals(name))
            {
                return s;
            }
        }

        return null;
    }
}
