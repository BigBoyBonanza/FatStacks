using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ArsenalSystem : MonoBehaviour
{
    public Pickup pickup;
    public Image ammoBar;
    public Text ammo;
    public enum GunType
    {
        match,
        freeze,
        launch,
        slapstick,
        bazooka,
        debug,
        none
    }
    [HideInInspector]
    public Gun equippedGun;
    [System.Serializable]
    public class ArsenalItem
    {
        public Gun _gun;
        public bool isInArsenal;
    }

    public ArsenalItem[] arsenal = new ArsenalItem[2];
    [SerializeField]
    private GunType startingGun;
    [HideInInspector]
    public int equippedGunIndex;
    public int prevEquippedGunIndex;
    [HideInInspector]
    public int arsenalSize;
    public bool canFire = true;
    public Image uiGunIcon;

    private bool fire1;
    private bool fire2;
    private void Awake()
    {
        pickup = GetComponentInParent<Pickup>();
    }
    void Start()
    {
        arsenalSize = EvaluateArsenalSize();
        if (Player.firstSpawnInScene)
            EquipGun(startingGun);
    }

    private void Update()
    {
        //Fire on fixed update
        fire1 = fire1 || Input.GetButtonDown("Fire1");
        fire2 = fire2 || Input.GetButtonDown("Fire2");

        ammoBar.fillAmount = Mathf.Lerp(ammoBar.fillAmount, equippedGun.getAmmoFill(), 0.1f);
        ammo.text = equippedGun.ammo.ToString();

        //Only allow weapon scrolling if arsenal is greater than 1
        if (arsenalSize > 0)
        {
            float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
            if (scrollDelta != 0)
            {
                int newGunIndex;
                int oldGunIndex = newGunIndex = equippedGunIndex;
                while (true)
                {
                    do
                    {
                        //Find a gun that is in the arsenal
                        newGunIndex = (int)(newGunIndex + Mathf.Sign(scrollDelta));
                        if (newGunIndex < 0)
                        {
                            newGunIndex = arsenal.Length - 1;
                        }
                        else if (newGunIndex == arsenal.Length)
                        {
                            newGunIndex = 0;
                        }
                        if (newGunIndex == oldGunIndex)
                        {
                            break;
                        }
                    }
                    while (arsenal[newGunIndex].isInArsenal == false);
                    //Equip the gun
                    EquipGun((GunType)newGunIndex);
                    if (equippedGun.canFire())
                    {
                        break;
                    }

                }
            }

            //Check for previous weapon
            if (Input.GetButtonDown("EquipPreviousWeapon"))
            {
                EquipGun((GunType)prevEquippedGunIndex);
            }

            //Check keys 1-9
            for (int key = 0; key < arsenal.Length; ++key)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + key))
                {
                    EquipGun((GunType)key);
                }
            }
        }
    }

    void FixedUpdate()
    {
        canFire = (equippedGunIndex != (int)GunType.none && equippedGun?.canFire() == true && Cursor.lockState == CursorLockMode.Locked);
        if (canFire)
        {
            if (fire1)
            {
                Ray ray = new Ray(transform.parent.position, transform.parent.rotation * Vector3.forward);
                equippedGun.fire1(ray);
                fire1 = false;
            }
            if (fire2)
            {
                Ray ray = new Ray(transform.parent.position, transform.parent.rotation * Vector3.forward);
                equippedGun.fire2(ray);
                fire2 = false;
            }
        }
        
    }
    public void AddGunToArsenal(GunType gun)
    {
        if (arsenal[(int)gun].isInArsenal == false)
        {
            arsenal[(int)gun].isInArsenal = true;
            arsenalSize += 1;
        }
    }
    public void EquipGun(GunType gun)
    {
        prevEquippedGunIndex = equippedGunIndex;
        equippedGunIndex = (int)gun;
        if (gun != GunType.none)
        {
            ShowAmmoInfo();
            ArsenalItem new_item = arsenal[(int)gun];
            if (new_item.isInArsenal)
            {
                Gun old_gun = equippedGun;
                equippedGun = new_item._gun;
                //Swap which game objects are active
                if (old_gun != null)
                {
                    old_gun.gameObject.SetActive(false);
                }
                equippedGun.gameObject.SetActive(true);
                uiGunIcon.sprite = equippedGun.gunData.gun_sprite;
            }
            else
            {
                Debug.Log(gun.ToString() + " was not in arsenal.");
                EquipGun(GunType.none);
            }
        }
        else
        {
            if (equippedGun != null)
            {
                equippedGun.gameObject.SetActive(false);
            }
            HideAmmoInfo();
        }

    }
    private int EvaluateArsenalSize()
    {
        int size = 0;
        foreach (ArsenalItem gun in arsenal)
        {
            size += (gun.isInArsenal) ? (1) : (0);
        }
        return size;
    }
    public void AddGunToArsenalAndEquip(GunType gun)
    {
        AddGunToArsenal(gun);
        EquipGun(gun);
    }
    public int AddAmmoToGun(GunType gun, int amount)
    {
        return arsenal[(int)gun]._gun.addAmmo(amount);
    }
    private void HideAmmoInfo()
    {
        ammo.canvasRenderer.SetAlpha(0);
        ammoBar.canvasRenderer.SetAlpha(0);
        uiGunIcon.canvasRenderer.SetAlpha(0);
    }
    private void ShowAmmoInfo()
    {
        ammo.canvasRenderer.SetAlpha(1);
        ammoBar.canvasRenderer.SetAlpha(1);
        uiGunIcon.canvasRenderer.SetAlpha(1);
    }
}
