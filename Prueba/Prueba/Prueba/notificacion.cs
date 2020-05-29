using System;
using System.Collections.Generic;
using System.Text;
using Plugin.LocalNotifications;

namespace Prueba
{
    class notificacion
    {
        public void notificar(string title,string  body, int id)
        {
            DateTime thisDay = DateTime.Today;
            Plugin.LocalNotifications.CrossLocalNotifications.Current.Show(title, body, id);
        }
    }
}
