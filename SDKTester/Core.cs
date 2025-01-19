using BoneLib;
using MelonLoader;
using MelonLoader.Utils;
using SDKTester.Commands;
using SDKTester.Commands.BuiltIn;
using UnityEngine;

[assembly: MelonInfo(typeof(SDKTester.Core), "SDKTester", "1.0.0", "notnotnotswipez", null)]
[assembly: MelonGame("Stress Level Zero", "BONELAB")]
[assembly: MelonOptionalDependencies("LabFusion")]

namespace SDKTester
{
    public class Core : MelonMod
    {
        string launchInstructionsPath = Path.Combine(MelonEnvironment.MelonBaseDirectory, "launchInstructions.txt");

        public static string loadLevelOverrideBarcode = "default";

        public static bool reloadNeedsPrevention = false;

        public static bool logUltEventCalls = false;

        public int timesLoaded = 0;
        public static int timesToLoadUntilSecondaryActionIsCalled = 0;

        private static float timer = 5f;
        private static bool waitingForAction = false;

        private Dictionary<string, GenericCommand> commandHandlers = new Dictionary<string, GenericCommand>();

        public override void OnInitializeMelon()
        {
            RegisterAllCommands();

            if (File.Exists(launchInstructionsPath))
            {
                ParseCommands();
            }
            else {
                MakeDefaultFile();
                ParseCommands();
            }
        }

        public static void StartActionTimer(float time) {
            timer = time;
            waitingForAction = true;
        }

        public override void OnUpdate()
        {
            if (waitingForAction) {
                timer -= Time.deltaTime;
                if (timer <= 0) {
                    waitingForAction = false;

                    ActionQueuer.ActionQueuer.RunActions();

                    if (timesLoaded >= timesToLoadUntilSecondaryActionIsCalled)
                    {
                        ActionQueuer.ActionQueuer.RunSecondaryActions();
                    }
                    else {
                        timesLoaded++;
                    }
                }
            }
        }

        public void RegisterAllCommands() {
            RegisterCommand<LoadLevelCommand>();
            RegisterCommand<UltEventLogCommand>();
            RegisterCommand<SpawnItemCommand>();
            RegisterCommand<SwapAvatarCommand>();

            try
            {
                RegisterCommand<FusionStartServerCommand>();
                RegisterCommand<FusionJoinServerCommand>();
            }
            catch (Exception e) {
                MelonLogger.Error("No Fusion Installed! Skipping registration of Fusion commands.");
            }
        }

        public void RegisterCommand<T>() where T : GenericCommand
        {
            GenericCommand command = (GenericCommand) Activator.CreateInstance(typeof(T));
            commandHandlers.Add(command.Command, command);
        }

        public void ParseCommands() {
            string[] lines = File.ReadAllLines(launchInstructionsPath);

            foreach (string line in lines)
            {
                if (line.StartsWith("#") || line == "") {
                    continue;
                }

                string[] spaceSplit = line.Split(' ');
                string command = spaceSplit[0];

                if (commandHandlers.ContainsKey(command)) {
                    try
                    {
                        commandHandlers[command].HandleParameters(spaceSplit);
                    }
                    catch (Exception e) {
                        MelonLogger.Error("Invalid usage for launch command: " + command);
                    }
                }
            }
        }

        public void MakeDefaultFile() {
            using (StreamWriter sw = File.CreateText(launchInstructionsPath))
            {
                sw.WriteLine("# ----------------");
                sw.WriteLine("# Commands available to you:");
                sw.WriteLine("# https://github.com/notnotnotswipez/SDKTester/wiki/Commands");
                sw.WriteLine("# ---------------- LINES STARTING WITH # ARE NOT READ!");
                sw.WriteLine("load_level default");
            }
        }
    }
}