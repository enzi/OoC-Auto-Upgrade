using BepInEx;
using UnityEngine;

namespace OoCAutoUpgrade;

[BepInPlugin(ModGuid, ModName, ModVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string ModGuid = "ooc.autoupgrade";
    private const string ModName = "OoC Auto Upgrade";
    private const string ModVersion = "1.0.0";

    private bool autoEnabled;

    private void Awake()
    {
        Logger.LogInfo($"{ModName} {ModVersion} loaded. Toggle with Ctrl+A.");
    }

    private void Update()
    {
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            && Input.GetKeyDown(KeyCode.A))
        {
            autoEnabled = !autoEnabled;
            Logger.LogInfo($"Auto-upgrade {(autoEnabled ? "ON" : "OFF")}");
            //DumpStructures();
        }

        if (!autoEnabled) return;

        DrainTrivialStructures();
    }

    private void DumpStructures()
    {
        var all = StructureSO.All;
        if (all == null)
        {
            Logger.LogInfo("StructureSO.All is null");
            return;
        }
        Logger.LogInfo($"StructureSO.All count={all.Count}");
        for (int i = 0; i < all.Count; i++)
        {
            var s = all[i];
            if (s == null) { Logger.LogInfo($"  [{i}] <null>"); continue; }
            string name;
            try { name = s.GetName(); } catch { name = "<GetName threw>"; }
            bool minimal = false, trivial = false, hasEnough = false, canPurchase = false, available = false;
            try { available = s.IsAvailable(); } catch { }
            try { canPurchase = s.CanPurchase(); } catch { }
            try { var c = s.GetPurchaseCost(); if (c != null) { minimal = c.IsMinimalAmount(); trivial = c.IsTrivialCost(); hasEnough = c.HasEnough(); } } catch { }
            Logger.LogInfo($"  [{i}] {name}  qty={s.quantity} q={s.queuedQuantity}  available={available} canPurchase={canPurchase} minimal={minimal} trivial={trivial} hasEnough={hasEnough}");
        }
    }

    private static void DrainTrivialStructures()
    {
        var all = StructureSO.All;
        if (all == null) return;

        const int maxPasses = 50;
        for (int pass = 0; pass < maxPasses; pass++)
        {
            bool advanced = false;
            for (int i = 0; i < all.Count; i++)
            {
                var s = all[i];
                if (s == null) continue;
                if (!s.IsAvailable()) continue;
                if (!s.CanPurchase()) continue;

                var cost = s.GetPurchaseCost();
                if (cost == null) continue;
                if (!cost.IsMinimalAmount()) continue;
                if (!cost.HasEnough()) continue;

                int before = s.quantity + s.queuedQuantity;
                s.Purchase();
                int after = s.quantity + s.queuedQuantity;
                if (after > before) advanced = true;
            }
            if (!advanced) break;
        }
    }
}
