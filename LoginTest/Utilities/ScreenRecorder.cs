using System.Diagnostics;

namespace TestCompa.Utilities 
{
    public class ScreenRecorder
    {
        private Process ffmpegProcess;

        public void StartRecording(string outputFile)
        {
            ffmpegProcess = new Process();
            ffmpegProcess.StartInfo.FileName = "ffmpeg";
            ffmpegProcess.StartInfo.Arguments = $"-y -f gdigrab -framerate 30 -i desktop -c:v libx264 -preset ultrafast -pix_fmt yuv420p \"{outputFile}\"";
            ffmpegProcess.StartInfo.CreateNoWindow = true;
            ffmpegProcess.StartInfo.UseShellExecute = false;
            ffmpegProcess.StartInfo.RedirectStandardError = true;
            ffmpegProcess.Start();
            Console.WriteLine("🟢 Recording started...");
        }

        public void StopRecording()
        {
            if (ffmpegProcess != null && !ffmpegProcess.HasExited)
            {
                ffmpegProcess.Kill();
                Console.WriteLine("🔴 Recording stopped.");
            }
        }
    }
}
