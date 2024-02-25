using UnityEngine;

public class Ball : MonoBehaviour
{
    public string GetParentName()
    {
        return transform.parent.name;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Goal")
        {
            GameManager.Instance.DisableClone(GetParentName());
        }
    }
}
