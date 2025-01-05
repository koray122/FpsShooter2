using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour
{
    public float damage = 10f; // Silahın her atışta vereceği hasar miktarı.
    public float range = 100f; // Silahın menzili.
    public float fireRate = 15f; // Silahın atış hızı.
    public Camera fpsCam; // Silahın bağlı olduğu birinci şahıs kamera referansı.
    public float impactForce = 20000f; // Merminin çarptığı nesneye uygulayacağı kuvvet.
    public ParticleSystem muzzleFlash; // Ateşleme efekti için partikül sistemi.
    public GameObject impactEffect; // Mermi çarptığında oluşacak görsel etki.
    public int maxAmmo = 10; // Silahın alabileceği maksimum mermi sayısı.
    public int currentAmmo; // Silahın şu anki mermi sayısı.
    public float reloadTime = 1f; // Silahın yeniden dolum süresi.
    private bool isReloading = false; // Yeniden doldurma işleminin yapılıp yapılmadığını belirten durum değişkeni.
    public Animator animator; // Silaha bağlı animatör bileşeni.
    private float nextTimeToFire = 0f; // Sonraki ateşleme zamanını belirler.
    public float minX, minY; // Geri tepme için minimum X ve Y açısı.
    public float maxX, maxY; // Geri tepme için maksimum X ve Y açısı.
    Vector3 rot; // Kameranın mevcut dönüş açısını saklar.
    public bool isWalk = false;

    public AudioSource gunAudioSource; // Silah sesi için AudioSource referansı
    public AudioClip shootSound; // Ateş etme sesi için AudioClip referansı

    private bool isSwitching = false; // Silah geçişinin yapılıp yapılmadığını kontrol etmek için

    // playerMovement referansı
    public playerMovement playerMovementScript; // Karakterin hareketini kontrol eden script.

    void Start()
    {
        currentAmmo = maxAmmo; // Oyuna başladığında mermi sayısını maksimum mermiyle başlatır.
    }

    void OnEnable()
    {
        isReloading = false; // Silah etkinleştiğinde yeniden doldurulmadığını belirtir.
        animator.SetBool("Reloading", false); // Yeniden yükleme animasyonunu durdurur.
        isSwitching = false;
    }

    void Update()
    {
        if (isReloading || isSwitching) // Yeniden doldurma veya silah geçişi yapılıyorsa, ateş etme işlemini geç.
        {
            return;
        }

        // Mermi sayısı kontrolü ve ateş etme işlemi
        if (currentAmmo < maxAmmo && Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
        }

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        // W tuşuna basıldığında isWalk'ı true yap ve animasyonu başlat
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (!isWalk) // Eğer isWalk false ise
            {
                StartCoroutine(SetWalkState());
            }
        }
        else
        {
            isWalk = false; // W'ye basılmıyorsa isWalk false
            animator.SetBool("isWalk", false); // Idle durumuna dön
        }

        rot = fpsCam.transform.localRotation.eulerAngles;
        if (rot.x != 0 || rot.y != 0)
        {
            fpsCam.transform.localRotation = Quaternion.Slerp(fpsCam.transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 3);
        }
    }

    IEnumerator SetWalkState()
    {
        isWalk = true; // isWalk'ı true yap
        animator.SetBool("isWalk", true); // Yürüyüş animasyonunu başlat

        // 0.1 saniye bekle
        yield return new WaitForSeconds(0.1f);

        isWalk = false; // 1 saniye sonra isWalk'ı false yap
        animator.SetBool("isWalk", false); // Idle durumuna geç
    }

    IEnumerator Reload()
    {
        isReloading = true; // Yeniden doldurma işleminin başladığını belirtir.
        animator.SetBool("Reloading", true); // Yeniden doldurma animasyonunu başlatır.
        Debug.Log("Reloading.."); // Konsola "Reloading.." mesajı yazar.
        yield return new WaitForSeconds(reloadTime); // Yeniden doldurma süresi kadar bekler.
        currentAmmo = maxAmmo; // Mermi sayısını maksimum değere ayarlar.
        isReloading = false; // Yeniden doldurma işleminin tamamlandığını belirtir.
        animator.SetBool("Reloading", false); // Yeniden doldurma animasyonunu durdurur.
    }

    void Shoot()
    {
        muzzleFlash.Play(); // Ateşleme efektini başlatır.
        currentAmmo--; // Mermi sayısını bir azaltır.
        gunAudioSource.PlayOneShot(shootSound); // Ateş etme sesini oynatır.

        // Hareket kontrolü: Sadece hareket varsa recoil uygula
        if (playerMovementScript != null) // Eğer playerMovement scriptine referans varsa:
        {
            float horizontal = Input.GetAxis("Horizontal"); // Yatay hareket kontrolü.
            float vertical = Input.GetAxis("Vertical"); // Dikey hareket kontrolü.

            if (horizontal != 0 || vertical != 0) // Eğer karakter hareket ediyorsa:
            {
                Recoil(); // Geri tepme işlemini uygular.
            }
        }

        // Tepme animasyonunu başlat
        animator.SetBool("tepme", true); // Tepme animasyonunu tetikler.

        RaycastHit hit; // Raycast ile merminin çarptığı nesneyi tespit eder.
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)) // Kamera önüne doğru bir ray gönderir.
        {
            Debug.Log(hit.transform.name); // Çarpılan nesnenin ismini konsola yazar.
            Target target = hit.transform.GetComponent<Target>(); // Çarpılan nesnenin "Target" bileşenini alır.
            if (target != null) // Eğer çarpılan nesne bir "Target" bileşenine sahipse:
            {
                target.TakeDamage(damage); // Hedefe hasar uygular.
            }
            if (hit.rigidbody != null) // Eğer çarpılan nesnenin bir Rigidbody'si varsa:
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce); // Çarpışma noktasına darbe kuvveti uygular.
            }
            GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal)); // Çarpma efektini yaratır.
            Destroy(impact, 2f); // Çarpma efektini 2 saniye sonra yok eder.
        }

        // Tepme animasyonunu durdur
        StartCoroutine(StopRecoilAnimation()); // Tepme animasyonunu durdurmak için bir coroutine kullanır.
    }

    private void Recoil()
    {
        float recX = Random.Range(minX, maxX); // Rastgele X geri tepme açısını belirler.
        float recY = Random.Range(minY, maxY); // Rastgele Y geri tepme açısını belirler.
        fpsCam.transform.localRotation = Quaternion.Euler(rot.x - recY, rot.y + recX, rot.z); // Kameranın dönüş açısını geri tepme açısıyla değiştirir.
    }

    private IEnumerator StopRecoilAnimation()
    {
        yield return new WaitForSeconds(0.1f); // Tepme animasyonunun ne kadar süre oynayacağını belirler.
        animator.SetBool("tepme", false); // Tepme animasyonunu durdurur.
    }

    // Silah değiştirme işlemi başlatıldığında bu metodu çağırın
    public void SwitchWeapon()
    {
        StartCoroutine(WeaponSwitching());
    }

    private IEnumerator WeaponSwitching()
    {
        isSwitching = true; // Geçiş durumunu aktif hale getir

        yield return new WaitForSeconds(0.5f); // 0.5 saniye bekle (ses kısmı kaldırıldı)
        isSwitching = false; // Geçiş durumunu devre dışı bırak
    }
}
