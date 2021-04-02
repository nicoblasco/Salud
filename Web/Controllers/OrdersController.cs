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
            ViewBag.Imprimir = PermissionViewModel.TienePermisoAcesso(WindowHelper.GetWindowId(ModuleDescription, WindowDescription));
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


        // GET: Orders
        public ActionResult ReportInsumos()
        {
            List<OrderIndexViewModel> list = new List<OrderIndexViewModel>();
            Usuario usuario = db.Usuarios.Find(SessionHelper.GetUser());
            List<Order> orders = new List<Order>();
            List<Center> lCentros = new List<Center>();
            List<Product> lMedicamentos = new List<Product>();
            if (usuario.Rol.IsAdmin)
                lCentros = db.Centers.Where(x => x.Enable).ToList();
            else
                lCentros = db.Centers.Where(x => x.Id == usuario.CenterId).ToList();
            lMedicamentos = db.Products.Where(x=>x.Enable).ToList();
            ViewBag.listaCentros = lCentros;
            ViewBag.listaMedicamentos = lMedicamentos;
            ViewBag.isAdmin = usuario.Rol.IsAdmin;


            return View();
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

        public JsonResult SearchReporteInsumo(string FechaDesde, string FechaHasta, string Centro, string Medicamento)
        {


            OrderReporteInsumoViewModel order = new OrderReporteInsumoViewModel();
            order = ArmarConsultaReporte(FechaDesde, FechaHasta, Centro, Medicamento);


            return Json(order, JsonRequestBehavior.AllowGet);

        }

        private OrderReporteInsumoViewModel ArmarConsultaReporte(string FechaDesde, string FechaHasta, string Centro, string Medicamento)
        {
            int CentroId = 0;            
            int MedicamentoId = 0;
            int totalSolicitados = 0;
            int totalEntregados = 0;
            Usuario usuario = db.Usuarios.Find(SessionHelper.GetUser());

            if (!String.IsNullOrEmpty(Centro))
                CentroId = Convert.ToInt32(Centro);

            if (!String.IsNullOrEmpty(Medicamento))
                MedicamentoId = Convert.ToInt32(Medicamento);


            OrderReporteInsumoViewModel order = new OrderReporteInsumoViewModel();
            order.Detalle = new List<OrderReporteInsumoDetalleViewModel>();
            DateTime? dtFechaDesde = null;
            DateTime? dtFechaHasta = null;

            if (!String.IsNullOrEmpty(FechaDesde))
                dtFechaDesde = Convert.ToDateTime(FechaDesde);

            if (!String.IsNullOrEmpty(FechaHasta))
                dtFechaHasta = Convert.ToDateTime(FechaHasta).AddDays(1).AddTicks(-1);

            List<OrderProduct> listaProducts = new List<OrderProduct>();

            try
            {
                if (!usuario.Rol.IsAdmin)
                    listaProducts = db.OrderProducts.Where(x => x.Order.CenterId == usuario.CenterId && x.Order.StatusId== (int)StatusOrder.Entregado).Include(x=>x.Order.Center).Include(x=>x.Product).ToList();
                else
                    listaProducts = db.OrderProducts.Where(x => x.Order.StatusId == (int)StatusOrder.Entregado) .Include(o => o.Order.Center).Include(x => x.Product).ToList();

                var lista = listaProducts
                .Where(x => !string.IsNullOrEmpty(Centro) ? (x.Order.CenterId == CentroId && x.Order.Center.Descripcion != null) : true)
                .Where(x => !string.IsNullOrEmpty(Medicamento) ? (x.ProductId == MedicamentoId && x.Product.Descripcion != null) : true)
                .Where(x => !string.IsNullOrEmpty(FechaDesde) ? (x.Order.InitialDate >= dtFechaDesde && x.Order.InitialDate != null) : true)
                .Where(x => !string.IsNullOrEmpty(FechaHasta) ? (x.Order.InitialDate <= dtFechaHasta && x.Order.InitialDate != null) : true)
                .OrderByDescending(x => x.Id);
                ;

                foreach (var item in lista)
                {
                    OrderReporteInsumoDetalleViewModel detail = new OrderReporteInsumoDetalleViewModel
                    {
                        Product = item.Product.Descripcion,
                        Date = item.Order.InitialDate.ToString("dd/MM/yyyy"),
                        Center = item.Order.Center.Descripcion,
                        NroPedido = item.Order.NroPedido,
                        Quantity = item.Quantity,
                        QuantityDelivered = item.QuantityDelivered                        

                    };

                    totalSolicitados += item.Quantity;
                    totalEntregados += item.QuantityDelivered;


                    order.Detalle.Add(detail);
                }

                order.TotalEntregados = totalEntregados;
                order.TotalSolicitados = totalSolicitados;

            }
            catch (Exception e)
            {

                throw;
            }


            return order;
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



        [HttpPost]
        public JsonResult GetTrackings(int Id)
        {
            try
            {
                Order order = db.Orders.Where(x => x.Id == Id).Include(x => x.OrderTrackings).FirstOrDefault();
                List<OrderTrackingViewModel> list = new List<OrderTrackingViewModel>();
                List<Status> statuses = db.Status.ToList();


                foreach (var item in order.OrderTrackings)
                {
                    OrderTrackingViewModel orderTracking = new OrderTrackingViewModel
                    {
                        Id = item.Id,
                        EstadoDesde  = statuses.Where(x=>x.Id==item?.SinceStatusId).FirstOrDefault()?.Descripcion,
                        EstadoHasta = statuses.Where(x => x.Id == item.ToStatusId).FirstOrDefault()?.Descripcion,
                        Usuario = item.Usuario.Nombreusuario,
                        Observacion = item.Observation,
                        Fecha = item.Fecha.ToString("dd/MM/yyyy")

                    };


                    list.Add(orderTracking);
                }


                return Json(list.OrderByDescending(x => x.Id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                throw;
            }
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
           // var centerProducts = db.CenterProducts.Where(x => x.CenterId == ordervm.CenterId).ToList();
            var center = db.Centers.Where(x => x.Id == ordervm.CenterId).Include(x => x.CenterProducts).FirstOrDefault();
            var centerProducts = center.CenterProducts;
            ordervm.TieneStockSinCargar = center.TieneStockSinCargar();

            foreach (var item in products)
            {
                OrderProductViewModel orderProduct = new OrderProductViewModel
                {
                    Product = item.Descripcion,
                    ProductId = item.Id,
                    ProductType = item.SupplieMedical.Descripcion,
                    Quantity = 0,
                    QuantityDelivered =0,
                    OutStock = item.OutStock,
                    Stock = centerProducts.Where(x => x.ProductId == item.Id).Select(x => x.Stock).FirstOrDefault()
                };

                ordervm.OrderProductViewModels.Add(orderProduct); 
            }
           
            return View(ordervm);
        }

       
        public ActionResult Details(int Id)
        {
            OrderEditViewModel ordervm = new OrderEditViewModel();
            Usuario usuario = db.Usuarios.Find(SessionHelper.GetUser());
            Order order = db.Orders.Find(Id);
            ViewBag.Title = "Pedido del Centro " + order.Center.Descripcion;
            ViewBag.SubTitle = "Estado: " + order.Status.Descripcion;
            ViewBag.isAdmin = usuario.Rol.IsAdmin;
            ordervm.Id = order.Id;
            ordervm.CenterId = order.CenterId;
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
                    QuantityDelivered = order.OrderProducts.Where(x => x.ProductId == item.Id).Select(x => x.QuantityDelivered).FirstOrDefault(),
                    Quantity = order.OrderProducts.Where(x => x.ProductId == item.Id).Select(x => x.Quantity).FirstOrDefault(),
                    OutStock = item.OutStock,
                    Stock = centerProducts.Where(x => x.ProductId == item.Id).Select(x => x.Stock).FirstOrDefault()
                };

                ordervm.OrderProductViewModels.Add(orderProduct);
            }

            return View(ordervm);
        }

        public JsonResult Get(int id)
        {
            var order= GetOrder(id);
            return Json(order, JsonRequestBehavior.AllowGet);
        }

        public OrderEditViewModel GetOrder(int id)
        {
            OrderEditViewModel ordervm = new OrderEditViewModel();
            Order order = db.Orders.Find(id);
            ordervm.CenterId = order.CenterId;
            ordervm.Id = order.Id;
            ViewBag.PasaAEntregado = (order.Status.Id == (int)StatusOrder.Solicitado);
            ordervm.OrderProductViewModels = new List<OrderProductViewModel>();
            ordervm.Center = order.Center.Descripcion;
            ordervm.Date = order.InitialDate.ToString("dd/MM/yyyy");
            ordervm.NroPedido = order.NroPedido.ToString();
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
                    QuantityDelivered = order.OrderProducts.Where(x => x.ProductId == item.Id).Select(x => x.QuantityDelivered).FirstOrDefault(),
                    OutStock = item.OutStock,
                    Stock = centerProducts.Where(x => x.ProductId == item.Id).Select(x => x.Stock).FirstOrDefault()
                };

                ordervm.OrderProductViewModels.Add(orderProduct);
            }
            return ordervm;
        }


        public ActionResult Edit(int Id)
        {

            Usuario usuario = db.Usuarios.Find(SessionHelper.GetUser());
            Order order = db.Orders.Find(Id);            
            ViewBag.Title = "Pedido del Centro " + order.Center.Descripcion;
            ViewBag.SubTitle = "Estado: " + order.Status.Descripcion;
            ViewBag.isAdmin = usuario.Rol.IsAdmin;


            return View(GetOrder(Id));
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
                        Quantity = item.Quantity,
                        QuantityDelivered = 0,
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

                AuditHelper.Auditar("Alta", order.Id.ToString(), className, ModuleDescription, WindowDescription);

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
            Usuario usuario = db.Usuarios.Find(SessionHelper.GetUser());
            bool isAdmin = usuario.Rol.IsAdmin;
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
                        Quantity = productoDB?.Quantity??0
                    };

                    //Si es Admin modifica la cantidad entregada, si es un operador modifica la cantidad solicitada
                    if (isAdmin)
                    {
                        orderProduct.QuantityDelivered = item.QuantityDelivered;
                    }
                    else
                        orderProduct.Quantity = item.Quantity;

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

                    order_dtl = order.OrderProducts.Where(x=>x.QuantityDelivered==0).Where(q => !tempIdList.Contains(q.Id)).ToList();

                    foreach (var item in order.OrderProducts)
                    {
                        var orden = order_upd.Where(x => x.Id == item.Id).FirstOrDefault();
                        if (orden != null)
                        {
                        if (isAdmin)
                            item.QuantityDelivered = orden.QuantityDelivered;
                        else
                            item.Quantity = orden.Quantity;
                    }
                    }

                    foreach (var item in order_add)
                    {
                        //order.OrderProducts.Add(item);
                        db.OrderProducts.Add(item);
                    }

                if (!isAdmin) {
                    foreach (var item in order_dtl)
                    {
                        //order.OrderProducts.Remove(item);
                        db.OrderProducts.Remove(item);
                    }
                }


                


                db.Entry(order).State = EntityState.Modified;

                
                
                db.SaveChanges();


                if (orderEdit.ChangeStatus)
                {
                    // order.StatusId = (int)StatusOrder.Entregado;
                    CambiarEstaudoViewModel cambiarEstado = new CambiarEstaudoViewModel
                    {
                        Id = order.Id,
                        StatusId = (int)StatusOrder.Entregado
                    };
                    CambiarEstadoLogic(cambiarEstado);
                }


                AuditHelper.Auditar("Modificacion", order.Id.ToString(), className, ModuleDescription, WindowDescription);
                responseObject = new
                {
                    responseCode = 0
                };
            }
            catch (Exception e)
            {
                responseObject = new
                {
                    responseCode = -10
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

                AuditHelper.Auditar("Baja", Id.ToString(), className, ModuleDescription, WindowDescription);

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

        private void CambiarEstadoLogic(CambiarEstaudoViewModel viewModel)
        {
            Order order = db.Orders.Where(x => x.Id == viewModel.Id).Include(x => x.OrderProducts).FirstOrDefault();
            int oldStatudId = order.StatusId;
            order.StatusId = viewModel.StatusId;
            bool esValidoElCambio = false;

            //Verifico si el cambio de estado es correcto
            switch (oldStatudId)
            {
                case (int)StatusOrder.Solicitado:
                    //El solicitado Puede pasar a Aceptado, rechazado
                    esValidoElCambio = (order.StatusId == (int)StatusOrder.Aceptado || order.StatusId == (int)StatusOrder.Rechazado);
                    break;
                case (int)StatusOrder.Aceptado:
                    //El entregado Puede pasar a Solicitado, Entregado o rechazado
                    esValidoElCambio = (order.StatusId == (int)StatusOrder.Solicitado || order.StatusId == (int)StatusOrder.Entregado || order.StatusId == (int)StatusOrder.Rechazado);
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

            //if (esValidoElCambio == false)
            //    return Json(new { responseCode = "-10" });


            //Si el estado es Entregado hace una actualizacion automatica del stock
            if (order.StatusId == (int)StatusOrder.Entregado)
            {
                //Recupero los que productos que tiene dados de alta
                var centerProducts = db.CenterProducts.Where(x => x.CenterId == order.CenterId).ToList();

                foreach (var item in order.OrderProducts)
                {
                    var prod = centerProducts.Where(x => x.ProductId == item.ProductId).FirstOrDefault();
                    if (prod != null)
                    {
                        //Ya axiste
                        prod.Stock = prod.Stock + item.QuantityDelivered;
                        //products_upd.Add(prod);
                        db.Entry(prod).State = EntityState.Modified;
                    }
                    else
                    {
                        //Si no existe, lo impacto
                        CenterProduct centerProduct = new CenterProduct
                        {
                            CenterId = item.Order.CenterId,
                            ProductId = item.ProductId,
                            Stock = item.QuantityDelivered
                        };

                        db.CenterProducts.Add(centerProduct);
                    }



                }


            }


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

            AuditHelper.Auditar("Modificacion", order.Id.ToString(), className, ModuleDescription, WindowDescription);

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

                CambiarEstadoLogic(viewModel);
                responseObject = new
                {
                    responseCode = 0
                };
            }
            catch (Exception e)
            {
                responseObject = new
                {
                    responseCode = -10
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
