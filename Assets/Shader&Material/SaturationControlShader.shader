Shader "Custom/SaturationControlShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}                      // ��Ҫ������
        _SaturationCenter ("Saturation Center", Vector) = (0, 0, 0, 0)  // ��ɫ������ռ��λ��
        _Radius ("Radius", Float) = 1.0                            // �������Ͷȵ�����뾶
        _LowSaturation ("Low Saturation", Range(0, 1)) = 0.3      // �ڵͱ��Ͷ�����ĵ����̶�
    }

    SubShader
    {
        // �����Ҫ͸������Ⱦ��������������û�ϣ�Blend Transparent��
        Tags { "RenderType"="Transparent" }
        LOD 200

        Pass
        {
            // ����͸���ȴ���Ļ��ģʽ (��������͸������ʹ�õ�)
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // ��������
            sampler2D _MainTex;                         // ����
            float4 _MainTex_ST;                         // ����任
            float3 _SaturationCenter;                   // ���ĵ�
            float _Radius;                              // �뾶
            float _LowSaturation;                       // �ͱ��Ͷ�����ı��Ͷ�

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 worldPos : TEXCOORD1; // ���ڴ�����������
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            // ������ɫ��
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex); // ת��Ϊ�ü��ռ�����
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);   // ��ȡ��������
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz; // �Ѷ���ת������������
                return o;
            }

            // Ƭ����ɫ��
            fixed4 frag(v2f i) : SV_Target
            {
                // ����������ɫ
                fixed4 col = tex2D(_MainTex, i.uv);

                // ���� Alpha ͨ��
                float alpha = col.a;

                // ��������ص����ĵľ���
                float distToCenter = distance(i.worldPos, _SaturationCenter);

                // Ĭ�ϱ��Ͷ�
                float saturationAmount = 1.0;

                // ������������ĵ�ľ�����ڰ뾶���򽵵ͱ��Ͷ�
                if (distToCenter > _Radius)
                {
                    saturationAmount = _LowSaturation;
                }

                // ����ɫתΪ�Ҷ�
                float gray = dot(col.rgb, float3(0.299, 0.587, 0.114)); // NTSC ��׼�Ҷȼ��㷽��

                // ����������ɫ��ʹ�ñ��ͶȲ�ֵ��lerp��
                col.rgb = lerp(float3(gray, gray, gray), col.rgb, saturationAmount);

                // ����ԭʼ�� Alpha ͨ��
                col.a = alpha;

                return col;
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}
