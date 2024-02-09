// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader"Custom/TransparentPlatforms"
{
Properties
    {
        _Color        ("Direct Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _MainTex    ("Main Diffuse (RGBA)", 2D) = "" {}
        _AlphaTex    ("AlphaPad (RGBA)", 2D)    = "" {}
    }
    
    SubShader
    {
        
        
        
            Tags
        {
            "Queue" = "Geometry+1"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
        }
        Pass
        {
        
Colormask A

AlphaTest Less 0.5
            Offset -1000000, 1
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
#include "UnityCG.cginc"
            
            
uniform sampler2D _AlphaTex;
            
            
            
struct v2f
{
    float4 pos : SV_POSITION;
    float2 uv : TEXCOORD0;
};
            
            
            
v2f vert(appdata_base v)
{
    v2f o;
    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv = v.texcoord.xy;
    return o;
}
            
            
            
half4 frag(v2f i) : COLOR
{
    half4 sample = tex2D(_AlphaTex, i.uv);
                
    return sample;
}
            
            
            
            ENDCG
            
        }
        
            Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
        }
Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {        
        
Colormask RGB

Zwrite Off

AlphaTest Greater 0.5
            //Offset -0.01, 1
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
#include "UnityCG.cginc"
            
            
            
uniform sampler2D _MainTex;
half4 _Color;
            
            
            
struct v2f
{
    float4 pos : SV_POSITION;
    float2 uv : TEXCOORD0;
};
            
            
            
v2f vert(appdata_base v)
{
    v2f o;
    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv = v.texcoord.xy;
    return o;
}
            
            
            
half4 frag(v2f i) : COLOR
{
    half4 sample = tex2D(_MainTex, i.uv) * _Color;
                
    return sample;
}
            
            
            
            ENDCG
            
        }
        
        
    }
    
Fallback"VertexLit"
}