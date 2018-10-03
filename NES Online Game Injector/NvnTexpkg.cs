using System.Diagnostics;

namespace NES_Online_Game_Injector
{
    class Nvntexpkg
    {
        Process Nconvert = new Process();

        public Nvntexpkg()
        {
            Nconvert.StartInfo.FileName = "NvnTexpkg.exe";
            Nconvert.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        }

        public void RunCommand(string Args)
        {
            Nconvert.StartInfo.Arguments = Args;
            Nconvert.Start();
            Nconvert.WaitForExit();
        }
    }
}
