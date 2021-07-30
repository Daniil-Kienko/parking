using CoolParking.BL.Interfaces;

namespace CoolParking.BL.Tests
{
    class FakeLogService : ILogService
    {
        public string LogPath => "";

        public string Read()
        {
            return "";
        }

        public void Write(string logInfo)
        {
        }
    }
}
