using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float time;
    public float damage;
    public AudioClip explodeSound;

    private void Update()
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            GetComponent<Animator>().SetTrigger("Explode");
            // 애니메이션 길이가 2초면 2초뒤 폭발, 1초면 1초뒤 폭발
            Destroy(gameObject, 2);

        }
    }
    public void PlaySound()
    {
        GetComponent<AudioSource>().PlayOneShot(explodeSound);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<Health>().Damage(damage);
        }
    }
}
