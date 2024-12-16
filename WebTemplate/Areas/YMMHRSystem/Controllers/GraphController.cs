using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YMMHRSystemLogic;

namespace WebTemplate.Areas.YMMHRSystem.Controllers
{
    [SessionExpireFilter]
    public class GraphController : Controller
    {

        Process process = new Process();
        Graph graph = new Graph();

        // TRANSPORT
        public ActionResult Transport()
        {
            try
            {
                if (Permission.QueryPermission("TRANSPORTGRAPH.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    return View("~/Areas/YMMHRSystem/Views/Graph/Transport.cshtml");
                }
                else
                {
                    return View("~/Views/Shared/AccessDenied.cshtml");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult LoadTransportGraphs()
        {
            try
            {
                // MONTHLY & ACCUMULATED TRANSPORT SERVICES
                List<DtoGraphInt> monthlyTransports = graph.GetMonthlyTransports(Session["KiFirstHalfYear"].ToString());
                List<DtoGraphInt> monthlyExtraTransports = graph.GetMonthlyExtraTransports(Session["KiFirstHalfYear"].ToString());
                List<DtoGraphString> monthlyTransportsFinal = new List<DtoGraphString>(){
                    new DtoGraphString() { X = "April", Y = "0" },
                    new DtoGraphString() { X = "May", Y = "0" },
                    new DtoGraphString() { X = "June", Y = "0" },
                    new DtoGraphString() { X = "July", Y = "0" },
                    new DtoGraphString() { X = "August", Y = "0" },
                    new DtoGraphString() { X = "September", Y = "0" },
                    new DtoGraphString() { X = "October", Y = "0" },
                    new DtoGraphString() { X = "November", Y = "0" },
                    new DtoGraphString() { X = "December", Y = "0" },
                    new DtoGraphString() { X = "January", Y = "0" },
                    new DtoGraphString() { X = "February", Y = "0" },
                    new DtoGraphString() { X = "March", Y = "0" },
                };
                List<DtoGraphString> monthlyExtraTransportsFinal = new List<DtoGraphString>(){
                    new DtoGraphString() { X = "April", Y = "0" },
                    new DtoGraphString() { X = "May", Y = "0" },
                    new DtoGraphString() { X = "June", Y = "0" },
                    new DtoGraphString() { X = "July", Y = "0" },
                    new DtoGraphString() { X = "August", Y = "0" },
                    new DtoGraphString() { X = "September", Y = "0" },
                    new DtoGraphString() { X = "October", Y = "0" },
                    new DtoGraphString() { X = "November", Y = "0" },
                    new DtoGraphString() { X = "December", Y = "0" },
                    new DtoGraphString() { X = "January", Y = "0" },
                    new DtoGraphString() { X = "February", Y = "0" },
                    new DtoGraphString() { X = "March", Y = "0" },
                };

                foreach (var item in monthlyTransportsFinal)
                {
                    var itemFound = monthlyTransports.FirstOrDefault(x => x.X == DateTime.ParseExact(item.X, "MMMM", CultureInfo.InvariantCulture).Month);
                    if (itemFound != null)
                    {
                        item.Y = itemFound.Y.ToString();
                    }
                }

                List<string> monthlyTransportsX = monthlyTransportsFinal.Select(o => o.X).ToList();
                List<string> monthlyTransportsY = monthlyTransportsFinal.Select(o => o.Y).ToList();

                foreach (var item in monthlyExtraTransportsFinal)
                {
                    var itemFound = monthlyExtraTransports.FirstOrDefault(x => x.X == DateTime.ParseExact(item.X, "MMMM", CultureInfo.InvariantCulture).Month);
                    if (itemFound != null)
                    {
                        item.Y = itemFound.Y.ToString();
                    }
                }
                List<string> monthlyExtraTransportsY = monthlyExtraTransportsFinal.Select(o => o.Y).ToList();

                DtoGraph totalTransport = graph.GetTotalTransport(Session["KiFirstHalfYear"].ToString());
                DtoGraph totalExtraTransport = graph.GetTotalExtraTransport(Session["KiFirstHalfYear"].ToString());
                List<string> accumulatedTransportX = new List<string>();
                accumulatedTransportX.Add(totalTransport.X);
                accumulatedTransportX.Add(totalExtraTransport.X);
                List<int> accumulatedTransportY = new List<int>();
                accumulatedTransportY.Add(totalTransport.Y);
                accumulatedTransportY.Add(totalExtraTransport.Y);

                // SERVICES BY PROCESS
                List<DtoProcess> processes = process.LoadMultiple();
                List<DtoGraph> totalTransportByProcess = graph.GetTotalExtraTransportByProcess(Session["KiFirstHalfYear"].ToString());
                List<DtoGraph> totalTransportByProcessFinal = new List<DtoGraph>();

                foreach (var item in processes)
                {
                    totalTransportByProcessFinal.Add(new DtoGraph
                    {
                        X = item.Name,
                        Y = 0
                    });
                }

                foreach (var item in totalTransportByProcessFinal)
                {
                    var itemFound = totalTransportByProcess.FirstOrDefault(x => x.X == item.X);
                    if (itemFound != null)
                    {
                        item.Y = itemFound.Y;
                    }
                }
                List<string> totalTransportByProcessX = totalTransportByProcessFinal.Select(o => o.X).ToList();
                List<int> totalTransportByProcessY = totalTransportByProcessFinal.Select(o => o.Y).ToList();

                // COSTS BY PROCESS
                List<DtoGraphStringDecimal> totalTransportCostByProcess = graph.GetCostExtraTransportByProcess(Session["KiFirstHalfYear"].ToString());
                List<DtoGraphStringDecimal> totalTransportCostByProcessFinal = new List<DtoGraphStringDecimal>();

                foreach (var item in processes)
                {
                    totalTransportCostByProcessFinal.Add(new DtoGraphStringDecimal
                    {
                        X = item.Name,
                        Y = 0.0m
                    });
                }

                foreach (var item in totalTransportCostByProcessFinal)
                {
                    var itemFound = totalTransportCostByProcess.FirstOrDefault(x => x.X == item.X);
                    if (itemFound != null)
                    {
                        item.Y = itemFound.Y;
                    }
                }
                List<string> totalTransportCostByProcessX = totalTransportCostByProcessFinal.Select(o => o.X).ToList();
                List<decimal> totalTransportCostByProcessY = totalTransportCostByProcessFinal.Select(o => o.Y).ToList();

                // MONTHLY & ACCUMULATED TRANSPORT COSTS
                List<DtoGraphDecimal> monthlyTransportCosts = graph.GetMonthlyTransportCosts(Session["KiFirstHalfYear"].ToString());
                List<DtoGraphDecimal> monthlyExtraTransportCosts = graph.GetMonthlyExtraTransportCosts(Session["KiFirstHalfYear"].ToString());
                List<DtoGraphString> monthlyTransportCostsFinal = new List<DtoGraphString>(){
                    new DtoGraphString() { X = "April", Y = "0" },
                    new DtoGraphString() { X = "May", Y = "0" },
                    new DtoGraphString() { X = "June", Y = "0" },
                    new DtoGraphString() { X = "July", Y = "0" },
                    new DtoGraphString() { X = "August", Y = "0" },
                    new DtoGraphString() { X = "September", Y = "0" },
                    new DtoGraphString() { X = "October", Y = "0" },
                    new DtoGraphString() { X = "November", Y = "0" },
                    new DtoGraphString() { X = "December", Y = "0" },
                    new DtoGraphString() { X = "January", Y = "0" },
                    new DtoGraphString() { X = "February", Y = "0" },
                    new DtoGraphString() { X = "March", Y = "0" },
                };
                List<DtoGraphString> monthlyExtraTransportCostsFinal = new List<DtoGraphString>(){
                    new DtoGraphString() { X = "April", Y = "0" },
                    new DtoGraphString() { X = "May", Y = "0" },
                    new DtoGraphString() { X = "June", Y = "0" },
                    new DtoGraphString() { X = "July", Y = "0" },
                    new DtoGraphString() { X = "August", Y = "0" },
                    new DtoGraphString() { X = "September", Y = "0" },
                    new DtoGraphString() { X = "October", Y = "0" },
                    new DtoGraphString() { X = "November", Y = "0" },
                    new DtoGraphString() { X = "December", Y = "0" },
                    new DtoGraphString() { X = "January", Y = "0" },
                    new DtoGraphString() { X = "February", Y = "0" },
                    new DtoGraphString() { X = "March", Y = "0" },
                };

                foreach (var item in monthlyTransportCostsFinal)
                {
                    var itemFound = monthlyTransportCosts.FirstOrDefault(x => x.X == DateTime.ParseExact(item.X, "MMMM", CultureInfo.InvariantCulture).Month);
                    if (itemFound != null)
                    {
                        item.Y = itemFound.Y.ToString();
                    }
                }

                List<string> monthlyTransportCostsX = monthlyTransportCostsFinal.Select(o => o.X).ToList();
                List<string> monthlyTransportCostsY = monthlyTransportCostsFinal.Select(o => o.Y).ToList();

                foreach (var item in monthlyExtraTransportCostsFinal)
                {
                    var itemFound = monthlyExtraTransportCosts.FirstOrDefault(x => x.X == DateTime.ParseExact(item.X, "MMMM", CultureInfo.InvariantCulture).Month);
                    if (itemFound != null)
                    {
                        item.Y = itemFound.Y.ToString();
                    }
                }
                List<string> monthlyExtraTransportCostsY = monthlyExtraTransportCostsFinal.Select(o => o.Y).ToList();

                DtoGraphStringDecimal totalTransportCost = graph.GetTotalTransportCost(Session["KiFirstHalfYear"].ToString());
                DtoGraphStringDecimal totalExtraTransportCost = graph.GetTotalExtraTransportCost(Session["KiFirstHalfYear"].ToString());
                List<string> accumulatedTransportCostX = new List<string>();
                accumulatedTransportCostX.Add(totalTransportCost.X);
                accumulatedTransportCostX.Add(totalExtraTransportCost.X);
                List<decimal> accumulatedTransportCostY = new List<decimal>();
                accumulatedTransportCostY.Add(totalTransportCost.Y);
                accumulatedTransportCostY.Add(totalExtraTransportCost.Y);


                List<object> iData = new List<object>();
                iData.Add(monthlyTransportsX);
                iData.Add(monthlyTransportsY);
                iData.Add(monthlyExtraTransportsY);
                iData.Add(accumulatedTransportX);
                iData.Add(accumulatedTransportY);
                iData.Add(totalTransportByProcessX);
                iData.Add(totalTransportByProcessY);
                iData.Add(monthlyTransportCostsX);
                iData.Add(monthlyTransportCostsY);
                iData.Add(monthlyExtraTransportCostsY);
                iData.Add(accumulatedTransportCostX);
                iData.Add(accumulatedTransportCostY);
                iData.Add(totalTransportCostByProcessX);
                iData.Add(totalTransportCostByProcessY);


                return Json(iData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult RefreshServicesByType(string firstHalfKi)
        {
            try
            {
                List<DtoGraphInt> monthlyTransports = graph.GetMonthlyTransports(firstHalfKi);
                List<DtoGraphInt> monthlyExtraTransports = graph.GetMonthlyExtraTransports(firstHalfKi);
                List<DtoGraphString> monthlyTransportsFinal = new List<DtoGraphString>(){
                    new DtoGraphString() { X = "April", Y = "0" },
                    new DtoGraphString() { X = "May", Y = "0" },
                    new DtoGraphString() { X = "June", Y = "0" },
                    new DtoGraphString() { X = "July", Y = "0" },
                    new DtoGraphString() { X = "August", Y = "0" },
                    new DtoGraphString() { X = "September", Y = "0" },
                    new DtoGraphString() { X = "October", Y = "0" },
                    new DtoGraphString() { X = "November", Y = "0" },
                    new DtoGraphString() { X = "December", Y = "0" },
                    new DtoGraphString() { X = "January", Y = "0" },
                    new DtoGraphString() { X = "February", Y = "0" },
                    new DtoGraphString() { X = "March", Y = "0" },
                };
                List<DtoGraphString> monthlyExtraTransportsFinal = new List<DtoGraphString>(){
                    new DtoGraphString() { X = "April", Y = "0" },
                    new DtoGraphString() { X = "May", Y = "0" },
                    new DtoGraphString() { X = "June", Y = "0" },
                    new DtoGraphString() { X = "July", Y = "0" },
                    new DtoGraphString() { X = "August", Y = "0" },
                    new DtoGraphString() { X = "September", Y = "0" },
                    new DtoGraphString() { X = "October", Y = "0" },
                    new DtoGraphString() { X = "November", Y = "0" },
                    new DtoGraphString() { X = "December", Y = "0" },
                    new DtoGraphString() { X = "January", Y = "0" },
                    new DtoGraphString() { X = "February", Y = "0" },
                    new DtoGraphString() { X = "March", Y = "0" },
                };

                foreach (var item in monthlyTransportsFinal)
                {
                    var itemFound = monthlyTransports.FirstOrDefault(x => x.X == DateTime.ParseExact(item.X, "MMMM", CultureInfo.InvariantCulture).Month);
                    if (itemFound != null)
                    {
                        item.Y = itemFound.Y.ToString();
                    }
                }

                List<string> monthlyTransportsY = monthlyTransportsFinal.Select(o => o.Y).ToList();

                foreach (var item in monthlyExtraTransportsFinal)
                {
                    var itemFound = monthlyExtraTransports.FirstOrDefault(x => x.X == DateTime.ParseExact(item.X, "MMMM", CultureInfo.InvariantCulture).Month);
                    if (itemFound != null)
                    {
                        item.Y = itemFound.Y.ToString();
                    }
                }
                List<string> monthlyExtraTransportsY = monthlyExtraTransportsFinal.Select(o => o.Y).ToList();

                DtoGraph totalTransport = graph.GetTotalTransport(firstHalfKi);
                DtoGraph totalExtraTransport = graph.GetTotalExtraTransport(firstHalfKi);
                List<int> accumulatedTransportY = new List<int>();
                accumulatedTransportY.Add(totalTransport.Y);
                accumulatedTransportY.Add(totalExtraTransport.Y);

                List<object> iData = new List<object>();
                iData.Add(monthlyTransportsY);
                iData.Add(monthlyExtraTransportsY);
                iData.Add(accumulatedTransportY);

                return Json(iData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult RefreshServicesByProcess(string firstHalfKi, string month)
        {
            try
            {
                List<DtoProcess> processes = process.LoadMultiple();
                List<DtoGraph> totalTransportByProcess = graph.GetTotalExtraTransportByProcess(firstHalfKi, month);
                List<DtoGraph> totalTransportByProcessFinal = new List<DtoGraph>();

                foreach (var item in processes)
                {
                    totalTransportByProcessFinal.Add(new DtoGraph
                    {
                        X = item.Name,
                        Y = 0
                    });
                }

                foreach (var item in totalTransportByProcessFinal)
                {
                    var itemFound = totalTransportByProcess.FirstOrDefault(x => x.X == item.X);
                    if (itemFound != null)
                    {
                        item.Y = itemFound.Y;
                    }
                }
                List<int> totalTransportByProcessY = totalTransportByProcessFinal.Select(o => o.Y).ToList();

                List<object> iData = new List<object>();
                iData.Add(totalTransportByProcessY);

                return Json(iData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult RefreshExtraTransportCostByProcess(string firstHalfKi, string month)
        {
            try
            {
                List<DtoProcess> processes = process.LoadMultiple();
                List<DtoGraphStringDecimal> totalTransportCostByProcess = graph.GetCostExtraTransportByProcess(firstHalfKi, month);
                List<DtoGraphStringDecimal> totalTransportCostByProcessFinal = new List<DtoGraphStringDecimal>();

                foreach (var item in processes)
                {
                    totalTransportCostByProcessFinal.Add(new DtoGraphStringDecimal
                    {
                        X = item.Name,
                        Y = 0.0m
                    });
                }

                foreach (var item in totalTransportCostByProcessFinal)
                {
                    var itemFound = totalTransportCostByProcess.FirstOrDefault(x => x.X == item.X);
                    if (itemFound != null)
                    {
                        item.Y = itemFound.Y;
                    }
                }
                List<decimal> totalTransportCostByProcessY = totalTransportCostByProcessFinal.Select(o => o.Y).ToList();

                List<object> iData = new List<object>();
                iData.Add(totalTransportCostByProcessY);

                return Json(iData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult RefreshTotalTransportCost(string firstHalfKi)
        {
            try
            {
                List<DtoGraphDecimal> monthlyTransportCosts = graph.GetMonthlyTransportCosts(firstHalfKi);
                List<DtoGraphDecimal> monthlyExtraTransportCosts = graph.GetMonthlyExtraTransportCosts(firstHalfKi);
                List<DtoGraphString> monthlyTransportCostsFinal = new List<DtoGraphString>(){
                    new DtoGraphString() { X = "April", Y = "0" },
                    new DtoGraphString() { X = "May", Y = "0" },
                    new DtoGraphString() { X = "June", Y = "0" },
                    new DtoGraphString() { X = "July", Y = "0" },
                    new DtoGraphString() { X = "August", Y = "0" },
                    new DtoGraphString() { X = "September", Y = "0" },
                    new DtoGraphString() { X = "October", Y = "0" },
                    new DtoGraphString() { X = "November", Y = "0" },
                    new DtoGraphString() { X = "December", Y = "0" },
                    new DtoGraphString() { X = "January", Y = "0" },
                    new DtoGraphString() { X = "February", Y = "0" },
                    new DtoGraphString() { X = "March", Y = "0" },
                };
                List<DtoGraphString> monthlyExtraTransportCostsFinal = new List<DtoGraphString>(){
                    new DtoGraphString() { X = "April", Y = "0" },
                    new DtoGraphString() { X = "May", Y = "0" },
                    new DtoGraphString() { X = "June", Y = "0" },
                    new DtoGraphString() { X = "July", Y = "0" },
                    new DtoGraphString() { X = "August", Y = "0" },
                    new DtoGraphString() { X = "September", Y = "0" },
                    new DtoGraphString() { X = "October", Y = "0" },
                    new DtoGraphString() { X = "November", Y = "0" },
                    new DtoGraphString() { X = "December", Y = "0" },
                    new DtoGraphString() { X = "January", Y = "0" },
                    new DtoGraphString() { X = "February", Y = "0" },
                    new DtoGraphString() { X = "March", Y = "0" },
                };

                foreach (var item in monthlyTransportCostsFinal)
                {
                    var itemFound = monthlyTransportCosts.FirstOrDefault(x => x.X == DateTime.ParseExact(item.X, "MMMM", CultureInfo.InvariantCulture).Month);
                    if (itemFound != null)
                    {
                        item.Y = itemFound.Y.ToString();
                    }
                }

                List<string> monthlyTransportCostsY = monthlyTransportCostsFinal.Select(o => o.Y).ToList();

                foreach (var item in monthlyExtraTransportCostsFinal)
                {
                    var itemFound = monthlyExtraTransportCosts.FirstOrDefault(x => x.X == DateTime.ParseExact(item.X, "MMMM", CultureInfo.InvariantCulture).Month);
                    if (itemFound != null)
                    {
                        item.Y = itemFound.Y.ToString();
                    }
                }
                List<string> monthlyExtraTransportCostsY = monthlyExtraTransportCostsFinal.Select(o => o.Y).ToList();

                DtoGraphStringDecimal totalTransportCost = graph.GetTotalTransportCost(firstHalfKi);
                DtoGraphStringDecimal totalExtraTransportCost = graph.GetTotalExtraTransportCost(firstHalfKi);
                List<decimal> accumulatedTransportCostY = new List<decimal>();
                accumulatedTransportCostY.Add(totalTransportCost.Y);
                accumulatedTransportCostY.Add(totalExtraTransportCost.Y);

                List<object> iData = new List<object>();
                iData.Add(monthlyTransportCostsY);
                iData.Add(monthlyExtraTransportCostsY);
                iData.Add(accumulatedTransportCostY);

                return Json(iData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        //DINER
        public ActionResult Diner()
        {
            try
            {
                if (Permission.QueryPermission("DINERGRAPH.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    return View("~/Areas/YMMHRSystem/Views/Graph/Diner.cshtml");
                }
                else
                {
                    return View("~/Views/Shared/AccessDenied.cshtml");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult LoadDinerGraphs()
        {
            try
            {
                int kiSecondHalf = Convert.ToInt32(Session["KiFirstHalfYear"]) + 1;
                // DINER SERVICES BY TYPE
                List<DtoGraph> dinerServiceByType = graph.GetDinerServiceByType(Session["KiFirstHalfYear"].ToString() + "0401", kiSecondHalf.ToString() + "0331");
                List<string> dinerServiceByTypeX = dinerServiceByType.Select(o => o.X).ToList();
                List<int> dinerServiceByTypeY = dinerServiceByType.Select(o => o.Y).ToList();
                // DINER COST SERVICES BY TYPE
                List<DtoGraphStringDecimal> dinerCostServicesCostByType = graph.GetDinerServiceCostByType(Session["KiFirstHalfYear"].ToString() + "0401", kiSecondHalf.ToString() + "0331");
                List<string> dinerCostServicesByTypeX = dinerCostServicesCostByType.Select(o => o.X).ToList();
                List<decimal> dinerCostServicesByTypeY = dinerCostServicesCostByType.Select(o => o.Y).ToList();
                // PAYMENT COMPARATIVE
                List<DtoGraphStringDecimal> paymentComparative = graph.GetPaymentComparative(Session["KiFirstHalfYear"].ToString() + "0401", kiSecondHalf.ToString() + "0331");
                List<DtoGraphStringDecimal> paymentComparativeFinal = new List<DtoGraphStringDecimal>() {
                    new DtoGraphStringDecimal(){ X = "YMM", Y = 0},
                    new DtoGraphStringDecimal(){ X = "Associate", Y = 0},
                    new DtoGraphStringDecimal(){ X = "Total", Y = paymentComparative.Sum(item => item.Y)}
                };
                List<DtoGraphStringDecimal> paymentComparativeGroupBy = paymentComparative.GroupBy(x => x.X).Select(x => new DtoGraphStringDecimal { X = x.Key, Y = x.Sum(y => y.Y) }).ToList();

                foreach (var item in paymentComparativeFinal)
                {
                    var itemFound = paymentComparativeGroupBy.FirstOrDefault(x => x.X == item.X);
                    if (itemFound != null)
                    {
                        item.Y += itemFound.Y;
                    }
                }

                List<string> paymentComparativeX = paymentComparativeFinal.Select(o => o.X).ToList();
                List<decimal> paymentComparativeY = paymentComparativeFinal.Select(o => o.Y).ToList();

                // EXTRA DINER SERVICES BY PROCESS
                List<DtoProcess> processes = process.LoadMultiple();
                List<DtoGraphXYZ> extraDinerServicesByProcess = graph.GetExtraDinerServicesByProcess(Session["KiFirstHalfYear"].ToString() + "0401", kiSecondHalf.ToString() + "0331");
                List<DtoGraph> extraDinerServicesByWeekendFinal = new List<DtoGraph>();
                List<DtoGraph> extraDinerServicesByBoxLunchFinal = new List<DtoGraph>();
                List<DtoGraph> extraDinerServicesBySpecialFinal = new List<DtoGraph>();
                List<DtoGraph> extraDinerServicesByOtherFinal = new List<DtoGraph>();
                List<DtoGraphXYZ> extraDinerServicesByWeekend = extraDinerServicesByProcess.Where(o => o.Y == "Weekend").ToList();
                List<DtoGraphXYZ> extraDinerServicesByBoxLunch = extraDinerServicesByProcess.Where(o => o.Y == "Box Lunch").ToList();
                List<DtoGraphXYZ> extraDinerServicesBySpecial = extraDinerServicesByProcess.Where(o => o.Y == "Special").ToList();
                List<DtoGraphXYZ> extraDinerServicesByOther = extraDinerServicesByProcess.Where(o => o.Y == "Other").ToList();

                foreach (var item in processes)
                {
                    extraDinerServicesByWeekendFinal.Add(new DtoGraph
                    {
                        X = item.Name,
                        Y = 0
                    });
                }

                foreach (var item in processes)
                {
                    extraDinerServicesByBoxLunchFinal.Add(new DtoGraph
                    {
                        X = item.Name,
                        Y = 0
                    });
                }

                foreach (var item in processes)
                {
                    extraDinerServicesBySpecialFinal.Add(new DtoGraph
                    {
                        X = item.Name,
                        Y = 0
                    });
                }

                foreach (var item in processes)
                {
                    extraDinerServicesByOtherFinal.Add(new DtoGraph
                    {
                        X = item.Name,
                        Y = 0
                    });
                }

                foreach (var item in extraDinerServicesByWeekendFinal)
                {
                    var itemFound = extraDinerServicesByWeekend.FirstOrDefault(x => x.X == item.X);
                    if (itemFound != null)
                    {
                        item.Y = itemFound.Z;
                    }
                }

                foreach (var item in extraDinerServicesByBoxLunchFinal)
                {
                    var itemFound = extraDinerServicesByBoxLunch.FirstOrDefault(x => x.X == item.X);
                    if (itemFound != null)
                    {
                        item.Y = itemFound.Z;
                    }
                }

                foreach (var item in extraDinerServicesBySpecialFinal)
                {
                    var itemFound = extraDinerServicesBySpecial.FirstOrDefault(x => x.X == item.X);
                    if (itemFound != null)
                    {
                        item.Y = itemFound.Z;
                    }
                }

                foreach (var item in extraDinerServicesByOtherFinal)
                {
                    var itemFound = extraDinerServicesByOther.FirstOrDefault(x => x.X == item.X);
                    if (itemFound != null)
                    {
                        item.Y = itemFound.Z;
                    }
                }

                List<string> extraDinerServicesByWeekendX = extraDinerServicesByWeekendFinal.Select(o => o.X).ToList();
                List<int> extraDinerServicesByWeekendY = extraDinerServicesByWeekendFinal.Select(o => o.Y).ToList();
                List<int> extraDinerServicesByBoxLunchY = extraDinerServicesByBoxLunchFinal.Select(o => o.Y).ToList();
                List<int> extraDinerServicesBySpecialY = extraDinerServicesBySpecialFinal.Select(o => o.Y).ToList();
                List<int> extraDinerServicesByOtherY = extraDinerServicesByOtherFinal.Select(o => o.Y).ToList();

                List<object> iData = new List<object>();
                iData.Add(dinerServiceByTypeX);
                iData.Add(dinerServiceByTypeY);
                iData.Add(dinerCostServicesByTypeX);
                iData.Add(dinerCostServicesByTypeY);
                iData.Add(paymentComparativeX);
                iData.Add(paymentComparativeY);
                iData.Add(extraDinerServicesByWeekendX);
                iData.Add(extraDinerServicesByWeekendY);
                iData.Add(extraDinerServicesByBoxLunchY);
                iData.Add(extraDinerServicesBySpecialY);
                iData.Add(extraDinerServicesByOtherY);

                return Json(iData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult RefreshDinerServicesByType(DateTime startDate, DateTime endDate)
        {
            try
            {
                List<DtoGraph> dinerServiceByType = graph.GetDinerServiceByType(startDate.ToString("yyyMMdd"), endDate.AddDays(1).ToString("yyyMMdd"));
                List<string> dinerServiceByTypeX = dinerServiceByType.Select(o => o.X).ToList();
                List<int> dinerServiceByTypeY = dinerServiceByType.Select(o => o.Y).ToList();

                List<object> iData = new List<object>();
                iData.Add(dinerServiceByTypeX);
                iData.Add(dinerServiceByTypeY);

                return Json(iData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult RefreshDinerCostServicesByType(DateTime startDate, DateTime endDate)
        {
            try
            {
                List<DtoGraphStringDecimal> dinerCostServicesCostByType = graph.GetDinerServiceCostByType(startDate.ToString("yyyMMdd"), endDate.AddDays(1).ToString("yyyMMdd"));
                List<string> dinerCostServicesByTypeX = dinerCostServicesCostByType.Select(o => o.X).ToList();
                List<decimal> dinerCostServicesByTypeY = dinerCostServicesCostByType.Select(o => o.Y).ToList();

                List<object> iData = new List<object>();
                iData.Add(dinerCostServicesByTypeX);
                iData.Add(dinerCostServicesByTypeY);

                return Json(iData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult RefreshPaymentComparative(DateTime startDate, DateTime endDate)
        {
            try
            {
                List<DtoGraphStringDecimal> paymentComparative = graph.GetPaymentComparative(startDate.ToString("yyyMMdd"), endDate.AddDays(1).ToString("yyyMMdd"));
                List<DtoGraphStringDecimal> paymentComparativeFinal = new List<DtoGraphStringDecimal>() {
                    new DtoGraphStringDecimal(){ X = "YMM", Y = 0},
                    new DtoGraphStringDecimal(){ X = "Associate", Y = 0},
                    new DtoGraphStringDecimal(){ X = "Total", Y = paymentComparative.Sum(item => item.Y)}
                };
                List<DtoGraphStringDecimal> paymentComparativeGroupBy = paymentComparative.GroupBy(x => x.X).Select(x => new DtoGraphStringDecimal { X = x.Key, Y = x.Sum(y => y.Y) }).ToList();

                foreach (var item in paymentComparativeFinal)
                {
                    var itemFound = paymentComparativeGroupBy.FirstOrDefault(x => x.X == item.X);
                    if (itemFound != null)
                    {
                        item.Y += itemFound.Y;
                    }
                }

                List<string> paymentComparativeX = paymentComparativeFinal.Select(o => o.X).ToList();
                List<decimal> paymentComparativeY = paymentComparativeFinal.Select(o => o.Y).ToList();

                List<object> iData = new List<object>();
                iData.Add(paymentComparativeX);
                iData.Add(paymentComparativeY);

                return Json(iData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult RefreshExtraDinerServicesByProcess(DateTime startDate, DateTime endDate)
        {
            try
            {
                List<DtoProcess> processes = process.LoadMultiple();
                List<DtoGraphXYZ> extraDinerServicesByProcess = graph.GetExtraDinerServicesByProcess(startDate.ToString("yyyMMdd"), endDate.AddDays(1).ToString("yyyMMdd"));
                List<DtoGraph> extraDinerServicesByWeekendFinal = new List<DtoGraph>();
                List<DtoGraph> extraDinerServicesByBoxLunchFinal = new List<DtoGraph>();
                List<DtoGraph> extraDinerServicesBySpecialFinal = new List<DtoGraph>();
                List<DtoGraph> extraDinerServicesByOtherFinal = new List<DtoGraph>();
                List<DtoGraphXYZ> extraDinerServicesByWeekend = extraDinerServicesByProcess.Where(o => o.Y == "Weekend").ToList();
                List<DtoGraphXYZ> extraDinerServicesByBoxLunch = extraDinerServicesByProcess.Where(o => o.Y == "Box Lunch").ToList();
                List<DtoGraphXYZ> extraDinerServicesBySpecial = extraDinerServicesByProcess.Where(o => o.Y == "Special").ToList();
                List<DtoGraphXYZ> extraDinerServicesByOther = extraDinerServicesByProcess.Where(o => o.Y == "Other").ToList();

                foreach (var item in processes)
                {
                    extraDinerServicesByWeekendFinal.Add(new DtoGraph
                    {
                        X = item.Name,
                        Y = 0
                    });
                }

                foreach (var item in processes)
                {
                    extraDinerServicesByBoxLunchFinal.Add(new DtoGraph
                    {
                        X = item.Name,
                        Y = 0
                    });
                }

                foreach (var item in processes)
                {
                    extraDinerServicesBySpecialFinal.Add(new DtoGraph
                    {
                        X = item.Name,
                        Y = 0
                    });
                }

                foreach (var item in processes)
                {
                    extraDinerServicesByOtherFinal.Add(new DtoGraph
                    {
                        X = item.Name,
                        Y = 0
                    });
                }

                foreach (var item in extraDinerServicesByWeekendFinal)
                {
                    var itemFound = extraDinerServicesByWeekend.FirstOrDefault(x => x.X == item.X);
                    if (itemFound != null)
                    {
                        item.Y = itemFound.Z;
                    }
                }

                foreach (var item in extraDinerServicesByBoxLunchFinal)
                {
                    var itemFound = extraDinerServicesByBoxLunch.FirstOrDefault(x => x.X == item.X);
                    if (itemFound != null)
                    {
                        item.Y = itemFound.Z;
                    }
                }

                foreach (var item in extraDinerServicesBySpecialFinal)
                {
                    var itemFound = extraDinerServicesBySpecial.FirstOrDefault(x => x.X == item.X);
                    if (itemFound != null)
                    {
                        item.Y = itemFound.Z;
                    }
                }

                foreach (var item in extraDinerServicesByOtherFinal)
                {
                    var itemFound = extraDinerServicesByOther.FirstOrDefault(x => x.X == item.X);
                    if (itemFound != null)
                    {
                        item.Y = itemFound.Z;
                    }
                }
                List<string> extraDinerServicesByWeekendX = extraDinerServicesByWeekendFinal.Select(o => o.X).ToList();
                List<int> extraDinerServicesByWeekendY = extraDinerServicesByWeekendFinal.Select(o => o.Y).ToList();
                List<int> extraDinerServicesByBoxLunchY = extraDinerServicesByBoxLunchFinal.Select(o => o.Y).ToList();
                List<int> extraDinerServicesBySpecialY = extraDinerServicesBySpecialFinal.Select(o => o.Y).ToList();
                List<int> extraDinerServicesByOtherY = extraDinerServicesByOtherFinal.Select(o => o.Y).ToList();

                List<object> iData = new List<object>();
                iData.Add(extraDinerServicesByWeekendX);
                iData.Add(extraDinerServicesByWeekendY);
                iData.Add(extraDinerServicesByBoxLunchY);
                iData.Add(extraDinerServicesBySpecialY);
                iData.Add(extraDinerServicesByOtherY);

                return Json(iData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        //SINDICATE
        public ActionResult Sindicate()
        {
            try
            {
                if (Permission.QueryPermission("SINDICATEGRAPH.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    return View("~/Areas/YMMHRSystem/Views/Graph/Sindicate.cshtml");
                }
                else
                {
                    return View("~/Views/Shared/AccessDenied.cshtml");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult LoadSindicateGraphs()
        {
            try
            {
                // MONTHLY & ACCUMULATED TRANSPORT SERVICES
                List<DtoGraphDecimal> monthlySindicateAdministrativeSupport = graph.GetMonthlySindicateAdministrativeSupport(Session["KiFirstHalfYear"].ToString());
                List<DtoGraphDecimal> monthlySindicateUnionFee = graph.GetMonthlySindicateUnionFee(Session["KiFirstHalfYear"].ToString());
                List<DtoGraphString> monthlySindicateAdministrativeSupportFinal = new List<DtoGraphString>(){
                    new DtoGraphString() { X = "April", Y = "0" },
                    new DtoGraphString() { X = "May", Y = "0" },
                    new DtoGraphString() { X = "June", Y = "0" },
                    new DtoGraphString() { X = "July", Y = "0" },
                    new DtoGraphString() { X = "August", Y = "0" },
                    new DtoGraphString() { X = "September", Y = "0" },
                    new DtoGraphString() { X = "October", Y = "0" },
                    new DtoGraphString() { X = "November", Y = "0" },
                    new DtoGraphString() { X = "December", Y = "0" },
                    new DtoGraphString() { X = "January", Y = "0" },
                    new DtoGraphString() { X = "February", Y = "0" },
                    new DtoGraphString() { X = "March", Y = "0" },
                };
                List<DtoGraphString> monthlySindicateUnionFeeFinal = new List<DtoGraphString>(){
                    new DtoGraphString() { X = "April", Y = "0" },
                    new DtoGraphString() { X = "May", Y = "0" },
                    new DtoGraphString() { X = "June", Y = "0" },
                    new DtoGraphString() { X = "July", Y = "0" },
                    new DtoGraphString() { X = "August", Y = "0" },
                    new DtoGraphString() { X = "September", Y = "0" },
                    new DtoGraphString() { X = "October", Y = "0" },
                    new DtoGraphString() { X = "November", Y = "0" },
                    new DtoGraphString() { X = "December", Y = "0" },
                    new DtoGraphString() { X = "January", Y = "0" },
                    new DtoGraphString() { X = "February", Y = "0" },
                    new DtoGraphString() { X = "March", Y = "0" },
                };

                foreach (var item in monthlySindicateAdministrativeSupportFinal)
                {
                    var itemFound = monthlySindicateAdministrativeSupport.FirstOrDefault(x => x.X == DateTime.ParseExact(item.X, "MMMM", CultureInfo.InvariantCulture).Month);
                    if (itemFound != null)
                    {
                        item.Y = itemFound.Y.ToString();
                    }
                }

                List<string> monthlySindicatesX = monthlySindicateAdministrativeSupportFinal.Select(o => o.X).ToList();
                List<string> monthlySindicatesY = monthlySindicateAdministrativeSupportFinal.Select(o => o.Y).ToList();

                foreach (var item in monthlySindicateUnionFeeFinal)
                {
                    var itemFound = monthlySindicateUnionFee.FirstOrDefault(x => x.X == DateTime.ParseExact(item.X, "MMMM", CultureInfo.InvariantCulture).Month);
                    if (itemFound != null)
                    {
                        item.Y = itemFound.Y.ToString();
                    }
                }
                List<string> monthlySindicateUnionFeeY = monthlySindicateUnionFeeFinal.Select(o => o.Y).ToList();

                List<DtoGraphStringDecimal> totalSindicate = graph.GetTotalSindicateAmounts(Session["KiFirstHalfYear"].ToString());

                List<string> accumulatedSindicateX = totalSindicate.Select(o => o.X).ToList();
                List<decimal> accumulatedSindicateY = totalSindicate.Select(o => o.Y).ToList();

                List<object> iData = new List<object>();
                iData.Add(monthlySindicatesX);
                iData.Add(monthlySindicatesY);
                iData.Add(monthlySindicateUnionFeeY);
                iData.Add(accumulatedSindicateX);
                iData.Add(accumulatedSindicateY);

                return Json(iData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult RefreshSindicateAmounts(string firstHalfKi)
        {
            try
            {
                // MONTHLY & ACCUMULATED TRANSPORT SERVICES
                List<DtoGraphDecimal> monthlySindicateAdministrativeSupport = graph.GetMonthlySindicateAdministrativeSupport(firstHalfKi);
                List<DtoGraphDecimal> monthlySindicateUnionFee = graph.GetMonthlySindicateUnionFee(firstHalfKi);
                List<DtoGraphString> monthlySindicateAdministrativeSupportFinal = new List<DtoGraphString>(){
                    new DtoGraphString() { X = "April", Y = "0" },
                    new DtoGraphString() { X = "May", Y = "0" },
                    new DtoGraphString() { X = "June", Y = "0" },
                    new DtoGraphString() { X = "July", Y = "0" },
                    new DtoGraphString() { X = "August", Y = "0" },
                    new DtoGraphString() { X = "September", Y = "0" },
                    new DtoGraphString() { X = "October", Y = "0" },
                    new DtoGraphString() { X = "November", Y = "0" },
                    new DtoGraphString() { X = "December", Y = "0" },
                    new DtoGraphString() { X = "January", Y = "0" },
                    new DtoGraphString() { X = "February", Y = "0" },
                    new DtoGraphString() { X = "March", Y = "0" },
                };
                List<DtoGraphString> monthlySindicateUnionFeeFinal = new List<DtoGraphString>(){
                    new DtoGraphString() { X = "April", Y = "0" },
                    new DtoGraphString() { X = "May", Y = "0" },
                    new DtoGraphString() { X = "June", Y = "0" },
                    new DtoGraphString() { X = "July", Y = "0" },
                    new DtoGraphString() { X = "August", Y = "0" },
                    new DtoGraphString() { X = "September", Y = "0" },
                    new DtoGraphString() { X = "October", Y = "0" },
                    new DtoGraphString() { X = "November", Y = "0" },
                    new DtoGraphString() { X = "December", Y = "0" },
                    new DtoGraphString() { X = "January", Y = "0" },
                    new DtoGraphString() { X = "February", Y = "0" },
                    new DtoGraphString() { X = "March", Y = "0" },
                };

                foreach (var item in monthlySindicateAdministrativeSupportFinal)
                {
                    var itemFound = monthlySindicateAdministrativeSupport.FirstOrDefault(x => x.X == DateTime.ParseExact(item.X, "MMMM", CultureInfo.InvariantCulture).Month);
                    if (itemFound != null)
                    {
                        item.Y = itemFound.Y.ToString();
                    }
                }

                List<string> monthlySindicatesY = monthlySindicateAdministrativeSupportFinal.Select(o => o.Y).ToList();

                foreach (var item in monthlySindicateUnionFeeFinal)
                {
                    var itemFound = monthlySindicateUnionFee.FirstOrDefault(x => x.X == DateTime.ParseExact(item.X, "MMMM", CultureInfo.InvariantCulture).Month);
                    if (itemFound != null)
                    {
                        item.Y = itemFound.Y.ToString();
                    }
                }
                List<string> monthlySindicateUnionFeeY = monthlySindicateUnionFeeFinal.Select(o => o.Y).ToList();

                List<DtoGraphStringDecimal> totalSindicate = graph.GetTotalSindicateAmounts(firstHalfKi);

                List<decimal> accumulatedSindicateY = totalSindicate.Select(o => o.Y).ToList();

                List<object> iData = new List<object>();
                iData.Add(monthlySindicatesY);
                iData.Add(monthlySindicateUnionFeeY);
                iData.Add(accumulatedSindicateY);

                return Json(iData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        //WORKER RECORD
        public ActionResult WorkerFile()
        {
            try
            {
                if (Permission.QueryPermission("WORKERGRAPH.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    return View("~/Areas/YMMHRSystem/Views/Graph/WorkerFile.cshtml");
                }
                else
                {
                    return View("~/Views/Shared/AccessDenied.cshtml");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult LoadWorkerFileGraphs()
        {
            try
            {
                //HEADCOUNT
                List<DtoHeadCount> monthlyHeadCount = graph.GetHeadCount(Session["KiFirstHalfYear"].ToString());
                List<string> headCountMonth = monthlyHeadCount.Select(o => o.Month).ToList();
                List<int> headCountMen = monthlyHeadCount.Select(o => o.Men).ToList();
                List<int> headCountWomen = monthlyHeadCount.Select(o => o.Women).ToList();
                List<int> headCountTotal = monthlyHeadCount.Select(o => o.Total).ToList();

                List<DtoGraph> totalHeadCount = graph.GetTotalHeadCount();
                List<string> headCountX = totalHeadCount.Select(o => o.X).ToList();
                List<int> headCountY = totalHeadCount.Select(o => o.Y).ToList();

                //TURNOVER
                List<DtoGraphStringDecimal> turnOver = graph.GetTurnover(Session["KiFirstHalfYear"].ToString());
                List<string> turnOverX = turnOver.Select(o => o.X).ToList();
                List<decimal> turnOverY = turnOver.Select(o => o.Y).ToList();

                List<DtoGraphStringDecimal> averageTurnOver = graph.GetAverageTurnover(Session["KiFirstHalfYear"].ToString());
                List<string> averageTurnOverX = averageTurnOver.Select(o => o.X).ToList();
                List<decimal> averageTurnOverY = averageTurnOver.Select(o => o.Y).ToList();

                // YEARS OF SERVICE 
                List<DtoGraph> yearsOfService = graph.GetYearsOfService();
                List<string> yearsOfServiceX = yearsOfService.Select(o => o.X).ToList();
                List<int> yearsOfServiceY = yearsOfService.Select(o => o.Y).ToList();

                //ADMISSION DISMISSAL
                List<DtoGraph> monthlyAdmission = graph.GetMonthlyAdmissions(Session["KiFirstHalfYear"].ToString());
                List<DtoGraph> monthlyDismissal = graph.GetMonthlyDismissals(Session["KiFirstHalfYear"].ToString());
                List<string> monthlyAdmissionX = monthlyAdmission.Select(o => o.X).ToList();
                List<int> monthlyAdmissionY = monthlyAdmission.Select(o => o.Y).ToList();
                List<int> monthlyDismissalY = monthlyDismissal.Select(o => o.Y).ToList();

                List<DtoGraph> totalAdmissionDismissal = graph.GetTotalAdmissionDismissal(Session["KiFirstHalfYear"].ToString());
                List<string> totalAdmissionDismissalX = totalAdmissionDismissal.Select(o => o.X).ToList();
                List<int> totalAdmissionDismissalY = totalAdmissionDismissal.Select(o => o.Y).ToList();

                //DISMISSAL BY TYPE
                List<DtoGraph> monthlyDismissDismissal = graph.GetMonthlyDismissDismissals(Session["KiFirstHalfYear"].ToString());
                List<DtoGraph> monthlyResignationDismissal = graph.GetMonthlyResignationDismissals(Session["KiFirstHalfYear"].ToString());
                List<DtoGraph> monthlyEndOfContractDismissal = graph.GetMonthlyEndOfContractDismissals(Session["KiFirstHalfYear"].ToString());
                List<string> monthlyDismissDismissalX = monthlyDismissDismissal.Select(o => o.X).ToList();
                List<int> monthlyDismissDismissalY = monthlyDismissDismissal.Select(o => o.Y).ToList();
                List<int> monthlyResignationDismissalY = monthlyResignationDismissal.Select(o => o.Y).ToList();
                List<int> monthlyEndOfContractDismissalY = monthlyEndOfContractDismissal.Select(o => o.Y).ToList();

                List<DtoGraph> totalDismissalsByType = graph.GetTotalDismissalsByType(Session["KiFirstHalfYear"].ToString());
                List<string> totalDismissalsByTypeX = totalDismissalsByType.Select(o => o.X).ToList();
                List<int> totalDismissalsByTypeY = totalDismissalsByType.Select(o => o.Y).ToList();

                // WORKER AGES 
                List<DtoGraph> workerAges = graph.GetWorkerAges();
                List<string> workerAgesX = workerAges.Select(o => o.X).ToList();
                List<int> workerAgesY = workerAges.Select(o => o.Y).ToList();

                List<object> iData = new List<object>();
                iData.Add(headCountMonth);
                iData.Add(headCountMen);
                iData.Add(headCountWomen);
                iData.Add(headCountTotal);
                iData.Add(headCountX);
                iData.Add(headCountY);
                iData.Add(turnOverX);
                iData.Add(turnOverY);
                iData.Add(averageTurnOverX);
                iData.Add(averageTurnOverY);
                iData.Add(yearsOfServiceX);
                iData.Add(yearsOfServiceY);
                iData.Add(monthlyAdmissionX);
                iData.Add(monthlyAdmissionY);
                iData.Add(monthlyDismissalY);
                iData.Add(totalAdmissionDismissalX);
                iData.Add(totalAdmissionDismissalY);
                iData.Add(monthlyDismissDismissalX);
                iData.Add(monthlyDismissDismissalY);
                iData.Add(monthlyResignationDismissalY);
                iData.Add(monthlyEndOfContractDismissalY);
                iData.Add(totalDismissalsByTypeX);
                iData.Add(totalDismissalsByTypeY);
                iData.Add(workerAgesX);
                iData.Add(workerAgesY);

                return Json(iData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult RefreshHeadCount(string firstHalfKi)
        {
            try
            {
                List<DtoHeadCount> monthlyHeadCount = graph.GetHeadCount(firstHalfKi);
                List<int> headCountMen = monthlyHeadCount.Select(o => o.Men).ToList();
                List<int> headCountWomen = monthlyHeadCount.Select(o => o.Women).ToList();
                List<int> headCountTotal = monthlyHeadCount.Select(o => o.Total).ToList();

                List<DtoGraph> totalHeadCount = graph.GetTotalHeadCount();
                List<int> headCountY = totalHeadCount.Select(o => o.Y).ToList();

                List<object> iData = new List<object>();
                iData.Add(headCountMen);
                iData.Add(headCountWomen);
                iData.Add(headCountTotal);
                iData.Add(headCountY);

                return Json(iData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult RefreshTurnover(string firstHalfKi)
        {
            try
            {
                List<DtoGraphStringDecimal> turnOver = graph.GetTurnover(firstHalfKi);
                List<decimal> turnOverY = turnOver.Select(o => o.Y).ToList();

                List<DtoGraphStringDecimal> averageTurnOver = graph.GetAverageTurnover(firstHalfKi);
                List<decimal> averageTurnOverY = averageTurnOver.Select(o => o.Y).ToList();

                List<object> iData = new List<object>();
                iData.Add(turnOverY);
                iData.Add(averageTurnOverY);

                return Json(iData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult RefreshAdmissionDismissal(string firstHalfKi)
        {
            try
            {
                List<DtoGraph> monthlyAdmission = graph.GetMonthlyAdmissions(firstHalfKi);
                List<DtoGraph> monthlyDismissal = graph.GetMonthlyDismissals(firstHalfKi);
                List<int> monthlyAdmissionY = monthlyAdmission.Select(o => o.Y).ToList();
                List<int> monthlyDismissalY = monthlyDismissal.Select(o => o.Y).ToList();

                List<DtoGraph> totalAdmissionDismissal = graph.GetTotalAdmissionDismissal(firstHalfKi);
                List<int> totalAdmissionDismissalY = totalAdmissionDismissal.Select(o => o.Y).ToList();

                List<object> iData = new List<object>();
                iData.Add(monthlyAdmissionY);
                iData.Add(monthlyDismissalY);
                iData.Add(totalAdmissionDismissalY);

                return Json(iData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult RefreshDismissalByType(string firstHalfKi)
        {
            try
            {
                List<DtoGraph> monthlyDismissDismissal = graph.GetMonthlyDismissDismissals(firstHalfKi);
                List<DtoGraph> monthlyResignationDismissal = graph.GetMonthlyResignationDismissals(firstHalfKi);
                List<DtoGraph> monthlyEndOfContractDismissal = graph.GetMonthlyEndOfContractDismissals(firstHalfKi);
                List<string> monthlyDismissDismissalX = monthlyDismissDismissal.Select(o => o.X).ToList();
                List<int> monthlyDismissDismissalY = monthlyDismissDismissal.Select(o => o.Y).ToList();
                List<int> monthlyResignationDismissalY = monthlyResignationDismissal.Select(o => o.Y).ToList();
                List<int> monthlyEndOfContractDismissalY = monthlyEndOfContractDismissal.Select(o => o.Y).ToList();

                List<DtoGraph> totalDismissalsByType = graph.GetTotalDismissalsByType(firstHalfKi);
                List<string> totalDismissalsByTypeX = totalDismissalsByType.Select(o => o.X).ToList();
                List<int> totalDismissalsByTypeY = totalDismissalsByType.Select(o => o.Y).ToList();

                List<object> iData = new List<object>();
                iData.Add(monthlyDismissDismissalX);
                iData.Add(monthlyDismissDismissalY);
                iData.Add(monthlyResignationDismissalY);
                iData.Add(monthlyEndOfContractDismissalY);
                iData.Add(totalDismissalsByTypeX);
                iData.Add(totalDismissalsByTypeY);

                return Json(iData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        //RECORDS
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Record()
        {
            try
            {
                if (Permission.QueryPermission("RECORD.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    return View("~/Areas/YMMHRSystem/Views/Graph/Record.cshtml");
                }
                else
                {
                    return View("~/Views/Shared/AccessDenied.cshtml");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Exports Worker Records
        /// </summary>
        public void ExportWorkerRecord(int status)
        {
            WorkerFile workerFile = new WorkerFile();
            List<DtoWorkerFile> workerList = workerFile.LoadMultiple(status);

            ExcelPackage excelPackage = new ExcelPackage();
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("WorkerRecord");

            worksheet.Cells[1, 1].Value = "Worker ID";
            worksheet.Cells[1, 2].Value = "Names";
            worksheet.Cells[1, 3].Value = "Admission Date";
            worksheet.Cells[1, 4].Value = "SSN";
            worksheet.Cells[1, 5].Value = "RFC";
            worksheet.Cells[1, 6].Value = "CURP";
            worksheet.Cells[1, 7].Value = "Process";
            worksheet.Cells[1, 8].Value = "Job Title";
            worksheet.Cells[1, 9].Value = "Date of Birth";
            worksheet.Cells[1, 10].Value = "Civil Status";
            worksheet.Cells[1, 11].Value = "Education";
            worksheet.Cells[1, 12].Value = "Email";
            worksheet.Cells[1, 13].Value = "Telephone";
            worksheet.Cells[1, 14].Value = "Type";
            worksheet.Cells[1, 15].Value = "Gender";
            worksheet.Cells[1, 16].Value = "Status";
            worksheet.Cells[1, 17].Value = "Rehirable";
            worksheet.Cells[1, 18].Value = "Dismissal Date";
            var range = worksheet.Cells[1, 1, 1, 18];
            range.Style.Font.Bold = true;


            int row = 2;
            foreach (var item in workerList)
            {
                worksheet.Cells[row, 1].Value = item.WorkerId;
                worksheet.Cells[row, 2].Value = item.Names;
                worksheet.Cells[row, 3].Value = item.AdmissionDate?.ToShortDateString();
                worksheet.Cells[row, 4].Value = item.NSS;
                worksheet.Cells[row, 5].Value = item.RFC;
                worksheet.Cells[row, 6].Value = item.CURP;
                worksheet.Cells[row, 7].Value = item.Process;
                worksheet.Cells[row, 8].Value = item.Job;
                worksheet.Cells[row, 9].Value = item.DoB?.ToShortDateString();
                worksheet.Cells[row, 10].Value = item.CivilStatus;
                worksheet.Cells[row, 11].Value = item.Education;
                worksheet.Cells[row, 12].Value = item.Email;
                worksheet.Cells[row, 13].Value = item.Telephone;
                worksheet.Cells[row, 14].Value = item.Type;
                worksheet.Cells[row, 15].Value = item.Gender ? "Male" : "Female";
                worksheet.Cells[row, 16].Value = item.Status ? "Active" : "Inactive" ;
                worksheet.Cells[row, 17].Value = item.Rehirable ? "Yes" : "No";
                worksheet.Cells[row, 18].Value = item.DismissalDate?.ToShortDateString();
                row++;
            }


            range = worksheet.Cells[worksheet.Dimension.Address];
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            range.AutoFitColumns();
            //optional use this to make all columms just a bit wider, text would sometimes still overflow after AutoFitColumns().
            for (int col = 2; col <= worksheet.Dimension.End.Column; col++)
            {
                worksheet.Column(col).Width = worksheet.Column(col).Width + 5;
            }
            //Save the workbook to disk in xlsx format

            //convert the excel package to a byte array
            byte[] bin = excelPackage.GetAsByteArray();
            //clear the buffer stream
            Response.ClearHeaders();
            Response.Clear();
            Response.Buffer = true;

            //set the correct contenttype
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            //set the correct length of the data being send
            Response.AddHeader("content-length", bin.Length.ToString());

            //set the filename for the excel package
            Response.AddHeader("content-disposition", "attachment; filename=\"WorkerRecords.xlsx\"");

            //send the byte array to the browser
            Response.OutputStream.Write(bin, 0, bin.Length);

            //cleanup
            Response.Flush();
        }
        public void ExportTranslations(DateTime? startDate, DateTime? endDate)
        {
            Translation translation = new Translation();
            List<DtoTranslation> translationList = translation.LoadMultipleWithFilter(startDate?.ToString("yyyyMMdd"), endDate?.AddDays(1).ToString("yyyyMMdd"));

            ExcelPackage excelPackage = new ExcelPackage();
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("TranslationRecord");

            worksheet.Cells[1, 1].Value = "Issue";
            worksheet.Cells[1, 2].Value = "Description";
            worksheet.Cells[1, 3].Value = "Complexity Level";
            worksheet.Cells[1, 4].Value = "Status";
            worksheet.Cells[1, 5].Value = "Assigned To";
            worksheet.Cells[1, 6].Value = "Deadline";
            worksheet.Cells[1, 7].Value = "Final Date";
            worksheet.Cells[1, 8].Value = "Associate";
            var range = worksheet.Cells[1, 1, 1, 8];
            range.Style.Font.Bold = true;

            int row = 2;
            foreach (var item in translationList)
            {
                worksheet.Cells[row, 1].Value = item.Issue;
                worksheet.Cells[row, 2].Value = item.Description;
                worksheet.Cells[row, 3].Value = item.ComplexityLevel;
                worksheet.Cells[row, 4].Value = item.Status == 3 ? "Complete" : item.Status == 2 ? "In Progress" : "New";
                worksheet.Cells[row, 5].Value = item.AssignedTo;
                worksheet.Cells[row, 6].Value = item.Deadline?.ToShortDateString();
                worksheet.Cells[row, 7].Value = item.FinalDate?.ToShortDateString();
                worksheet.Cells[row, 8].Value = item.Associate;              
                row++;
            }


            range = worksheet.Cells[worksheet.Dimension.Address];
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            range.AutoFitColumns();
            //optional use this to make all columms just a bit wider, text would sometimes still overflow after AutoFitColumns().
            for (int col = 2; col <= worksheet.Dimension.End.Column; col++)
            {
                worksheet.Column(col).Width = worksheet.Column(col).Width + 5;
            }
            //Save the workbook to disk in xlsx format

            //convert the excel package to a byte array
            byte[] bin = excelPackage.GetAsByteArray();
            //clear the buffer stream
            Response.ClearHeaders();
            Response.Clear();
            Response.Buffer = true;

            //set the correct contenttype
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            //set the correct length of the data being send
            Response.AddHeader("content-length", bin.Length.ToString());

            //set the filename for the excel package
            Response.AddHeader("content-disposition", "attachment; filename=\"TranslationRecords.xlsx\"");

            //send the byte array to the browser
            Response.OutputStream.Write(bin, 0, bin.Length);

            //cleanup
            Response.Flush();
        }
        public void ExportDiners(DateTime startDate, DateTime endDate)
        {
            Diner diner = new Diner();
            List<DtoDiner> dinerList = diner.LoadMultipleWithFilter(startDate.ToString("yyyyMMdd"), endDate.AddDays(1).ToString("yyyyMMdd"));

            ExcelPackage excelPackage = new ExcelPackage();
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("DinerRecord");

            worksheet.Cells[1, 1].Value = "Type";
            worksheet.Cells[1, 2].Value = "Dishes";
            worksheet.Cells[1, 3].Value = "Unit Price";
            worksheet.Cells[1, 4].Value = "Total";
            worksheet.Cells[1, 5].Value = "Start Date";
            worksheet.Cells[1, 6].Value = "Finish Date";
            var range = worksheet.Cells[1, 1, 1, 6];
            range.Style.Font.Bold = true;

            int row = 2;
            foreach (var item in dinerList)
            {
                worksheet.Cells[row, 1].Value = item.Type;
                worksheet.Cells[row, 2].Value = item.Dishes;
                worksheet.Cells[row, 3].Value = item.UnitPrice;
                worksheet.Cells[row, 3].Style.Numberformat.Format = "$ #,###,###.00";
                worksheet.Cells[row, 4].Value = item.Total;
                worksheet.Cells[row, 4].Style.Numberformat.Format = "$ #,###,###.00";
                worksheet.Cells[row, 5].Value = item.StartDate?.ToShortDateString();
                worksheet.Cells[row, 6].Value = item.FinishDate?.ToShortDateString();
                row++;
            }


            range = worksheet.Cells[worksheet.Dimension.Address];
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            range.AutoFitColumns();
            //optional use this to make all columms just a bit wider, text would sometimes still overflow after AutoFitColumns().
            for (int col = 2; col <= worksheet.Dimension.End.Column; col++)
            {
                worksheet.Column(col).Width = worksheet.Column(col).Width + 5;
            }
            //Save the workbook to disk in xlsx format

            //convert the excel package to a byte array
            byte[] bin = excelPackage.GetAsByteArray();
            //clear the buffer stream
            Response.ClearHeaders();
            Response.Clear();
            Response.Buffer = true;

            //set the correct contenttype
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            //set the correct length of the data being send
            Response.AddHeader("content-length", bin.Length.ToString());

            //set the filename for the excel package
            Response.AddHeader("content-disposition", "attachment; filename=\"DinerRecords.xlsx\"");

            //send the byte array to the browser
            Response.OutputStream.Write(bin, 0, bin.Length);

            //cleanup
            Response.Flush();
        }
        public void ExportExtraDiners(DateTime startDate, DateTime endDate)
        {
            Diner diner = new Diner();
            List<DtoExtraordinaryDiner> dinerList = diner.LoadMultipleExtraWithFilter(startDate.ToString("yyyyMMdd"), endDate.AddDays(1).ToString("yyyyMMdd"));

            ExcelPackage excelPackage = new ExcelPackage();
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("ExtraDinerRecord");

            worksheet.Cells[1, 1].Value = "Associate Name";
            worksheet.Cells[1, 2].Value = "Process";
            worksheet.Cells[1, 3].Value = "Date";
            worksheet.Cells[1, 4].Value = "Time";
            worksheet.Cells[1, 5].Value = "Type";
            worksheet.Cells[1, 6].Value = "Motive";
            worksheet.Cells[1, 7].Value = "Cost";
            worksheet.Cells[1, 8].Value = "Lading Cost";
            worksheet.Cells[1, 9].Value = "Status";
            worksheet.Cells[1, 10].Value = "Date Added";
            var range = worksheet.Cells[1, 1, 1, 10];
            range.Style.Font.Bold = true;

            int row = 2;
            foreach (var item in dinerList)
            {
                worksheet.Cells[row, 1].Value = item.AssociateName;
                worksheet.Cells[row, 2].Value = item.Process;
                worksheet.Cells[row, 3].Value = item.Date?.ToShortDateString();
                worksheet.Cells[row, 4].Value = item.Time.ToString(@"hh\:mm");
                worksheet.Cells[row, 5].Value = item.Type;
                worksheet.Cells[row, 6].Value = item.Motive;
                worksheet.Cells[row, 7].Value = item.TypeCost;
                worksheet.Cells[row, 7].Style.Numberformat.Format = "$ #,###,###.00";
                worksheet.Cells[row, 8].Value = item.LadingCost;
                worksheet.Cells[row, 8].Style.Numberformat.Format = "$ #,###,###.00";
                worksheet.Cells[row, 9].Value = item.Status == 1 ? "Generated" : item.Status == 3 ? "Confirmed" : "Cancelled";
                worksheet.Cells[row, 10].Value = item.DateAdded.ToString("yyyy-MM-dd HH:mm:ss");
                row++;
            }

            range = worksheet.Cells[worksheet.Dimension.Address];
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            range.AutoFitColumns();
            //optional use this to make all columms just a bit wider, text would sometimes still overflow after AutoFitColumns().
            for (int col = 2; col <= worksheet.Dimension.End.Column; col++)
            {
                worksheet.Column(col).Width = worksheet.Column(col).Width + 5;
            }
            //Save the workbook to disk in xlsx format

            //convert the excel package to a byte array
            byte[] bin = excelPackage.GetAsByteArray();
            //clear the buffer stream
            Response.ClearHeaders();
            Response.Clear();
            Response.Buffer = true;

            //set the correct contenttype
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            //set the correct length of the data being send
            Response.AddHeader("content-length", bin.Length.ToString());

            //set the filename for the excel package
            Response.AddHeader("content-disposition", "attachment; filename=\"ExtraDinerRecords.xlsx\"");

            //send the byte array to the browser
            Response.OutputStream.Write(bin, 0, bin.Length);

            //cleanup
            Response.Flush();
        }
        public void ExportTransports(DateTime startDate, DateTime endDate)
        {
            Transport transport = new Transport();
            List<DtoTransport> transportList = transport.LoadMultipleWithFilter(startDate.ToString("yyyyMMdd"), endDate.AddDays(1).ToString("yyyyMMdd"));

            ExcelPackage excelPackage = new ExcelPackage();
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("TransportRecord");

            worksheet.Cells[1, 1].Value = "Route";
            worksheet.Cells[1, 2].Value = "Start Date";
            worksheet.Cells[1, 3].Value = "Finish Date";
            worksheet.Cells[1, 4].Value = "Type";
            worksheet.Cells[1, 5].Value = "Cost";
            var range = worksheet.Cells[1, 1, 1, 6];
            range.Style.Font.Bold = true;

            int row = 2;
            foreach (var item in transportList)
            {
                worksheet.Cells[row, 1].Value = item.Route;
                worksheet.Cells[row, 2].Value = item.StartDate.ToShortDateString();
                worksheet.Cells[row, 3].Value = item.FinishDate.ToShortDateString();
                worksheet.Cells[row, 4].Value = item.Type ? "One Way" : "Round";
                worksheet.Cells[row, 5].Value = item.Cost;
                worksheet.Cells[row, 5].Style.Numberformat.Format = "$ #,###,###.00";
                row++;
            }

            range = worksheet.Cells[worksheet.Dimension.Address];
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            range.AutoFitColumns();
            //optional use this to make all columms just a bit wider, text would sometimes still overflow after AutoFitColumns().
            for (int col = 2; col <= worksheet.Dimension.End.Column; col++)
            {
                worksheet.Column(col).Width = worksheet.Column(col).Width + 5;
            }
            //Save the workbook to disk in xlsx format

            //convert the excel package to a byte array
            byte[] bin = excelPackage.GetAsByteArray();
            //clear the buffer stream
            Response.ClearHeaders();
            Response.Clear();
            Response.Buffer = true;

            //set the correct contenttype
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            //set the correct length of the data being send
            Response.AddHeader("content-length", bin.Length.ToString());

            //set the filename for the excel package
            Response.AddHeader("content-disposition", "attachment; filename=\"TransportRecords.xlsx\"");

            //send the byte array to the browser
            Response.OutputStream.Write(bin, 0, bin.Length);

            //cleanup
            Response.Flush();
        }
        public void ExportExtraTransports(DateTime startDate, DateTime endDate)
        {
            Transport transport = new Transport();
            List<DtoExtraordinaryTransport> transportList = transport.LoadMultipleExtraWithFilter(startDate.ToString("yyyyMMdd"), endDate.AddDays(1).ToString("yyyyMMdd"));

            ExcelPackage excelPackage = new ExcelPackage();
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("ExtraTransportRecord");

            worksheet.Cells[1, 1].Value = "Associate";
            worksheet.Cells[1, 2].Value = "Process";
            worksheet.Cells[1, 3].Value = "Route";
            worksheet.Cells[1, 4].Value = "Stop";
            worksheet.Cells[1, 5].Value = "Start Time";
            worksheet.Cells[1, 6].Value = "Start Date";
            worksheet.Cells[1, 7].Value = "Finish Time";
            worksheet.Cells[1, 8].Value = "Finish Date";
            worksheet.Cells[1, 9].Value = "Motive";
            worksheet.Cells[1, 10].Value = "Cost";
            worksheet.Cells[1, 11].Value = "Status";
            worksheet.Cells[1, 12].Value = "Date Added";

            var range = worksheet.Cells[1, 1, 1, 12];
            range.Style.Font.Bold = true;

            int row = 2;
            foreach (var item in transportList)
            {
                worksheet.Cells[row, 1].Value = item.AssociateName;
                worksheet.Cells[row, 2].Value = item.Process;
                worksheet.Cells[row, 3].Value = item.Route;
                worksheet.Cells[row, 4].Value = item.Stop;
                worksheet.Cells[row, 5].Value = item.StartTime.ToString(@"hh\:mm");
                worksheet.Cells[row, 6].Value = item.StartDate?.ToShortDateString();
                worksheet.Cells[row, 7].Value = item.FinishTime.ToString(@"hh\:mm"); 
                worksheet.Cells[row, 8].Value = item.FinishDate?.ToShortDateString();
                worksheet.Cells[row, 9].Value = item.Motive;
                worksheet.Cells[row, 10].Value = item.Cost;
                worksheet.Cells[row, 10].Style.Numberformat.Format = "$ #,###,###.00";
                worksheet.Cells[row, 11].Value = item.Status == 1 ? "Generated" : item.Status == 3 ? "Confirmed" : "Cancelled";
                worksheet.Cells[row, 12].Value = item.Date.ToString("yyyy-MM-dd HH:mm:ss");
                row++;
            }

            range = worksheet.Cells[worksheet.Dimension.Address];
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            range.AutoFitColumns();
            //optional use this to make all columms just a bit wider, text would sometimes still overflow after AutoFitColumns().
            for (int col = 2; col <= worksheet.Dimension.End.Column; col++)
            {
                worksheet.Column(col).Width = worksheet.Column(col).Width + 5;
            }
            //Save the workbook to disk in xlsx format

            //convert the excel package to a byte array
            byte[] bin = excelPackage.GetAsByteArray();
            //clear the buffer stream
            Response.ClearHeaders();
            Response.Clear();
            Response.Buffer = true;

            //set the correct contenttype
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            //set the correct length of the data being send
            Response.AddHeader("content-length", bin.Length.ToString());

            //set the filename for the excel package
            Response.AddHeader("content-disposition", "attachment; filename=\"ExtraTransportRecords.xlsx\"");

            //send the byte array to the browser
            Response.OutputStream.Write(bin, 0, bin.Length);

            //cleanup
            Response.Flush();
        }
        public void ExportLegalRequirements(DateTime startDate, DateTime endDate)
        {
            LegalRequirement legalRequirement = new LegalRequirement();
            List<DtoLegalRequirement> legalRequirementList = legalRequirement.LoadMultipleWithFilter(startDate.ToString("yyyyMMdd"), endDate.AddDays(1).ToString("yyyyMMdd"));

            ExcelPackage excelPackage = new ExcelPackage();
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("LegalRequirementRecord");

            worksheet.Cells[1, 1].Value = "Legal Procedure";
            worksheet.Cells[1, 2].Value = "Institution";
            worksheet.Cells[1, 3].Value = "Emission Date";
            worksheet.Cells[1, 4].Value = "Expiration Date";
            worksheet.Cells[1, 5].Value = "Renewal Date";
            var range = worksheet.Cells[1, 1, 1, 5];
            range.Style.Font.Bold = true;

            int row = 2;
            foreach (var item in legalRequirementList)
            {
                worksheet.Cells[row, 1].Value = item.LegalProcedure;
                worksheet.Cells[row, 2].Value = item.Institution;
                worksheet.Cells[row, 3].Value = item.EmissionDate?.ToShortDateString();
                worksheet.Cells[row, 4].Value = item.ExpirationDate?.ToShortDateString();
                worksheet.Cells[row, 5].Value = item.RenewalDate?.ToShortDateString();
                row++;
            }


            range = worksheet.Cells[worksheet.Dimension.Address];
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            range.AutoFitColumns();
            //optional use this to make all columms just a bit wider, text would sometimes still overflow after AutoFitColumns().
            for (int col = 2; col <= worksheet.Dimension.End.Column; col++)
            {
                worksheet.Column(col).Width = worksheet.Column(col).Width + 5;
            }
            //Save the workbook to disk in xlsx format

            //convert the excel package to a byte array
            byte[] bin = excelPackage.GetAsByteArray();
            //clear the buffer stream
            Response.ClearHeaders();
            Response.Clear();
            Response.Buffer = true;

            //set the correct contenttype
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            //set the correct length of the data being send
            Response.AddHeader("content-length", bin.Length.ToString());

            //set the filename for the excel package
            Response.AddHeader("content-disposition", "attachment; filename=\"LegalRequirementsRecords.xlsx\"");

            //send the byte array to the browser
            Response.OutputStream.Write(bin, 0, bin.Length);

            //cleanup
            Response.Flush();
        }

        public void ExportAccessAuthorization(DateTime startDate, DateTime endDate)
        {
            EntryExitAuthorization entryExitAuthorization = new EntryExitAuthorization();
            List<DtoEntryExitAuthorization> dtoEntryExitAuthorizationsList = entryExitAuthorization.LoadMultipleWithFilter(startDate.ToString("yyyyMMdd"), endDate.AddDays(1).ToString("yyyyMMdd"));
            
            Types types = new Types();
            Motive motive = new Motive();
            
            ExcelPackage excelPackage = new ExcelPackage();
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("AccessAuthorizationRecords");

            // Definir encabezados
            worksheet.Cells[1, 1].Value = "IdRecord";
            worksheet.Cells[1, 2].Value = "Associate";
            worksheet.Cells[1, 3].Value = "Process";
            worksheet.Cells[1, 4].Value = "DateFor";
            worksheet.Cells[1, 8].Value = "TimeFor";
            worksheet.Cells[1, 5].Value = "Type";
            worksheet.Cells[1, 6].Value = "Salary";
            worksheet.Cells[1, 7].Value = "Motive";
            worksheet.Cells[1, 9].Value = "First Authorization";
            worksheet.Cells[1, 10].Value = "First Authorized By";
            worksheet.Cells[1, 11].Value = "Second Authorization";
            worksheet.Cells[1, 12].Value = "Second Authorized By";
            worksheet.Cells[1, 13].Value = "Current State";
            worksheet.Cells[1, 14].Value = "Created By";
            worksheet.Cells[1, 15].Value = "Creation Date";

            var range = worksheet.Cells[1, 1, 1, 13];
            range.Style.Font.Bold = true;

            // Llenar datos
            int row = 2;
            foreach (var item in dtoEntryExitAuthorizationsList)
            {
                worksheet.Cells[row, 1].Value = item.IdRecordsInOut;
                worksheet.Cells[row, 2].Value = item.Associate;
                worksheet.Cells[row, 3].Value = item.Process;
                worksheet.Cells[row, 4].Value = item.DateFor.ToShortDateString();
                worksheet.Cells[row, 8].Value = item.TimeFor.ToString();
                worksheet.Cells[row, 5].Value = types.GetTypeDescription(item.IdType);
                worksheet.Cells[row, 6].Value = item.IdSalary == 1 ? "Sin Goce" : "Con Goce";
                worksheet.Cells[row, 7].Value = motive.GetMotiveDescription(item.IdMotive);               
                worksheet.Cells[row, 9].Value = item.FirtsAuthorization.HasValue ? (item.FirtsAuthorization.Value ? "Yes" : "No") : "N/A";
                worksheet.Cells[row, 10].Value = item.FirtsAuthorizedby;
                worksheet.Cells[row, 11].Value = item.SecondAuthorization.HasValue ? (item.SecondAuthorization.Value ? "Yes" : "No") : "N/A";
                worksheet.Cells[row, 12].Value = item.SecondAuthorizedby;
                worksheet.Cells[row, 13].Value = item.CurrentState;
                worksheet.Cells[row, 14].Value = item.CreateBy;
                worksheet.Cells[row, 15].Value = item.CreateDate?.ToShortDateString();
                row++;
            }

            // Estilizar columnas
            range = worksheet.Cells[worksheet.Dimension.Address];
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            range.AutoFitColumns();

            for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
            {
                worksheet.Column(col).Width = worksheet.Column(col).Width + 5;
            }

            // Convertir Excel a byte array
            byte[] bin = excelPackage.GetAsByteArray();

            // Enviar el archivo al cliente
            Response.ClearHeaders();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-length", bin.Length.ToString());
            Response.AddHeader("content-disposition", "attachment; filename=\"AccessAuthorizationRecords.xlsx\"");
            Response.OutputStream.Write(bin, 0, bin.Length);

            // Cleanup
            Response.Flush();
        }
    }
}