using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormStrutture
{
    public partial class Form1 : Form
    {
        #region Dichiarazioni
        public struct prodotto
        {
            public string nome;
            public float prezzo;
            public int quantità;
        }
        public prodotto[] p;
        public int dim;
        public float somma;
        public string path = @"ListaProdotti.csv";
        #endregion
        #region Funzioni evento
        public Form1()
        {
            InitializeComponent();
            p = new prodotto[100];
            dim = 0;
        }
        private void agg_Click(object sender, EventArgs e)
        {
            if (dim < p.Length)
            {
                bool esist = false;
                int ps = 0;
                for (int i = 0; i < dim; i++)
                {
                    if (prodinpt.Text == p[i].nome)
                    {
                        esist = true;
                        ps = i;
                    }
                }
                if (!esist)
                {
                    p[dim].nome = prodinpt.Text;
                    p[dim].prezzo = float.Parse(prezinpt.Text);
                    p[dim].quantità = 1;
                    dim++;
                }
                else
                {
                    p[ps].quantità++;
                }
                Visualizza(p);
            }
            else
            {
                MessageBox.Show("Array pieno!", "Errore!");
            }
        }
        private void canc_Click(object sender, EventArgs e)
        {
            CancellaS(prodinpt.Text, p, ref dim);
        }
        private void mod_Click(object sender, EventArgs e)
        {
            Modifica(modprin.Text, float.Parse(modprezzin.Text), p);
        }
        private void modprezzo_Click(object sender, EventArgs e)
        {
            VariazionePrezzo(dim, float.Parse(modprezzoperc.Text));
        }
        private void sommap_Click(object sender, EventArgs e)
        {
            SommaProdotti(dim);
        }
        private void minmax_Click(object sender, EventArgs e)
        {
            RicercaMinMax(ref dim);
        }
        private void recupera_Click(object sender, EventArgs e)
        {
            using (StreamReader sr = File.OpenText(path))
            {
                string s;

                if (!File.Exists(path))
                {
                    MessageBox.Show("Il file non esiste!", "Errore!");
                }
                else
                {
                    while ((s = sr.ReadLine()) != null)
                    {
                        string[] parts = s.Split(';');
                        string nome = parts[0];
                        float prezzo = float.Parse(parts[1]);
                        p[dim].nome = nome;
                        p[dim].prezzo = prezzo;
                        dim++;
                        Visualizza(p);
                    }
                    MessageBox.Show("Lista recuperata correttamente!");
                }
            }
        }
        private void savetofile_Click(object sender, EventArgs e)
        {
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    for (int i = 0; i < dim; i++)
                    {
                        sw.WriteLine($"{p[i].nome}; {p[i].prezzo.ToString("0.00")}");
                    }
                    MessageBox.Show("File creato correttamente!");
                }
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(path, append:false))
                {
                    for (int i = 0; i < dim; i++)
                    {
                        sw.WriteLine($"{p[i].nome}; {p[i].prezzo.ToString("0.00")}");
                    }
                }
            }
        }
        private void ext_Click(object sender, EventArgs e)
        {
            var rispExt = MessageBox.Show("È sicuro di voler terminare l'applicazione?", "Uscita programma", MessageBoxButtons.YesNo);
            if (rispExt == DialogResult.Yes)
            {
                var savefile = MessageBox.Show("Salvare modifiche alla lista?", "Salvataggio lista", MessageBoxButtons.YesNo);
                if (savefile == DialogResult.No)
                {
                    File.Delete(path);
                }
                Application.Exit();
            }
        }
        #endregion
        #region Funzioni servizio
        public string prodString(prodotto p)
        {
            return "Nome: " + p.nome + " prezzo: " + p.prezzo.ToString("0.00") + "€ quantità: " + p.quantità;
        }
        public void Visualizza(prodotto[] p)
        {
            listView1.Items.Clear();
            for (int i = 0; i < dim; i++)
            {
                listView1.Items.Add(prodString(p[i]));
            }
        }
        public int RicercaS(string e, prodotto[] p, int dim)
        {
            int risultatoricerca = -1;
            for (int i = 0; i < dim; i++)
            {
                if (p[i].nome == e)
                {
                    risultatoricerca = i;
                    break;
                }
            }
            return risultatoricerca;
        }
        public void CancellaS(string e, prodotto[] p, ref int dim)
        {
            var rispCanc = MessageBox.Show("È sicuro di voler eliminare l'elemento?", "Conferma rimozione elemento", MessageBoxButtons.YesNo);
            if (rispCanc == DialogResult.Yes)
            {
                int search = RicercaS(prodinpt.Text, p, dim);
                if (search == -1)
                {
                    MessageBox.Show("Elemento non trovato!", "Errore!");
                }
                else
                {
                    for (int j = search; j < dim - 1; j++)
                    {
                        p[j] = p[j + 1];
                    }
                    dim--;
                    Visualizza(p);
                    MessageBox.Show("Elemento eliminato correttamente!");
                }
            }
        }
        public void Modifica(string e, float pos, prodotto[] p)
        {
            int psx = RicercaS(prodinpt.Text, p, dim);
            var rispMod = MessageBox.Show("È sicuro di voler modificare l'elemento?", "Conferma modifica elemento", MessageBoxButtons.YesNo);
            if (rispMod == DialogResult.Yes)
            {
                if (psx == -1)
                {
                    MessageBox.Show("Elemento non trovato!", "Errore!");
                }
                else
                {
                    p[psx].nome = e;
                    p[psx].prezzo = pos;
                    listView1.Clear();
                    Visualizza(p);
                    MessageBox.Show("Elemento modificato correttamente!");
                }
            }
        }
        public void SommaProdotti(int dim)
        {
            somma = 0;
            for (int i = 0; i < dim; i++)
            {
                somma += p[i].prezzo;
            }
            MessageBox.Show($"La somma dei prezzi dei prodotti è di {somma.ToString("0.00")}€", "Somma prodotti");
        }
        public void VariazionePrezzo(int dim, float perc)
        {
            var modPr = MessageBox.Show("È sicuro di voler modificare il prezzo dell'elemento (in base alla percentuale)?", "Conferma modifica prezzo elemento", MessageBoxButtons.YesNo);
            if (modPr == DialogResult.Yes)
            {
                for (int i = 0; i < dim; i++)
                {
                    p[i].prezzo = p[i].prezzo + (p[i].prezzo * perc / 100);
                }
                Visualizza(p);
                MessageBox.Show("Prezzo dell'elemento modificato correttamente!");
            }
        }
        public void RicercaMinMax(ref int dim)
        {
            float min = p[0].prezzo;
            float max = p[0].prezzo;
            for (int i = 1; i < dim; i++)
            {
                if (p[i].prezzo > max)
                {
                    max = p[i].prezzo;
                }
                if (p[i].prezzo < min)
                {
                    min = p[i].prezzo;
                }
            }
            MessageBox.Show($"Il prezzo più basso è di {min.ToString("0.00")}€, mentre quello più alto è di {max.ToString("0.00")}€");
        }
        #endregion
    }
}