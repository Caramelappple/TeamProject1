using UnityEngine;

public class KHG_BombBullet : MonoBehaviour
{
    public float speed;
    private LSO_PlayerMovement pm;
    private void Start()
    {
     
        pm = FindAnyObjectByType<LSO_PlayerMovement>();
    }

    private void FixedUpdate()
    {
        
    }

}
