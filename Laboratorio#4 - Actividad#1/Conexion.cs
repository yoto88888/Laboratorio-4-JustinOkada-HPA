using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ProyectoProductos
{
    public class Conexion
    {
        private static string cadenaConexion = "Server=localhost;Database=mi_proyecto_db;Uid=root;Pwd=1234";

        public static MySqlConnection ObtenerConexion()
        {
            try
            {
                // Crear un tipo de dato de MySqlConnection
                MySqlConnection conexion = new MySqlConnection(cadenaConexion);
                conexion.Open();
                return conexion;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error al conectar: " + ex.Message);
                return null;
            } // fin del catch
        } // fin del método estático MySqlConnection
        public static bool InsertSeguro(string tbName, Dictionary<string, object> data)
        {
            var columns = string.Join(", ", data.Keys);
            var placeholders = "@" + string.Join(", @", data.Keys);

            string sql = $"INSERT INTO {tbName} ({columns}) VALUES ({placeholders})";

            try
            {
                // Pedimos la conexión usando nuestra clase externa
                using (MySqlConnection conexion = ObtenerConexion())
                {
                    if (conexion == null) return false;

                    using (MySqlCommand stmt = new MySqlCommand(sql, conexion))
                    {
                        foreach (var kvp in data)
                        {
                            stmt.Parameters.AddWithValue("@" + kvp.Key, kvp.Value ?? DBNull.Value);
                        }

                        stmt.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error en INSERT: " + ex.Message);
                return false;
            }
        }//fin del método Insertar Seguro

        public static bool UpdateSeguro(string tbName, Dictionary<string, object> data, string idColumn, int idValue)
        {
            // Construimos la cláusula SET: columna1 = @columna1, columna2 = @columna2...
            var setParts = new List<string>();
            foreach (var key in data.Keys)
            {
                setParts.Add($"{key} = @{key}");
            }
            string setClause = string.Join(", ", setParts);

            // Armamos la consulta SQL completa incluyendo el WHERE
            string sql = $"UPDATE {tbName} SET {setClause} WHERE {idColumn} = @idCondicion";

            try
            {
                using (MySqlConnection conexion = ObtenerConexion())
                {
                    if (conexion == null) return false;

                    using (MySqlCommand stmt = new MySqlCommand(sql, conexion))
                    {
                        // Agregamos los parámetros de los campos a actualizar
                        foreach (var kvp in data)
                        {
                            stmt.Parameters.AddWithValue("@" + kvp.Key, kvp.Value ?? DBNull.Value);
                        }

                        // Agregamos el parámetro para la condición WHERE de forma segura
                        stmt.Parameters.AddWithValue("@idCondicion", idValue);

                        stmt.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error en UPDATE: " + ex.Message);
                return false;
            }
        }
    }
}
