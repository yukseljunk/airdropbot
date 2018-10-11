using System.Data.SqlClient;
using System.Xml;

namespace AirdropBot
{
    public class DatabaseCommand : ICommand
    {
        #region Implementation of ICommand

        public string Command(XmlNode node)
        {
            // "<database  command=\"select * from WashTrade where id=(select max(id) from WashTrade)\" variable=\"singlerow\" server=\"YUKSELDELL\\SQLEXPRESS\" database=\"airdropbot\" />";
            var server = node.Attributes["server"];
            if (server == null) return "server not defined";
            if (string.IsNullOrEmpty(server.Value)) return "server cannot be empty!";


            var database = node.Attributes["database"];
            if (database == null) return "database not defined";
            if (string.IsNullOrEmpty(database.Value)) return "database cannot be empty!";

            var command = node.Attributes["command"];
            if (command == null) return "command not defined";
            if (string.IsNullOrEmpty(command.Value)) return "command cannot be empty!";

            var variableToAssign = "";
            var variable = node.Attributes["variable"];
            if (variable != null && variable.Value != "")
            {
                variableToAssign = variable.Value;
            }

            SqlConnection myConnection = new SqlConnection("server=" + server.Value +
                                       ";Trusted_Connection=true;Integrated Security=true;" +
                                       "database=" + database.Value +
                                       ";connection timeout=30");


            return "";
        }

        public string Tag
        {
            get { return "database"; }
        }

        public string Html
        {
            get
            {
                return "<database  command=\"select * from WashTrade where id=(select max(id) from WashTrade)\" variable=\"singlerow\" server=\"YUKSELDELL\\SQLEXPRESS\" database=\"airdropbot\" />";
            }
        }
        #endregion
    }
}