using UnityEngine;

public class AbilitySystem : MonoBehaviour
{
    Ability ability;

    void Awake()
    {
        ability = GetComponent<Ability>();
    }

    public void Use(PlayerController player)
    {
        ability?.Activate(player);
    }
}