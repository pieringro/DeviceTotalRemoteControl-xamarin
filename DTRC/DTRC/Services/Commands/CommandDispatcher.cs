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

                matchingCommands.Add(
                    APlayBeepCommand.Id, DependencyService.Get<APlayBeepCommand>());
                matchingCommands.Add(
                    ATakePictureCommand.Id, DependencyService.Get<ATakePictureCommand>());
            }
        }


        private Dictionary<string, ACommand> matchingCommands;


        public bool ExecuteCommand(string commandId) {
            bool result = false;
            if (matchingCommands.ContainsKey(commandId)) {
                ACommand commandToBeExecute = matchingCommands[commandId];
                result = commandToBeExecute.Execute();
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
                result = commandToBeExecute.Execute();
            }
            return result;
        }
    }
}
