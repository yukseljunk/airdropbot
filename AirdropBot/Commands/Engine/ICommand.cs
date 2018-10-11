using System.Xml;

namespace AirdropBot
{
    public interface ICommand
    {
        string Command(XmlNode node);
        string Tag { get; }
        string Html { get; }
    }
}