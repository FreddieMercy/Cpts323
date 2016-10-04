using System.Runtime.InteropServices;

namespace MapRSS_LogicEngine
{
    public static class InternetAvailability
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);

        public static bool IsAvailable()
        {
            int description;
            return InternetGetConnectedState(out description, 0);
        }
    }
}
