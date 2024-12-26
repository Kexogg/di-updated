namespace TagsCloudContainerCore.Facade;

public interface ITagCloud
{
    byte[] FromFile(string path);
    byte[] FromString(string data);
    byte[] FromBytes(byte[] data);
}