using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps; // �Ω�B�z Tilemaps

public class BlinkingBlock : MonoBehaviour
{
    [Header("���q����ɶ��]�w�]��^")]
    public float existDuration = 5f;        // �s�b���q������ɶ�
    public float blinkDuration = 2f;        // �{�{���q������ɶ�
    public float disappearDuration = 3f;    // �������q������ɶ�

    [Header("�{�{�]�w")]
    public float blinkInterval = 0.2f;      // �{�{�����j�ɶ��]��^

    private bool isBlinking = false;         // �O�_���b�{�{

    private Tilemap tilemap;                 // Tilemap �ե󪺤ޥ�
    private TilemapCollider2D tilemapCollider; // TilemapCollider2D �ե󪺤ޥ�

    void Start()
    {
        // ��� Tilemap �M TilemapCollider2D �ե�
        tilemap = GetComponent<Tilemap>();
        tilemapCollider = GetComponent<TilemapCollider2D>();

        // �ˬd�ե�O�_�s�b
        if (tilemap == null)
        {
            Debug.LogError("����� Tilemap �ե�I�бN Tilemap �ե���[��� GameObject�C");
        }

        if (tilemapCollider == null)
        {
            Debug.LogError("����� TilemapCollider2D �ե�I�бN TilemapCollider2D �ե���[��� GameObject�C");
        }

        // �Ұʴ`����{
        StartCoroutine(CycleBlinkDisappear());
    }

    /// <summary>
    /// �`������ �s�b �� �{�{ �� ���� �����q
    /// </summary>
    /// <returns></returns>
    IEnumerator CycleBlinkDisappear()
    {
        while (true)
        {
            // �s�b���q
            SetVisible(true);
            tilemapCollider.enabled = true;
            yield return new WaitForSeconds(existDuration);

            // �{�{���q
            float blinkElapsed = 0f;
            isBlinking = true;
            while (blinkElapsed < blinkDuration)
            {
                ToggleRendererVisibility(); // �Ȥ�����V�����i����
                yield return new WaitForSeconds(blinkInterval);
                blinkElapsed += blinkInterval;
            }
            isBlinking = false;

            // �T�O����b�{�{��O���i����
            SetVisible(false);
            tilemapCollider.enabled = false;

            // �������q
            yield return new WaitForSeconds(disappearDuration);
        }
    }

    /// <summary>
    /// �Ȥ�����V�����i���ʡA�O���I�����ҥ�
    /// </summary>
    void ToggleRendererVisibility()
    {
        if (tilemap != null)
        {
            // ���� Tilemap ���C��z����
            Color currentColor = tilemap.color;
            bool newVisibility = currentColor.a > 0f;
            tilemap.color = newVisibility ? new Color(1f, 1f, 1f, 0f) : Color.white;
        }
    }

    /// <summary>
    /// �]�w������i���ʩM�I�������A
    /// </summary>
    /// <param name="visible">�O�_�i��</param>
    void SetVisible(bool visible)
    {
        if (tilemap != null)
        {
            tilemap.color = visible ? Color.white : new Color(1f, 1f, 1f, 0f); // �����z����ܤ��i��
        }

        if (tilemapCollider != null)
        {
            tilemapCollider.enabled = visible; // �ҥΩθT�θI����
        }
    }

    /// <summary>
    /// �i��G�ߧY����`���èϤ���O���i��
    /// </summary>
    public void StopCycle()
    {
        StopAllCoroutines();
        SetVisible(true);
        if (tilemapCollider != null)
        {
            tilemapCollider.enabled = true;
        }
        isBlinking = false;
    }

    /// <summary>
    /// ���m����A�Ϩ䭫�s�i���åi�A��Ĳ�o�{�{
    /// </summary>
    public void ResetBlock()
    {
        SetVisible(true);
        if (tilemapCollider != null)
        {
            tilemapCollider.enabled = true; // �T�O�I�����ҥ�
        }
        isBlinking = false;
    }
}
