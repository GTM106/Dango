using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class LightmapPrefab : MonoBehaviour
{

	[System.Serializable]
	class LightmapParameter
	{
		public int lightmapIndex = -1;
		public Vector4 scaleOffset = Vector4.zero;
		public Renderer renderer;

		public void UpdateLightmapUVs()
		{
			renderer.lightmapScaleOffset = scaleOffset;
			renderer.lightmapIndex = lightmapIndex;
		}
	}

	void Start()
	{
		foreach (var lightmapParameter in lightmapParameters)
		{
			lightmapParameter.UpdateLightmapUVs();
		}
	}

	//[HideInInspector]
	[SerializeField]
	LightmapParameter[] lightmapParameters;

	[ContextMenu("Setup")]
	void Setup()
	{
		var renderer = GetComponentsInChildren<Renderer>();
		lightmapParameters = new LightmapParameter[renderer.Length];

		for (int i = 0; i < renderer.Length; i++)
		{
			var currentRenderer = renderer[i];
			lightmapParameters[i] = new LightmapParameter()
			{
				lightmapIndex = currentRenderer.lightmapIndex,
				scaleOffset = currentRenderer.lightmapScaleOffset,
				renderer = currentRenderer,
			};
		}
	}
}