Shader "Custom/uv"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}

        //X方向のシフトとスピードに関するパラメータを追加
        _XShift("Xuv Shift", Range(-1.0, 1.0)) = 0.1
        _XSpeed("X Scroll Speed", Range(0.0, 1.0)) = 0.0

        _Color("Color",Color) = (1,1,1,1)
        _Alpha("Alpha",Range(0.0, 1.0)) = 1.0
    }
        SubShader
    {
        Tags
        {
            //レンダリング順に関する指示
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }
        //透明なテクスチャを使用する場合に必要なプロパティ
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            //追加したパラメータを宣言する
            float _XShift;
            float _XSpeed;

            float4 _Color;
            float _Alpha;


            //バーテクスシェーダー（変更なし）
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            //フラグメントシェーダー（変更箇所）
            fixed4 frag(v2f i) : SV_Target
            {
                //Speed
                _XShift = _XShift * _XSpeed;

                //add Shift
                i.uv.x += _XShift / 10000;

                //i.uvの適用
                fixed4 col = tex2D(_MainTex, i.uv);
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }

            ENDCG
        }
    }
}
