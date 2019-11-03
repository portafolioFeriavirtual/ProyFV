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
    /// Interaction logic for ProcesoVenta.xaml
    /// </summary>
    public partial class ProcesoVenta : Page
    {


        ConexionOracle conn = new ConexionOracle();

        public ProcesoVenta()
        {
            InitializeComponent();
        }

        private void ListarClientes()
        {
            OracleConnection ora = conn.Conexion();
            ora.Open();
            OracleCommand comando = new OracleCommand("SP_LISTAR_USUARIOS_ROL", ora);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            comando.Parameters.Add("V_CURSOR", OracleType.Cursor).Direction = System.Data.ParameterDirection.Output;
            comando.Parameters.Add("p_ID_ROL", OracleType.Number).Value = 1;
            comando.Parameters.Add("OUT_GLOSA", OracleType.VarChar, 50).Direction = System.Data.ParameterDirection.Output;
            comando.Parameters.Add("OUT_ESTADO", OracleType.Number, 1).Direction = System.Data.ParameterDirection.Output;

            OracleDataReader dr = comando.ExecuteReader();
            DataTable tabla = new DataTable();
            tabla.Load(dr);

            dgProcesoVenta.ItemsSource = tabla.DefaultView; ;

            ora.Close();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ListarClientes();
        }

        private void limpiar()
        {
            txtRut.Text = "";
            txtNombre.Text = "";
            txtAppat.Text = "";
            txtApmat.Text = "";
            txtPass1.Password = "";
            txtPass2.Password = "";
            btnActualizar.IsEnabled = false;
            btnIngresar.IsEnabled = true;
            btnEliminar.IsEnabled = false; 
        }

        private void BtnIngresar_Click(object sender, RoutedEventArgs e)
        {
            OracleConnection ora = conn.Conexion();
            try
            {
                ora.Open();
                OracleCommand comando = new OracleCommand("SP_INGRESAR_USUARIO", ora);
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Parameters.Add("P_RUT", OracleType.VarChar).Value = txtRut.Text;
                comando.Parameters.Add("p_NOMBRE", OracleType.VarChar).Value = txtNombre.Text;
                comando.Parameters.Add("p_AP_PATERNO", OracleType.VarChar).Value = txtAppat.Text;
                comando.Parameters.Add("p_AP_MATERNO", OracleType.VarChar).Value = txtApmat.Text;
                comando.Parameters.Add("p_CONTRASENIA", OracleType.VarChar).Value = txtPass1.Password.ToString(); // falta validar passwords iguales
                comando.Parameters.Add("p_ESTADO", OracleType.Number).Value = 1;
                comando.Parameters.Add("p_ROL", OracleType.Number).Value = 1;
                comando.Parameters.Add("OUT_GLOSA", OracleType.VarChar, 50).Direction = System.Data.ParameterDirection.Output;
                comando.Parameters.Add("OUT_ESTADO", OracleType.Number, 1).Direction = System.Data.ParameterDirection.Output;
                comando.Parameters.Add("OUT_ID", OracleType.Number, 4).Direction = System.Data.ParameterDirection.Output;

                comando.ExecuteNonQuery();
                MessageBox.Show("Cliente Ingresado.");

            }
            catch (Exception)
            {
                MessageBox.Show("Error al ingresar.");
            }
            

            ListarClientes();
            ora.Close();

        }

        private void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            OracleConnection ora = conn.Conexion();
            try
            {
                ora.Open();
                OracleCommand comando = new OracleCommand("SP_UPDATE_USUARIO", ora);
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Parameters.Add("P_RUT", OracleType.VarChar).Value = txtRut.Text;
                comando.Parameters.Add("p_NOMBRE", OracleType.VarChar).Value = txtNombre.Text;
                comando.Parameters.Add("p_AP_PATERNO", OracleType.VarChar).Value = txtAppat.Text;
                comando.Parameters.Add("p_AP_MATERNO", OracleType.VarChar).Value = txtApmat.Text;
                comando.Parameters.Add("p_CONTRASENIA", OracleType.VarChar).Value = txtPass1.Password.ToString(); // falta validar passwords iguales
                comando.Parameters.Add("p_ESTADO", OracleType.Number).Value = 1;
                comando.Parameters.Add("p_ROL", OracleType.Number).Value = 1;
                comando.Parameters.Add("OUT_GLOSA", OracleType.VarChar, 50).Direction = System.Data.ParameterDirection.Output;
                comando.Parameters.Add("OUT_ESTADO", OracleType.Number, 1).Direction = System.Data.ParameterDirection.Output;

                comando.ExecuteNonQuery();
                MessageBox.Show("Cliente Actualizado.");
                limpiar();
                
            }
            catch (Exception)
            {
                MessageBox.Show("Error al actualizar.");
            }


            ListarClientes();
            ora.Close();
        }

        private void DgProcesoVenta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                txtRut.Text = dr["RUT_USUARIO"].ToString();
                txtNombre.Text = dr["NOMBRE_USUARIO"].ToString();
                txtAppat.Text = dr["AP_PATERNO"].ToString();
                txtApmat.Text = dr["AP_MATERNO"].ToString();
                btnActualizar.IsEnabled = true;
                btnIngresar.IsEnabled = false;
                btnEliminar.IsEnabled = true; 
            }
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            limpiar();
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            OracleConnection ora = conn.Conexion();
            try
            {
                ora.Open();
                OracleCommand comando = new OracleCommand("SP_DELETE_USUARIO", ora);
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Parameters.Add("P_RUT", OracleType.VarChar).Value = txtRut.Text;
                comando.Parameters.Add("OUT_GLOSA", OracleType.VarChar, 50).Direction = System.Data.ParameterDirection.Output;
                comando.Parameters.Add("OUT_ESTADO", OracleType.Number, 1).Direction = System.Data.ParameterDirection.Output;

                comando.ExecuteNonQuery();
                MessageBox.Show("Cliente Eliminado.");
                limpiar();

            }
            catch (Exception)
            {
                MessageBox.Show("Error al eliminar.");
            }

            ListarClientes();
            ora.Close();
        }
    }
}
