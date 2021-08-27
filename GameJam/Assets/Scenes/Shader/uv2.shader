Shader "Custom/uv2"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}

    //X�����̃V�t�g�ƃX�s�[�h�Ɋւ���p�����[�^��ǉ�
    _XShift("Xuv Shift", Range(-1.0, 1.0)) = 0.1
    _XSpeed("X Scroll Speed", Range(0.0, 1.0)) = 0.0

    _Color("Color",Color) = (1,1,1,1)
    _Alpha("Alpha",Range(0.0, 1.0)) = 1.0
    }
        SubShader
    {
        Tags
        {
            //�����_�����O���Ɋւ���w��
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }
        //�����ȃe�N�X�`�����g�p����ꍇ�ɕK�v�ȃv���p�e�B
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
            //�ǉ������p�����[�^��錾����
            float _XShift;
            float _XSpeed;

            float4 _Color;
            float _Alpha;


            //�o�[�e�N�X�V�F�[�_�[�i�ύX�Ȃ��j
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            //�t���O�����g�V�F�[�_�[�i�ύX�ӏ��j
            fixed4 frag(v2f i) : SV_Target
            {
                //Speed
                _XShift = _XShift * _XSpeed;

            //add Shift
            i.uv.x += _XShift / 10000;

            //i.uv�̓K�p
            fixed4 col = tex2D(_MainTex, i.uv) * _Color;
            col.a = _Alpha;
            UNITY_APPLY_FOG(i.fogCoord, col);
            return col;
        }

        ENDCG
    }
    }
}
