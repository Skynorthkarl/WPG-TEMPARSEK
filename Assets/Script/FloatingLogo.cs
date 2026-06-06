using UnityEngine;

public class FloatingLogo : MonoBehaviour
{
    [Header("Pengaturan Gerakan")]
    [SerializeField] private float kecepatan = 2.0f; // Seberapa cepat logo naik turun
    [SerializeField] private float jarakPosisi = 15.0f; // Seberapa jauh jarak naik turunnya (dalam pixel)

    private Vector3 posisiAwal;

    void Start()
    {
        // Menyimpan posisi awal logo saat game pertama kali dimulai
        posisiAwal = transform.localPosition;
    }

    void Update()
    {
        // Rumus Sinus untuk membuat gerakan naik turun yang halus seperti gelombang
        float posisiBaruY = Mathf.Sin(Time.time * kecepatan) * jarakPosisi;
        
        // Terapkan posisi baru ke koordinat Y logo
        transform.localPosition = new Vector3(posisiAwal.x, posisiAwal.y + posisiBaruY, posisiAwal.z);
    }
}