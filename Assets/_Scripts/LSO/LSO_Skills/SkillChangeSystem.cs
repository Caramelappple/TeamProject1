using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillChangeSystem : MonoBehaviour
{
    private bool isInRange = false;
    private GameObject[] currentSkills;
    private GameObject targetSkill;
    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame && isInRange)
        {
            Debug.Log("스킬 1로 변경");
            ChangeSkill(0, targetSkill);
        }
        else if (Keyboard.current.rKey.wasPressedThisFrame && isInRange)
        {
            Debug.Log("스킬 2로 변경");
            ChangeSkill(1, targetSkill);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            isInRange = true;
            targetSkill = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
            isInRange = false;
    }

    private void ChangeSkill(int index, GameObject targetSkill)
    {
        GameObject currentSkill = currentSkills[index];
        

    }
}
