Shader "Custom/InvertNormals"
{
    Properties
    {
        _MainTex ("HDRI Cubemap", Cube) = "" { } // Use Cube for Cubemap texture
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            Cull Front // Render the back faces instead of front faces
            Lighting Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 normal : TEXCOORD0;
            };

            // Declare the Cubemap to sample HDRI from
            samplerCUBE _MainTex;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = -v.normal; // Invert the normal direction
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // Sample the HDRI Cubemap using the inverted normal direction
                half4 sampledColor = texCUBE(_MainTex, i.normal); // Sample the Cubemap based on the normal
                return sampledColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
