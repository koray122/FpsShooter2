using UnityEngine;
using TMPro;

public class AmmoDisplay : MonoBehaviour
{
    public silahdegistir silahdegistir; // silahdegistir scriptinin referansý
    public TextMeshProUGUI ammoText; // Mermi sayýsýný gösterecek Text elemaný

    void Update()
    {
        // Seçili silahýn indeksini al
        int selectedWeaponIndex = silahdegistir.selectedWeapon;

        // Seçilen silahýn mermi sayýsýný göster
        if (silahdegistir.guns.Length > selectedWeaponIndex)
        {
            gun currentGun = silahdegistir.guns[selectedWeaponIndex];
            ammoText.text = "Ammo: " + currentGun.currentAmmo + " / " + currentGun.maxAmmo;
        }
    }
}
