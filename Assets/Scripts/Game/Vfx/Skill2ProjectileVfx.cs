using Lotus.CoreFramework;
using UnityEngine;

public class Skill2ProjectileVfx : ProjectileVfx
{
    private readonly int[] projectilesEulerAngle = new int[] { -5, 5, 0, 10, -10 };


    protected override void Awake()
    {
        
    }

    protected override void OnShow()
    {
        Firing();
    }

    protected override void Firing()
    {
        if (data == null)
        {
            LogTool.LogErrorEditorOnly("Chưa truyền project data!");
            return;
        }

        transform.LookAt(targetAttack);

        if (muzzleFx != null)
        {
            muzzleFx.transform.SetParent(null);
            muzzleFx.PlayVfx();
        }

        for (int i = 0; i < projectilesEulerAngle.Length; i++)
        {
            string projectileName = $"Hero_Projectile_{DataManager.HeroData.items[ItemType.Bow].itemLevel}";
            ProjectileVfx vfx = this.DequeueProjectileVfx(projectileName, transform);
            vfx.SetLocalPosition(Vector3.zero).SetLocalEulerAngle(new Vector3(0, projectilesEulerAngle[i], 0)).Initial(data);
            vfx.SetTargetAttackPosition(vfx.transform.position + vfx.transform.forward * 100f).SetParent((Transform)null).Show();
        }
    }

    protected override void Explosition()
    {
        
    }

    protected override void OnTriggerEnter(Collider other)
    {
        
    }
}
