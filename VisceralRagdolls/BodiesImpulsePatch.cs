using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Aki.Reflection.Patching;
using EFT;
using EFT.Ballistics;
using EFT.Interactive;
using UnityEngine;

namespace VisceralRagdolls
{
	// Token: 0x02000002 RID: 2
	public class BodiesImpulsePatch : ModulePatch
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		protected override MethodBase GetTargetMethod()
		{
			return typeof(BallisticsCalculator).GetMethod("Shoot", BindingFlags.Instance | BindingFlags.Public);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002068 File Offset: 0x00000268
		[PatchPostfix]
		private static void Postfix(EftBulletClass shot)
		{
			StaticManager.Instance.StartCoroutine(BodiesImpulsePatch.WatchShot(shot));
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000207B File Offset: 0x0000027B
		private static IEnumerator WatchShot(EftBulletClass shot)
		{
			while (!shot.IsShotFinished)
			{
				yield return null;
			}
			if (shot.HitCollider == null)
			{
				yield break;
			}
			BulletClass bulletClass = shot.Ammo as BulletClass;
			if (bulletClass == null)
			{
				yield break;
			}
			float num;
			if (!BodiesImpulsePatch._dictionary.TryGetValue(bulletClass.Caliber, out num))
			{
				yield break;
			}
			Rigidbody attachedRigidbody = shot.HitCollider.attachedRigidbody;
			if (attachedRigidbody == null)
			{
				yield break;
			}
			num /= (float)((bulletClass.ProjectileCount > 0) ? bulletClass.ProjectileCount : 1);
			float num2;
			if (BodiesImpulsePatch._bonedictionary.TryGetValue(shot.HitCollider.name, out num2) && bulletClass.Caliber != "12g")
			{
				num *= num2;
			}
			if (attachedRigidbody.gameObject.GetComponent<ObservedLootItem>())
			{
				num *= VisceralEntry.Instance.objectIntensity.Value;
			}
			attachedRigidbody.AddForceAtPosition(shot.Direction * (num * VisceralEntry.Instance.ShotIntensity.Value), shot.HitPoint);
			yield break;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000208A File Offset: 0x0000028A
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

		// Token: 0x04000001 RID: 1
		private static Dictionary<string, float> _dictionary = new Dictionary<string, float>
		{
			{
				"12g",
				150f
			},
			{
				"762x51",
				65f
			},
			{
				"762x39",
				45f
			},
			{
				"9x39",
				33f
			},
			{
				"545x39",
				35f
			},
			{
				"9x18PM",
				12f
			},
			{
				"762x35",
				60f
			},
			{
				"556x45NATO",
				30f
			},
			{
				"127x55",
				100f
			},
			{
				"127x108",
				350f
			},
			{
				"366TKM",
				60f
			},
			{
				"40x46",
				200f
			},
			{
				"26x75",
				70f
			},
			{
				"30x29",
				350f
			},
			{
				"762x54R",
				95f
			},
			{
				"86x70",
				800f
			},
			{
				"9x19PARA",
				12f
			},
			{
				"1143x23ACP",
				12f
			},
			{
				"Caliber9x21",
				5f
			},
			{
				"57x28",
				40f
			},
			{
				"23x75",
				200f
			},
			{
				"25x59mm",
				180f
			},
			{
				"12.7x99",
				110f
			},
			{
				"68x51",
				40f
			}
		};

		// Token: 0x04000002 RID: 2
		private static Dictionary<string, float> _bonedictionary = new Dictionary<string, float>
		{
			{
				"Plate_Korund_chest",
				0.6f
			},
			{
				"Plate_Granit_SAPI_chest",
				0.6f
			},
			{
				"Plate_Granit_SAPI_back",
				0.6f
			},
			{
				"Plate_6B13_back",
				0.6f
			},
			{
				"LeftSideChestDown",
				0.6f
			},
			{
				"RightSideChestDown",
				0.6f
			},
			{
				"SpineLowerChest",
				0.8f
			},
			{
				"SpineTopChest",
				0.8f
			},
			{
				"LeftSideChestTop",
				0.8f
			},
			{
				"RightSideChestTop",
				0.8f
			},
			{
				"Base HumanSpine3",
				0.8f
			},
			{
				"Base HumanSpine2",
				0.8f
			},
			{
				"Base HumanSpine1",
				0.8f
			},
			{
				"Base HumanPelvis",
				0.8f
			},
			{
				"NeckForward",
				1f
			},
			{
				"NeckBackward",
				1f
			},
			{
				"Base HumanHead",
				1f
			},
			{
				"Parietal",
				1f
			},
			{
				"BackHead",
				1f
			},
			{
				"EarsL",
				1f
			},
			{
				"EarsR",
				1f
			},
			{
				"Eyes",
				1f
			},
			{
				"Jaw",
				1f
			}
		};
	}
}
