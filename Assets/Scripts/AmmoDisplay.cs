using UnityEngine;
using TMPro;

public class AmmoDisplay : MonoBehaviour
{
    public silahdegistir silahdegistir; // silahdegistir scriptinin referans�
    public TextMeshProUGUI ammoText; // Mermi say�s�n� g�sterecek Text eleman�

    void Update()
    {
        // Se�ili silah�n indeksini al
        int selectedWeaponIndex = silahdegistir.selectedWeapon;

        // Se�ilen silah�n mermi say�s�n� g�ster
        if (silahdegistir.guns.Length > selectedWeaponIndex)
        {
            gun currentGun = silahdegistir.guns[selectedWeaponIndex];
            ammoText.text = "Ammo: " + currentGun.currentAmmo + " / " + currentGun.maxAmmo;
        }
    }
}
