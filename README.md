# 🛒 CRUD de Productos - C# WinForms & MySQL

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![Windows Forms](https://img.shields.io/badge/Windows%20Forms-%235C2D91.svg?style=for-the-badge&logo=windows&logoColor=white)
![MySQL](https://img.shields.io/badge/mysql-%2300f.svg?style=for-the-badge&logo=mysql&logoColor=white)

Este repositorio contiene la **Actividad #1 del Laboratorio #4**, la cual consiste en una aplicación de escritorio desarrollada en C# (Windows Forms) que implementa un sistema completo de gestión (CRUD) para un inventario de productos. 

La aplicación se conecta a una base de datos MySQL y destaca por su enfoque en la seguridad (prevención de inyección SQL) y un diseño de código modular.

## ✨ Características Principales

- **Gestión Completa (CRUD):** Crear, Leer, Actualizar y Eliminar productos.
- **Soporte de Imágenes:** Permite cargar imágenes desde el equipo y guardarlas directamente en la base de datos como arreglos de bytes (`LONGBLOB`).
- **Búsqueda Dinámica:** Filtro en tiempo real que actualiza el `DataGridView` conforme el usuario escribe en el campo de búsqueda.
- **Consultas Parametrizadas Seguras:** Uso de `MySqlCommand` con diccionarios de datos para generar sentencias dinámicas `INSERT` y `UPDATE` que previenen ataques de inyección SQL.
- **Validación de Datos:** Verificación de tipos (Enteros y Decimales) mediante `TryParse` y reglas de negocio para evitar datos inconsistentes.

## 🛠️ Tecnologías y Herramientas

- **Lenguaje:** C# (.NET Framework)
- **Interfaz Gráfica:** Windows Forms (WinForms)
- **Base de Datos:** MySQL
- **Librería de Datos:** `MySql.Data` (MySQL Connector/NET)

## 📂 Arquitectura del Proyecto

El código está estructurado para separar las responsabilidades:

- `Form1.cs`: Manejo de la interfaz de usuario, eventos de botones y validaciones visuales.
- `Conexion.cs`: Clase estática encargada de gestionar la conexión a MySQL y ejecutar comandos seguros (`InsertSeguro`, `UpdateSeguro`).
- `GetPRoducts.cs`: Lógica de acceso a datos específica para leer y filtrar registros mediante `MySqlDataReader`.
- `Producto.cs`: Clase Modelo (POCO) que representa la entidad del producto en memoria.

## ⚙️ Instalación y Configuración

Sigue estos pasos para ejecutar el proyecto en tu entorno local:

### 1. Requisitos Previos
- Visual Studio (2019 o superior).
- Servidor MySQL local (XAMPP, WAMP, o MySQL Server).

### 2. Base de Datos
Crea la base de datos y la tabla ejecutando el siguiente script en tu gestor de MySQL (ej. phpMyAdmin o MySQL Workbench):

```sql
CREATE DATABASE mi_proyecto_db;
USE mi_proyecto_db;

CREATE TABLE productos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    precio DECIMAL(10,2) NOT NULL,
    cantidad INT NOT NULL,
    imagen LONGBLOB
);
```

### 3. Configurar Conexión
Abre el proyecto en Visual Studio, dirígete al archivo `Conexion.cs` y verifica que la cadena de conexión coincida con tus credenciales locales:

```csharp
private static string cadenaConexion = "Server=localhost;Database=mi_proyecto_db;Uid=root;Pwd=tu_contraseña;";
```

### 4. Restaurar Paquetes NuGet
Asegúrate de tener instalado el paquete `MySql.Data`. Si no lo tienes, ábrelo en la Consola del Administrador de Paquetes:
```bash
Install-Package MySql.Data
```

### 5. Ejecutar
Inicia la aplicación presionando `F5` o el botón "Iniciar" en Visual Studio.

---
*Desarrollado como parte de las actividades prácticas de laboratorio de programación.*
