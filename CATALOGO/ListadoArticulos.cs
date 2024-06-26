﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using dominio;
using negocio;

namespace CATALOGO
{
    public partial class ListadoArticulos : Form
    {

        private List<Articulo> listarArticulo;

        ArticuloService articuloService = new ArticuloService();

        public ListadoArticulos()
        {
            InitializeComponent();
        }


        private void ListadoArticulos_Load(object sender, EventArgs e)
        {
            listarArticulo = cargar();
            dataGridView1.DataSource = listarArticulo;
            ocultarColumnas();
            //CargarImagen(listarArticulo[0].IMAGEN.Url);
            filtrosDesplegables();
        }

        private void filtrosDesplegables()
        {
            MarcaService marcaService = new MarcaService();
            CategoriaService categoriaService = new CategoriaService();

            cboMarcas.DataSource = marcaService.listar();
            cboMarcas.ValueMember = "Id";
            cboMarcas.DisplayMember = "Descripcion";
            cboMarcas.SelectedIndex = -1;

            cboCategoria.DataSource = categoriaService.listar();
            cboCategoria.ValueMember = "Id";
            cboCategoria.DisplayMember = "Descripcion";
            cboCategoria.SelectedIndex = -1;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            Articulo articuloSeleccionado = (Articulo)dataGridView1.CurrentRow.DataBoundItem;
            CargarImagen(articuloSeleccionado.IMAGEN.Url);
        }

        private void CargarImagen(string URLimagen)
        {
            //try
            //{
            //    picBoxArticulo.Load(URLimagen);
            //}
            //catch (Exception ex)
            //{
            //    picBoxArticulo.Load("https://media.istockphoto.com/id/1409329028/vector/no-picture-available-placeholder-thumbnail-icon-illustration-design.jpg?s=612x612&w=0&k=20&c=_zOuJu755g2eEUioiOUdz_mHKJQJn-tDgIAhQzyeKUQ=");
            //}
        }

        private void MostrarDetalleArticulo(int id, DataGridView gridTable)
        {
            try
            {
                DetalleArticulo detalleForm = new DetalleArticulo(id, gridTable);
                detalleForm.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void buttonVerDetalle(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow != null && dataGridView1.CurrentRow.DataBoundItem != null)
                {
                    Articulo articuloSeleccionado = (Articulo)dataGridView1.CurrentRow.DataBoundItem;
                    MostrarDetalleArticulo(articuloSeleccionado.ID, dataGridView1);
                }
                else
                {
                    MessageBox.Show("No hay artículos para mostrar o no se ha seleccionado ninguno válido.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private List<Articulo> cargar()
        {
            return articuloService.ListarArticulos();
        }

        private void ocultarColumnas()
        {
            dataGridView1.Columns["IMAGEN"].Visible = false;
            dataGridView1.Columns["DESCRIPCION"].Visible = false;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string filtro = txtBuscador.Text;

            List<Articulo> listarArticuloFiltrada;

            if (filtro != "")
            {
                listarArticuloFiltrada = listarArticulo.FindAll(articulo => articulo.NOMBRE.ToUpper().Contains(filtro.ToUpper())
                || articulo.CODIGO.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listarArticuloFiltrada = listarArticulo;
                cboMarcas.SelectedIndex = -1;
                cboCategoria.SelectedIndex = -1;
            }

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = listarArticuloFiltrada;

            ocultarColumnas();
        }

        private void agregarArticuloToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAgregar alta = new frmAgregar(dataGridView1);
            alta.ShowDialog();
            cargar();
        }

        private void modificarArticuloToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Articulo articuloSeleccionado = (Articulo)dataGridView1.CurrentRow.DataBoundItem;
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("NO SELECCIONASTE UN ARTICULO");
            }
            else
            {
                articuloSeleccionado = (Articulo)dataGridView1.CurrentRow.DataBoundItem;
                frmAgregar modificar = new frmAgregar(articuloSeleccionado, dataGridView1);
                modificar.ShowDialog();
                cargar();
            }
        }

        private void cboMarcas_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filtroMarcas = cboMarcas.Text;
            string filtroCategorias = cboCategoria.Text;

            List<Articulo> listarArticuloFiltrada;

            try
            {
                if (filtroMarcas != "")
                {
                    if (filtroCategorias != "")
                    {
                        listarArticuloFiltrada = listarArticulo.FindAll(articulo =>
                            articulo.MARCA?.Descripcion != null && articulo.MARCA.Descripcion.ToUpper().Contains(filtroMarcas.ToUpper()) &&
                            articulo.CATEGORIA?.Descripcion != null && articulo.CATEGORIA.Descripcion.ToUpper().Contains(filtroCategorias.ToUpper())
                        );
                    }
                    else
                    {
                        listarArticuloFiltrada = listarArticulo.FindAll(articulo =>
                            articulo.MARCA?.Descripcion != null && articulo.MARCA.Descripcion.ToUpper().Contains(filtroMarcas.ToUpper()) &&
                            articulo.CATEGORIA != null);
                    }
                }
                else
                {
                    listarArticuloFiltrada = listarArticulo;
                }

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = listarArticuloFiltrada;

                ocultarColumnas();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cboCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {

            string filtroMarcas = cboMarcas.Text;
            string filtroCategorias = cboCategoria.Text;

            List<Articulo> listarArticuloFiltrada;

            try
            {
                if (!string.IsNullOrWhiteSpace(filtroCategorias))
                {
                    if (!string.IsNullOrWhiteSpace(filtroCategorias))
                    {
                        listarArticuloFiltrada = listarArticulo.FindAll(articulo =>
                            articulo.MARCA?.Descripcion != null 
                            && articulo.MARCA.Descripcion.ToUpper().Contains(filtroMarcas.ToUpper()) 
                            && articulo.CATEGORIA?.Descripcion != null 
                            && articulo.CATEGORIA.Descripcion.ToUpper().Contains(filtroCategorias.ToUpper()));
                    }
                    else
                    {
                        listarArticuloFiltrada = listarArticulo.FindAll(articulo =>
                            articulo.MARCA?.Descripcion != null && articulo.MARCA.Descripcion.ToUpper().Contains(filtroMarcas.ToUpper()) &&
                            articulo.CATEGORIA != null);
                    }
                }
                else
                {
                    listarArticuloFiltrada = listarArticulo;
                }

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = listarArticuloFiltrada;

                ocultarColumnas();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            
            cboMarcas.SelectedIndex = -1;
            cboCategoria.SelectedIndex = -1;

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = listarArticulo;

            cargar();

            ocultarColumnas();
        }

        private void agregarNuevaImagenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Articulo articuloSeleccionado = (Articulo)dataGridView1.CurrentRow.DataBoundItem;
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("NO SELECCIONASTE UN ARTICULO");
            }
            else
            {
                articuloSeleccionado = (Articulo)dataGridView1.CurrentRow.DataBoundItem;
                AgregarImagen agregarNuevaImagen = new AgregarImagen(articuloSeleccionado, dataGridView1);
                agregarNuevaImagen.ShowDialog();
                cargar();
            }
        }

        private void agregarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CategoriaForm agregarCategoria = new CategoriaForm(0, dataGridView1, cboCategoria);
            agregarCategoria.ShowDialog();
            cargar();
            filtrosDesplegables();
        }

        private void modificarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CategoriaForm modificarCategoria = new CategoriaForm(1, dataGridView1, cboCategoria);
            modificarCategoria.ShowDialog();
            cargar();
            filtrosDesplegables();
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CategoriaForm eliminarCategoria = new CategoriaForm(2, dataGridView1, cboCategoria);
            eliminarCategoria.ShowDialog();
            cargar();
            filtrosDesplegables();
        }

        private void agregarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MarcaForm agregarMarca = new MarcaForm(0, dataGridView1, cboMarcas);
            agregarMarca.ShowDialog();
            cargar();
            filtrosDesplegables();
        }

        private void modificarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MarcaForm modificarMarca = new MarcaForm(1, dataGridView1, cboMarcas);
            modificarMarca.ShowDialog();
            cargar();
            filtrosDesplegables();
        }

        private void eliminarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MarcaForm eliminarMarca = new MarcaForm(2, dataGridView1, cboMarcas);
            eliminarMarca.ShowDialog();
            cargar();
            filtrosDesplegables();
        }
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception is FormatException && e.Context == DataGridViewDataErrorContexts.Formatting)
            {
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "NULL";
                e.Cancel = true;
            }
        }
    }
}
