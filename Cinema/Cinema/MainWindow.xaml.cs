using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace Cinema
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static bool[] postiLiberi = new bool[31];            //ogni posto lo rappresento con un booleano

        private static int prenotazione1;                    
        private static int prenotazione2;                               ///ho messo static un po dappertutto perchè se non lo mettevo mi dava errore ad esempio 
                                                                        ///nella assegnazione delle prenotazioni

        Thread t1 = new Thread(new ThreadStart(OccupaPostoCassa1));
        Thread t2 = new Thread(new ThreadStart(OccupaPostoCassa2));

        private static object x = new object();

        public MainWindow()
        {
            InitializeComponent();
            Svuota();                                    ///ho messo il metodo per svuotare il cinema nel main in modo tale che all'inizio dell'esecuzione del programma mi inizializza
                                                         ///tutti i posti liberi 
        }

        private void btnSvuotaCinema_Click(object sender, RoutedEventArgs e)
        {
            Svuota();
        }

        private void btnPrenota_Click(object sender, RoutedEventArgs e)
        {
            bool ripartire = true;
            try
            {
                prenotazione1 = int.Parse(txtCassa1.Text);                     //ho fatto -- perchè nell'interfaccia ho assegnato ai sedili i valori a partire da 1 e non da 0
                prenotazione1--;                                                                        
                prenotazione2 = int.Parse(txtCassa2.Text);
                prenotazione2--;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
            if (ripartire == true)
            {
                t1.Start();                           //cercando di non farli stoppare per la seconda prenotazione
                t2.Start();
            }
                                                   //faccio partire i thread appena clicco sul bottone prenota
        }

        static void OccupaPostoCassa1()                             //metodo che prende il posto nella Cassa1 e lo occupa se è libero       
        {
            lock (x)                                                //devo assegnarli solo un posto per volta
            {
                try
                {

                    if (prenotazione1 < 0 || prenotazione1 > 31)
                    {
                        throw new Exception();
                    }
                    else
                    {
                        if (postiLiberi[prenotazione1] == true)
                        {
                            postiLiberi[prenotazione1] = false;
                        }
                        else
                        {
                            throw new Exception("il posto è già occupato, ci dispiace");
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        static void OccupaPostoCassa2()                             //metodo che prende il posto nella Cassa2 e lo occupa se è libero
        {
            lock (x)                                                
            {

                try
                {

                    if (prenotazione2 < 0 || prenotazione2 > 32)
                    {
                        throw new Exception();
                    }
                    else
                    {
                        if (postiLiberi[prenotazione2] == true)
                        {
                            postiLiberi[prenotazione2] = false;
                        }
                        else
                        {
                            throw new Exception("il posto è già occupato, ci dispiace");
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

        }

        void Svuota()
        {
            for (int i = 0; i < postiLiberi.Length; i++)
            {
                postiLiberi[i] = true;                              //metto i posti a true per dire che sono liberi
            }
        }
    }
}
