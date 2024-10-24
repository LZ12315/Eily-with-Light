Shader "Custom/SaturationControlShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}        // ��Ҫ������
        _SaturationCenter ("Saturation Center", Vector) = (0, 0, 0, 0) // ��ɫ������ռ��λ��
        _Radius ("Radius", Float) = 1.0               // �������Ͷȵ�����뾶
        _LowSaturation ("Low Saturation", Range(0, 1)) = 0.3 // �ڵͱ��Ͷ�����ĵ����̶�
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // ��������
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float3 _SaturationCenter;
            float _Radius;
            float _LowSaturation;

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
                o.pos = UnityObjectToClipPos(v.vertex); // ת��Ϊ��Ļ����
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);   // ��������
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz; // ������ת������������
                return o;
            }

            // Fragment ��ɫ��
            fixed4 frag(v2f i) : SV_Target
            {
                // ����������ɫ
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // �����Ƭ�����ɫ�����ĵ㣩�ľ���
                float distToCenter = distance(i.worldPos, _SaturationCenter);

                // ���履�Ͷȵ�������
                float saturationAmount = 1.0;

                // ������������ĵ�ľ�����ڰ뾶���򽵵ͱ��Ͷ�
                if (distToCenter > _Radius)
                {
                    saturationAmount = _LowSaturation;
                }

                // ����ɫתΪ�Ҷ�
                float gray = dot(col.rgb, float3(0.299, 0.587, 0.114)); // �Ҷȼ��㹫ʽ
                
                // ͨ�����Ͷȵ�����ɫ
                col.rgb = lerp(float3(gray, gray, gray), col.rgb, saturationAmount);

                return col;
            }
            ENDCG
        }
    }
}
