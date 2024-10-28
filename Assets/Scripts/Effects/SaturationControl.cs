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
            // 将角色的世界坐标传递给 Shader
            material.SetVector("_SaturationCenter", character.position);

            // 传递半径
            material.SetFloat("_Radius", SaturationSender.Instance.radius);

            // 传递低饱和度值
            material.SetFloat("_LowSaturation", lowSaturation);
        }
    }
}
