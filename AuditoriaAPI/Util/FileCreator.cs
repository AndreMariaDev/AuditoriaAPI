using System;
using System.IO;

namespace AuditoriaAPI.Util
{
    public class FileCreator
    {
        public string sPathLogTxt = @AppDomain.CurrentDomain.BaseDirectory + "\\Log\\" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

        public void FileCreationLog()
        {
            try
            {
                string sDateTime = DateTime.Now.ToString();

                StreamWriter oFileWriter = new StreamWriter(sPathLogTxt, true);

                oFileWriter.WriteLine("\n" + sDateTime);

                oFileWriter.Close();
            }
            catch (IOException ex)
            {
                throw ex;
            }

        }


        public void FileCreationLog(String str, bool success)
        {
            try
            {
                string sDateTime = DateTime.Now.ToString();

                StreamWriter oFileWriter = new StreamWriter(sPathLogTxt, true);

                if (success)
                {
                    oFileWriter.WriteLine("\n" + sDateTime + " SendEmailSuccess: " + str);
                }
                else
                {
                    oFileWriter.WriteLine("\n" + sDateTime + " SendEmailFailed: " + str);
                }


                oFileWriter.Close();

            }
            catch (IOException ex)
            {
                throw ex;
            }

        }

        public void WriteTxtLog(string text)
        {
            try
            {

                StreamWriter oFileWriter = new StreamWriter(this.sPathLogTxt, true);

                oFileWriter.WriteLine("\n" + text);

                oFileWriter.Close();

            }
            catch (IOException ex)
            {
                throw ex;
            }

        }

        public Boolean isExists(string TxtPath)
        {
            return File.Exists(TxtPath);
        }
    }
}