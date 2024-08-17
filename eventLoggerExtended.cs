using System;
using System.IO;
using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Scripting;
using Eplan.EplApi.Base;

public class EventLogger
{
    private const string LOG_FILE_PATH = @"C:\Test\Events.csv";

    [DeclareEventHandler("onActionEnd.String.*")]
    public long MyEventHandlerFunction(IEventParameter iEventParameter)
    {
        try
        {
            EventParameterString oEventParameterString = new EventParameterString(iEventParameter);
            string strActionName = oEventParameterString.String;
            
            // Obtener información adicional
            string userName = Environment.UserName;
            string projectName = GetCurrentProjectName();
            
            
            // Registrar el evento en el archivo
            LogEvent(strActionName, userName, projectName, eplanVersion);           
        }
        catch (Exception exc)
        {
            new BaseException("Error al procesar el evento: " + exc.Message, MessageLevel.Error).FixMessage();
        }

        return 0;
    }

    private void LogEvent(string actionName, string userName, string projectName, string eplanVersion)
    {
        try
        {
            if (!File.Exists(LOG_FILE_PATH))
            {
                
                File.WriteAllText(LOG_FILE_PATH, "Fecha,Hora,Accion,Usuario,Proyecto,VersionEPLAN\n");
            }

            using (StreamWriter sw = File.AppendText(LOG_FILE_PATH))
            {
                string logEntry = $"{DateTime.Now:yyyy-MM-dd},{DateTime.Now:HH:mm:ss},{actionName},{userName},{projectName},{eplanVersion}";
                sw.WriteLine(logEntry);
            }
        }
        catch (Exception exc)
        {
            new BaseException("Error al escribir en el archivo de registro: " + exc.Message, MessageLevel.Error).FixMessage();
        }
    }

    private string GetCurrentProjectName()
    {
        try
        {
            //comand line here
        }
        catch
        {
            return "Sin proyecto";
        }
    }

}
