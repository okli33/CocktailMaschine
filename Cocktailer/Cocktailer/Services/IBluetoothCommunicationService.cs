namespace Cocktailer.Services
{
    public interface IBluetoothCommunicationService
    {
        byte[] Read();
        void Write(string Message);
        void Connect();
        bool Connected { get; }
    }
}
