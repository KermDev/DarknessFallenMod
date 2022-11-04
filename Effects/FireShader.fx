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

float4 PixelPass(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(samplerTexture, coords);
    
    color.rgb *= noise(time % .1);
    
    return color * color.a;
}

technique Technique1
{
    pass PixelPass
    {
        PixelShader = compile ps_2_0 PixelPass();
    }
}