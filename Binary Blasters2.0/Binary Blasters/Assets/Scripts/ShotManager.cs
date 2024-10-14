using UnityEngine;

public class ShotManager : MonoBehaviour
{
    [SerializeField] public float shootInterval = 1.0f;
    [SerializeField] public float speed = 200f;
    [SerializeField] public float damage = 10f;
    [SerializeField] public float size = 1f;
    [SerializeField] public int shootType = 1; // Variavel que define o estilo de tiro

    private float shootTimer;
    public float ShootInterval
    {
        get { return shootInterval; }
        set { shootInterval = Mathf.Max(0.0f, value); }
    }

    public Bullet bulletPrefab;
    public ResourceMagnet resourceMagnet;

    private void Update()
    {
        HandleShooting();
    }

    private void HandleShooting()
    {
        shootTimer += Time.deltaTime;

        //if (shootTimer >= shootInterval)
        if (shootTimer >= shootInterval && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
        {
            Shoot();
            FindObjectOfType<AudioManager>().Play("PlayerShoot"); //SFX do Tiro
            shootTimer = 0;
        }
    }

    private void Shoot()
    {
        switch (shootType)
        {
            case 1:
                ShootType1();
                break;
            case 2:
                ShootType2();
                break;
            case 3:
                ShootType3();
                break;
            case 4:
                ShootType4();
                break;
            default:
                Debug.LogError("Invalid shoot type!");
                break;
        }
    }

    private void ShootType1() // 1 Line
    {
        Bullet bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.Project(transform.up, speed, damage, size);
    }

    private void ShootType2() // 2 Lines
    {
        float bulletSpacing = 0.2f; // Espaçamento entre os tiros em unidades de escala

        Vector3 bulletRotationEulerAngles = transform.rotation.eulerAngles; // Rotação atual do objeto pai
        Quaternion bulletRotation = Quaternion.Euler(bulletRotationEulerAngles); // Converte a rotação em Quaternion

        Bullet bullet1 = Instantiate(bulletPrefab, transform.position + transform.right * bulletSpacing, bulletRotation);
        bullet1.Project(transform.up, speed, damage, size);

        Bullet bullet2 = Instantiate(bulletPrefab, transform.position - transform.right * bulletSpacing, bulletRotation);
        bullet2.Project(transform.up, speed, damage, size);
    }

    private void ShootType3() // 3 Lines
    {
        float bulletSpacing = 0f; // Espaçamento entre os tiros em unidades de escala
        float bulletAngleOffset = 6f; // Ângulo de ajuste para os tiros laterais

        Vector3 bulletRotationEulerAngles = transform.rotation.eulerAngles; // Rotação atual do objeto pai
        Quaternion bulletRotation = Quaternion.Euler(bulletRotationEulerAngles); // Converte a rotação em Quaternion

        Bullet bullet1 = Instantiate(bulletPrefab, transform.position + transform.right * bulletSpacing, bulletRotation);
        bullet1.Project(transform.up, speed, damage, size);

        Vector3 leftDirection = Quaternion.AngleAxis(-bulletAngleOffset, transform.forward) * transform.up;
        Bullet bullet2 = Instantiate(bulletPrefab, transform.position, bulletRotation);
        bullet2.Project(leftDirection, speed, damage, size);

        Vector3 rightDirection = Quaternion.AngleAxis(bulletAngleOffset, transform.forward) * transform.up;
        Bullet bullet3 = Instantiate(bulletPrefab, transform.position, bulletRotation);
        bullet3.Project(rightDirection, speed, damage, size);
    }


    private void ShootType4() // 5 Lines
    {
        float bulletSpacing = 0f; // Espaçamento entre os tiros em unidades de escala
        float bulletAngleOffset = 5f; // Ângulo de ajuste para os tiros laterais
        float bulletAngleOffsetOuter = 9f; // Ângulo de ajuste para os tiros laterais

        Vector3 bulletRotationEulerAngles = transform.rotation.eulerAngles; // Rotação atual do objeto pai
        Quaternion bulletRotation = Quaternion.Euler(bulletRotationEulerAngles); // Converte a rotação em Quaternion

        Bullet bullet1 = Instantiate(bulletPrefab, transform.position + transform.right * bulletSpacing, bulletRotation);
        bullet1.Project(transform.up, speed, damage, size);

        Vector3 leftDirection = Quaternion.AngleAxis(-bulletAngleOffset, transform.forward) * transform.up;
        Bullet bullet2 = Instantiate(bulletPrefab, transform.position, bulletRotation);
        bullet2.Project(leftDirection, speed, damage, size);

        Vector3 rightDirection = Quaternion.AngleAxis(bulletAngleOffset, transform.forward) * transform.up;
        Bullet bullet3 = Instantiate(bulletPrefab, transform.position, bulletRotation);
        bullet3.Project(rightDirection, speed, damage, size);

        Vector3 lefterDirection = Quaternion.AngleAxis(-bulletAngleOffsetOuter, transform.forward) * transform.up;
        Bullet bullet4 = Instantiate(bulletPrefab, transform.position, bulletRotation);
        bullet4.Project(lefterDirection, speed, damage, size);

        Vector3 righterDirection = Quaternion.AngleAxis(bulletAngleOffsetOuter, transform.forward) * transform.up;
        Bullet bullet5 = Instantiate(bulletPrefab, transform.position, bulletRotation);
        bullet5.Project(righterDirection, speed, damage, size);
    }
}
