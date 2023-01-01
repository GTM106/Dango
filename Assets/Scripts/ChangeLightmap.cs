using UnityEngine;
using System.Collections;

public class ChangeLightmap : MonoBehaviour
{
	[SerializeField] Texture2D[] sLight, sDir, tLight, tDir;
	[SerializeField] LightProbes sLightprobes;
	[SerializeField] Cubemap sCubemap, tCubemap;


	LightmapData[] stageLight;
	LightmapData[] tutorialLight;

	// Use this for initialization
	void Start()
	{
		stageLight = new LightmapData[sLight.Length];
		tutorialLight = new LightmapData[tLight.Length];

		for (int i = 0; i < stageLight.Length; i++)
		{
			LightmapData data = new LightmapData();
			data.lightmapDir = sDir[i];
			data.lightmapColor = sLight[i];
			stageLight[i] = data;
		}
		for (int i = 0; i < tutorialLight.Length; i++)
		{
			LightmapData data = new LightmapData();
			data.lightmapDir = tDir[i];
			data.lightmapColor = tLight[i];
			tutorialLight[i] = data;
		}
	}

	public void StageLight()
    {
		LightmapSettings.lightmaps = stageLight;
		LightmapSettings.lightProbes = sLightprobes;
		RenderSettings.customReflection = sCubemap;
	}

	public void TutorialLight()
	{
		LightmapSettings.lightmaps = tutorialLight;
		RenderSettings.customReflection = tCubemap;
	}
}

