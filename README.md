# -_crud-_.net_asp.net_mvc_oracle_19C_- :.
🧩 CRUD en .NET (ASP.NET MVC) con Oracle 19c:

<img width="1536" height="1024" alt="image" src="https://github.com/user-attachments/assets/c15b1d91-9ead-4345-bbc7-5899d220df54" />  

```
Solución completa y funcional para un CRUD en ASP.NET MVC con persistencia en Oracle 19c, mostrando los datos en una tabla HTML (interfaz web).

📦 Incluye:
Script Oracle (tabla + secuencia + trigger)
Modelo (Entity)
Cadena de conexión
Repositorio (ADO.NET con Oracle)
Controlador MVC
Vistas Razor (Index, Create, Edit, Delete)
Tabla HTML (5 columnas)

🧩 1. Script Oracle 19c:
-- Tabla
CREATE TABLE CLIENTES (
    ID            NUMBER PRIMARY KEY,
    CONSECUTIVO   NUMBER,
    NOMBRE        VARCHAR2(100),
    EMAIL         VARCHAR2(100),
    TELEFONO      VARCHAR2(50)
);

-- Secuencia
CREATE SEQUENCE SEQ_CLIENTES START WITH 1 INCREMENT BY 1;

-- Trigger autoincremental
CREATE OR REPLACE TRIGGER TRG_CLIENTES
BEFORE INSERT ON CLIENTES
FOR EACH ROW
BEGIN
    SELECT SEQ_CLIENTES.NEXTVAL INTO :NEW.ID FROM DUAL;
END;
/

🧩 2. Paquete requerido (NuGet):

Instalar:
Oracle.ManagedDataAccess

🧩 3. Web.config (Cadena de conexión):
<connectionStrings>
  <add name="OracleDbContext"
       connectionString="User Id=TU_USUARIO;Password=TU_PASSWORD;Data Source=localhost:1521/XEPDB1;"
       providerName="Oracle.ManagedDataAccess.Client" />
</connectionStrings>

🧩 4. Modelo (Models/Cliente.cs):
namespace CrudOracleMVC.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public int Consecutivo { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
    }
}

🧩 5. Repositorio (Data/ClienteRepository.cs):
using System.Collections.Generic;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using CrudOracleMVC.Models;

namespace CrudOracleMVC.Data
{
    public class ClienteRepository
    {
        private string connectionString = ConfigurationManager
            .ConnectionStrings["OracleDbContext"].ConnectionString;

        public List<Cliente> GetAll()
        {
            var lista = new List<Cliente>();

            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM CLIENTES ORDER BY ID";

                using (var cmd = new OracleCommand(sql, conn))
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Cliente
                        {
                            Id = int.Parse(dr["ID"].ToString()),
                            Consecutivo = int.Parse(dr["CONSECUTIVO"].ToString()),
                            Nombre = dr["NOMBRE"].ToString(),
                            Email = dr["EMAIL"].ToString(),
                            Telefono = dr["TELEFONO"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public void Insert(Cliente c)
        {
            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();

                string sql = @"INSERT INTO CLIENTES 
                               (CONSECUTIVO, NOMBRE, EMAIL, TELEFONO)
                               VALUES (:con, :nom, :ema, :tel)";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(":con", c.Consecutivo);
                    cmd.Parameters.Add(":nom", c.Nombre);
                    cmd.Parameters.Add(":ema", c.Email);
                    cmd.Parameters.Add(":tel", c.Telefono);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Cliente c)
        {
            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();

                string sql = @"UPDATE CLIENTES SET 
                               CONSECUTIVO=:con,
                               NOMBRE=:nom,
                               EMAIL=:ema,
                               TELEFONO=:tel
                               WHERE ID=:id";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(":con", c.Consecutivo);
                    cmd.Parameters.Add(":nom", c.Nombre);
                    cmd.Parameters.Add(":ema", c.Email);
                    cmd.Parameters.Add(":tel", c.Telefono);
                    cmd.Parameters.Add(":id", c.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Cliente GetById(int id)
        {
            Cliente c = null;

            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();

                string sql = "SELECT * FROM CLIENTES WHERE ID=:id";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(":id", id);

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            c = new Cliente
                            {
                                Id = int.Parse(dr["ID"].ToString()),
                                Consecutivo = int.Parse(dr["CONSECUTIVO"].ToString()),
                                Nombre = dr["NOMBRE"].ToString(),
                                Email = dr["EMAIL"].ToString(),
                                Telefono = dr["TELEFONO"].ToString()
                            };
                        }
                    }
                }
            }

            return c;
        }

        public void Delete(int id)
        {
            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();

                string sql = "DELETE FROM CLIENTES WHERE ID=:id";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(":id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

🧩 6. Controlador (Controllers/ClienteController.cs):
using System.Web.Mvc;
using CrudOracleMVC.Data;
using CrudOracleMVC.Models;

namespace CrudOracleMVC.Controllers
{
    public class ClienteController : Controller
    {
        ClienteRepository repo = new ClienteRepository();

        public ActionResult Index()
        {
            var lista = repo.GetAll();
            return View(lista);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Cliente c)
        {
            repo.Insert(c);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            return View(repo.GetById(id));
        }

        [HttpPost]
        public ActionResult Edit(Cliente c)
        {
            repo.Update(c);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            return View(repo.GetById(id));
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            repo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}

🧩 7. Vista Index (Views/Cliente/Index.cshtml):
@model IEnumerable<CrudOracleMVC.Models.Cliente>

<h2>Lista de Clientes</h2>

<p>
    @Html.ActionLink("Nuevo Cliente", "Create")
</p>

<table border="1" style="width:100%">
    <tr>
        <th>ID</th>
        <th>Consecutivo</th>
        <th>Nombre</th>
        <th>Email</th>
        <th>Teléfono</th>
        <th>Acciones</th>
    </tr>

@foreach (var item in Model)
{
    <tr>
        <td>@item.Id</td>
        <td>@item.Consecutivo</td>
        <td>@item.Nombre</td>
        <td>@item.Email</td>
        <td>@item.Telefono</td>
        <td>
            @Html.ActionLink("Editar", "Edit", new { id = item.Id }) |
            @Html.ActionLink("Eliminar", "Delete", new { id = item.Id })
        </td>
    </tr>
}
</table>

🧩 8. Vista Create (Views/Cliente/Create.cshtml):
@model CrudOracleMVC.Models.Cliente

<h2>Nuevo Cliente</h2>

@using (Html.BeginForm())
{
    <p>Consecutivo: @Html.TextBoxFor(m => m.Consecutivo)</p>
    <p>Nombre: @Html.TextBoxFor(m => m.Nombre)</p>
    <p>Email: @Html.TextBoxFor(m => m.Email)</p>
    <p>Teléfono: @Html.TextBoxFor(m => m.Telefono)</p>

    <input type="submit" value="Guardar" />
}

🧩 9. Vista Edit (Views/Cliente/Edit.cshtml):
@model CrudOracleMVC.Models.Cliente

<h2>Editar Cliente</h2>

@using (Html.BeginForm())
{
    @Html.HiddenFor(m => m.Id)

    <p>Consecutivo: @Html.TextBoxFor(m => m.Consecutivo)</p>
    <p>Nombre: @Html.TextBoxFor(m => m.Nombre)</p>
    <p>Email: @Html.TextBoxFor(m => m.Email)</p>
    <p>Teléfono: @Html.TextBoxFor(m => m.Telefono)</p>

    <input type="submit" value="Actualizar" />
}

🧩 10. Vista Delete (Views/Cliente/Delete.cshtml):
@model CrudOracleMVC.Models.Cliente

<h2>Eliminar Cliente</h2>

<p>¿Seguro que deseas eliminar este registro?</p>

<div>
    <b>@Model.Nombre</b>
</div>

@using (Html.BeginForm())
{
    @Html.HiddenFor(m => m.Id)
    <input type="submit" value="Eliminar" />
}

✅ Resultado:

Tendrás:

✔ CRUD completo (Create, Read, Update, Delete)
✔ Conexión real a Oracle 19c
✔ Tabla HTML con 5 columnas:
ID
Consecutivo
Nombre
Email
Teléfono
✔ Interfaz web MVC funcional

🚀 Mejoras opcionales:

Puedes escalar esta solución agregando:

Entity Framework + Oracle
Bootstrap (UI moderna)
Paginación (para miles de registros)
Validaciones (DataAnnotations)
API REST (.NET Web API)
Arquitectura en capas (Service + UnitOfWork) :. . . / .
