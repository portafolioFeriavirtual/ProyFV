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
    /// Interaction logic for Contratos.xaml
    /// </summary>
    public partial class Contratos : Page
    {


        ConexionOracle conn = new ConexionOracle();

        public Contratos()
        {
            InitializeComponent();
        }

        private void ListarContratos()
        {
            OracleConnection ora = conn.Conexion();
            ora.Open();
            OracleCommand comando = new OracleCommand("SP_LISTAR_CONTRATOS", ora);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            comando.Parameters.Add("V_CURSOR", OracleType.Cursor).Direction = System.Data.ParameterDirection.Output;
            comando.Parameters.Add("OUT_GLOSA", OracleType.VarChar, 50).Direction = System.Data.ParameterDirection.Output;
            comando.Parameters.Add("OUT_ESTADO", OracleType.Number, 1).Direction = System.Data.ParameterDirection.Output;

            OracleDataReader dr = comando.ExecuteReader();
            DataTable tabla = new DataTable();
            tabla.Load(dr);

            dgContratos.ItemsSource = tabla.DefaultView; ;

            ora.Close();
        }

        private void ListarProductoresCB()
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

            cbProductor.ItemsSource = tabla.DefaultView;
            cbProductor.SelectedValuePath = "ID_PRODUCTOR";
            cbProductor.DisplayMemberPath = "NOMBRE_PRODUCTOR";

            ora.Close();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ListarContratos();
            ListarProductoresCB();
        }

        private void limpiar()
        {
            cbProductor.SelectedValue = null;
            dpInicio.SelectedDate = null;
            dpFin.SelectedDate = null;
            btnActualizar.IsEnabled = false;
            btnIngresar.IsEnabled = true;
        }

        private void BtnIngresar_Click(object sender, RoutedEventArgs e)
        {
            OracleConnection ora = conn.Conexion();
            /*try
            {*/
                ora.Open();
                OracleCommand comando = new OracleCommand("SP_INGRESAR_CONTRATO", ora);
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Parameters.Add("P_FECHA_INICIO", OracleType.DateTime).Value = dpInicio.SelectedDate;
                comando.Parameters.Add("P_FECHA_TERMINO", OracleType.DateTime).Value = dpFin.SelectedDate;
                comando.Parameters.Add("P_ID_PRODUCTOR", OracleType.Number).Value = cbProductor.SelectedValue;
                comando.Parameters.Add("OUT_GLOSA", OracleType.VarChar, 50).Direction = System.Data.ParameterDirection.Output;
                comando.Parameters.Add("OUT_ESTADO", OracleType.Number, 1).Direction = System.Data.ParameterDirection.Output;

                comando.ExecuteNonQuery();
                MessageBox.Show("Contrato Ingresado.");

            /*}
            catch (Exception)
            {
                MessageBox.Show("Error al ingresar.");
            }*/


            ListarContratos();
            ora.Close();

        }

        private void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            OracleConnection ora = conn.Conexion();
            try
            {
                ora.Open();
                OracleCommand comando = new OracleCommand("SP_UPDATE_CONTRATO", ora);
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Parameters.Add("P_ID_PRODUCTOR", OracleType.Number).Value = Convert.ToInt32(txtProductorId.Text);
                comando.Parameters.Add("P_FECHA_INICIO", OracleType.DateTime).Value = dpInicio.SelectedDate;
                comando.Parameters.Add("P_FECHA_TERMINO", OracleType.DateTime).Value = dpFin.SelectedDate;
                comando.Parameters.Add("OUT_GLOSA", OracleType.VarChar, 50).Direction = System.Data.ParameterDirection.Output;
                comando.Parameters.Add("OUT_ESTADO", OracleType.Number, 1).Direction = System.Data.ParameterDirection.Output;

                comando.ExecuteNonQuery();
                MessageBox.Show("Contrato Actualizado.");
                limpiar();
                
            }
            catch (Exception)
            {
                MessageBox.Show("Error al actualizar.");
            }


            ListarContratos();
            ora.Close();
        }

        private void DgContratos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                txtProductorId.Text = dr["ID_PRODUCTOR"].ToString();
                cbProductor.SelectedValue = Convert.ToInt32(dr["ID_PRODUCTOR"].ToString());
                dpInicio.SelectedDate = Convert.ToDateTime(dr["FECHA_INICIO_CONTRATO"].ToString());
                dpFin.SelectedDate = Convert.ToDateTime(dr["FECHA_TERMINO_CONTRATO"].ToString());
                btnActualizar.IsEnabled = true;
                btnIngresar.IsEnabled = false;
            }
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            limpiar();
        }

    }
}
