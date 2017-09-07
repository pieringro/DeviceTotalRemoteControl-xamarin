using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DTRC.Services.Commands
{
    /// <summary>
    /// Decide quale command deve essere eseguito
    /// </summary>
    public class CommandDispatcher
    {
        private static CommandDispatcher instance;
        public static CommandDispatcher getInstance() {
            if (instance == null) {
                instance = new CommandDispatcher();
            }
            return instance;
        }

        private CommandDispatcher() {
            InitCommandsInstances();
        }

        private void InitCommandsInstances() {
            if (matchingCommands == null || matchingCommands.Count == 0) {
                matchingCommands = new Dictionary<string, ACommand>();

//                matchingCommands.Add(
//                    APlayBeepCommand.Id, DependencyService.Get<APlayBeepCommand>());
//                matchingCommands.Add(
//                    ATakePictureCommand.Id, DependencyService.Get<ATakePictureCommand>());
//                matchingCommands.Add(
//                    ARecordAudioCommand.Id, DependencyService.Get<ARecordAudioCommand>());

                matchingCommands.Add(
                    APlayBeepCommand.Id, XLabs.Ioc.Resolver.Resolve<APlayBeepCommand>());
                matchingCommands.Add(
                    ATakePictureCommand.Id, XLabs.Ioc.Resolver.Resolve<ATakePictureCommand>());
                matchingCommands.Add(
                    ARecordAudioCommand.Id, XLabs.Ioc.Resolver.Resolve<ARecordAudioCommand>());
            }
        }


        private Dictionary<string, ACommand> matchingCommands;

        private static ISettings AppSettings => CrossSettings.Current;


        public bool ExecuteCommand(string commandId) {
            bool result = false;
            if (matchingCommands.ContainsKey(commandId)) {
                ACommand commandToBeExecute = matchingCommands[commandId];
                //before execute command, check if it is enable by settings
                bool commandEnable = DTRC.Helpers.Settings.CommandKeyEnabled(commandId);
                if (commandEnable) {
                    result = commandToBeExecute.Execute();
                }
            }
            return result;
        }


        public bool ExecuteCommandWithParams(string commandId, IDictionary<string, string> parameters) {
            bool result = false;
            CommandParameter commandParams = new CommandParameter();
            commandParams.dict = parameters;
            if (matchingCommands.ContainsKey(commandId)) {
                ACommand commandToBeExecute = matchingCommands[commandId];
                commandToBeExecute.SetData(commandParams);
                //before execute command, check if it is enable by settings
                bool commandEnable = DTRC.Helpers.Settings.CommandKeyEnabled(commandId);
                if (commandEnable) {
                    result = commandToBeExecute.Execute();
                }
            }
            return result;
        }
    }
}
