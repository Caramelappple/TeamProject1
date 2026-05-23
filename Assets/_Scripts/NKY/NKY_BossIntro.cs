using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class NKY_BossIntro : MonoBehaviour
{
    [Header("seting")]
    [SerializeField] private RectTransform top;
    [SerializeField] private RectTransform bottom;
    [SerializeField] private TextMeshProUGUI bossNameText;
    public string bossName;

    [Header("target")]
    [SerializeField] private Transform boss;

    [Header("move set")]
    public float textMoveDistance;
    public float textMoveTime;

    private Camera _camera;
    Vector2 startSize;
    Vector2 introSize;

    private void Start()
    {
        bossNameText.text = bossName;
        bossNameText.color = new Color(bossNameText.color.r, bossNameText.color.g, bossNameText.color.b, 0);
        startSize = new Vector2(top.sizeDelta.x, 10);
        introSize = new Vector2(top.sizeDelta.x, 270);
        top.sizeDelta = startSize;
        bottom.sizeDelta = startSize;
        _camera = Camera.main;
        gameObject.SetActive(false);
    }

    public IEnumerator PlayIntro()
    {
        _camera.transform.DOMove(boss.position, 2.5f).SetEase(Ease.OutQuint);
        top.DOSizeDelta(introSize, 2f).SetEase(Ease.OutQuint);
        bottom.DOSizeDelta(introSize, 2f).SetEase(Ease.OutQuint);
        yield return new WaitForSeconds(1f);
        bossNameText.DOFade(1f, 0.8f);
        yield return new WaitForSeconds(0.6f);
        bossNameText.transform.DOMove(bossNameText.transform.position + (Vector3.left * textMoveDistance), textMoveTime).SetEase(Ease.InQuint);
        yield return new WaitForSeconds(textMoveTime);
        yield return EndIntro();
    }

    public IEnumerator EndIntro()
    {
        bossNameText.gameObject.SetActive(false);
        top.DOSizeDelta(startSize, 2f).SetEase(Ease.OutQuint);
        bottom.DOSizeDelta(startSize, 2f).SetEase(Ease.OutQuint);
        yield return new WaitForSeconds(1.8f);
        gameObject.SetActive(false);
    }


}
