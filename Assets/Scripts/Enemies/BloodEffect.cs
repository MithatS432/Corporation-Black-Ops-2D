using UnityEngine;

public class BloodEffect : MonoBehaviour
{
    public float lifeTime = 2f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}