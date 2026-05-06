using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class KHG_poisonField : MonoBehaviour
{
    [SerializeField] private GameObject poison;
    private void Update()
    {

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {

            GameObject spawnedPoison = Instantiate(poison, transform.position, Quaternion.identity);

            StartCoroutine(DestroyAfterTime(spawnedPoison, 5f));
        }

    }

    private IEnumerator DestroyAfterTime(GameObject target, float delay)

    {
        yield return new WaitForSeconds(delay);
        if (target != null)

        {
            Destroy(target);
        }

    }

}


