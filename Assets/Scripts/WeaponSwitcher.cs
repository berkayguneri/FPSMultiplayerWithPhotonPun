using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public PhotonView playerSetupView;

    public Animation anim;
    public AnimationClip switchClip;

    private int selectedWeapon = 0;


    private void Start()
    {
        SelectWeapon();
    }


    private void Update()
    {
        int previouseSelecetedWeapon = selectedWeapon;

        SwitchWeaponWithAlpa();

        SwitchWeaponWIthScroll();

        if (previouseSelecetedWeapon != selectedWeapon)
            SelectWeapon();

    }

    private void SwitchWeaponWIthScroll()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (selectedWeapon >= transform.childCount - 1)
            {
                selectedWeapon = 0;
            }
            else
            {
                selectedWeapon = 1;
            }
        }


        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (selectedWeapon <= 0)
            {
                selectedWeapon = transform.childCount - 1;
            }
            else
            {
                selectedWeapon += 1;
            }
        }
    }

    private void SwitchWeaponWithAlpa()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedWeapon = 1;
        }

        
    }

    void SelectWeapon()
    {
        playerSetupView.RPC("SetTPWeapon", RpcTarget.All, selectedWeapon);

        if (selectedWeapon >= transform.childCount)
        {
            selectedWeapon = transform.childCount - 1;
        }

        anim.Stop();
        anim.Play(switchClip.name);

        int i = 0;

        foreach (Transform _weapon in transform)
        {
            if (i == selectedWeapon)
            {
                _weapon.gameObject.SetActive(true);
            }
            else
            {
                _weapon.gameObject.SetActive(false);
            }


            i++;
        }
    }
}
