Shader "Unlit/Algas"
{
    Properties
    {
        _Color ("Color", Color) = (0.0, 1.0, 0.0, 1.0)
        _Amplitude ("Amplitude", Float) = 0.1
        _Frequency ("Frequency", Float) = 2.0
        _Speed ("Speed", Float) = 1.0
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float _Amplitude;
            float _Frequency;
            float _Speed;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                
                // Offset vertices based on sine wave to simulate underwater algae movement
                float time = _Time.y * _Speed;
                float wave = sin(v.vertex.x * _Frequency + time) * _Amplitude;

                // Apply the offset to the y-axis of the vertices
                v.vertex.y += wave;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Output color of the algae
                return _Color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}