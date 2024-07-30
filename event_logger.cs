using System;
using System.IO;
using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Scripting;
using Eplan.EplApi.Base;

public class EventLogger
{
    private const string LOG_FILE_PATH = @"C:\Test\Events.txt";

    [DeclareEventHandler("onActionEnd.String.*")]
    public long MyEventHandlerFunction(IEventParameter iEventParameter)
    {
        try
        {
            EventParameterString oEventParameterString = new EventParameterString(iEventParameter);
            string strActionName = oEventParameterString.String;

            // Registrar el evento en el archivo
            LogEvent(strActionName);           
        }
        catch (InvalidCastException exc)
        {
            System.Windows.Forms.MessageBox.Show("Parameter error: " + exc.Message, "MyEventHandler");
        }

        return 0;
    }

    private void LogEvent(string actionName)
    {
        try
        {
            using (StreamWriter sw = File.AppendText(LOG_FILE_PATH))
            {
                sw.WriteLine("oCLI.Execute(\"onActionEnd.String.{0}\");", actionName);
            }
        }
        catch (InvalidCastException exc)
        {
            System.Windows.Forms.MessageBox.Show("Parameter error: " + exc.Message, "MyEventHandler");
        }
    }

    
}
