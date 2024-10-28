using UnityEngine;

public class SaturationControl : MonoBehaviour
{
    public Material material;
    public Transform character;
    private SpriteRenderer spriteRenderer;

    public float radius = 2.0f;
    public float radiusBreatheCoef = 0.65f;
    public float radiusBreatheSpeed = 0.5f;
    public float lowSaturation = 0.3f;

    private float radiusStartValue;
    private float radiusEndValue;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
    }

    private void Start()
    {
        radiusStartValue = radius;
        radiusEndValue = radius * radiusBreatheCoef;
    }

    void Update()
    {
        RadiusBreath();

        if (material != null && character != null)
        {
            // 将角色的世界坐标传递给 Shader
            material.SetVector("_SaturationCenter", character.position);

            // 传递半径
            material.SetFloat("_Radius", radius);

            // 传递低饱和度值
            material.SetFloat("_LowSaturation", lowSaturation);
        }
    }

    private void RadiusBreath()
    {
        float t = Mathf.PingPong(Time.time * radiusBreatheSpeed, 1f);
        radius = Mathf.Lerp(radiusStartValue, radiusEndValue, t);
    }
}
