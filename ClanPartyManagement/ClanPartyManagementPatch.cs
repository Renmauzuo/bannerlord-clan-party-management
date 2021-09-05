using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection;

namespace ClanPartyManagement
{
    [HarmonyPatch(typeof(SPInventoryVM), "RefreshValues")]
    class ClanPartyManagementPatch
    {
        public static void Prefix(ref SPInventoryVM __instance)
        {
            TroopRoster newRoster = TroopRoster.CreateDummyTroopRoster();
            newRoster.Add(__instance.TroopRoster);
            foreach (Hero hero in Clan.PlayerClan.Heroes)
            {
                if (hero.IsAlive && !hero.IsChild && !newRoster.Contains(hero.CharacterObject) && ((hero.CurrentSettlement != null && hero.CurrentSettlement == Hero.MainHero.CurrentSettlement) || (hero.PartyBelongedTo != null && hero.PartyBelongedTo.Army != null && hero.PartyBelongedTo.Army == MobileParty.MainParty.Army)))
                {
                    __instance.CharacterList.AddItem(new SelectorItemVM(hero.Name));
                    newRoster.AddToCounts(hero.CharacterObject, 1);
                }
            }
            __instance.TroopRoster = newRoster;
        }
    }
}
