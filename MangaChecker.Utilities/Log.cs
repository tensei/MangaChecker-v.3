using System.Reflection;
using log4net;
using log4net.Config;

[assembly: XmlConfigurator(Watch = true)]
namespace MangaChecker.Utilities
{
    public static class Log {
        public static readonly ILog Loggger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static Log() {
        }
    }
}
