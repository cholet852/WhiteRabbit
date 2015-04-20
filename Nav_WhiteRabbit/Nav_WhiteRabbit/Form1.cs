using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace Nav_WhiteRabbit
{
    public partial class Form1 : MetroForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        WebBrowser browser = new WebBrowser();
        int i = 0;

        #region LOAD
        //Initialisation au démarrage du navigateur
        private void Form1_Load(object sender, EventArgs e)
        {
            //Initialisation du menu gauche
            metroPanel1.Width = 10;
            metroPanel1.BackColor = Color.Black;

            //Initialisation du menu bas
            metroPanel2.Height = 10;
            metroPanel2.BackColor = Color.Black;
            

            //Creation du navigateur, suppression des erreurs de scripts, style centrer, visible, adresse google
            browser.DocumentCompleted += browser_DocumentCompleted;
           
            //Gestion des onglets, ancrage au 4 coins, onglet vide nommé "New Tab", incremente nombre onglet avec i
            metroTabControl1.TabPages.Add("New Tab");
            metroTabControl1.SelectTab(i);
            metroTabControl1.SelectedTab.Controls.Add(browser);
            metroTabControl1.Dock = DockStyle.Fill;
            i += 1;

            addNewTab();
            
            //Affiche Google par defaut dans la barre d'adresse et valide
            textBoxAddress.Text = "http://www.google.fr";
            buttonGo_Click(null,null);

            //Les boutons precedent, suivant et stop sont grisé au demarage
           // buttonBack.Enabled = false;
           // buttonForward.Enabled = false;
           // buttonStop.Enabled = false;


        }
#endregion

        #region ONGLETS

        private void addNewTab()
        {
            //Creation d'un nouvel objet TabPage
            TabPage tpage = new TabPage();
            
            //Ajoute l'onglet nouvellement créer à la collection de controle d'onglet
            metroTabControl1.TabPages.Insert(metroTabControl1.TabCount - 1, tpage);

            //Creation d'un objet navigateur
            browser = new WebBrowser();
            browser.Navigate("http://www.google.fr");

            //Ajoute le navigateur a l'onglet précedement créer
            tpage.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
            browser.ScriptErrorsSuppressed = true;
            browser.Visible = true;
            metroTabControl1.SelectTab(tpage);

            //Ajoute quelque gestion d'evenement pour l'objet navigateur
            
        }

        //Quand le chargement est fini
        void browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
           metroTabControl1.SelectedTab.Text = ((WebBrowser)metroTabControl1.SelectedTab.Controls[0]).DocumentTitle;textBoxAddress.Text = browser.Url.AbsoluteUri;
           buttonStop.Enabled = false;
           textBoxAddress.Text = browser.Url.ToString();
           progressBar1.Value = 0;
        }


        //Quand le chargement commence
        private void Navigate(string address)
        {

            if (string.IsNullOrEmpty(address)) return;
            if (address.Equals("about:blank")) return;
            if (!address.StartsWith("http://") && !address.StartsWith("https://"))
            {
                address = "http://" + address;
            }
            try
            {
                browser.Navigate(new Uri(address));
            }
            catch (System.UriFormatException)
            {
                return;
            }

            buttonStop.Enabled = true; //On active le bouton stop


            //Si il ya une page suivante alors on active le bouton suivant
            if (browser.CanGoForward)
            {
                buttonForward.Enabled = true;
            }
            else
            {
                buttonForward.Enabled = false;
            }

            //Si il ya une page precedente alors on active le bouton precedent
            if (browser.CanGoBack)
            {
                buttonBack.Enabled = true;
            }
            else
            {
                buttonBack.Enabled = false;
            }

            while (true)
            {
                if (progressBar1.Value >= 100)
                    break;
                progressBar1.Value += 5;
            }
        }

        #endregion

        #region NAVIGATION
        //Boutons de Navigation

        //Bouton de recherche
        private void buttonGo_Click(object sender, EventArgs e)
        {
           // ((WebBrowser)metroTabControl1.SelectedTab.Controls[0]).Navigate(textBoxAddress.Text);
        }


        //Bouton precedent
        private void buttonBack_Click(object sender, EventArgs e)
        {
            ((WebBrowser)metroTabControl1.SelectedTab.Controls[0]).GoBack();
        }


        //Bouton suivant
        private void buttonForward_Click(object sender, EventArgs e)
        {
            ((WebBrowser)metroTabControl1.SelectedTab.Controls[0]).GoForward();
        }


        //Bouton actualiser
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            ((WebBrowser)metroTabControl1.SelectedTab.Controls[0]).Refresh();
        }


        //Bouton stop
        private void buttonStop_Click(object sender, EventArgs e)
        {
            buttonStop.Enabled = false;
            browser.Stop();
            progressBar1.Value = 0;
        }


        //Bouton home
        private void buttonHome_Click(object sender, EventArgs e)
        {
            ((WebBrowser)metroTabControl1.SelectedTab.Controls[0]).Navigate("http://www.google.com");
        }

        private void textBoxAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                ((WebBrowser)metroTabControl1.SelectedTab.Controls[0]).Navigate(textBoxAddress.Text);
            }
        }

        //Ajouter un onglet
        private void button1_Click(object sender, EventArgs e)
        {
            browser = new WebBrowser();
            browser.ScriptErrorsSuppressed = true;
            browser.Dock = DockStyle.Fill;
            browser.Visible = true;
            browser.DocumentCompleted += browser_DocumentCompleted;
            browser.Navigate("http://www.google.fr");
            metroTabControl1.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Right & AnchorStyles.Left;
            metroTabControl1.TabPages.Add("New Tab");
            metroTabControl1.SelectTab(i);
            metroTabControl1.SelectedTab.Controls.Add(browser);
            i += 1;
        }

        //Supprimmer un onglet
        private void button2_Click(object sender, EventArgs e)
        {
            if (metroTabControl1.TabPages.Count - 1 > 0)
            {
                metroTabControl1.TabPages.RemoveAt(metroTabControl1.SelectedIndex);
                metroTabControl1.SelectTab(metroTabControl1.TabPages.Count - 1);
                i -= 1;
            }
        }

        #endregion

       


        #region MENU 
        //Gestion apparition menu gauche
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.metroPanel1.Size.Width >= 80) this.timer1.Enabled = false;
            else this.metroPanel1.Width += 15;
        }

        private void timer2_Tick_1(object sender, EventArgs e)
        {
            if (this.metroPanel1.Size.Width <= 10) this.timer2.Enabled = false;
            else this.metroPanel1.Width -= 15;
        }


        private void metroPanel1_MouseHover(object sender, EventArgs e)
        {
            this.timer2.Enabled = false;
            this.timer1.Enabled = true;
        }

        private void metroPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.X >= 80)
            {
                this.timer1.Enabled = false;
                this.timer2.Enabled = true;
            }
        }


        //Gestion apparition menu bas
        private void timer3_Tick(object sender, EventArgs e)
        {
            if (this.metroPanel2.Size.Height >= 50) this.timer3.Enabled = false;
            else this.metroPanel2.Height += 15;
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            if (this.metroPanel2.Size.Height <= 10) this.timer4.Enabled = false;
            else this.metroPanel2.Height -= 15;
        }

        private void metroPanel2_MouseHover(object sender, EventArgs e)
        {
            this.timer4.Enabled = false;
            this.timer3.Enabled = true;
        }

        private void metroPanel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Y >= 60)
            {
                this.timer3.Enabled = false;
                this.timer4.Enabled = true;
            }
        }

        #endregion

    }
}
