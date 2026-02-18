using UnityEngine;

public class TriggerScript : MonoBehaviour
{

    public bool Enter;
    public GameObject BuildingSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Enter)
        {
            BuildingSprite.SetActive(false);
        }
        else
        {
            BuildingSprite.SetActive(true);
        }
    }
}
