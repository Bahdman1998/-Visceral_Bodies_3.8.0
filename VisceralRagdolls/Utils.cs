using System;
using UnityEngine.SceneManagement;

namespace VisceralRagdolls
{
	// Token: 0x02000008 RID: 8
	public class Utils
	{
		// Token: 0x06000018 RID: 24 RVA: 0x0000282D File Offset: 0x00000A2D
		private void SceneManager_activeSceneChanged(Scene oldScene, Scene newScene)
		{
			if (!VisceralEntry.Instance)
			{
				return;
			}
			VisceralEntry.Instance.IsSoT = Utils.CheckForGameplayMap(newScene.name);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002852 File Offset: 0x00000A52
		public static bool CheckForGameplayMap(string sceneName)
		{
			return sceneName == "City_Scripts";
		}
	}
}
