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

    private bool isVisible = true;           // ��e������i�����A

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
            while (blinkElapsed < blinkDuration)
            {
                ToggleVisibility();
                yield return new WaitForSeconds(blinkInterval);
                blinkElapsed += blinkInterval;
            }

            // �T�O����b�{�{��O���i����
            SetVisible(false);
            tilemapCollider.enabled = false;

            // �������q
            yield return new WaitForSeconds(disappearDuration);
        }
    }

    /// <summary>
    /// ����������i����
    /// </summary>
    void ToggleVisibility()
    {
        isVisible = !isVisible;
        UpdateVisibility();
    }

    /// <summary>
    /// �]�w������i����
    /// </summary>
    /// <param name="visible">�O�_�i��</param>
    void SetVisible(bool visible)
    {
        isVisible = visible;
        UpdateVisibility();
    }

    /// <summary>
    /// �ھ� isVisible �ܼƧ�s������i���ʩM�I��
    /// </summary>
    void UpdateVisibility()
    {
        if (tilemap != null)
        {
            // ��ܩ����� Tilemap
            tilemap.color = isVisible ? Color.white : new Color(1f, 1f, 1f, 0f); // �����z����ܤ��i��
        }

        if (tilemapCollider != null)
        {
            tilemapCollider.enabled = isVisible; // �ҥΩθT�θI����
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
    }
}
