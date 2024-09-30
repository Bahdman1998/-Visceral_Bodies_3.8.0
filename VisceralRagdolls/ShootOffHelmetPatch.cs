using System;
using System.Reflection;
using Aki.Reflection.Patching;
using EFT;
using EFT.Ballistics;
using EFT.InventoryLogic;
using MPT.Core.Coop.Matchmaker;
using UnityEngine;

namespace VisceralRagdolls
{
	// Token: 0x02000007 RID: 7
	public class ShootOffHelmetPatch : ModulePatch
	{
		// Token: 0x06000015 RID: 21 RVA: 0x00002780 File Offset: 0x00000980
		protected override MethodBase GetTargetMethod()
		{
			return typeof(Player).GetMethod("ReceiveDamage", BindingFlags.Instance | BindingFlags.Public);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002798 File Offset: 0x00000998
		[PatchPostfix]
		private static void Postfix(Player __instance, EBodyPart part, MaterialType special)
		{
			if (VisceralEntry.Instance.ShootHelmetOff.Value && MatchmakerAcceptPatches.IsServer && __instance.IsAI && part == null && (special == 34 || special == 36) && Random.Range(0f, 100f) <= VisceralEntry.Instance.HelmetShootOffChance.Value)
			{
				Slot slot = __instance.Inventory.Equipment.GetSlot(11);
				if (slot.ContainedItem != null)
				{
					__instance.GClass2761_0.ThrowItem(slot.ContainedItem, Array.Empty<GStruct369>(), null, false);
				}
			}
		}
	}
}
