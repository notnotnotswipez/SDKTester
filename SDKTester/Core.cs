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

        public static bool fusionServerQueued = false;

        public static bool logUltEventCalls = false;

        public int timesLoaded = 0;

        public float timer = 5f;
        public bool waitingForAction = false;

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

            Hooking.OnUIRigCreated += () =>
            {
                waitingForAction = true;
                timer = 2f;
            };

            
        }

        public override void OnUpdate()
        {
            if (waitingForAction) {
                timer -= Time.deltaTime;
                if (timer <= 0) {
                    waitingForAction = false;
                    MelonLogger.Msg("Running queued actions.");

                    ActionQueuer.ActionQueuer.RunActions();

                    if (fusionServerQueued)
                    {
                        timesLoaded++;

                        // We either joined or started a server. (Scene was reloaded twice)
                        if (timesLoaded == 2)
                        {
                            ActionQueuer.ActionQueuer.RunSecondaryActions();
                        }
                    }
                    else
                    {
                        ActionQueuer.ActionQueuer.RunSecondaryActions();
                    }
                }
            }
        }

        public void RegisterAllCommands() {
            RegisterCommand<LoadLevelCommand>();
            RegisterCommand<FusionStartServerCommand>();
            RegisterCommand<FusionJoinServerCommand>();
            RegisterCommand<UltEventLogCommand>();
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
                    commandHandlers[command].HandleParameters(spaceSplit);
                }
            }
        }

        public void MakeDefaultFile() {
            using (StreamWriter sw = File.CreateText(launchInstructionsPath))
            {
                sw.WriteLine("# ----------------");
                sw.WriteLine("# Commands available to you:");
                sw.WriteLine("# load_level <barcode> (default means VOID G114)");
                sw.WriteLine("# spawn_object <barcode> <x> <y> <z>");
                sw.WriteLine("# swap_avatar <barcode>");
                sw.WriteLine("# start_fusion_server");
                sw.WriteLine("# join_fusion_server <steamid>");
                sw.WriteLine("# log_ult_events <true/false>");
                sw.WriteLine("# ---------------- LINES STARTING WITH # ARE NOT READ!");
                sw.WriteLine("load_level default");
            }
        }
    }
}