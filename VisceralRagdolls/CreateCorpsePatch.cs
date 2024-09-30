using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Aki.Reflection.Patching;
using EFT;
using UnityEngine;

namespace VisceralRagdolls
{
	// Token: 0x02000003 RID: 3
	public class CreateCorpsePatch : ModulePatch
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000023B5 File Offset: 0x000005B5
		protected override MethodBase GetTargetMethod()
		{
			return typeof(Player).GetMethod("CreateCorpse", BindingFlags.Instance | BindingFlags.Public, null, Array.Empty<Type>(), null);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000023D4 File Offset: 0x000005D4
		[PatchPostfix]
		private static void Postfix(Player __instance)
		{
			if (VisceralEntry.Instance.BodyCollision.Value)
			{
				if (__instance.IsYourPlayer)
				{
					return;
				}
				foreach (Transform transform in from t in CreateCorpsePatch.EnumerateHierarchyCore(__instance.Transform.Original)
				where CreateCorpsePatch.TargetBones.Any((string u) => t.name.ToLower().Contains(u))
				select t)
				{
					transform.gameObject.layer = 6;
				}
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000246C File Offset: 0x0000066C
		private static IEnumerable<Transform> EnumerateHierarchyCore(Transform root)
		{
			Queue<Transform> transformQueue = new Queue<Transform>();
			transformQueue.Enqueue(root);
			while (transformQueue.Count > 0)
			{
				Transform transform = transformQueue.Dequeue();
				if (transform)
				{
					for (int i = 0; i < transform.childCount; i++)
					{
						transformQueue.Enqueue(transform.GetChild(i));
					}
					yield return transform;
				}
			}
			yield break;
		}

		// Token: 0x04000003 RID: 3
		private static string[] TargetBones = new string[]
		{
			"thigh",
			"calf",
			"foot",
			"spine3",
			"forearm",
			"head"
		};
	}
}
