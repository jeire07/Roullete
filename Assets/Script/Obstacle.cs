using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Animator Anim;

    private void OnCollisionEnter2D()
    {
        Anim.SetBool("Boom", true);
    }
}
