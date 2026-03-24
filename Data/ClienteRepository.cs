using System.Collections.Generic;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using crud_cliente_mvc_asp_net.Models;  

namespace crud_cliente_mvc_asp_net.Data
{
    public class ClienteRepository
    {
        private string connectionString =
            ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;

        public List<Cliente> GetAll()
        {
            var lista = new List<Cliente>();

            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM CLIENTES_F ORDER BY ID";

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

                string sql = @"INSERT INTO CLIENTES_F
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

                string sql = @"UPDATE CLIENTES_F SET
                                CONSECUTIVO = :con, 
                                NOMBRE = :nom,
                                EMAIL = :ema, 
                                TELEFONO = :tel
                                WHERE ID = :id";

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

                string sql = "SELECT * FROM CLIENTES_F WHERE ID = :id";

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

                string sql = "DELETE FROM CLIENTES_F WHERE ID = :id";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(":id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }   
}