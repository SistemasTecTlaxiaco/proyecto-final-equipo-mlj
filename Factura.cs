﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace BDFARMACIA
{
    public partial class Factura : Form
    {
        Conexion conMysql = new Conexion();
        public Factura()
        {
            InitializeComponent();
        }

        private void Factura_Load(object sender, EventArgs e)
        {

        
        // Create an unbound DataGridView by declaring a column count.
        dataGridView1.ColumnCount = 5;
            dataGridView1.ColumnHeadersVisible = true;
            

            // Set the column header style.
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();

        columnHeaderStyle.BackColor = Color.Beige;
            columnHeaderStyle.Font = new Font("Verdana", 8, FontStyle.Bold);
        dataGridView1.ColumnHeadersDefaultCellStyle = columnHeaderStyle;

            // Set the column header names.
            dataGridView1.Columns[0].Name = "Cliente";
            dataGridView1.Columns[1].Name = "Producto";
            dataGridView1.Columns[2].Name = "Precio";
            dataGridView1.Columns[3].Name = "Cantidad";
            dataGridView1.Columns[4].Name = "Total";

            // Set the column header width.
            dataGridView1.Columns[0].Width = 80;
            dataGridView1.Columns[1].Width = 90;
            dataGridView1.Columns[2].Width = 120;
            dataGridView1.Columns[3].Width = 60;
            dataGridView1.Columns[4].Width = 140;
            
            //cargar comboBox
            conMysql.Connect();
            String sql = "select id, Nombre from clientes";
            
            String sql2 = "select id,Producto from productos";
            
            conMysql.CargarCombo(comboBoxcliente, sql, "Nombre", "id");
            conMysql.CargarCombo(comboBoxproducto, sql2, "Producto", "id");
        }

        private void buttonagregar_Click(object sender, EventArgs e)
        {
            agregarcarrito();
        }

        private void buttontotalcompra_Click(object sender, EventArgs e)
        {
            totalCuenta();
        }

        private void Guardar_Click(object sender, EventArgs e)
        {
            guardar();

        }

        private void buttonsalir_Click(object sender, EventArgs e)
        {
            limpiar();
        }
        public void limpiar()
        {
            textBoxunidad.Text = "";
            textBoxtotal.Text = "";
            
            comboBoxcliente.Text = "";
            comboBoxproducto.Text = "";
            dataGridView1.DataSource = "";
        }
        public void totalCuenta()
        {
            //Variable donde almacenaremos el resultado de la sumatoria.
            double sumatoria = 0;
            //Método con el que recorreremos todas las filas de nuestro Datagridview
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                //Aquí seleccionaremos la columna que contiene los datos numericos.
                sumatoria += Convert.ToDouble(row.Cells["Total"].Value);
            }
            //Por ultimo asignamos el resultado a el texto de nuestro Label
            textBoxtotal.Text = Convert.ToString(sumatoria);
        }
        public void agregarcarrito()
        {
            String sql3 = "select Precio from productos where id = " + comboBoxproducto.SelectedValue;
            DataRow precio = conMysql.getRow(sql3);

            int total = 0;
            int unidad = int.Parse(textBoxunidad.Text);
            var precio_unidad = (int)precio[0];

            total = precio_unidad * unidad;

            dataGridView1.Rows.Add(comboBoxcliente.SelectedValue, comboBoxproducto.SelectedValue, precio_unidad, unidad,  total);
        }



        public void guardar()
        {
            if (textBoxunidad.Text.Trim() == String.Empty && textBoxtotal.Text.Trim() == String.Empty)
            {
                MessageBox.Show(" Error!");
                return;
            }

            if (textBoxunidad.Text.Trim() == String.Empty)
            {
                MessageBox.Show(" Error!");
                return;
            }

            if (textBoxtotal.Text.Trim() == String.Empty)
            {
                MessageBox.Show(" Error");
                return;
            }

            dateTimePicker1.Value = DateTime.Now;

            String sql = String.Format("insert into factura (fecha,Id_cliente,Total)" +
                          " values('{0}','{1}','{2}')",
                          dateTimePicker1.Value.ToString("yyyy-MM-dd"), comboBoxcliente.SelectedValue, textBoxtotal.Text.Trim());
            try
            {
                if (conMysql.Query(sql) == 1)
                {
                    MessageBox.Show("Se realizo la factura");
                }
                else
                {
                    MessageBox.Show("No se realizo la factura");
                }

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }

            DataRow busq1 = conMysql.getRow("select max(id) from factura");
            String sql2 = String.Format("insert into factura1 (Id_Factura,Id_Producto,Cantidad)" +
                          " values('{0}','{1}','{2}')",
                          busq1[0], comboBoxcliente.SelectedValue, textBoxunidad.Text.Trim());
            try
            {
                if (conMysql.Query(sql2) == 1)
                {
                    MessageBox.Show(" Registro de Factura éxitoso \n" +
                                    " Numero Factura: " + busq1[0] + "\n" +
                                    " Cliente: " + comboBoxcliente.Text + "\n" +
                                    " Producto: " + comboBoxproducto.Text + "\n" +
                                    " Cantidad: " + textBoxunidad.Text + "\n" +
                                    " Total :$ " + textBoxtotal.Text + "\n" +
                                    " ");
                }
                else
                {
                    MessageBox.Show("ERROR!");
                }
                limpiar();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
