Shader "Custom/Triplanar"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _SecondaryTex ("SecondaryTex", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _SecondaryTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldNormal;
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void Unity_Rotate_Degrees_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            Rotation = Rotation * (3.1415926f/180.0f);
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix * 2 - 1;
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
            Out = UV;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 yz_proj = 0;
            Unity_Rotate_Degrees_float(IN.worldPos.yz, .5, 270, yz_proj);

            fixed4 top = tex2D(_MainTex, IN.worldPos.xz * .1);
            fixed4 bot = tex2D(_SecondaryTex, IN.worldPos.xz *.1);
            fixed4 lr = tex2D(_SecondaryTex, yz_proj * .1); //left/right
            fixed4 fb = tex2D(_SecondaryTex, IN.worldPos.xy * .1); //front/back

            fixed upper_col = max(0, smoothstep(IN.worldNormal.y, IN.worldNormal.y -0.1, 0.5) * top);
            fixed lower_col = max(0, smoothstep(-IN.worldNormal.y, -IN.worldNormal.y -0.8, 0.5) * bot);
            fixed side_col = IN.worldNormal.x * lr;
            fixed front_col = IN.worldNormal * fb;
            // Albedo comes from a texture tinted by color
            fixed4 c = (upper_col + lower_col + abs(side_col) + abs(front_col)) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
