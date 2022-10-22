sampler2D sampleTexture;

struct VertexShaderOutput
{
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

float time;

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR
{
    float4 output = tex2D(sampleTexture, input.TextureCoordinates);

    output.gb += 2;
    
    return output * output.a;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}