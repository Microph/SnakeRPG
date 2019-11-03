using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor; //Note, this script must reside in a folder called 'Editor' or the compilation will fail at this point!

public class CustomBuildScript
{
    static string[] GetScenePaths()
    {
        string[] scenes = new string[EditorBuildSettings.scenes.Length];
        List<string> activeScenes = new List<string>();
        for (int i = 0; i < scenes.Length; i++)
        {
            if (EditorBuildSettings.scenes[i].enabled)
            {
                activeScenes.Add(EditorBuildSettings.scenes[i].path);
            }
        }
        return activeScenes.ToArray();
    }

    /**
     * returns false if one or more required environment variables are not defined
     * */
    static bool EnvironmentVariablesMissing(string[] envvars)
    {
        string value;
        bool missing = false;
        foreach (string envvar in envvars)
        {
            value = Environment.GetEnvironmentVariable(envvar);
            if (value == null)
            {
                Console.Write("BUILD ERROR: Required Environment Variable is not set: ");
                Console.WriteLine(envvar);
                missing = true;
            }
        }

        return missing;
    }

    /**
     * Main entry point
     * - check if all required environment variables are defined
     * - configure the android build
     * - build the apk (path read from the command line argument)
     */
    [MenuItem("Build/Build Android")]
    public static void BuildAndroid()
    {
        //string[] envvars = new string[]
        //{
        //  "ANDROID_KEYSTORE_NAME", "ANDROID_KEYSTORE_PASSWORD", "ANDROID_KEYALIAS_NAME", "ANDROID_KEYALIAS_PASSWORD", "ANDROID_SDK_ROOT"
        //};
        //if (EnvironmentVariablesMissing(envvars))
        //{
        //    Environment.ExitCode = -1;
        //    return; // note, we can not use Environment.Exit(-1) - the buildprocess will just hang afterwards
        //}

        ////Available Playersettings: https://docs.unity3d.com/ScriptReference/PlayerSettings.Android.html

        ////set the internal apk version to the current unix timestamp, so this increases with every build
        PlayerSettings.Android.bundleVersionCode = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

        //set the other settings from environment variables
        EditorPrefs.SetString("AndroidSdkRoot", Environment.GetEnvironmentVariable("ANDROID_SDK_ROOT"));
        //PlayerSettings.Android.keystoreName = Environment.GetEnvironmentVariable("ANDROID_KEYSTORE_NAME");
        //PlayerSettings.Android.keystorePass = Environment.GetEnvironmentVariable("ANDROID_KEYSTORE_PASSWORD");
        //PlayerSettings.Android.keyaliasName = Environment.GetEnvironmentVariable("ANDROID_KEYALIAS_NAME");
        //PlayerSettings.Android.keyaliasPass = Environment.GetEnvironmentVariable("ANDROID_KEYALIAS_PASSWORD");

        //EditorPrefs.SetString("AndroidSdkRoot", "C:\\Users\\ADMIN\\AppData\\Local\\Android\\sdk");
        //EditorPrefs.SetString("AndroidNdkRoot", "D:\\android-ndk-r16b-windows-x86_64\\android-ndk-r16b");
        //PlayerSettings.Android.keystoreName = "D:\\ZerobitProject\\AthenionTCG\\ATHENION_UNITY\\athenion_keystore.keystore";
        //PlayerSettings.Android.keystorePass = "atnzb-2019feb14";
        //PlayerSettings.Android.keyaliasName = "athenion_keystore.keystore";
        //PlayerSettings.Android.keyaliasPass = "atnzb-2019feb14";

        //Get the apk file to be built from the command line argument
        string outputapk = "";
        if (Application.isBatchMode)
        {
            outputapk = Environment.GetEnvironmentVariable("BUILD_OUTPUT_PATH");
            //outputapk = "C:/Users/Microph/Desktop/test-build/test.apk";
        }
        else
        {
            outputapk = "C:/Users/Microph/Desktop/test-build/test.apk";
        }

        BuildPipeline.BuildPlayer(GetScenePaths(), outputapk, BuildTarget.Android, BuildOptions.None);
    }
}