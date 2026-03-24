using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crud_cliente_mvc_asp_net.Models
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