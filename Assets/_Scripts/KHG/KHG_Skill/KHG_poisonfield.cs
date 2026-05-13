using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class KHG_PoisonField : MonoBehaviour//,LSO_ISkill
{
    [SerializeField] private GameObject poisonPrefab;
    [SerializeField] private float spawnDistance = 1.5f;
    [SerializeField] private float coolTime = 3f;
    private LSO_PlayerMovement playerMovement;
    private bool canUseSkill = true;

    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame && canUseSkill)
        {

            Vector2 lookDirection = playerMovement.lastDir;

            Vector3 spawnPos = transform.position + new Vector3(lookDirection.x, lookDirection.y, 0) * spawnDistance;

            GameObject poison = Instantiate(poisonPrefab, spawnPos, Quaternion.identity);

            // 오브젝트에 방향으로 속도 부여 (Rigidbody2D가 있을 경우)
            Rigidbody2D rb = poison.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = lookDirection.normalized * 5f; // 발사 속도
            }

            Destroy(poison, 5f);

            StartCoroutine(SkillCoolTime());
        }
    }

    private IEnumerator SkillCoolTime()
    {
        canUseSkill = false;
        yield return new WaitForSeconds(coolTime);
        canUseSkill = true;
    }

    public void UseSkill(GameObject player)
    {
        //Vector2 lookDirection = playerMovement.GetLastDir();

        //Vector3 spawnPos = transform.position + new Vector3(lookDirection.x, lookDirection.y, 0) * spawnDistance;

        //GameObject poison = Instantiate(poisonPrefab, spawnPos, Quaternion.identity);

        //// 오브젝트에 방향으로 속도 부여 (Rigidbody2D가 있을 경우)
        //Rigidbody2D rb = poison.GetComponent<Rigidbody2D>();
        //if (rb != null)
        //{
        //    rb.linearVelocity = lookDirection.normalized * 5f; // 발사 속도
        //}

        //Destroy(poison, 5f);

    }

    public IEnumerator CoolTime(float time)
    {
        throw new System.NotImplementedException();
    }
}