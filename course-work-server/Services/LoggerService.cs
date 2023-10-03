

using System;
using System.Text;

namespace course_work_server.Services
{
    public class LoggerService
    {
        public LoggerService() { }

        public void LogError (Exception ex)
        {
            string exception = ex.Message + "\n\n" + ex.Source + "\n\n" + ex.Data + "\n\n" + ex.StackTrace;
            Console.WriteLine(ex);

            string nowTime = DateTime.Now.Day + " " + DateTime.Now.Month + " " + DateTime.Now.Year + " time " + DateTime.Now.Hour + " " + DateTime.Now.Minute + " " + DateTime.Now.Second;
            var fs = File.Create("Logs/" + nowTime + ".txt");
            fs.Write(Encoding.Unicode.GetBytes(exception), 0, Encoding.Unicode.GetByteCount(exception));
            fs.Close();
        }
    }
}
