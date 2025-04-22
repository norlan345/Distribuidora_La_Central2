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
    public class ProductoController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public ProductoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllProductos")]
        public string GetProductos()
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("UsuarioAppCon").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Producto;", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Producto> productoList = new List<Producto>();
            Response response = new Response();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Producto producto = new Producto();
                    producto.codigoProducto = Convert.ToInt32(dt.Rows[i]["codigoProducto"]);
                    producto.descripcion = Convert.ToString(dt.Rows[i]["descripcion"]);
                    producto.cantidad = Convert.ToInt32(dt.Rows[i]["cantidad"]);
                    producto.categoria = Convert.ToString(dt.Rows[i]["categoria"]);
                    producto.descuento = Convert.ToDecimal(dt.Rows[i]["descuento"]);
                    producto.costo = Convert.ToDecimal(dt.Rows[i]["costo"]);
                    producto.bodega = Convert.ToString(dt.Rows[i]["bodega"]);
                    producto.idProveedor = Convert.ToInt32(dt.Rows[i]["idProveedor"]);
                    productoList.Add(producto);
                }
            }

            if (productoList.Count > 0)
                return JsonConvert.SerializeObject(productoList);
            else
            {
                response.StatusCode = 100;
                response.ErrorMessage = "No data found";
                return JsonConvert.SerializeObject(response);
            }
        }


        [HttpPost("registrar-producto")]
        public IActionResult RegistrarProducto([FromBody] Producto producto)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("UsuarioAppCon"));

            // Verificar si ya existe un producto con la misma descripción y proveedor
            SqlDataAdapter checkProducto = new SqlDataAdapter("SELECT * FROM Producto WHERE descripcion = @descripcion AND idProveedor = @idProveedor", con);
            checkProducto.SelectCommand.Parameters.AddWithValue("@descripcion", producto.descripcion);
            checkProducto.SelectCommand.Parameters.AddWithValue("@idProveedor", producto.idProveedor);

            DataTable dt = new DataTable();
            checkProducto.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                return BadRequest("El producto ya existe con este proveedor.");
            }

            // Insertar el nuevo producto (sin especificar el código porque es IDENTITY)
            SqlCommand cmd = new SqlCommand(@"INSERT INTO Producto 
                (descripcion, cantidad, categoria, descuento, costo, bodega, idProveedor)
                VALUES (@descripcion, @cantidad, @categoria, @descuento, @costo, @bodega, @idProveedor)", con);

            cmd.Parameters.AddWithValue("@descripcion", producto.descripcion);
            cmd.Parameters.AddWithValue("@cantidad", producto.cantidad);
            cmd.Parameters.AddWithValue("@categoria", producto.categoria);
            cmd.Parameters.AddWithValue("@descuento", producto.descuento);
            cmd.Parameters.AddWithValue("@costo", producto.costo);
            cmd.Parameters.AddWithValue("@bodega", producto.bodega);
            cmd.Parameters.AddWithValue("@idProveedor", producto.idProveedor);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i > 0)
            {
                return Ok("Producto registrado exitosamente");
            }
            else
            {
                return StatusCode(500, "Error al registrar producto");
            }
        }




    }
}