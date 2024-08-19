using System;
using System.IO;
using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Scripting;
using Eplan.EplApi.Base;

public class EventLogger
{
    private const string LOG_FILE_PATH = @"C:\ELogger\EventsExtended.csv";
    private static bool isLogging = false;

    [DeclareEventHandler("onActionEnd.String.*")]
    public long MyEventHandlerFunction(IEventParameter iEventParameter)
    {
        if (isLogging) return 0;
        isLogging = true;

        try
        {
            EventParameterString oEventParameterString = new EventParameterString(iEventParameter);
            string strActionName = oEventParameterString.String;

            // Obtener información adicional
            string userName = PathMap.SubstitutePath("$(ENVVAR_USERNAME)");
            string projectName = PathMap.SubstitutePath("$(PROJECTNAME)");

            // Registrar el evento en el archivo
            LogEvent(strActionName, userName, projectName);
        }
        catch (Exception exc)
        {
            BaseException baseExc = new BaseException("Error al procesar el evento: " + exc.Message, MessageLevel.Error);
            baseExc.FixMessage();
        }
        finally
        {
            isLogging = false;
        }

        return 0;
    }

    private void LogEvent(string actionName, string userName, string projectName)
    {
        try
        {
            if (!File.Exists(LOG_FILE_PATH))
            {
                File.WriteAllText(LOG_FILE_PATH, "Fecha,Hora,Accion,Usuario,Proyecto\n");
            }

            // Escapar las comas en los campos
            actionName = actionName.Replace(",", "\\,");
            userName = userName.Replace(",", "\\,");
            projectName = projectName.Replace(",", "\\,");

            using (StreamWriter sw = File.AppendText(LOG_FILE_PATH))
            {
                string logEntry = string.Format("{0},{1},{2},{3},{4}",
                    DateTime.Now.ToString("yyyy-MM-dd"),
                    DateTime.Now.ToString("HH:mm:ss"),
                    actionName,
                    userName,
                    projectName);
                sw.WriteLine(logEntry);
            }
        }
        catch (Exception exc)
        {
            BaseException baseExc = new BaseException("Error al escribir en el archivo de registro: " + exc.Message, MessageLevel.Error);
            baseExc.FixMessage();
        }
    }
}
