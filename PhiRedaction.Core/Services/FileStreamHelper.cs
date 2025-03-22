using System.Text;

namespace PhiRedaction.Core.Services
{
    public static class FileStreamHelper
    {
        // Read a line from the file until newline or buffer limit is reached
        public static string ReadLineWithBufferLimit(FileStream fs, int maxLineSize)
        {
            var lineBuffer = new List<byte>();
            int totalBytesRead = 0;

            byte[] newlineBytes = Encoding.UTF8.GetBytes(Environment.NewLine);
            int newlineIndex = 0; // Tracks progress through newline sequence

            while (true)
            {
                int byteRead = fs.ReadByte();
                if (byteRead == -1)  // End of file
                    break;

                byte b = (byte)byteRead;
                totalBytesRead++;

                // Check for newline sequence
                if (b == newlineBytes[newlineIndex])
                {
                    newlineIndex++;

                    if (newlineIndex == newlineBytes.Length) // Full newline matched
                    {
                        // Append newline bytes to the lineBuffer
                        lineBuffer.AddRange(newlineBytes);
                        break;
                    }
                }
                else
                {
                    // Reset newline detection if interrupted
                    if (newlineIndex > 0)
                    {
                        lineBuffer.AddRange(newlineBytes[..newlineIndex]); // Add partially matched newline bytes
                        newlineIndex = 0;
                    }
                    lineBuffer.Add(b);
                }

                if (totalBytesRead >= maxLineSize)
                    break;
            }

            return Encoding.UTF8.GetString(lineBuffer.ToArray());
        }
    }
}
