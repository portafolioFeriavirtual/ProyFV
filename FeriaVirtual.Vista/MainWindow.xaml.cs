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
using FeriaVirtual.Negocio;
using System.Data.OracleClient;
using System.Data;

namespace FeriaVirtual.Vista
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnClientes_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Clientes();
        }
        
        private void BtnTransportistas_Click(object sender, RoutedEventArgs e)
        {
            //Main.Content = new Transportistas();
        }

        private void BtnProductores_Click(object sender, RoutedEventArgs e)
        {
           // Main.Content = new Productores();
        }

        private void BtnComerciantes_Click(object sender, RoutedEventArgs e)
        {
            //Main.Content = new Comerciantes();
        }

        private void BtnConsultores_Click(object sender, RoutedEventArgs e)
        {
            //Main.Content = new Consultores();
        }

        private void BtnContratos_Click(object sender, RoutedEventArgs e)
        {
           // Main.Content = new Contratos();
        }

        /*private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ConexionOracle conn = new ConexionOracle();
            OracleConnection ora = conn.Conexion();

            ora.Open();
            OracleCommand comando = new OracleCommand("SP_LISTAR_USUARIOS",ora);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            comando.Parameters.Add("V_CURSOR", OracleType.Cursor).Direction =System.Data.ParameterDirection.Output;
            comando.Parameters.Add("OUT_GLOSA", OracleType.VarChar,50).Direction = System.Data.ParameterDirection.Output;
            comando.Parameters.Add("OUT_ESTADO", OracleType.Number,1).Direction = System.Data.ParameterDirection.Output;

            OracleDataReader dr = comando.ExecuteReader();
            DataTable tabla = new DataTable();
            tabla.Load(dr);

            dgClientes.ItemsSource = tabla.DefaultView; ;
        }*/
    }
}
