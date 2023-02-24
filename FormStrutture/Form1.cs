using System;
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
        }
        public prodotto[] p;
        public int dim;
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
                p[dim].nome = prodinpt.Text;
                p[dim].prezzo = float.Parse(prezinpt.Text);
                dim++;
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
            listView1.Clear();
            Visualizza(p);
        }
        #endregion

        #region Funzioni servizio
        public string prodString(prodotto p)
        {
            return "Nome: " + p.nome + " prezzo: " + p.prezzo.ToString();
        }
        public void Visualizza(prodotto[] p)
        {
            listView1.Items.Clear();
            for (int i = 0; i < dim; i++)
            {
                listView1.Items.Add(prodString(p[i]));
            }
        }
        public int RicercaS(string e, prodotto[] p)
        {
            int risultatoricerca = 0;
            for (int i = 0; i < p.Length; i++)
            {
                if (p[i].nome == e)
                {
                    risultatoricerca = i;
                    break;
                }
                else
                {
                    risultatoricerca = -1;
                }
            }
            return risultatoricerca;
        }
        public void CancellaS(string e, prodotto[] p, ref int dim)
        {
            var rispCanc = MessageBox.Show("È sicuro di voler eliminare l'elemento?", "Conferma rimozione elemento", MessageBoxButtons.YesNo);
            if (rispCanc == DialogResult.Yes)
            {
                if (RicercaS(prodinpt.Text, p) == -1)
                {
                    MessageBox.Show("Elemento non trovato!", "Errore!");
                }
                else
                {
                    for (int j = 0; j < dim - 1; j++)
                    {
                        p[j] = p[j + 1];
                    }
                    dim--;
                    MessageBox.Show("Elemento eliminato correttamente!");
                }
            }
        }
        #endregion
    }
}
