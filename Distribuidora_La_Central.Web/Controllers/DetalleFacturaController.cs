using Distribuidora_La_Central.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace Distribuidora_La_Central.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleFacturaController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public DetalleFacturaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllDetalles")]
        public string GetDetalles()
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("UsuarioAppCon").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM DetalleFactura;", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<DetalleFactura> detalleList = new List<DetalleFactura>();
            Response response = new Response();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DetalleFactura detalle = new DetalleFactura();
                    detalle.idDetalle = Convert.ToInt32(dt.Rows[i]["idDetalle"]);
                    detalle.codigoFactura = Convert.ToInt32(dt.Rows[i]["codigoFactura"]);
                    detalle.codigoProducto = Convert.ToInt32(dt.Rows[i]["codigoProducto"]);
                    detalle.cantidad = Convert.ToInt32(dt.Rows[i]["cantidad"]);
                    detalle.precioUnitario = Convert.ToDecimal(dt.Rows[i]["precioUnitario"]);
                    detalle.subtotal = Convert.ToDecimal(dt.Rows[i]["subtotal"]);
                    detalleList.Add(detalle);
                }
            }

            if (detalleList.Count > 0)
            {
                return JsonConvert.SerializeObject(detalleList);
            }
            else
            {
                response.StatusCode = 100;
                response.ErrorMessage = "No data found";
                return JsonConvert.SerializeObject(response);
            }
        }
    }
}