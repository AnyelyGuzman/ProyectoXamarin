using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Prueba
{
    class RespuestaVerHorario
    {
        public List<DiaVerHorario> horarios { get; set; }
    }

    class DiaVerHorario
    {
        public int id_dia { get; set; }
        public string dia { get; set; }
        public int id_Semestre { get; set; }
        public int id_Materia { get; set; }
        public int id_profesor { get; set; }
        public int id_Salon { get; set; }
        public int id_Hora { get; set; }
        public int id_usuario { get; set; }
        public string Hora { get; set; }
        public string Nombre { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string bloque { get; set; }
        public string salon { get; set; }
        public string Semestre { get; set; }

    }

    class ListRender
    {
        public string Hora { get; set; }
        
        public string Nombre { get; set; }
        
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string bloque { get; set; }
        public string salon { get; set; }
        public string Semestre { get; set; }
    }

    class DiasVerHorario
    {
        public int id_dia { get; set; }

        public string dia { get; set; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Ver : ContentPage
    {
        private RespuestaHorario respuesta;
        private List<DiasVerHorario> respuestaD;
        private List<DiaVerHorario> respuestaDi;
        private int diaSelect;


        public Ver()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            try
            {
                constante constante = new constante();
                HttpClient client = new HttpClient();
                var response = await client.GetAsync(constante.getUrl() + "horario.php?opcion=dias");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    respuestaD = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DiasVerHorario>>(jsonString);
                    foreach (DiasVerHorario element in respuestaD)
                    {
                        dia.Items.Add(element.dia);
                    }
                }
            }
            catch (Exception ex)
            {
                title.Text = ex.Message;
            }
            base.OnAppearing();
        }

        private async void diaSelected(object sender, EventArgs e)
        {
            notificacion objNotificacion = new notificacion();
            objNotificacion.notificar("Notificacion", "Clases", 0);
            
            try
            {
                constante constante = new constante();
                HttpClient client = new HttpClient(); 
                var postData = new List<KeyValuePair<string, string>>();
                var getdiaSelect = dia.Items[dia.SelectedIndex];

                postData.Add(new KeyValuePair<string, string>("dia", getdiaSelect));
                postData.Add(new KeyValuePair<string, string>("id_usuario", Application.Current.Properties["usuario"] as string));

                var content = new System.Net.Http.FormUrlEncodedContent(postData);
                var response = await client.PostAsync(constante.getUrl() + "horario.php", content);
                

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    respuestaDi = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DiaVerHorario>>(jsonString);


                    List<ListRender> Dias = new List<ListRender>();
                    foreach (DiaVerHorario element in respuestaDi)
                    {
                        Dias.Add(new ListRender{
                            Hora = "Hora: " + element.Hora,
                            Nombre = "Materia: " + element.Nombre,
                            nombre = "Profesor: " + element.nombre + " " + element.apellido,
                            apellido = element.apellido,
                            bloque = "Salon: " + element.bloque + " " + element.salon,
                            salon = element.salon,
                            Semestre = "Semestre: " + element.Semestre
                        });
                    }
                    list.ItemsSource = Dias;
                }
            }
            catch (Exception ex)
            {
                title.Text = ex.Message;
            }
            base.OnAppearing();
        }

    }
}