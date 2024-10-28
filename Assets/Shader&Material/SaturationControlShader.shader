Shader "Custom/SaturationControlShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}                      // 主要的纹理
        _SaturationCenter ("Saturation Center", Vector) = (0, 0, 0, 0)  // 角色在世界空间的位置
        _Radius ("Radius", Float) = 1.0                            // 正常饱和度的区域半径
        _LowSaturation ("Low Saturation", Range(0, 1)) = 0.3      // 在低饱和度区域的调整程度
    }

    SubShader
    {
        // 如果需要透明度渲染，添加这行来启用混合（Blend Transparent）
        Tags { "RenderType"="Transparent" }
        LOD 200

        Pass
        {
            // 用于透明度处理的混合模式 (正如大多数透明对象使用的)
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // 定义属性
            sampler2D _MainTex;                         // 纹理
            float4 _MainTex_ST;                         // 纹理变换
            float3 _SaturationCenter;                   // 中心点
            float _Radius;                              // 半径
            float _LowSaturation;                       // 低饱和度区域的饱和度

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
                o.pos = UnityObjectToClipPos(v.vertex); // 转换为裁剪空间坐标
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);   // 获取纹理坐标
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz; // 把顶点转到世界坐标中
                return o;
            }

            // 片段着色器
            fixed4 frag(v2f i) : SV_Target
            {
                // 采样纹理颜色
                fixed4 col = tex2D(_MainTex, i.uv);

                // 保留 Alpha 通道
                float alpha = col.a;

                // 计算该像素到中心的距离
                float distToCenter = distance(i.worldPos, _SaturationCenter);

                // 默认饱和度
                float saturationAmount = 1.0;

                // 如果像素与中心点的距离大于半径，则降低饱和度
                if (distToCenter > _Radius)
                {
                    saturationAmount = _LowSaturation;
                }

                // 将颜色转为灰度
                float gray = dot(col.rgb, float3(0.299, 0.587, 0.114)); // NTSC 标准灰度计算方法

                // 计算最终颜色，使用饱和度插值（lerp）
                col.rgb = lerp(float3(gray, gray, gray), col.rgb, saturationAmount);

                // 保留原始的 Alpha 通道
                col.a = alpha;

                return col;
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}
