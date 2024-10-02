using UnityEngine;

public class Item : MonoBehaviour
{
    public void ItemDestroy()
    {
        Destroy(gameObject, 1.5f);
    }
}
