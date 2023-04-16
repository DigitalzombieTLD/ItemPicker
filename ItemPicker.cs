using MelonLoader;
using UnityEngine;
using Il2CppInterop;
using Il2CppInterop.Runtime.Injection; 
using System.Collections;
using Il2Cpp;


namespace ItemPicker
{
	public class ItemPickerMain : MelonMod
	{
        public static GameObject playerObject;
        public static int layerMask = 0;
        public static int stoneCounter = 0, stickCounter = 0, customCounter = 0;
        public static List<string> customItems = new List<string>();

        public override void OnInitializeMelon()
		{
            layerMask |= 1 << 17; // gear layer		
            Settings.OnLoad();

            loadCustomItemList();
        }

		
        public override void OnUpdate()
		{
            if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.buttonPickSticks))
            {
                RaycastHit[] sphereTargethit;
                sphereTargethit = Physics.SphereCastAll(GameManager.GetVpFPSPlayer().transform.position, Settings.options.pickupRadius, GameManager.GetVpFPSPlayer().transform.TransformDirection(Vector3.down), GameManager.GetVpFPSPlayer().Controller.m_Controller.height * 0.8f, layerMask);

                foreach (RaycastHit foo in sphereTargethit)
                {
                    GearItem foundItem = foo.transform.gameObject.GetComponent<GearItem>();

                    if (foundItem && foundItem.enabled)
                    {
                        if (foundItem.name.Contains("GEAR_Stick") && !foundItem.m_InPlayerInventory && (Settings.options.pickupChoice == 0 || Settings.options.pickupChoice == 2))
                        {
                            
                            GameManager.GetPlayerManagerComponent().ProcessPickupItemInteraction(foundItem, false, false, false);
                            stickCounter++;
                        }

                        if (foundItem.name.Contains("GEAR_Stone") && !foundItem.m_InPlayerInventory && (Settings.options.pickupChoice == 1 || Settings.options.pickupChoice == 2))
                        {
                       
                            GameManager.GetPlayerManagerComponent().ProcessPickupItemInteraction(foundItem, false, false, false);
                            stoneCounter++;
                        }

                        if (customItems.Contains(foundItem.name) && !foundItem.m_InPlayerInventory && Settings.options.pickUpAdditionalItems == true)
                        {
                       
                            GameManager.GetPlayerManagerComponent().ProcessPickupItemInteraction(foundItem, false, false, false);
                            customCounter++;
                        }
                    }
                }

                int calorieCost = 0;
                int itemCount = stickCounter + stoneCounter + customCounter;

                if (itemCount != 0 && Settings.options.calorieCost != 0)
                {
                    calorieCost = itemCount * Settings.options.calorieCost;
                    GameManager.GetHungerComponent().RemoveReserveCalories(calorieCost);
                }

                MelonLogger.Msg("Picked up " + stickCounter + " sticks and " + stoneCounter + " stones in radius of " + Settings.options.pickupRadius + " meter and spent " + calorieCost + " calories.");

                if (Settings.options.pickUpAdditionalItems)
                {
                    MelonLogger.Msg("Oh and " + customCounter + " items from your custom list also!");
                }

                customCounter = 0;
                stoneCounter = 0;
                stickCounter = 0;
            }
        }
        public static void loadCustomItemList()
        {
            customItems.Clear();

            if (File.Exists("Mods\\ItemPickerCustomList.txt") && Settings.options.pickUpAdditionalItems)
            {
                using (StreamReader sr = File.OpenText("Mods\\ItemPickerCustomList.txt"))
                {
                    while (!sr.EndOfStream)
                    {
                        customItems.Add(sr.ReadLine());
                    }
                    sr.Close();
                }

                MelonLogger.Msg("Loaded custom list with " + customItems.Count + " items");
            }
        }
    }
}