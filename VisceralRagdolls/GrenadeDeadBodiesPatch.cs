using System;
using System.Reflection;
using Aki.Reflection.Patching;
using EFT;
using UnityEngine;

namespace VisceralRagdolls
{
	// Token: 0x02000006 RID: 6
	public class GrenadeDeadBodiesPatch : ModulePatch
	{
		// Token: 0x06000012 RID: 18 RVA: 0x000026CC File Offset: 0x000008CC
		protected override MethodBase GetTargetMethod()
		{
			return typeof(Grenade).GetMethod("Explosion", BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000026E4 File Offset: 0x000008E4
		[PatchPostfix]
		private static void Postfix(IExplosiveItem grenadeItem, Vector3 grenadePosition)
		{
			float num = Random.Range(grenadeItem.MinExplosionDistance, grenadeItem.MaxExplosionDistance);
			foreach (RaycastHit raycastHit in Physics.SphereCastAll(new Ray(grenadePosition, Vector3.up), num, grenadeItem.MaxExplosionDistance, GClass2987.HitMask))
			{
				Rigidbody component = raycastHit.collider.GetComponent<Rigidbody>();
				if (component != null)
				{
					component.AddExplosionForce(grenadeItem.GetStrength * 0.5f * VisceralEntry.Instance.GrenadeExplIntensity.Value, grenadePosition, num);
				}
			}
		}
	}
}
