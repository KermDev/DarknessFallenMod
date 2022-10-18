texture sampleTexture;
sampler2D samplerTexture = sampler_state
{
    texture = <sampleTexture>;
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = wrap;
    AddressV = wrap;
};

float time;

float2 imageSize;
float4 source;

float4 ShinePass(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(samplerTexture, coords);
    
    float localCoordsY = (coords.y * imageSize.y - source.y) / source.w;
    
    float val = 0.3 * (sin((coords.x * 4 + time)) * 5 + 1);
    
    float3 cache_color = color.brb;
    color.rgb -= color.rgb * val;
    color.rgb += cache_color * val;
    
    return color * color.a;
}

technique Technique1
{
    pass ShinePass
    {
        PixelShader = compile ps_2_0 ShinePass();
    }
}