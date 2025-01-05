using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class silahdegistir : MonoBehaviour
{
    public int selectedWeapon = 0;
    public gun[] guns; // Gun türünde bir dizi eklendi

    void Start()
    {
        SelectWeapon();
    }

    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        // Mouse scroll ile silah geçiþi
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= guns.Length - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = guns.Length - 1;
            else
                selectedWeapon--;
        }

        // Klavyedeki tuþlarla silah geçiþi
        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedWeapon = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2) && guns.Length > 1) selectedWeapon = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3) && guns.Length > 2) selectedWeapon = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4) && guns.Length > 3) selectedWeapon = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5) && guns.Length > 4) selectedWeapon = 4;

        // Silah deðiþikliði kontrolü
        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (gun weapon in guns) // gun dizisi kullanýldý
        {
            weapon.gameObject.SetActive(i == selectedWeapon);
            i++;
        }
    }
}
