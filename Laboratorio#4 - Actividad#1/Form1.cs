using ProyectoProductos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laboratorio_4___Actividad_1
{
    public partial class Form1 : Form
    {
        //Cre Producto para Guardar los datos.
        //private Producto myNuevoProducto = new Producto();

        private List<Producto> listaProductos;
        private Dictionary<string, object> myProducto = new Dictionary<string, object>();
        int idProducto = 0;
        public Form1()
        {
            InitializeComponent();
            listaProductos = new List<Producto>();
        }

        private void CargarDatosProductos()
        {
            myProducto["Cantidad"] = int.Parse(txtCantidad.Text.Trim());
            myProducto["Precio"] = decimal.Parse(txtPrecio.Text.Trim());
            myProducto["Nombre"] = txtNombre.Text.Trim();
            myProducto["Imagen"] = ImageToByteArray(pictureBox1.Image);
        }

        private byte[] ImageToByteArray(Image image)
        {
            if (image == null)
                return null;

            using (MemoryStream mMemoryStream = new MemoryStream())
            {
                // Guardamos la imagen usando su formato original (RawFormat)
                image.Save(mMemoryStream, image.RawFormat);
                return mMemoryStream.ToArray();
            }
        }

        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow fila = dgvProductos.Rows[e.RowIndex];

            idProducto = Convert.ToInt32(fila.Cells["Id"].Value);
            txtNombre.Text = Convert.ToString(fila.Cells["Producto"].Value);
            txtPrecio.Text = Convert.ToDecimal(fila.Cells["Precio"].Value).ToString();
            txtCantidad.Text = Convert.ToInt32(fila.Cells["Cantidad"].Value).ToString();

            btnGuardar.Enabled = false;
            btnModificar.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Limpiar boton
        }

        private void button2_Click(object sender, EventArgs e) // boton modificar
        {
            CargarDatosProductos();
            //Asegurar que mi arreglo tiene los datos
            //MessageBox.Show("el Nombre del Producto es: " + myProducto["Nombre"]);
            // Actualiza el producto donde el id_producto sea igual a 5

            MessageBox.Show("el id del producto e: " + idProducto);

            bool resultado = Conexion.UpdateSeguro("productos", myProducto, "id", idProducto);

            if (resultado)
            {
                Console.WriteLine("Actualización exitosa.");
            }
        }

        private void button1_Click(object sender, EventArgs e) //Boton insertar
        {

            if (!datosCorrectos())
            {
                return; // No vamos hacer nada - se detine en este punto, puedes crear un punto
            }

            CargarDatosProductos();

            if (Conexion.InsertSeguro("productos", myProducto))
            {
                MessageBox.Show("Se ha guardado satisfactoriamente el registro");
                // Aquí refrescas el grid volviendo a consultar la base de datos
                cargarProductos();
                limpiarCampos(); //Limpiara los campos del formulario
            }//fin del if InsertSeguro
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cargarProductos();
        }

        private void cargarProductos(string filtro = "")
        {
            dgvProductos.Rows.Clear();
            dgvProductos.Refresh();
            listaProductos = GetPRoducts.GetProductos(filtro);

            foreach (var prod in listaProductos)
            {
                Image img = null;

                if (prod.Imagen != null && prod.Imagen.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(prod.Imagen))
                    {
                        using (Bitmap bmp = new Bitmap(ms))
                        {
                            img = new Bitmap(bmp); // Esto clona la imagen y evita que falle
                        }
                    }
                }

                dgvProductos.Rows.Add(prod.Id, prod.Nombre, prod.Precio, prod.Cantidad, img);
            }//fin del foreach listaProductos
        }//fin de cargarProductos

        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            cargarProductos(txtBusqueda.Text.Trim());
        }

        private void limpiarCampos()
        {
            // 1. Vaciamos las cajas de texto
            txtNombre.Text = "";
            txtPrecio.Text = "";
            txtCantidad.Text = "";

            // 2. Quitamos la imagen cargada en el PictureBox
            pictureBox1.Image = null;

            // 3. Reiniciamos el ID seleccionado para evitar actualizar un producto por error
            idProducto = 0;

            // 4. Restauramos los botones a su estado original
            btnGuardar.Enabled = true;
            btnModificar.Enabled = false;
        }

        private bool datosCorrectos()
        {
            // 1. Validar que no estén vacíos
            if (txtNombre.Text.Trim().Equals(""))
            {
                MessageBox.Show("Ingrese el Nombre del Producto");
                return false;
            }

            if (txtPrecio.Text.Trim().Equals(""))
            {
                MessageBox.Show("Ingrese el Precio");
                return false;
            }

            if (txtCantidad.Text.Trim().Equals(""))
            {
                MessageBox.Show("Ingrese la Cantidad");
                return false;
            }

            // 2. AQUÍ VA EL CÓDIGO EXTRAÍDO (Validar que sean números válidos)
            if (!decimal.TryParse(txtPrecio.Text.Trim(), out decimal precio))
            {
                MessageBox.Show("Ingrese un  Precio correcto");
                return false;
            }

            if (!int.TryParse(txtCantidad.Text.Trim(), out int cantidad))
            {
                MessageBox.Show("Ingrese una cantidad  correcto");
                return false;
            }

            return true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Seleccionar imagen del producto";
                openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.bmp";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Carga la imagen seleccionada en el PictureBox y ajusta su tamaño
                    pictureBox1.Image = Image.FromFile(openFileDialog.FileName);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
        }
    }
}
