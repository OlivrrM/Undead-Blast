using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item")]
    public string identifier;
    public Transform equipTransform;
    [HideInInspector] public bool equip;

    public bool holdKeyMain;
    public bool holdKeySecondary;

    public Vector3 unequipPos;
    public float equipSpeed;

    public Vector3 unequipRot;
    public float unequipSpeed;

    float timeAfterEquip;
    float timeAfterEquipSpeedIncrease;

    bool hasUsedThisEquip;

    public float holdWalkSpeedMultiplier = 1f;

    public float onMainActionCd;
    float currentOnMainActionCd;
    public float onSecondaryActionCd;
    float currentOnSecondaryActionCd;

    public GameObject thrownItem;

    [Header("Inspect")]
    public AnimationClip inspectAnimation;
    [HideInInspector] public bool hasInspect;
    [HideInInspector] public float currentInspectTime;
    public float inspectSpeed;
    private void Awake()
    {
        equipTransform.localPosition = unequipPos;
        equipTransform.localRotation = Quaternion.Euler(unequipRot);
        hasInspect = inspectAnimation != null;
    }
    public virtual void Start()
    {
        
    }
    public virtual void Update()
    {
        if (equip)
        {
            if (holdKeyMain) { if (Input.GetMouseButton(0) && currentOnMainActionCd <= 0f) { OnMainAction(); } }
            else { if (Input.GetMouseButtonDown(0) && currentOnMainActionCd <= 0f) { OnMainAction(); } }
            if (holdKeySecondary) { if (Input.GetMouseButton(1) && currentOnSecondaryActionCd <= 0f) { OnSecondaryAction(); } }
            else { if (Input.GetMouseButtonDown(1) && currentOnSecondaryActionCd <= 0f) { OnSecondaryAction(); } }
        }

        timeAfterEquip += Time.deltaTime;
        if (timeAfterEquip > 1f)
        {
            timeAfterEquipSpeedIncrease += Time.deltaTime * 100;
        }
        else
        {
            timeAfterEquipSpeedIncrease = 1f;
        }

        if (equip)
        {
            equipTransform.localPosition = Vector3.Lerp(equipTransform.localPosition, Vector3.zero, Time.deltaTime * equipSpeed * (hasUsedThisEquip ? 5f : 1f));
            equipTransform.localRotation = Quaternion.Lerp(equipTransform.localRotation, Quaternion.Euler(Vector3.zero), Time.deltaTime * equipSpeed * timeAfterEquipSpeedIncrease * (hasUsedThisEquip ? 5f : 1f));
        }
        else
        {
            equipTransform.localPosition = Vector3.Lerp(equipTransform.localPosition, unequipPos, Time.deltaTime * equipSpeed);
            equipTransform.localRotation = Quaternion.Lerp(equipTransform.localRotation, Quaternion.Euler(unequipRot), Time.deltaTime * unequipSpeed * timeAfterEquipSpeedIncrease);
        }

        currentOnMainActionCd -= Time.deltaTime;
        currentOnSecondaryActionCd -= Time.deltaTime;

        /*if (Cache.inventory.equippedItem == this)
        {
            Cache.walkSpeedManager.UpdateValue("EquippedItem", holdWalkSpeedMultiplier);
        }*/

        if (hasInspect)
        {
            bool canceled = false;
            if (currentInspectTime > 0f)
            {
                currentInspectTime -= Time.deltaTime;
                if (currentInspectTime <= 0f)
                {
                    OnInspectFinish();
                }
                if (Input.GetKeyDown(KeyCode.I))
                {
                    canceled = true;
                    OnInspectCancel();
                }
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (currentInspectTime <= 0f && !canceled)
                {
                    OnInspect();
                }
            }
        }
    }
    public virtual void OnInspect()
    {
        currentInspectTime = inspectAnimation.length / inspectSpeed;
    }
    public virtual void OnInspectFinish()
    {

    }
    public virtual void OnInspectCancel()
    {
        currentInspectTime = 0f;
    }
    public virtual void OnMainAction()
    {
        hasUsedThisEquip = true;
        currentOnMainActionCd = onMainActionCd;
    }
    public virtual void OnSecondaryAction()
    {
        currentOnSecondaryActionCd = onSecondaryActionCd;
    }
    public virtual void OnEquip()
    {
        equip = true;
        timeAfterEquip = 0;
        hasUsedThisEquip = false;
    }
    public virtual void OnUnequip()
    {
        equip = false;
        hasUsedThisEquip = false;
        timeAfterEquip = 0;
    }
}
