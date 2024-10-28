using System.Collections;
using UnityEngine;

public class SaturationControl : MonoBehaviour
{
    public Material material;
    public Transform character;
    private SpriteRenderer spriteRenderer;

    public float radius = 2.0f;
    public float lowSaturation = 0.3f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
        character = SaturationSender.Instance.playerTransform;
    }

    void Update()
    {
        if (material != null && character != null)
        {
            // ����ɫ���������괫�ݸ� Shader
            material.SetVector("_SaturationCenter", character.position);

            // ���ݰ뾶
            material.SetFloat("_Radius", SaturationSender.Instance.radius);

            // ���ݵͱ��Ͷ�ֵ
            material.SetFloat("_LowSaturation", lowSaturation);
        }
    }
}
