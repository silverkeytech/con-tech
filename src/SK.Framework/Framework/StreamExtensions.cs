namespace SK.Framework;

public static class StreamExtensions
{
    public static byte[] ToByteArray(this Stream input)
    {
        input.Seek(0, SeekOrigin.Begin);

        byte[] buffer = new byte[16 * 1024];
        using (var ms = new MemoryStream())
        {
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, read);
            }
            return ms.ToArray();
        }
    }
}
