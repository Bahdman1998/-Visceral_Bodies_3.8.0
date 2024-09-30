using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Aki.Reflection.Patching;
using Comfort.Common;
using EFT;
using UnityEngine;

namespace VisceralRagdolls
{
	// Token: 0x02000005 RID: 5
	public class GameStartedPatch : ModulePatch
	{
		// Token: 0x0600000F RID: 15 RVA: 0x00002516 File Offset: 0x00000716
		protected override MethodBase GetTargetMethod()
		{
			return typeof(GameWorld).GetMethod("OnGameStarted");
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000252C File Offset: 0x0000072C
		[PatchPostfix]
		private static void Postfix(GameWorld __instance)
		{
			TarkovApplication obj = (TarkovApplication)Singleton<ClientApplication<ISession>>.Instance;
			RaidSettings raidSettings = (RaidSettings)typeof(TarkovApplication).GetField("_raidSettings", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(obj);
			IEnumerable<GameObject> enumerable = from go in Object.FindObjectsOfType<GameObject>()
			where go.layer == LayerMask.NameToLayer("Grass")
			select go;
			IEnumerable<GameObject> enumerable2 = from go in Object.FindObjectsOfType<GameObject>()
			where go.layer == LayerMask.NameToLayer("Foliage")
			select go;
			GameObject gameObject = GameObject.Find("TerrainsAI");
			if (enumerable != null)
			{
				if (raidSettings.SelectedLocation.Name == "Streets of Tarkov")
				{
					using (IEnumerator<GameObject> enumerator = enumerable.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							GameObject gameObject2 = enumerator.Current;
							gameObject2.layer = LayerMask.NameToLayer("PlayerSpiritAura");
						}
						goto IL_10C;
					}
				}
				foreach (GameObject gameObject3 in enumerable)
				{
					gameObject3.SetActive(false);
				}
			}
			IL_10C:
			if (enumerable2 != null)
			{
				foreach (GameObject gameObject4 in enumerable2)
				{
					gameObject4.layer = LayerMask.NameToLayer("PlayerSpiritAura");
				}
			}
			if (gameObject != null && gameObject.activeSelf)
			{
				gameObject.SetActive(false);
			}
		}
	}
}
