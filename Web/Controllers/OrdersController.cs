using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TemplateNBL.Enumerations;
using TemplateNBL.Helpers;
using TemplateNBL.Models;
using TemplateNBL.ViewModel;

namespace TemplateNBL.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public string ModuleDescription = "Menu Principal";
        public string WindowDescription = "Pedidos";
        public string className = "Orders";

        // GET: Orders
        public ActionResult Index()
        {
            ViewBag.Editar = PermissionViewModel.TienePermisoAcesso(WindowHelper.GetWindowId(ModuleDescription, WindowDescription));
            ViewBag.Ver = PermissionViewModel.TienePermisoAlta(WindowHelper.GetWindowId(ModuleDescription, WindowDescription));
            ViewBag.Baja = PermissionViewModel.TienePermisoBaja(WindowHelper.GetWindowId(ModuleDescription, WindowDescription));
            List<OrderIndexViewModel> list = new List<OrderIndexViewModel>();
            Usuario usuario = db.Usuarios.Find(SessionHelper.GetUser());
            List<Order> orders = new List<Order>();
            List<Center> lCentros = new List<Center>();
            List<Status> lEstados = new List<Status>();
            if (usuario.Rol.IsAdmin)
                lCentros = db.Centers.Where(x => x.Enable).ToList();
            else
                lCentros = db.Centers.Where(x => x.Id == usuario.CenterId).ToList();
            lEstados = db.Status.ToList();
            ViewBag.listaCentros = lCentros;
            ViewBag.listaEstados = lEstados;
            ViewBag.isAdmin = usuario.Rol.IsAdmin;


            if (!usuario.Rol.IsAdmin)
                orders = db.Orders.Where(x=>x.CenterId==usuario.CenterId).Include(o => o.Center).OrderByDescending(x => x.Id).ToList();
            else
                orders = db.Orders.Include(o => o.Center).Include(o => o.Usuario).OrderByDescending(x => x.Id).ToList();

            foreach (var item in orders)
            {
                OrderIndexViewModel orderIndexViewModel = new OrderIndexViewModel
                {
                    Id = item.Id,
                    Center = item.Center?.Descripcion,
                    Fecha = item.InitialDate.ToString("dd/MM/yyyy"),
                    Usuario = item.Usuario?.Nombreusuario,
                    Estado = item.Status.Descripcion,
                    NroPedido = item.NroPedido.ToString("000000")

                };


                switch (item.StatusId)
                {
                    case (int)StatusOrder.Solicitado:
                        orderIndexViewModel.Ver = true;
                        orderIndexViewModel.Modificar = true;
                        orderIndexViewModel.Eliminar = true;                       
                        break;
                    case (int) StatusOrder.Aceptado:
                        orderIndexViewModel.Ver = true;
                        if (usuario.Rol.IsAdmin)
                        {
                            orderIndexViewModel.Modificar = true;
                            orderIndexViewModel.Eliminar = true;
                        }
                        break;
                    case (int)StatusOrder.Rechazado:
                        orderIndexViewModel.Ver = true;
                        if (usuario.Rol.IsAdmin)
                        {
                            orderIndexViewModel.Eliminar = true;
                        }
                        break;
                    case (int)StatusOrder.Entregado:
                        orderIndexViewModel.Ver = true;
                        if (usuario.Rol.IsAdmin)
                        {
                            orderIndexViewModel.Eliminar = true;
                        }
                        break;
                }

                list.Add(orderIndexViewModel);

            }

            return View(list);
        }




        [HttpPost]
        public JsonResult GetOrderSearch()
        {
            try
            {
                Usuario usuario = db.Usuarios.Find(SessionHelper.GetUser());
                List<Order> orders = new List<Order>();
                List<OrderIndexViewModel> list = new List<OrderIndexViewModel>();
                if (!usuario.Rol.IsAdmin)
                    orders = db.Orders.Where(x => x.CenterId == usuario.CenterId).Include(o => o.Center).OrderByDescending(x => x.NroPedido).ToList();
                else
                    orders = db.Orders.Include(o => o.Center).Include(o => o.Usuario).OrderByDescending(x => x.NroPedido).ToList();
                


                foreach (var item in orders)
                {
                    OrderIndexViewModel orderIndexViewModel = new OrderIndexViewModel
                    {
                        Id = item.Id,
                        Center = item.Center?.Descripcion,
                        Fecha = item.InitialDate.ToString("dd/MM/yyyy"),
                        Usuario = item.Usuario?.Nombreusuario,
                        Estado = item.Status.Descripcion,
                        NroPedido = item.NroPedido.ToString("000000")

                    };

                    switch (item.StatusId)
                    {
                        case (int)StatusOrder.Solicitado:
                            orderIndexViewModel.Ver = true;
                            orderIndexViewModel.Modificar = true;
                            orderIndexViewModel.Eliminar = true;
                            break;
                        case (int)StatusOrder.Aceptado:
                            orderIndexViewModel.Ver = true;
                            if (usuario.Rol.IsAdmin)
                            {
                                orderIndexViewModel.Modificar = true;
                                orderIndexViewModel.Eliminar = true;
                            }
                            break;
                        case (int)StatusOrder.Rechazado:
                            orderIndexViewModel.Ver = true;
                            if (usuario.Rol.IsAdmin)
                            {
                                orderIndexViewModel.Eliminar = true;
                            }
                            break;
                        case (int)StatusOrder.Entregado:
                            orderIndexViewModel.Ver = true;
                            if (usuario.Rol.IsAdmin)
                            {
                                orderIndexViewModel.Eliminar = true;
                            }
                            break;
                    }

                    list.Add(orderIndexViewModel);
                }


                return Json(list.OrderByDescending(x => x.Id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public JsonResult SearchOrder(string NroPedido, string FechaDesde, string FechaHasta, string Centro, string Estado)
        {


            List<OrderIndexViewModel> turns = new List<OrderIndexViewModel>();
            turns = ArmarConsulta(NroPedido, FechaDesde, FechaHasta, Centro, Estado);


            return Json(turns, JsonRequestBehavior.AllowGet);

        }


        private List<OrderIndexViewModel> ArmarConsulta(string NroPedido, string FechaDesde, string FechaHasta, string Centro, string Estado)
        {
            int CentroId = 0;
            int intNroPedido = 0;
            int EstadoId = 0;
            Usuario usuario = db.Usuarios.Find(SessionHelper.GetUser());

            if (!String.IsNullOrEmpty(Centro))
                CentroId = Convert.ToInt32(Centro);

            if (!String.IsNullOrEmpty(NroPedido))
                intNroPedido = Convert.ToInt32(NroPedido);

            if (!String.IsNullOrEmpty(Estado))
                EstadoId = Convert.ToInt32(Estado);


            List<OrderIndexViewModel> orders = new List<OrderIndexViewModel>();
            DateTime? dtFechaDesde = null;
            DateTime? dtFechaHasta = null;
            List<Order> listaOrder = new List<Order>();

            if (!String.IsNullOrEmpty(FechaDesde))
                dtFechaDesde = Convert.ToDateTime(FechaDesde);

            if (!String.IsNullOrEmpty(FechaHasta))
                dtFechaHasta = Convert.ToDateTime(FechaHasta).AddDays(1).AddTicks(-1);


            try
            {


                if (!usuario.Rol.IsAdmin)
                    listaOrder = db.Orders.Where(x => x.CenterId == usuario.CenterId).Include(o => o.Center).ToList();
                else
                    listaOrder = db.Orders.Include(o => o.Center).Include(o => o.Usuario).ToList();

                var lista = listaOrder
                .Where(x => !string.IsNullOrEmpty(NroPedido) ? (x.NroPedido == intNroPedido && x.NroPedido != 0) : true)
                .Where(x => !string.IsNullOrEmpty(Centro) ? (x.CenterId == CentroId && x.Center.Descripcion != null) : true)
                .Where(x => !string.IsNullOrEmpty(Estado) ? (x.StatusId == EstadoId && x.Status.Descripcion != null) : true)
                .Where(x => !string.IsNullOrEmpty(FechaDesde) ? (x.InitialDate >= dtFechaDesde && x.InitialDate != null) : true)
                .Where(x => !string.IsNullOrEmpty(FechaHasta) ? (x.InitialDate <= dtFechaHasta && x.InitialDate != null) : true)
                .OrderByDescending(x => x.Id).Take(1000);
                ;

                foreach (var item in lista)
                {
                    OrderIndexViewModel orderIndexViewModel = new OrderIndexViewModel
                    {
                        Id = item.Id,
                        Center = item.Center?.Descripcion,
                        Fecha = item.InitialDate.ToString("dd/MM/yyyy"),
                        Usuario = item.Usuario?.Nombreusuario,
                        Estado = item.Status.Descripcion,
                        NroPedido = item.NroPedido.ToString("000000")

                    };

                    switch (item.StatusId)
                    {
                        case (int)StatusOrder.Solicitado:
                            orderIndexViewModel.Ver = true;
                            orderIndexViewModel.Modificar = true;
                            orderIndexViewModel.Eliminar = true;
                            break;
                        case (int)StatusOrder.Aceptado:
                            orderIndexViewModel.Ver = true;
                            if (usuario.Rol.IsAdmin)
                            {
                                orderIndexViewModel.Modificar = true;
                                orderIndexViewModel.Eliminar = true;
                            }
                            break;
                        case (int)StatusOrder.Rechazado:
                            orderIndexViewModel.Ver = true;
                            if (usuario.Rol.IsAdmin)
                            {
                                orderIndexViewModel.Eliminar = true;
                            }
                            break;
                        case (int)StatusOrder.Entregado:
                            orderIndexViewModel.Ver = true;
                            if (usuario.Rol.IsAdmin)
                            {
                                orderIndexViewModel.Eliminar = true;
                            }
                            break;
                    }

                    orders.Add(orderIndexViewModel);
                }

            }
            catch (Exception e)
            {

                throw;
            }

            return orders.ToList();
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            OrderCreateViewModel ordervm = new OrderCreateViewModel();
            Usuario usuario = db.Usuarios.Find(SessionHelper.GetUser());
            ViewBag.Title = "Pedido del Centro " + usuario.Center?.Descripcion;
            ordervm.CenterId = usuario.CenterId.Value;
            ordervm.OrderProductViewModels = new List<OrderProductViewModel>();
            var products = db.Products.Where(x => x.Enable).ToList();
            var centerProducts = db.CenterProducts.Where(x => x.CenterId == ordervm.CenterId).ToList();

            foreach (var item in products)
            {
                OrderProductViewModel orderProduct = new OrderProductViewModel
                {
                    Product = item.Descripcion,
                    ProductId = item.Id,
                    ProductType = item.SupplieMedical.Descripcion,
                    Quantity = 0,
                    OutStock = item.OutStock,
                    Stock = centerProducts.Where(x => x.ProductId == item.Id).Select(x => x.Stock).FirstOrDefault()
                };

                ordervm.OrderProductViewModels.Add(orderProduct); 
            }
           
            return View(ordervm);
        }

       
        public ActionResult Details(int Id)
        {
            OrderCreateViewModel ordervm = new OrderCreateViewModel();
            Usuario usuario = db.Usuarios.Find(SessionHelper.GetUser());
            Order order = db.Orders.Find(Id);
            ViewBag.Title = "Pedido del Centro " + order.Center.Descripcion;
            ViewBag.SubTitle = "Estado: " + order.Status.Descripcion;
            ordervm.CenterId = usuario.CenterId.Value;
            ordervm.OrderProductViewModels = new List<OrderProductViewModel>();
            var products = db.Products.Where(x => x.Enable).ToList();
            var centerProducts = db.CenterProducts.Where(x => x.CenterId == ordervm.CenterId).ToList();            

            foreach (var item in products)
            {
                OrderProductViewModel orderProduct = new OrderProductViewModel
                {
                    Product = item.Descripcion,
                    ProductId = item.Id,
                    ProductType = item.SupplieMedical.Descripcion,
                    Quantity =  order.OrderProducts.Where(x=>x.ProductId==item.Id).Select(x=>x.Quantity).FirstOrDefault(),
                    OutStock = item.OutStock,
                    Stock = centerProducts.Where(x => x.ProductId == item.Id).Select(x => x.Stock).FirstOrDefault()
                };

                ordervm.OrderProductViewModels.Add(orderProduct);
            }

            return View(ordervm);
        }


        public ActionResult Edit(int Id)
        {
            OrderEditViewModel ordervm = new OrderEditViewModel();
            Usuario usuario = db.Usuarios.Find(SessionHelper.GetUser());
            Order order = db.Orders.Find(Id);
            ViewBag.Title = "Pedido del Centro " + order.Center.Descripcion;
            ViewBag.SubTitle = "Estado: " + order.Status.Descripcion;
            ordervm.CenterId = order.CenterId;
            ordervm.Id = order.Id;
            ordervm.OrderProductViewModels = new List<OrderProductViewModel>();
            var products = db.Products.Where(x => x.Enable).ToList();
            var centerProducts = db.CenterProducts.Where(x => x.CenterId == ordervm.CenterId).ToList();

            foreach (var item in products)
            {
                OrderProductViewModel orderProduct = new OrderProductViewModel
                {
                    Product = item.Descripcion,
                    ProductId = item.Id,
                    ProductType = item.SupplieMedical.Descripcion,
                    Quantity = order.OrderProducts.Where(x => x.ProductId == item.Id).Select(x => x.Quantity).FirstOrDefault(),
                    OutStock = item.OutStock,
                    Stock = centerProducts.Where(x => x.ProductId == item.Id).Select(x => x.Stock).FirstOrDefault()
                };

                ordervm.OrderProductViewModels.Add(orderProduct);
            }

            return View(ordervm);
        }

        public JsonResult CreateOrder(OrderCreateViewModel orderCreate)
        {
            var responseObject = new
            {
                responseCode = -1
            };

            if (orderCreate == null)
            {
                return Json(new { responseCode = "-10" });
            }

            try
            {
                int centerId = orderCreate.CenterId;
                int statusId = (int) StatusOrder.Solicitado;
                int usuarioId = SessionHelper.GetUser();

                Order order = new Order
                {
                    CenterId = centerId,
                    InitialDate = DateTime.Now,
                    UsuarioId = usuarioId,
                    StatusId = statusId,
                    OrderProducts = new List<OrderProduct>(),
                    OrderTrackings = new List<OrderTracking>()
                };

                foreach (var item in orderCreate.OrderProductViewModels)
                {
                    OrderProduct orderProduct = new OrderProduct
                    {
                        OrderId = order.Id,
                        ProductId= item.ProductId,
                        Quantity = item.Quantity
                    };


                    order.OrderProducts.Add(orderProduct);
                }

                OrderTracking tracking = new OrderTracking
                {
                    OrderId = order.Id,
                    ToStatusId = statusId,
                    UsuarioId = usuarioId,
                    Fecha = DateTime.Now
                };

                order.OrderTrackings.Add(tracking);

                //Obtengo el Max Numero de pedido y le sumo 1

                var maxNumeroPedido = db.Orders.Max(x => x.NroPedido);
                order.NroPedido = maxNumeroPedido + 1;
                db.Orders.Add(order);
                db.SaveChanges();

                

                responseObject = new
                {
                    responseCode = 0
                };
            }
            catch (Exception e)
            {
                responseObject = new
                {
                    responseCode = -1
                };

            }

            return Json(responseObject);
        }


        public JsonResult EditOrder(OrderEditViewModel orderEdit)
        {
            var responseObject = new
            {
                responseCode = -1
            };

            if (orderEdit == null)
            {
                return Json(new { responseCode = "-10" });
            }

            try
            {
                int usuarioId = SessionHelper.GetUser();
                Order order = db.Orders.Find(orderEdit.Id);
                List<OrderProduct> order_add = new List<OrderProduct>();
                List<OrderProduct> order_upd = new List<OrderProduct>();
                List<OrderProduct> order_dtl = new List<OrderProduct>();

                //Estos son los que se modificaron o agregaron
                foreach (var item in orderEdit.OrderProductViewModels)
                {
                    var productoDB = order.OrderProducts.Where(x => x.ProductId == item.ProductId).FirstOrDefault();

                    OrderProduct orderProduct = new OrderProduct
                    {
                        OrderId = orderEdit.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    };

                    //Si el producto ya estaba dado de alta lo modifico
                    if (productoDB != null)
                    {
                        orderProduct.Id = productoDB.Id;
                        order_upd.Add(orderProduct);
                    }
                    else
                    {
                        order_add.Add(orderProduct);
                        
                    }
                    

                }
                //Ahora faltan los que ya no vienieron

                List<int> tempIdList = order_upd.Select(x => x.Id).ToList();

                order_dtl = order.OrderProducts.Where(q => !tempIdList.Contains(q.Id)).ToList();

                foreach (var item in order.OrderProducts)
                {
                    var orden = order_upd.Where(x => x.Id == item.Id).FirstOrDefault();
                    if (orden!=null)
                    {
                        item.Quantity = orden.Quantity;
                    }
                }

                foreach (var item in order_add)
                {
                    //order.OrderProducts.Add(item);
                    db.OrderProducts.Add(item);
                }


                foreach (var item in order_dtl)
                {
                    //order.OrderProducts.Remove(item);
                    db.OrderProducts.Remove(item);
                }

                db.Entry(order).State = EntityState.Modified;

                
                
                db.SaveChanges();



                responseObject = new
                {
                    responseCode = 0
                };
            }
            catch (Exception e)
            {
                responseObject = new
                {
                    responseCode = -1
                };

            }

            return Json(responseObject);
        }


        public JsonResult DeleteOrder(int Id)
        {
            var responseObject = new
            {
                responseCode = -1
            };

            if (Id == 0)
            {                
                return Json(new { responseCode = "-10" });
            }

            try
            {

                Order order = db.Orders.Find(Id);
                db.OrderProducts.RemoveRange(order.OrderProducts);
                db.OrderTrackings.RemoveRange(order.OrderTrackings);
                db.Orders.Remove(order);
                db.SaveChanges();

                responseObject = new
                {
                    responseCode = 0
                };
            }
            catch (Exception e)
            {
                responseObject = new
                {
                    responseCode = -1
                };

            }

            return Json(responseObject);
        }

        public JsonResult CambiarEstado(CambiarEstaudoViewModel viewModel)
        {
            var responseObject = new
            {
                responseCode = -1
            };

            if (viewModel == null)
            {
                return Json(new { responseCode = "-10" });
            }

            try
            {
                Order order = db.Orders.Find(viewModel.Id);
                int oldStatudId = order.StatusId;
                order.StatusId = viewModel.StatusId;
                bool esValidoElCambio = false;

                //Verifico si el cambio de estado es correcto
                switch (oldStatudId)
                {
                    case (int)StatusOrder.Solicitado:
                        //El solicitado Puede pasar a Aceptado, rechazado
                        esValidoElCambio = (order.StatusId == (int) StatusOrder.Aceptado || order.StatusId == (int) StatusOrder.Rechazado);
                        break;
                    case (int)StatusOrder.Aceptado:
                        //El entregado Puede pasar a Solicitado, Entregado o rechazado
                        esValidoElCambio = (order.StatusId == (int)StatusOrder.Solicitado || order.StatusId == (int)StatusOrder.Entregado || order.StatusId == (int)StatusOrder.Rechazado) ;
                        break;
                    case (int)StatusOrder.Rechazado:
                        //El solicitado Puede pasar a Aceptado
                        esValidoElCambio = (order.StatusId == (int)StatusOrder.Aceptado);
                        break;
                    case (int)StatusOrder.Entregado:
                        //El solicitado Puede pasar a Aceptado
                        esValidoElCambio = (order.StatusId == (int)StatusOrder.Aceptado);
                        break;
                    default:
                        esValidoElCambio = false;
                        break;
                }

                if (esValidoElCambio==false)
                    return Json(new { responseCode = "-10" });

                db.Entry(order).State = EntityState.Modified;

                OrderTracking tracking = new OrderTracking
                {
                    Fecha = DateTime.Now,
                    OrderId = viewModel.Id,
                    ToStatusId = oldStatudId,
                    SinceStatusId = viewModel.StatusId,
                    UsuarioId = SessionHelper.GetUser(),
                    Observation = viewModel.Observation
                };

                db.OrderTrackings.Add(tracking);

                db.SaveChanges();

                responseObject = new
                {
                    responseCode = 0
                };
            }
            catch (Exception e)
            {
                responseObject = new
                {
                    responseCode = -1
                };

            }

            return Json(responseObject);
        }

        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
