
    
float4 ArmorBasic(float4 sampleColor : COLOR0) : COLOR0
{
    return sampleColor;
}
    
technique Technique1
{
    pass ArmorBasic
    {
        PixelShader = compile ps_2_0 ArmorBasic();
    }
}