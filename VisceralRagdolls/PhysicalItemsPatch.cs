using System;
using System.Reflection;
using Aki.Reflection.Patching;
using EFT.Interactive;
using UnityEngine;

namespace VisceralRagdolls
{
	// Token: 0x02000004 RID: 4
	public class PhysicalItemsPatch : ModulePatch
	{
		// Token: 0x0600000C RID: 12 RVA: 0x000024C1 File Offset: 0x000006C1
		protected override MethodBase GetTargetMethod()
		{
			return typeof(LootItem).GetMethod("IsRigidbodyDone", BindingFlags.Instance | BindingFlags.Public, null, Array.Empty<Type>(), null);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000024E0 File Offset: 0x000006E0
		[PatchPrefix]
		private static bool Prefix(LootItem __instance, ref bool __result)
		{
			if (VisceralEntry.Instance.ItemForce.Value)
			{
				__instance.gameObject.layer = LayerMask.NameToLayer("Deadbody");
				__result = false;
				return false;
			}
			return true;
		}
	}
}
