using System;
using MelonLoader;
using Harmony;
using UnityEngine;
using Il2Cpp;

namespace ItemPicker
{
	[HarmonyPatch(typeof(PlayerManager), "InteractiveObjectsProcessInteraction")]
	public class ExecuteInteractAction
	{
		public static bool Prefix(ref PlayerManager __instance)
		{
			if(Settings.options.skipInspect && __instance.HasInteractiveObjectUnderCrossHair())
			{
				GameObject objectUnderCrosshair = __instance.GetInteractiveObjectUnderCrosshairs(20f);

                if (Settings.options.skipInspectAll)
				{
					if(objectUnderCrosshair.name.Contains("GEAR_") && !objectUnderCrosshair.name.Contains("CardGame"))
					{
						GameManager.GetPlayerManagerComponent().ProcessPickupItemInteraction(objectUnderCrosshair.GetComponent<GearItem>(), false, false, false);
						return false;
					}
				}
				else
				{
					if(objectUnderCrosshair.name.Contains("GEAR_Stick")|| objectUnderCrosshair.name.Contains("GEAR_Stone"))
					{
						GameManager.GetPlayerManagerComponent().ProcessPickupItemInteraction(objectUnderCrosshair.GetComponent<GearItem>(), false, false, false);
						return false;
					}
				}
			}

			// if object is not the cardgame             
			return true;
		}
	}
}