using UnityEngine;
using System.Collections.Generic;

public class Firearm : Item
{
    [Header("Stats")]
    public float damage;
    public float range;
    public float impactForce;
    public LayerMask hitLayerMask;

    [Header("Reloading")]
    public int ammoClipSize;
    [HideInInspector] public int currentClipAmount;

    public float reloadTime;
    public float reloadDisableShootTime;
    bool reloadRequiredComplete;
    [HideInInspector] public float currentReloadTime;

    public static bool globalCurrentlyReloading;

    [Header("Graphics")]
    public Transform muzzleFlashPos;
    public GameObject muzzleFlash;
    public List<GameObject> shootSfx;

    public static Transform currentMuzzleFlashPos;

    [Header("Model Recoil")]
    public LocalOffsetManager localOffsetManager;
    public Vector3 moveRecoilA;
    public Vector3 moveRecoilB;
    Vector3 targetMoveRecoil;
    Vector3 currentMoveRecoil;
    public float moveRecoilSpeed;
    public float moveRecoilSmoothness;

    public LocalRotationOffsetManager localRotationOffsetManager;
    public Vector3 rotRecoilA;
    public Vector3 rotRecoilB;
    Vector3 targetRotRecoil;
    Vector3 currentRotRecoil;
    public float rotRecoilSpeed;
    public float rotRecoilSmoothness;

    [Header("Run Offset")]
    public Vector2 runBobMultiplier = Vector2.one;
    public Vector3 runPosOffset = Vector3.zero;
    public Vector3 runRotOffset = Vector3.zero;
    public float runModelOffsetSmoothness = 5f;
    Vector3 currentRunPosOffset;
    Vector3 currentRunRotOffset;

    [Header("Camera Recoil")]
    //public LocalRotationOffsetManager cameraRecoilManager;
    public float wCamRecoilA;
    public float wCamRecoilB;
    float currentWCamRecoil;
    float targetWCamRecoil;
    public float wCamRecoilSpeed;
    public float wCamRecoilSmoothness;

    public Vector3 camRecoilA;
    public Vector3 camRecoilB;
    Vector3 currentCamRecoil;
    [HideInInspector] public Vector3 targetCamRecoil;
    public float camRecoilSpeed;
    public float camRecoilSmoothness;

    int shotsThisFrame;

    [Header("Fixed Recoil")]
    public Vector3 fixedRecoilA;
    public Vector3 fixedRecoilB;
    public Vector3[] fixedRecoils;
    public int fixedRecoilSeed;
    public int fixedRecoilPointer;
    public float fixedRecoilPointerDecayTime;
    float currentFixedRecoilPointerDecayTime;
    public float fixedRecoilPointerDecaysUntilMultiplier;
    int currentFixedRecoilPointerDecaysUntilMultiplier;
    public float fixedRecoilPointerSpeedupMultiplier;

    Vector3 targetCamRecoilVelocity;
    Vector3 currentCamRecoilVelocity;

    [HideInInspector] public float shotgunSpreadMultiplier = 1f;

    [Header("Ricochet")]
    public List<GameObject> ricochets = new List<GameObject>();

    public override void Start()
    {
        globalCurrentlyReloading = false;
        base.Start();
        localOffsetManager.AddValue(transform.name + "MoveRecoil", Vector3.zero);
        localRotationOffsetManager.AddValue(transform.name + "RotRecoil", Vector3.zero);
        //cameraRecoilManager.AddValue(transform.name + "CamRecoil", Vector3.zero);
        Cache.cameraRotationOffsetManager.AddOffset(transform.name + "Recoil", new CameraRotationOffset{offset = Vector4.zero});
        localOffsetManager.AddValue(transform.name + "RunOffset", Vector3.zero);
        localRotationOffsetManager.AddValue(transform.name + "RunRotOffset", Vector3.zero);
        reloadRequiredComplete = true;
        GenerateFixedRecoil();
    }
    public void GenerateFixedRecoil()
    {
        System.Random rand = new System.Random(fixedRecoilSeed);
        fixedRecoils = new Vector3[ammoClipSize];
        for (int i = 0; i < fixedRecoils.Length; i++)
        {
            fixedRecoils[i] = new Vector3(Utilities.NextFloat(rand, fixedRecoilA.x, fixedRecoilB.x), Utilities.NextFloat(rand, fixedRecoilA.y, fixedRecoilB.y), Utilities.NextFloat(rand, fixedRecoilA.z, fixedRecoilB.z));
        }
    }
    public override void OnMainAction()
    {
        base.OnMainAction();

        if (currentClipAmount > 0)
        {
            if (currentReloadTime < (reloadTime - reloadDisableShootTime))
            {
                Shoot();
            }
        }
        else
        {
            NoAmmoShoot();
        }
    }
    public void NoAmmoShoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnDryFire();
        }
    }
    protected virtual void OnDryFire()
    {

    }
    public void Shoot()
    {
        if (muzzleFlash != null) { Instantiate(muzzleFlash, muzzleFlashPos.position, muzzleFlashPos.rotation, muzzleFlashPos); }
        targetMoveRecoil = Utilities.RandomRangeVector3(moveRecoilA, moveRecoilB);
        targetRotRecoil = Utilities.RandomRangeVector3(rotRecoilA, rotRecoilB);
        targetWCamRecoil = Random.Range(wCamRecoilA, wCamRecoilB) * Utilities.CoinFlipn();
        targetCamRecoil += fixedRecoils[fixedRecoilPointer];
        if (fixedRecoilPointer < fixedRecoils.Length - 1)
        {
            fixedRecoilPointer++;
        }
        currentFixedRecoilPointerDecaysUntilMultiplier = 0;
        currentFixedRecoilPointerDecayTime = fixedRecoilPointerDecayTime * 2f;

        bool shotgun = this is Shotgun;

        if (shotgun)
        {
            if (shotsThisFrame == 0)
            {
                Instantiate(shootSfx[Random.Range(0,shootSfx.Count)], transform.position, Quaternion.identity);
            }
        }
        else { Instantiate(shootSfx[Random.Range(0, shootSfx.Count)], transform.position, Quaternion.identity); }

        shotsThisFrame++;

        Vector3 spread = shotgun ? fixedRecoils[fixedRecoilPointer] * shotgunSpreadMultiplier : Vector3.zero;

        Transform cam = Camera.main.transform;
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward + spread, out hit, range, hitLayerMask))
        {
            OnHit(hit);
        }

        currentClipAmount--;
    }
    protected virtual void OnHit(RaycastHit hit)
    {
        Rigidbody rb = hit.rigidbody;
        if (rb != null && !rb.isKinematic)
        {
            rb.AddForce(-hit.normal * impactForce);
        }

        IDamageable damageable = hit.transform.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.Damage(damage);
        }

        Instantiate(ricochets[Random.RandomRange(0,ricochets.Count)], hit.point, Quaternion.Euler(-hit.normal),hit.transform);

        if (Cache.references.bulletHoles.ContainsKey(hit.transform.gameObject.tag))
        {
            Instantiate(Cache.references.bulletHoles[hit.transform.gameObject.tag], hit.point, Quaternion.Euler(-hit.normal), hit.transform);
        }
    }
    public virtual void OnReloadComplete()
    {
        currentClipAmount = ammoClipSize;
        reloadRequiredComplete = true;
    }
    public virtual void OnReloadStart()
    {
        currentReloadTime = reloadTime;
        globalCurrentlyReloading = true;
        reloadRequiredComplete = false;
    }
    public virtual void OnReloadCancel()
    {
        currentReloadTime = 0;
        globalCurrentlyReloading = false;
    }
    public override void OnEquip()
    {
        base.OnEquip();
        Cache.itemRunBobManager.baseAmountMultiplier = runBobMultiplier;
        currentMuzzleFlashPos = muzzleFlashPos;
    }
    public override void OnUnequip()
    {
        base.OnUnequip();
        OnReloadCancel();
        Cache.itemRunBobManager.baseAmountMultiplier = Vector2.one;
    }
    public override void Update()
    {
        base.Update();
        targetMoveRecoil = Vector3.Lerp(targetMoveRecoil, Vector3.zero, Time.deltaTime * moveRecoilSpeed);
        currentMoveRecoil = Vector3.Lerp(currentMoveRecoil, targetMoveRecoil, Time.deltaTime * moveRecoilSmoothness);
        localOffsetManager.UpdateValue(transform.name + "MoveRecoil", currentMoveRecoil);

        targetRotRecoil = Vector3.Lerp(targetRotRecoil, Vector3.zero, Time.deltaTime * rotRecoilSpeed);
        currentRotRecoil = Vector3.Lerp(currentRotRecoil, targetRotRecoil, Time.deltaTime * rotRecoilSmoothness);
        localRotationOffsetManager.UpdateValue(transform.name + "RotRecoil", currentRotRecoil);

        // This stuff needs to be assigned to backported main camera rot manager
        targetWCamRecoil = Mathf.Lerp(targetWCamRecoil, 0f, Time.deltaTime * wCamRecoilSpeed);
        currentWCamRecoil = Mathf.Lerp(currentWCamRecoil, targetWCamRecoil, Time.deltaTime * wCamRecoilSmoothness);

        targetCamRecoil = Vector3.SmoothDamp(targetCamRecoil, Vector3.zero, ref targetCamRecoilVelocity, camRecoilSpeed);
        currentCamRecoil = Vector3.SmoothDamp(currentCamRecoil, targetCamRecoil, ref currentCamRecoilVelocity, camRecoilSmoothness);
        Cache.cameraRotationOffsetManager.UpdateOffset(transform.name + "Recoil", new CameraRotationOffset { offset = new Vector4(currentCamRecoil.x, currentCamRecoil.y, currentCamRecoil.z, currentWCamRecoil), forceBob = true });
        // /\/\/\

        currentRunPosOffset = Vector3.Lerp(currentRunPosOffset, FragMovementListener.running ? runPosOffset : Vector3.zero, Time.deltaTime * runModelOffsetSmoothness);
        currentRunRotOffset = Vector3.Lerp(currentRunRotOffset, FragMovementListener.running ? runRotOffset : Vector3.zero, Time.deltaTime * runModelOffsetSmoothness);
        localOffsetManager.UpdateValue(transform.name + "RunOffset", currentRunPosOffset);
        localRotationOffsetManager.UpdateValue(transform.name + "RunRotOffset", currentRunRotOffset);

        shotsThisFrame = 0;

        if (Input.GetKeyDown(KeyCode.R) && equip && !globalCurrentlyReloading)
        {
            if (currentClipAmount < ammoClipSize) { OnReloadStart(); }
        }
        if (currentReloadTime > 0f)
        {
            currentReloadTime -= Time.deltaTime;
            if (currentReloadTime <= (reloadTime - reloadDisableShootTime) && !reloadRequiredComplete)
            {
                OnReloadComplete();
            }
            if (currentReloadTime <= 0f && globalCurrentlyReloading)
            {
                globalCurrentlyReloading = false;
            }
        }
        if (currentFixedRecoilPointerDecayTime > 0f && fixedRecoilPointer > 0)
        {
            currentFixedRecoilPointerDecayTime -= Time.deltaTime;
            if (currentFixedRecoilPointerDecayTime <= 0)
            {
                fixedRecoilPointer--;
                currentFixedRecoilPointerDecaysUntilMultiplier++;
                currentFixedRecoilPointerDecayTime = fixedRecoilPointerDecayTime * (currentFixedRecoilPointerDecaysUntilMultiplier >= fixedRecoilPointerDecaysUntilMultiplier ? fixedRecoilPointerSpeedupMultiplier : 1f);
            }
        }
    }
}
