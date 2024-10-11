using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float time;

    private void Update()
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            GetComponent<Animator>().SetTrigger("Explode");
            Destroy(gameObject, 2);

        }
    }

}
