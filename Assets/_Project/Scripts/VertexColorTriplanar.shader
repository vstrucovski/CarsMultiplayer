Shader "Custom/VertexColorTriplanarGradientVertical"
{
    Properties
    {
        _ColorXYStart ("Color XY Start", Color) = (1,0,0,1)
        _ColorXYEnd ("Color XY End", Color) = (0,1,0,1)
        _ColorYZStart ("Color YZ Start", Color) = (0,0,1,1)
        _ColorYZEnd ("Color YZ End", Color) = (1,1,0,1)
        _ColorXZStart ("Color XZ Start", Color) = (1,0,1,1)
        _ColorXZEnd ("Color XZ End", Color) = (0,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 color : COLOR;
            };

            struct v2f
            {
                float3 worldPos : TEXCOORD0;
                float3 normal : NORMAL;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            fixed4 _ColorXYStart;
            fixed4 _ColorXYEnd;
            fixed4 _ColorYZStart;
            fixed4 _ColorYZEnd;
            fixed4 _ColorXZStart;
            fixed4 _ColorXZEnd;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.color = v.color;
                return o;
            }

            fixed4 CalculateGradient(fixed4 startColor, fixed4 endColor, float gradientPosition)
            {
                return lerp(startColor, endColor, gradientPosition);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Adjust the gradient position calculation for vertical gradients
                float gradientPosition = i.worldPos.y;

                // Calculate gradient color for each plane
                fixed4 colorXY = CalculateGradient(_ColorXYStart, _ColorXYEnd, gradientPosition);
                fixed4 colorYZ = CalculateGradient(_ColorYZStart, _ColorYZEnd, gradientPosition);
                fixed4 colorXZ = CalculateGradient(_ColorXZStart, _ColorXZEnd, gradientPosition);

                // Determine blend factor based on normal direction
                float blendX = abs(i.normal.x);
                float blendY = abs(i.normal.y);
                float blendZ = abs(i.normal.z);

                // Blend colors based on the normal and vertex color
                fixed4 col = blendX * colorXZ + blendY * colorXY + blendZ * colorYZ;
                col *= i.color; // Apply vertex color

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
