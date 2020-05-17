using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Prueba
{
    class RespuestaMain {
        public string respuesta { get; set; }
    }
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Application.Current.Properties.Clear();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                constante constante = new constante();
                HttpClient client = new HttpClient();
                var postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("usuario", usuario.Text));
                postData.Add(new KeyValuePair<string, string>("contrasena", contrasena.Text));

                var content = new System.Net.Http.FormUrlEncodedContent(postData);
                var response = await client.PostAsync(constante.getUrl() + "login.php", content);

                if(response.StatusCode == System.Net.HttpStatusCode.OK) {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var respuesta = Newtonsoft.Json.JsonConvert.DeserializeObject<RespuestaMain>(jsonString);
                    if (respuesta.respuesta != "Credenciales no validas")
                    {
                        Application.Current.Properties.Add("usuario", respuesta.respuesta);
                        await Navigation.PushAsync(new Menu());
                    }
                    else resp.Text = respuesta.respuesta;
                }
            }
            catch (Exception ex)
            {
                resp.Text = ex.Message;
            }

        }
    }
}
