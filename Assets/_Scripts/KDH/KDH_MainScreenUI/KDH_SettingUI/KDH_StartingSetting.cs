using System;
using UnityEngine;

public class KDH_StartingSetting : MonoBehaviour
{
    [SerializeField] private GameObject settingUI;
    private void Start()
    {
        settingUI.SetActive(false);
    }
}
