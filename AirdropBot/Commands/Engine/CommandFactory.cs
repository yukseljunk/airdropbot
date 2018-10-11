using System.Collections.Generic;
using System.Linq;
using CefSharp.WinForms;

namespace AirdropBot
{
    public class CommandFactory
    {
        private List<ICommand> _commands = new List<ICommand>()
                                              {
                                                  new DatabaseCommand(),
                                                  new WaitForFileCommand(),
                                                  new WriteToFileCommand(),
                                                  new ReadFile(),
                                                  new DeleteFile(),
                                                  new SetParam(),
                                                  new ReadHtml(),
                                                  new GetFromHtml(),
                                                  new Google2FaCommand(),
                                                  new DeleteElement(),
                                                  new SetAttribute(),
                                                  new DeleteCookies(),
                                                  new ScrollElement(),
                                                  new FocusElement()
                                              };

        public ICommand GetCommand(string tag)
        {
            var command= _commands.FirstOrDefault(c => c.Tag == tag);
            if (command is IBrowserManipulator)
            {
                ((IBrowserManipulator) command).Browser = CBrowser;
            }
            return command;
        }

        public ChromiumWebBrowser CBrowser { get; set; }
    }
}
