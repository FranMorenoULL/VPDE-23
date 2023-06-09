using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class SetPlatformOptionsPS4
{
	[MenuItem("SCE Publishing Utils/Set Publish Settings For PS4")]
	// Use this for initialization
	static void SetOptions()
	{
        // Param file settings.
        PlayerSettings.PS4.category = PlayerSettings.PS4.PS4AppCategory.Application;
		PlayerSettings.PS4.appVersion = "01.00";
		PlayerSettings.PS4.masterVersion = "01.00";
		// Use the title id from the SCE np example project, using for now as we don't yet have all the required PSN services enabled for our own test title..
		//PlayerSettings.PS4.contentID = "ED1987-NPXX51138_00-0123456789ABCDEF";
		// This is the Unity example title content ID
		PlayerSettings.PS4.contentID = "IV0002-NPXX51362_00-UNITYNPTOOLKIT00";
		// The title ID of NPXX51362_00 uses a NP Communication ID of NPWR05690_00 ... see https://ps4.scedev.net/titles/107929/products/118345
		
		
		
		PlayerSettings.productName = "Unity Np Toolkit Example";
		PlayerSettings.PS4.parentalLevel = 1;
		PlayerSettings.PS4.enterButtonAssignment = PlayerSettings.PS4.PS4EnterButtonAssignment.CrossButton;
		PlayerSettings.PS4.paramSfxPath = "";	// "Assets/Editor/SonyNPPS4PublishData/param.sfx";

		// PSN Settings.
		PlayerSettings.PS4.NPtitleDatPath = "Assets/Editor/SonyNPPS4PublishData/NPXX51362_00/nptitle.dat";
		PlayerSettings.PS4.npTrophyPackPath = "Assets/Editor/SonyNPPS4PublishData/trophy.trp";
		PlayerSettings.PS4.npAgeRating = 12;
		// This is the Unity example title secret ( NPXX51362 )  ...
		PlayerSettings.PS4.npTitleSecret = "0xbd,0x0a,0x1b,0x8a,0xb2,0x24,0x96,0x57,\n"
		                                + "0xb3,0xcb,0x49,0xe5,0x18,0xe4,0x50,0x39,\n"
		                                + "0x40,0x77,0x28,0xa2,0xb5,0xe7,0xb1,0x93,\n"
		                                + "0xca,0xb9,0x52,0x45,0x2b,0xd7,0x27,0x66,\n"
		                                + "0x0f,0x70,0xf5,0x9d,0x92,0x9a,0x8d,0x71,\n"
		                                + "0xb3,0x44,0x5a,0xfb,0x05,0xe6,0xb6,0xa9,\n"
		                                + "0xc0,0x7e,0x4c,0x72,0x82,0x33,0xa9,0xe6,\n"
		                                + "0x8e,0x98,0x4a,0x8a,0xe6,0xea,0xbf,0xc2,\n"
		                                + "0x64,0xd7,0x16,0x5f,0xcf,0x0f,0x4a,0x57,\n"
		                                + "0x11,0xe5,0x0a,0x09,0xd3,0x0f,0x78,0xe5,\n"
		                                + "0x61,0xd0,0x85,0x30,0x73,0x7f,0xdf,0x93,\n"
		                                + "0x63,0x42,0x82,0x56,0x65,0x79,0xe5,0xe1,\n"
		                                + "0xad,0xd9,0xfc,0x87,0xdd,0xb1,0x83,0x7a,\n"
		                                + "0x97,0x9d,0xfc,0xae,0x54,0xac,0x1f,0x67,\n"
		                                + "0xcf,0x70,0x01,0x6d,0xaf,0xb0,0xcf,0xfc,\n"
		                                + "0x2a,0xa0,0x9d,0x71,0xaa,0x81,0x03,0xd5";

        // Replace the old NpToolkit module with the NpToolkit2 version
        string[] modules = PlayerSettings.PS4.includedModules;

        if (modules.Length == 0)
        {
            Debug.Log("The player settings modules list is empty. Please open the player settings to initialise the list and try again.");
            return;
        }

        bool alreadySet = false;
        bool changed = false;

        for (int i = modules.Length - 1; i >= 0; i--)
        {
            if (modules[i].IndexOf("libSceNpToolkit.prx", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Debug.Log("Swapped module libSceNpToolkit.prx for libSceNpToolkit2.prx");
                modules[i] = "libSceNpToolkit2.prx";
                changed = true;
            }
            else if (modules[i].IndexOf("libSceNpToolkit2.prx", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                alreadySet = true;
            }
        }
        PlayerSettings.PS4.includedModules = modules;

        if ( alreadySet == false && changed == false)
        {
            Debug.LogError("Unable to find libSceNpToolkit.prx or libSceNpToolkit2.prx in modules list.");
        }

        AssetDatabase.Refresh();
    }

    // Replace whatever Input Manager you currently have with one to work with the Nptoolkit Sample
    [MenuItem("SCE Publishing Utils/Set Input Manager")]
    static void ReplaceInputManager()
    {
        // This is the InputManager asset that comes with the example project. Note that to avoid an import error, the '.asset' file extension has been removed
        string sourceFile = Path.Combine(Application.dataPath, "Editor/InputManager");

        // This is the InputManager in your ProjectSettings folder
        string targetFile = Application.dataPath;
        targetFile = targetFile.Replace("/Assets", "/ProjectSettings/InputManager.asset");

        // Replace the ProjectSettings file with the new one, and trigger a refresh so the Editor sees it
        FileUtil.ReplaceFile(sourceFile, targetFile);
        AssetDatabase.Refresh();

        Debug.Log("InputManager replaced!");
    }
}
