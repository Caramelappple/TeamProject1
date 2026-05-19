using UnityEngine;

public class NKY_GameManager : MonoBehaviour
{
    public LSO_PlayerMovement player;

    public static NKY_GameManager instance;

    private NKY_GameManager() 
    {

    }

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }
        return;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
}
