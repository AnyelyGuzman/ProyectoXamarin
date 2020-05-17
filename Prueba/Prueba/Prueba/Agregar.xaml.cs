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
    class RespuestaHorario
    {
        public List<SalonHorario> salones { get; set; }
        public List<HoraHorario> horas { get; set; }
        public List<DocenteHorario> docentes { get; set; }
        public List<MateriaHorario> materias { get; set; }
        public List<SemestreHorario> semestres { get; set; }
        public List<DiaHorario> dias { get; set; }

    }

    class SalonHorario
    {
        public int id_Salon { get; set; }
        public string bloque { get; set; }
        public int salon { get; set; }
    }

    class HoraHorario
    {
         public int id_hora { get; set; }
        public string hora { get; set; }
    }

    class DocenteHorario
    {
         public int id_profesor { get; set; } 
         public string nombre { get; set; }
        public string apellido { get; set; }
    }

    class MateriaHorario
    {
        public int id_materia { get; set; }
        public string nombre { get; set; }
    }

    class SemestreHorario
    {
        public int id_Semestre { get; set; }
        public string Semestre { get; set; }
    }

    class DiaHorario
    {
        public int id_dia { get; set; }
        public string dia { get; set; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Agregar : ContentPage
    {
        private RespuestaHorario respuesta;
        private int salonSelect;
        private int horaSelect;
        private int docenteSelect;
        private int materiaSelect;
        private int semestreSelect;
        private int diaSelect;


        public Agregar()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            try
            {
                constante constante = new constante();
                HttpClient client = new HttpClient();
                var response = await client.GetAsync(constante.getUrl() + "horario.php?opcion=consulta");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    respuesta = Newtonsoft.Json.JsonConvert.DeserializeObject<RespuestaHorario>(jsonString);
                    foreach (SemestreHorario element in respuesta.semestres)
                    {
                        semestre.Items.Add(element.Semestre);
                    }

                    foreach (MateriaHorario element in respuesta.materias)
                    {
                        materia.Items.Add(element.nombre);
                    }

                    foreach (SalonHorario element in respuesta.salones)
                    {
                        salon.Items.Add(element.bloque + " " + element.salon.ToString());
                    }

                    foreach (HoraHorario element in respuesta.horas)
                    {
                        hora.Items.Add(element.hora);
                    }

                    foreach (DocenteHorario element in respuesta.docentes)
                    {
                        docente.Items.Add(element.nombre.Trim() + " " + element.apellido.Trim());
                    }

                    foreach (DiaHorario element in respuesta.dias)
                    {
                        diaa.Items.Add(element.dia);
                    }
                }
            }
            catch (Exception ex)
            {
                title.Text = ex.Message;
            }
            base.OnAppearing();
        }

        private void semestreSelected(object sender, EventArgs e)
        {
            var getSemestreSelect = semestre.Items[semestre.SelectedIndex];
            SemestreHorario resp = respuesta.semestres.Find(x => x.Semestre.Contains(getSemestreSelect));
            semestreSelect = resp.id_Semestre;
        }

        private void materiaSelected(object sender, EventArgs e)
        {
            var getMateriaSelect = materia.Items[materia.SelectedIndex];
            MateriaHorario resp = respuesta.materias.Find(x => x.nombre.Contains(getMateriaSelect));
            materiaSelect = resp.id_materia;
        }

        private void docenteSelected(object sender, EventArgs e)
        {
            var getDocenteSelect = docente.Items[docente.SelectedIndex];
            String[] docenteSplit = getDocenteSelect.Split(' ');
            String nomre = "";
            String apellido = "";
                        
            if(docenteSplit.Length == 2)
            {
                nomre = docenteSplit[0];
                apellido = docenteSplit[1];
            }else if(docenteSplit.Length == 3)
            {
                nomre = docenteSplit[0] + " " + docenteSplit[1];
                apellido = docenteSplit[2];
            }
            
            DocenteHorario resp = respuesta.docentes.Find(x => x.nombre.Contains(nomre) && x.apellido.Contains(apellido));
            docenteSelect = resp.id_profesor;
        }

        private void horaSelected(object sender, EventArgs e)
        {
            var getHoraSelect = hora.Items[hora.SelectedIndex];
            HoraHorario resp = respuesta.horas.Find(x => x.hora.Contains(getHoraSelect));
            horaSelect = resp.id_hora;
        }

        private void salonSelected(object sender, EventArgs e)
        {
            var getSalonSelect = salon.Items[salon.SelectedIndex];
            String[] salonSplit = getSalonSelect.Split(' ');
            String bloque = salonSplit[0];
            String salonStr = salonSplit[1];
            SalonHorario resp = respuesta.salones.Find(x => x.bloque.Contains(bloque) && x.salon.ToString().Contains(salonStr));
            salonSelect = resp.id_Salon;
        }

        private void diaSelected(object sender, EventArgs e)
        {
            var getdiaSelect = diaa.Items[diaa.SelectedIndex];
            DiaHorario resp = respuesta.dias.Find(x => x.dia.Contains(getdiaSelect));
            diaSelect = resp.id_dia;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                constante constante = new constante();
                HttpClient client = new HttpClient();
                var postData = new List<KeyValuePair<string, string>>();

                postData.Add(new KeyValuePair<string, string>("idSalon", salonSelect.ToString()));
                postData.Add(new KeyValuePair<string, string>("idHora", horaSelect.ToString()));
                postData.Add(new KeyValuePair<string, string>("idMateria", materiaSelect.ToString()));
                postData.Add(new KeyValuePair<string, string>("idDocente", docenteSelect.ToString()));
                postData.Add(new KeyValuePair<string, string>("idSemestre", semestreSelect.ToString()));
                postData.Add(new KeyValuePair<string, string>("idDia", diaSelect.ToString()));
                postData.Add(new KeyValuePair<string, string>("idUsuario", Application.Current.Properties["usuario"] as string));

                var content = new System.Net.Http.FormUrlEncodedContent(postData);
                var response = await client.PostAsync(constante.getUrl() + "horario.php", content);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var respuesta = Newtonsoft.Json.JsonConvert.DeserializeObject<RespuestaMain>(jsonString);
                    resp.Text = respuesta.respuesta;
                }
            }
            catch (Exception ex)
            {
                resp.Text = ex.Message;
            }
        }
    }
}