using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CardSurvival_OrderSpoilage
{
    [HarmonyPatch(typeof(GraphicsManager), nameof(GraphicsManager.InspectCard))]
    public static class GraphicsManager_InspectCard_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {


            //-- Original
            //IL_02e2: ldfld class InspectionPopup GraphicsManager::InventoryInspectionPopup
            //IL_02e7: stfld class InspectionPopup GraphicsManager::CurrentInspectionPopup
            //// InventoryInspectionPopup.Setup(_Card);
            //IL_02ec: ldarg.0
            //IL_02ed: ldfld class InspectionPopup GraphicsManager::InventoryInspectionPopup
            //IL_02f2: ldarg.1
            //IL_02f3: callvirt instance void InspectionPopup::Setup(class InGameCardBase)
            //// CurrentInspectionPopup = CardInspectionPopup;
            //IL_02f8: ret

            //--Goal
            //IL_02e2: ldfld class InspectionPopup GraphicsManager::InventoryInspectionPopup
            //IL_02e7: stfld class InspectionPopup GraphicsManager::CurrentInspectionPopup

            // > Call OrderSpoilage
            //// InventoryInspectionPopup.Setup(_Card);
            //IL_02ec: ldarg.0
            //IL_02ed: ldfld class InspectionPopup GraphicsManager::InventoryInspectionPopup
            //IL_02f2: ldarg.1
            //IL_02f3: callvirt instance void InspectionPopup::Setup(class InGameCardBase)
            //// CurrentInspectionPopup = CardInspectionPopup;
            //IL_02f8: ret


            List<CodeInstruction> instructionList = instructions.ToList();

            //foreach (var instruction in instructionList)
            //{
            //    Plugin.LogInfo($"{instruction.ToString()} - {instruction.operand?.GetType().Name}");
            //}

            List<CodeInstruction> newInstructions = new CodeMatcher(instructionList)
                .MatchForward(true,
                    new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(GraphicsManager), nameof(GraphicsManager.InventoryInspectionPopup))),
                    new CodeMatch(OpCodes.Stfld, AccessTools.Field(typeof(GraphicsManager), nameof(GraphicsManager.CurrentInspectionPopup))),
                    new CodeMatch(OpCodes.Ldarg_0),
                    new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(GraphicsManager), nameof(GraphicsManager.InventoryInspectionPopup)))
                    )
                .ThrowIfNotMatch("Inventory and current popups.")
                .Advance(-1)
                .InsertAndAdvance(
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(GraphicsManager), nameof(GraphicsManager.CurrentInspectionPopup))),
                    new CodeInstruction(OpCodes.Ldarg_1), //The in game card
                    CodeInstruction.Call(typeof(GraphicsManager_InspectCard_Patch), nameof(GraphicsManager_InspectCard_Patch.OrderSpoilage))
                )
                .Advance(2)
                .MatchForward(true,
                    new CodeMatch(OpCodes.Ldarg_1),
                    new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(InspectionPopup), nameof(InspectionPopup.Setup), new Type[] { typeof(InGameCardBase) }))
                )
                .ThrowIfNotMatch("Did not find post Setup call")
                .InstructionEnumeration()
                .ToList();

            //Plugin.LogInfo("--------- after");
            //foreach (var instruction in newInstructions)
            //{
            //    Plugin.LogInfo($"{instruction.ToString()} - {instruction.operand?.GetType().Name}");
            //}

            return newInstructions;
        }

        public static void OrderSpoilage(InspectionPopup inspection, InGameCardBase card)
        {
            if (card.CardModel.name == "ClayPotCoolerOn")
            {
                card.CardsInInventory.Sort(DurabilityComparer);

                inspection.RefreshInventory();
            }
        }

        public static int DurabilityComparer(InventorySlot x, InventorySlot y)
        {
            if (x.IsFree || y.IsFree) return x.IsFree.CompareTo(y.IsFree) * -1;
            if (x.MainCard is null && y.MainCard is null) return 0;
            if (x.MainCard is not null && y.MainCard is null) return -1;
            if (x.MainCard is null && y.MainCard is not null) return 1;

            //non spoilage
            if (x.MainCard.CurrentSpoilage == 0 || y.MainCard.CurrentSpoilage == 0)
            {
                return x.MainCard.CurrentSpoilage.CompareTo(y.MainCard.CurrentSpoilage) * -1;
            }

            //Spoilage compare
            return x.MainCard.CurrentSpoilage.CompareTo(y.MainCard.CurrentSpoilage);

        }


    }
}
