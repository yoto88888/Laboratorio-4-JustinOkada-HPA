using MySql.Data.MySqlClient;
using ProyectoProductos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorio_4___Actividad_1
{
    internal class GetPRoducts
    {
        public static List<Producto> GetProductos(string filtro)
        {
            List<Producto> listaProductos = new List<Producto>();

            string query = "SELECT id, nombre, precio, cantidad, imagen FROM productos";

            // Si viene un filtro, modificamos el query de forma segura
            // (Nota: Idealmente con parámetros, pero adaptado al ejemplo visual que tienes)
            if (!string.IsNullOrEmpty(filtro))
            {
                query += " WHERE id LIKE @filtro OR nombre LIKE @filtro " +
                         " OR precio LIKE @filtro OR cantidad LIKE @filtro";
            }

            using (MySqlConnection conn = Conexion.ObtenerConexion())
            {
                if (conn == null) return listaProductos;

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    // Si hay filtro, agregamos el parámetro para evitar inyección SQL
                    if (!string.IsNullOrEmpty(filtro))
                    {
                        cmd.Parameters.AddWithValue("@filtro", "%" + filtro + "%");
                    }

                    // Ejecutamos el lector de datos
                    using (MySqlDataReader mReader = cmd.ExecuteReader())
                    {
                        // Recorremos el lector fila por fila mientras haya registros
                        while (mReader.Read())
                        {
                            Producto prod = new Producto();

                            // Mapeamos los campos de la base de datos a las propiedades de tu clase Producto
                            // (Ajusta los nombres de las columnas o índices según tu base de datos)

                            prod.Id = Convert.ToInt32(mReader["id"]);
                            prod.Nombre = mReader["nombre"].ToString();
                            prod.Precio = Convert.ToDecimal(mReader["precio"]);
                            prod.Cantidad = Convert.ToInt32(mReader["cantidad"]);
                            //prod.Imagen = (byte[])mReader.GetValue(4);

                            prod.Imagen = mReader["imagen"] != DBNull.Value ? (byte[])mReader["imagen"] : null;

                            // Agregamos el objeto listo a la lista genérica
                            listaProductos.Add(prod);
                        }
                        mReader.Close();
                    }//MySqlDataReader
                }//MySqlCommand
            }

            return listaProductos;
        }//fin del método listaProductos
    }
}
