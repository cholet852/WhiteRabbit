using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Nav_WhiteRabbit
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        WebBrowser browser = new WebBrowser();
        int i = 0;


        //Initialisation au démarrage du navigateur
        private void Form1_Load(object sender, EventArgs e)
        {

            panel2.Visible = false;

           
               
           


            //Creation du navigateur, suppression des erreurs de scripts, style centrer, visible, adresse google
            browser = new WebBrowser();
            browser.ScriptErrorsSuppressed = true;
            browser.Dock = DockStyle.Fill;
            browser.Visible = true;
            browser.DocumentCompleted += browser_DocumentCompleted;
            browser.Navigate("http://www.google.fr");

            //Gestion des onglets, ancrage au 4 coins, onglet vide nommé "New Tab", incremente nombre onglet avec i
            tabControl1.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Right & AnchorStyles.Left;
            tabControl1.TabPages.Add("New Tab");
            tabControl1.SelectTab(i);
            tabControl1.SelectedTab.Controls.Add(browser);
            i += 1;

            //Affiche Google par defaut dans la barre d'adresse et valide
            textBoxAddress.Text = "http://www.google.fr";
            buttonGo_Click(null,null);

            //Les boutons precedent, suivant et stop sont grisé au demarage
           // buttonBack.Enabled = false;
           // buttonForward.Enabled = false;
           // buttonStop.Enabled = false;

        }


        //Quand le chargement est fini
        void browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            tabControl1.SelectedTab.Text = ((WebBrowser)tabControl1.SelectedTab.Controls[0]).DocumentTitle;

            textBoxAddress.Text = browser.Url.AbsoluteUri;

            label1.Text = browser.StatusText;
            buttonStop.Enabled = false;
            textBoxAddress.Text = browser.Url.ToString();
            label1.Text = "Loaded";
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

            label1.Text = browser.StatusText; //On met le status à jour

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

            label1.Text = "Loading...";
            while (true)
            {
                if (progressBar1.Value >= 100)
                    break;
                progressBar1.Value += 5;
            }
        }


//Boutons de Navigation

        //Bouton de recherche
        private void buttonGo_Click(object sender, EventArgs e)
        {
            ((WebBrowser)tabControl1.SelectedTab.Controls[0]).Navigate(textBoxAddress.Text);
        }


        //Bouton precedent
        private void buttonBack_Click(object sender, EventArgs e)
        {
            ((WebBrowser)tabControl1.SelectedTab.Controls[0]).GoBack();
        }


        //Bouton suivant
        private void buttonForward_Click(object sender, EventArgs e)
        {
            ((WebBrowser)tabControl1.SelectedTab.Controls[0]).GoForward();
        }


        //Bouton actualiser
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            ((WebBrowser)tabControl1.SelectedTab.Controls[0]).Refresh();
        }


        //Bouton stop
        private void buttonStop_Click(object sender, EventArgs e)
        {
            buttonStop.Enabled = false;
            browser.Stop();
            label1.Text = "Stopped";
            progressBar1.Value = 0;
        }


        //Bouton home
        private void buttonHome_Click(object sender, EventArgs e)
        {
            ((WebBrowser)tabControl1.SelectedTab.Controls[0]).Navigate("http://www.google.com");
        }

        private void textBoxAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                ((WebBrowser)tabControl1.SelectedTab.Controls[0]).Navigate(textBoxAddress.Text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            browser = new WebBrowser();
            browser.ScriptErrorsSuppressed = true;
            browser.Dock = DockStyle.Fill;
            browser.Visible = true;
            browser.DocumentCompleted += browser_DocumentCompleted;
            browser.Navigate("http://www.google.fr");
            tabControl1.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Right & AnchorStyles.Left;
            tabControl1.TabPages.Add("New Tab");
            tabControl1.SelectTab(i);
            tabControl1.SelectedTab.Controls.Add(browser);
            i += 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Count - 1 > 0)
            {
                tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);
                tabControl1.SelectTab(tabControl1.TabPages.Count - 1);
                i -= 1;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Y < 2 & (panel2.Visible == false))
            {
                panel2.Visible = true;
                label2.Text = "menu apparition";
                tabControl1.Dock = DockStyle.None;
            }

            if (e.Y > 50 & (panel2.Visible == true))
            {
                panel2.Visible = false;
                label2.Text = "menu disparition";
                tabControl1.Dock = DockStyle.Fill;
            }
        }


    }
}
