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

        // 수류탄 투척 방법 1
        //Vector3 up = cam.transform.up;    //카메라의 y축 벡터
        //주시 방향 + (라디안으로 치환한 발사각의 탄젠트값 * y축 벡터)
        //Vector3 direction = forward + up * Mathf.Tan(projectileAngle * Mathf.Deg2Rad);

        //수류탄 투척 방법 2
        Vector3 direction = Quaternion.AngleAxis(-projectileAngle, cam.transform.right) * forward;

        direction.Normalize();  // 길이를 1로 맞춰주기 위해 정규화
        direction *= projectileForce;

        GameObject go = Instantiate(projectilePrefab);
        go.transform.position = firingPosition.position;

        go.GetComponent<Rigidbody>().AddForce(direction,ForceMode.Impulse);
        go.GetComponent<Bomb>().time = projectileTime;
       
    }
}
