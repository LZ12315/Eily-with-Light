Shader "Custom/SaturationControlShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}        // 主要的纹理
        _SaturationCenter ("Saturation Center", Vector) = (0, 0, 0, 0) // 角色在世界空间的位置
        _Radius ("Radius", Float) = 1.0               // 正常饱和度的区域半径
        _LowSaturation ("Low Saturation", Range(0, 1)) = 0.3 // 在低饱和度区域的调整程度
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

            // 定义属性
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float3 _SaturationCenter;
            float _Radius;
            float _LowSaturation;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 worldPos : TEXCOORD1; // 用于储存世界坐标
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            // 顶点着色器
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex); // 转换为屏幕坐标
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);   // 纹理坐标
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz; // 将顶点转换到世界坐标
                return o;
            }

            // Fragment 着色器
            fixed4 frag(v2f i) : SV_Target
            {
                // 采样纹理颜色
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // 计算该片段与角色（中心点）的距离
                float distToCenter = distance(i.worldPos, _SaturationCenter);

                // 定义饱和度调整参数
                float saturationAmount = 1.0;

                // 如果像素与中心点的距离大于半径，则降低饱和度
                if (distToCenter > _Radius)
                {
                    saturationAmount = _LowSaturation;
                }

                // 将颜色转为灰度
                float gray = dot(col.rgb, float3(0.299, 0.587, 0.114)); // 灰度计算公式
                
                // 通过饱和度调节颜色
                col.rgb = lerp(float3(gray, gray, gray), col.rgb, saturationAmount);

                return col;
            }
            ENDCG
        }
    }
}
