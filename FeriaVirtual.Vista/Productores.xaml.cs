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
    /// Interaction logic for Productores.xaml
    /// </summary>
    public partial class Productores : Page
    {


        ConexionOracle conn = new ConexionOracle();
      
        public Productores()
        {
            InitializeComponent();
        }

        private void ListarProductores()
        {
            OracleConnection ora = conn.Conexion();
            ora.Open();
            OracleCommand comando = new OracleCommand("SP_LISTAR_PRODUCTORES", ora);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            comando.Parameters.Add("V_CURSOR", OracleType.Cursor).Direction = System.Data.ParameterDirection.Output;
            comando.Parameters.Add("OUT_GLOSA", OracleType.VarChar, 50).Direction = System.Data.ParameterDirection.Output;
            comando.Parameters.Add("OUT_ESTADO", OracleType.Number, 1).Direction = System.Data.ParameterDirection.Output;

            OracleDataReader dr = comando.ExecuteReader();
            DataTable tabla = new DataTable();
            tabla.Load(dr);

            dgProductores.ItemsSource = tabla.DefaultView; ;

            ora.Close();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ListarProductores();
        }

        private bool validar()
        {
            bool valido = true;
            errRut.Content = "";
            errNombreUsuario.Content = "";
            errNombreProductor.Content = "";
            errPass.Content = "";


            if (Usuario.validaRut(txtRut.Text) == false)
            {
                valido = false;
                errRut.Content = "Rut no válido";
            }
            if (String.IsNullOrEmpty(txtRut.Text))
            {
                valido = false;
                errRut.Content = "Campo no puede estar vacío";
            }
            if (String.IsNullOrEmpty(txtNombreUsuario.Text))
            {
                valido = false;
                errNombreUsuario.Content = "Campo no puede estar vacío";
            }

            if (String.IsNullOrEmpty(txtNombreProductor.Text))
            {
                valido = false;
                errNombreProductor.Content = "Campo no puede estar vacío";
            }
   

            if (String.IsNullOrEmpty(txtPass1.Password.ToString()) || String.IsNullOrEmpty(txtPass2.Password.ToString()))
            {
                valido = false;
                errPass.Content = "Debe llenar ambos campos";
            }

            if (!(txtPass1.Password.ToString().Equals(txtPass2.Password.ToString())))
            {
                valido = false;
                errPass.Content = "Contraseñas no coinciden";
            }

            return valido;
        }


        private void limpiar()
        {
            txtRut.Text = "";
            txtNombreUsuario.Text = "";
            txtNombreProductor.Text = "";
            txtPass1.Password = "";
            txtPass2.Password = "";
            btnActualizar.IsEnabled = false;
            btnIngresar.IsEnabled = true;
            btnEliminar.IsEnabled = false; 
        }

        private void BtnIngresar_Click(object sender, RoutedEventArgs e)
        {
            bool valido;

            valido = validar();

            if (valido == true)
            {
                OracleConnection ora = conn.Conexion();
                try
                {
                    errRut.Content = "";
                    errNombreUsuario.Content = "";
                    errNombreProductor.Content = "";
                    errPass.Content = "";

                    ora.Open();
                    OracleCommand comando = new OracleCommand("SP_INGRESAR_USUARIO", ora);
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.Add("P_RUT", OracleType.VarChar).Value = txtRut.Text;
                    comando.Parameters.Add("p_NOMBRE", OracleType.VarChar).Value = txtNombreUsuario.Text;
                    comando.Parameters.Add("p_AP_PATERNO", OracleType.VarChar).Value = " ";
                    comando.Parameters.Add("p_AP_MATERNO", OracleType.VarChar).Value = " ";
                    comando.Parameters.Add("p_CONTRASENIA", OracleType.VarChar).Value = BCrypt.Net.BCrypt.HashPassword(txtPass1.Password.ToString());
                    comando.Parameters.Add("p_ESTADO", OracleType.Number).Value = 1;
                    comando.Parameters.Add("p_ROL", OracleType.Number).Value = 3;
                    comando.Parameters.Add("OUT_GLOSA", OracleType.VarChar, 50).Direction = System.Data.ParameterDirection.Output;
                    comando.Parameters.Add("OUT_ESTADO", OracleType.Number, 1).Direction = System.Data.ParameterDirection.Output;
                    comando.Parameters.Add("OUT_ID", OracleType.Number, 4).Direction = System.Data.ParameterDirection.Output;


                    comando.ExecuteNonQuery();
                    int userID = Convert.ToInt32(comando.Parameters["OUT_ID"].Value);
                    comando.Parameters.Clear();

                    OracleCommand comando2 = new OracleCommand("SP_INGRESAR_PRODUCTOR", ora);
                    comando2.CommandType = System.Data.CommandType.StoredProcedure;
                    comando2.Parameters.Add("P_ID_USUARIO", OracleType.Number).Value = userID;
                    comando2.Parameters.Add("P_NOMBRE_PRODUCTOR", OracleType.VarChar).Value = txtNombreProductor.Text;
                    comando2.Parameters.Add("p_ESTADO_PRODUCTOR", OracleType.Number).Value = 1;
                    comando2.Parameters.Add("OUT_GLOSA", OracleType.VarChar, 50).Direction = System.Data.ParameterDirection.Output;
                    comando2.Parameters.Add("OUT_ESTADO", OracleType.Number, 1).Direction = System.Data.ParameterDirection.Output;


                    comando2.ExecuteNonQuery();
                    MessageBox.Show("Productor Ingresado.");

                }
                catch (Exception)
                {
                    MessageBox.Show("Error al ingresar.");
                }


                ListarProductores();
                ora.Close();
            }
        }

        private void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            bool valido;

            valido = validar();

            if (valido == true)
            {

                OracleConnection ora = conn.Conexion();
                try
                {
                    errRut.Content = "";
                    errNombreUsuario.Content = "";
                    errNombreProductor.Content = "";
                    errPass.Content = "";

                    ora.Open();
                    OracleCommand comando2 = new OracleCommand("SP_UPDATE_PRODUCTOR", ora);
                    comando2.CommandType = System.Data.CommandType.StoredProcedure;
                    comando2.Parameters.Add("P_ID_PRODUCTOR", OracleType.Number, 4).Value = Convert.ToInt32(txtProductorId.Text);
                    comando2.Parameters.Add("P_NOMBRE_PRODUCTOR", OracleType.VarChar).Value = txtNombreProductor.Text;
                    comando2.Parameters.Add("p_ESTADO_PRODUCTOR", OracleType.Number).Value = 1;
                    comando2.Parameters.Add("OUT_GLOSA", OracleType.VarChar, 200).Direction = System.Data.ParameterDirection.Output;
                    comando2.Parameters.Add("OUT_ESTADO", OracleType.Number, 1).Direction = System.Data.ParameterDirection.Output;

                    comando2.ExecuteNonQuery();


                    OracleCommand comando = new OracleCommand("SP_UPDATE_USUARIO", ora);
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.Add("P_RUT", OracleType.VarChar).Value = txtRut.Text;
                    comando.Parameters.Add("P_NOMBRE", OracleType.VarChar).Value = txtNombreUsuario.Text;
                    comando.Parameters.Add("p_AP_PATERNO", OracleType.VarChar).Value = " ";
                    comando.Parameters.Add("p_AP_MATERNO", OracleType.VarChar).Value = " ";
                    comando.Parameters.Add("p_CONTRASENIA", OracleType.VarChar).Value = BCrypt.Net.BCrypt.HashPassword(txtPass1.Password.ToString());
                    comando.Parameters.Add("p_ESTADO", OracleType.Number).Value = 1;
                    comando.Parameters.Add("p_ROL", OracleType.Number).Value = 3;
                    comando.Parameters.Add("OUT_GLOSA", OracleType.VarChar, 200).Direction = System.Data.ParameterDirection.Output;
                    comando.Parameters.Add("OUT_ESTADO", OracleType.Number, 1).Direction = System.Data.ParameterDirection.Output;

                    comando.ExecuteNonQuery();
                    MessageBox.Show("Productor Actualizado.");
                    limpiar();

                }
                catch (Exception)
                {
                    MessageBox.Show("Error al actualizar.");
                }


                ListarProductores();
                ora.Close();
            }
        }

        private void DgProductores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                txtRut.Text = dr["RUT_USUARIO"].ToString();
                txtNombreUsuario.Text = dr["NOMBRE_USUARIO"].ToString();
                txtNombreProductor.Text = dr["NOMBRE_PRODUCTOR"].ToString();
                txtProductorId.Text = dr["ID_PRODUCTOR"].ToString();
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
            
                OracleCommand comando2 = new OracleCommand("SP_DELETE_PRODUCTOR", ora);
                comando2.CommandType = System.Data.CommandType.StoredProcedure;
                comando2.Parameters.Add("P_ID_PRODUCTOR", OracleType.Number,4).Value = Convert.ToInt32(txtProductorId.Text);
                comando2.Parameters.Add("OUT_GLOSA", OracleType.VarChar, 50).Direction = System.Data.ParameterDirection.Output;
                comando2.Parameters.Add("OUT_ESTADO", OracleType.Number, 1).Direction = System.Data.ParameterDirection.Output;

                comando2.ExecuteNonQuery();

                OracleCommand comando = new OracleCommand("SP_DELETE_USUARIO", ora);
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Parameters.Add("P_RUT", OracleType.VarChar).Value = txtRut.Text;
                comando.Parameters.Add("OUT_GLOSA", OracleType.VarChar, 50).Direction = System.Data.ParameterDirection.Output;
                comando.Parameters.Add("OUT_ESTADO", OracleType.Number, 1).Direction = System.Data.ParameterDirection.Output;

                comando.ExecuteNonQuery();
                MessageBox.Show("Productor Eliminado.");
                limpiar();

            }
            catch (Exception)
            {
                MessageBox.Show("Error al eliminar.");
            }

            ListarProductores();
            ora.Close();
        }
    }
}
