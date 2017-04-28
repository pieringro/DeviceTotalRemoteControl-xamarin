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
            matchingCommands = new Dictionary<string, ACommand>();
            InitCommandsInstances();
        }

        private void InitCommandsInstances() {
            matchingCommands.Add(
                APlayBeepCommand.Id, DependencyService.Get<APlayBeepCommand>());
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



        public void ExecuteCommandWithParams(string commandId, Dictionary<string, string> paramethers) {

        }
    }
}
