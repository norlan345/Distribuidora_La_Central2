﻿namespace Distribuidora_La_Central.Web.Models
{
    public class DetalleFactura
    {
        public int idDetalle { get; set; }
        public int codigoFactura { get; set; }
        public int codigoProducto { get; set; }
        public int cantidad { get; set; }
        public decimal precioUnitario { get; set; }
        public decimal subtotal { get; set; }
    }
}
