using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class KHG_poisonField : MonoBehaviour
{
    [SerializeField] private GameObject poisonPrefab;
    [SerializeField] private float spawnDistance = 1.5f;

    [SerializeField] private float coolTime = 3f;

    private Vector2 lookDirection = Vector2.down;

    private bool canUseSkill = true;

    private void Update()
    {
        Vector2 inputDir = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)
            inputDir.y += 1;

        if (Keyboard.current.sKey.isPressed)
            inputDir.y -= 1;

        if (Keyboard.current.aKey.isPressed)
            inputDir.x -= 1;

        if (Keyboard.current.dKey.isPressed)
            inputDir.x += 1;

        if (inputDir != Vector2.zero)
        {
            lookDirection = inputDir.normalized;
        }

        // 스킬 사용
        if (Keyboard.current.rKey.wasPressedThisFrame && canUseSkill)
        {
            Vector3 spawnPos = transform.position + new Vector3(lookDirection.x, lookDirection.y, 0) * spawnDistance;

            GameObject poison = Instantiate(poisonPrefab, spawnPos, Quaternion.identity);

            Destroy(poison, 5f);

        }
    }

}


