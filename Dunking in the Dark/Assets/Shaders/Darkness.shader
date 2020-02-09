// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Darkness"
{
	Properties
	{
		_Color("Main Color", Color) = (0.0,0.0,0.0,1)
		_PosOne("Character 1 Position", Float) = (0,0,0,0)
		_OneRad("Character 1 Radius", Float) = 5
		_PosTwo("Character 2 Position", Float) = (0,0,0,0)
		_TwoRad("Character 2 Radius", Float) = 5
		_PosGoal("Goal Position", Float) = (0,0,0,0)
		_GoalRad("Goal Show Radius", Float) = 0
	}
		SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Cull front
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			uniform float4 _PosOne;
			uniform float4 _PosTwo;
			uniform float _OneRad;
			uniform float _TwoRad;

			uniform float4 _PosGoal;
			uniform float _GoalRad;

				struct vertexInput {
				float4 vertex : POSITION;
				};

				struct vertexOutput {
					float4 pos : SV_POSITION;
					float4 position_in_world_space : TEXCOORD0;
				};

				vertexOutput vert(vertexInput input)
				{
					vertexOutput output;

					output.pos = UnityObjectToClipPos(input.vertex);
					output.position_in_world_space =
						mul(unity_ObjectToWorld, input.vertex);
					// transformation of input.vertex from object 
					// coordinates to world coordinates;
					return output;
				}

				float4 frag(vertexOutput input) : COLOR
				{
					float distOne = distance(input.position_in_world_space,
					_PosOne);
					float distTwo = distance(input.position_in_world_space,
					_PosTwo);
					float distThree = distance(input.position_in_world_space,
						_PosGoal);

					float hasOne = step(_OneRad, distOne);
					float hasTwo = step(_TwoRad, distTwo);
					float hasThree = step(_GoalRad, distThree);
					float canHide = hasOne * hasTwo * hasThree;
					// computes the distance between the fragment position 
					// and the origin (the 4th coordinate should always be 
					// 1 for points).

					return float4(0.0, 0.0, 0.0, canHide);

				}

				ENDCG


				/*
				// vertex shader
				// this time instead of using "appdata" struct, just spell inputs manually,
				// and instead of returning v2f struct, also just return a single output
				// float4 clip position
				float4 vert(float4 vertex : POSITION) : SV_POSITION
				{
				return UnityObjectToClipPos(vertex);
				}

				// color from the material
				fixed4 _Color;

				// pixel shader, no inputs needed
				fixed4 frag() : SV_Target
				{
					
					return _Color; // just return it
				}
				ENDCG*/
		}
	}
}
