using System;
using BepInEx;
using BepInEx.Configuration;
using Comfort.Common;
using EFT;
using UnityEngine;

namespace VisceralRagdolls
{
	// Token: 0x02000009 RID: 9
	[BepInPlugin("com.servph.visceralbodies", "Visceral Bodies", "1.2.0")]
	public class VisceralEntry : BaseUnityPlugin
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600001B RID: 27 RVA: 0x0000286C File Offset: 0x00000A6C
		// (set) Token: 0x0600001C RID: 28 RVA: 0x00002874 File Offset: 0x00000A74
		public Player LocalPlayer { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600001D RID: 29 RVA: 0x0000287D File Offset: 0x00000A7D
		// (set) Token: 0x0600001E RID: 30 RVA: 0x00002885 File Offset: 0x00000A85
		public Player HideoutPlayer { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001F RID: 31 RVA: 0x0000288E File Offset: 0x00000A8E
		// (set) Token: 0x06000020 RID: 32 RVA: 0x00002896 File Offset: 0x00000A96
		public ConfigEntry<bool> BodyCollision { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000021 RID: 33 RVA: 0x0000289F File Offset: 0x00000A9F
		// (set) Token: 0x06000022 RID: 34 RVA: 0x000028A7 File Offset: 0x00000AA7
		public ConfigEntry<bool> ItemForce { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000028B0 File Offset: 0x00000AB0
		// (set) Token: 0x06000024 RID: 36 RVA: 0x000028B8 File Offset: 0x00000AB8
		public ConfigEntry<bool> ShootHelmetOff { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000025 RID: 37 RVA: 0x000028C1 File Offset: 0x00000AC1
		// (set) Token: 0x06000026 RID: 38 RVA: 0x000028C9 File Offset: 0x00000AC9
		public ConfigEntry<bool> IsSlingingEnabled { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000027 RID: 39 RVA: 0x000028D2 File Offset: 0x00000AD2
		// (set) Token: 0x06000028 RID: 40 RVA: 0x000028DA File Offset: 0x00000ADA
		public ConfigEntry<float> ShotIntensity { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000029 RID: 41 RVA: 0x000028E3 File Offset: 0x00000AE3
		// (set) Token: 0x0600002A RID: 42 RVA: 0x000028EB File Offset: 0x00000AEB
		public ConfigEntry<float> HelmetShootOffChance { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000028F4 File Offset: 0x00000AF4
		// (set) Token: 0x0600002C RID: 44 RVA: 0x000028FC File Offset: 0x00000AFC
		public ConfigEntry<float> GrenadeExplIntensity { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002905 File Offset: 0x00000B05
		// (set) Token: 0x0600002E RID: 46 RVA: 0x0000290D File Offset: 0x00000B0D
		public ConfigEntry<float> objectIntensity { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002916 File Offset: 0x00000B16
		// (set) Token: 0x06000030 RID: 48 RVA: 0x0000291E File Offset: 0x00000B1E
		public ConfigEntry<float> timer { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002927 File Offset: 0x00000B27
		// (set) Token: 0x06000032 RID: 50 RVA: 0x0000292F File Offset: 0x00000B2F
		public ConfigEntry<float> x { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002938 File Offset: 0x00000B38
		// (set) Token: 0x06000034 RID: 52 RVA: 0x00002940 File Offset: 0x00000B40
		public ConfigEntry<float> y { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002949 File Offset: 0x00000B49
		// (set) Token: 0x06000036 RID: 54 RVA: 0x00002951 File Offset: 0x00000B51
		public ConfigEntry<float> z { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000037 RID: 55 RVA: 0x0000295A File Offset: 0x00000B5A
		// (set) Token: 0x06000038 RID: 56 RVA: 0x00002962 File Offset: 0x00000B62
		public bool IsSoT { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000039 RID: 57 RVA: 0x0000296B File Offset: 0x00000B6B
		// (set) Token: 0x0600003A RID: 58 RVA: 0x00002972 File Offset: 0x00000B72
		public static VisceralEntry Instance { get; private set; }

		// Token: 0x0600003B RID: 59 RVA: 0x0000297C File Offset: 0x00000B7C
		public void Awake()
		{
			EFTHardSettings.Instance.DEBUG_CORPSE_PHYSICS = true;
			VisceralEntry.Instance = this;
			new BodiesImpulsePatch().Enable();
			new CreateCorpsePatch().Enable();
			new GrenadeDeadBodiesPatch().Enable();
			new GameStartedPatch().Enable();
			new PhysicalItemsPatch().Enable();
			new ShootOffHelmetPatch().Enable();
			this.BodyCollision = base.Config.Bind<bool>("", "Player Body Collision", true, null);
			this.ItemForce = base.Config.Bind<bool>("", "Item Physics", true, "If you are getting too much lag turn this off. But most capable PC's should run this fine. (Besides on SoT)");
			this.ShootHelmetOff = base.Config.Bind<bool>("", "Shoot off Helmets", true, null);
			this.HelmetShootOffChance = base.Config.Bind<float>("", "Helmet Knock Off Chance", 35f, null);
			this.ShotIntensity = base.Config.Bind<float>("", "Bullet Intensity", 85f, null);
			this.GrenadeExplIntensity = base.Config.Bind<float>("", "Grenade Intensity", 190f, null);
			this.objectIntensity = base.Config.Bind<float>("", "Item Force Intensity", 1f, null);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002AB8 File Offset: 0x00000CB8
		public void Update()
		{
			if (!Singleton<GameWorld>.Instantiated)
			{
				this.LocalPlayer = null;
				this.gameWorld = null;
				return;
			}
			EFTHardSettings.Instance.DEBUG_CORPSE_PHYSICS = true;
			if (!EFTHardSettings.Instance.DEBUG_CORPSE_PHYSICS)
			{
				EFTHardSettings.Set("DEBUG_CORPSE_PHYSICS", "true");
			}
			this.gameWorld = Singleton<GameWorld>.Instance;
			if (this.LocalPlayer == null && this.gameWorld.RegisteredPlayers.Count > 0)
			{
				this.LocalPlayer = (Player)this.gameWorld.RegisteredPlayers[0];
				return;
			}
			if (this.HideoutPlayer == null && this.gameWorld.RegisteredPlayers.Count > 0)
			{
				this.HideoutPlayer = (Player)this.gameWorld.RegisteredPlayers[0];
				return;
			}
		}

		// Token: 0x04000012 RID: 18
		public GameWorld gameWorld;

		// Token: 0x04000013 RID: 19
		public GameObject weapGameObject;

		// Token: 0x04000014 RID: 20
		public BoxCollider boxCollider;

		// Token: 0x04000016 RID: 22
		public Rigidbody weapRigid;

		// Token: 0x04000017 RID: 23
		public Joint attachJoint;

		// Token: 0x04000018 RID: 24
		public CollisionDetectionMode collisionDetectionMode_0;
	}
}
