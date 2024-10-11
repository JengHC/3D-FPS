using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class ProjectileWeapon :Weapon // Weapon클래스 상속받음
{
    public GameObject projectilePrefab;
    public float projectileAngle = 30;
    public float projectileForce = 10;
    public float projectileTime = 5;

    // Fire 메서드를 override하여 재정의
    protected override void Fire()
    {
        ProjectileFire();
    }

    void ProjectileFire()
    {
        Camera cam = Camera.main;
        Vector3 forward = cam.transform.forward;
        
        Vector3 up = cam.transform.up;
        Vector3 direction = forward + up * Mathf.Tan(projectileAngle * Mathf.Deg2Rad);

        direction.Normalize();  // 길이를 1로 맞춰주기 위해 정규화
        direction *= projectileForce;

        GameObject go = Instantiate(projectilePrefab);
        go.transform.position = firingPosition.position;

        go.GetComponent<Rigidbody>().AddForce(direction,ForceMode.Impulse);
        go.GetComponent<Bomb>().time = projectileTime;
       
    }

}
