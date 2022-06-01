using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tools.Api.Models.Excel;

namespace Tools.Api.Controllers
{
    /// <summary>
    /// Excel
    /// </summary>
    [Route("Api/Excel")]
    [ApiController]
    public class ExcelController : Controller
    {
        /// <summary>
        /// UpLoadExcel
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpLoadExcel")]
        public ActionResult UpLoadExcel(IFormFile file)
        {
            try
            {
                List<ExcelDto> list = new List<ExcelDto>();

                using (var stream = new MemoryStream())
                {
                    file.CopyToAsync(stream);
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    using (var dt = Util.ReadExcel(stream))
                    {
                        for (int row = 0; row < dt.Rows.Count; row++)
                        {
                            if (dt.Rows[row]["款式"] != null)
                            {
                                string[] stylestr = new string[] { };

                                stylestr = dt.Rows[row]["款式"].ToString().Split("|");

                                if (dt.Rows[row]["尺寸"] != null)
                                {
                                    string[] sizestr = new string[] { };
                                    sizestr = dt.Rows[row]["尺寸"].ToString().Split("|");
                                    ExcelDto Main = new ExcelDto();
                                    Main.Id = Guid.NewGuid().ToString();
                                    Main.Title = dt.Rows[row]["标题"].ToString();
                                    Main.SubTitle = "";
                                    Main.LinkUrl = "";
                                    Main.Desc = "";
                                    Main.SeoTitle = "";
                                    Main.SeoDesc = "";
                                    Main.SeoKeyWord = "";
                                    Main.ProductShelf = "";
                                    Main.Logistic = "Y";
                                    Main.ProductFee = "N";
                                    Main.VirtualNum = "0";
                                    Main.FollowStock = "N";
                                    Main.StockRule = "";
                                    Main.EditionName = "Star";
                                    Main.Label = "";
                                    Main.SupName = "SHENMI";
                                    Main.SupUrl = "";
                                    Main.Attr = "M";
                                    Main.Style = "Style";
                                    Main.Size = "Size";
                                    Main.Color = "Color";
                                    Main.Price = "";
                                    Main.OldPrice = "";
                                    Main.Sku = "";
                                    Main.Weight = "";
                                    Main.Unit = "";
                                    Main.BarCode = "";
                                    Main.ProductStock = "";
                                    Main.ImageUrl = dt.Rows[row]["主图地址"].ToString();
                                    Main.FirstCategary = "";
                                    Main.SecondCategary = "";
                                    Main.ProductCategary = "";
                                    Main.Spu = dt.Rows[row]["SPU"].ToString();
                                    list.Add(Main);
                                    for (int j = 0; j < stylestr.Length; j++)
                                    {
                                        for (int k = 0; k < sizestr.Length; k++)
                                        {
                                            ExcelDto item = new ExcelDto();
                                            item.Id = Guid.NewGuid().ToString();
                                            item.Title = dt.Rows[row]["标题"].ToString();
                                            item.SubTitle = "";
                                            item.LinkUrl = "";
                                            item.Desc = "";
                                            item.SeoTitle = "";
                                            item.SeoDesc = "";
                                            item.SeoKeyWord = "";
                                            item.ProductShelf = "";
                                            item.Logistic = "";
                                            item.ProductFee = "";
                                            item.VirtualNum = "";
                                            item.FollowStock = "";
                                            item.StockRule = "";
                                            item.EditionName = "";
                                            item.Label = "";
                                            item.SupName = "";
                                            item.SupUrl = "";
                                            item.Attr = "P";
                                            item.Style = stylestr[j];
                                            item.Size = sizestr[k];
                                            item.Color = dt.Rows[row]["颜色"].ToString();
                                            item.Price = dt.Rows[row]["价格"].ToString();
                                            item.OldPrice = dt.Rows[row]["价格"].ToString();
                                            item.Sku = stylestr[j] + "-" + sizestr[k] + "-" + dt.Rows[row]["颜色"].ToString();
                                            item.Weight = "";
                                            item.Unit = "";
                                            item.BarCode = "";
                                            item.ProductStock = "";
                                            item.ImageUrl = dt.Rows[row]["主图地址"].ToString();
                                            item.FirstCategary = "";
                                            item.SecondCategary = "";
                                            item.ProductCategary = "";
                                            item.Spu = "";
                                            list.Add(item);
                                        }
                                    }
                                }
                                else
                                {
                                    sb.AppendLine((row + 1) + " 尺寸为空");
                                    continue;
                                }
                            }
                            else
                            {
                                sb.AppendLine((row + 1) + "款式为空");
                                continue;
                            }
                        }
                    }
                }

                #region MyRegion

                HSSFWorkbook book = new HSSFWorkbook();

                ISheet s1 = book.CreateSheet("Result");

                IRow r1 = s1.CreateRow(0);

                r1.CreateCell(0).SetCellValue("商品ID");
                r1.CreateCell(1).SetCellValue("商品标题");
                r1.CreateCell(2).SetCellValue("商品副标题");
                r1.CreateCell(3).SetCellValue("链接");
                r1.CreateCell(4).SetCellValue("商品描述");
                r1.CreateCell(5).SetCellValue("seo标题");
                r1.CreateCell(6).SetCellValue("seo描述");
                r1.CreateCell(7).SetCellValue("seo关键词");
                r1.CreateCell(8).SetCellValue("商品上架");
                r1.CreateCell(9).SetCellValue("需要物流");
                r1.CreateCell(10).SetCellValue("商品收税");
                r1.CreateCell(11).SetCellValue("虚拟销量");
                r1.CreateCell(12).SetCellValue("跟踪库存");
                r1.CreateCell(13).SetCellValue("库存规则");
                r1.CreateCell(14).SetCellValue("专辑名称");
                r1.CreateCell(15).SetCellValue("标签");
                r1.CreateCell(16).SetCellValue("供应商名称");
                r1.CreateCell(17).SetCellValue("供应商URL");
                r1.CreateCell(18).SetCellValue("商品属性");
                r1.CreateCell(19).SetCellValue("款式1");
                r1.CreateCell(20).SetCellValue("款式2");
                r1.CreateCell(21).SetCellValue("款式3");
                r1.CreateCell(22).SetCellValue("商品售价");
                r1.CreateCell(23).SetCellValue("商品原价价");
                r1.CreateCell(24).SetCellValue("商品SKU");
                r1.CreateCell(25).SetCellValue("商品重量");
                r1.CreateCell(26).SetCellValue("重量单位");
                r1.CreateCell(27).SetCellValue("商品条形码");
                r1.CreateCell(28).SetCellValue("商品库存");
                r1.CreateCell(29).SetCellValue("商品主图");
                r1.CreateCell(30).SetCellValue("一级分类");
                r1.CreateCell(31).SetCellValue("二级分类");
                r1.CreateCell(32).SetCellValue("产品分类");
                r1.CreateCell(33).SetCellValue("商品Spu");

                //第二行
                IRow r0 = s1.CreateRow(1);
                r0.HeightInPoints = 65;//行高
                r0.CreateCell(0).SetCellValue("(此行导入时不可删除)商品 ID 是系统生成的唯一标识符，新增商品无需填写");
                r0.CreateCell(1).SetCellValue("");
                r0.CreateCell(2).SetCellValue("");
                r0.CreateCell(3).SetCellValue("");
                r0.CreateCell(4).SetCellValue("");
                r0.CreateCell(5).SetCellValue("");
                r0.CreateCell(6).SetCellValue("");
                r0.CreateCell(7).SetCellValue("");
                r0.CreateCell(8).SetCellValue("");
                r0.CreateCell(9).SetCellValue("");
                r0.CreateCell(10).SetCellValue("");
                r0.CreateCell(11).SetCellValue("");
                r0.CreateCell(12).SetCellValue("");
                r0.CreateCell(13).SetCellValue("「1」库存为0允许购买「2」库存为0不允许购买「3」库存为0自动下架");
                r0.CreateCell(14).SetCellValue("");
                r0.CreateCell(15).SetCellValue("");
                r0.CreateCell(16).SetCellValue("");
                r0.CreateCell(17).SetCellValue("");
                r0.CreateCell(18).SetCellValue("「M」商品主体「P」款式部分「S」单商品");
                r0.CreateCell(19).SetCellValue("");
                r0.CreateCell(20).SetCellValue("");
                r0.CreateCell(21).SetCellValue("");
                r0.CreateCell(22).SetCellValue("");
                r0.CreateCell(23).SetCellValue("");
                r0.CreateCell(24).SetCellValue("");
                r0.CreateCell(25).SetCellValue("");
                r0.CreateCell(26).SetCellValue("");
                r0.CreateCell(27).SetCellValue("");
                r0.CreateCell(28).SetCellValue("");
                r0.CreateCell(29).SetCellValue("");
                r0.CreateCell(30).SetCellValue("");
                r0.CreateCell(31).SetCellValue("");
                r0.CreateCell(32).SetCellValue("");
                r0.CreateCell(33).SetCellValue("");

                var i = 2;
                foreach (var item in list)
                {
                    var k = 0;
                    NPOI.SS.UserModel.IRow rt = s1.CreateRow(i++);
                    rt.CreateCell(k++).SetCellValue(item.Id);
                    rt.CreateCell(k++).SetCellValue(item.Title);
                    rt.CreateCell(k++).SetCellValue(item.SubTitle);
                    rt.CreateCell(k++).SetCellValue(item.LinkUrl);
                    rt.CreateCell(k++).SetCellValue(item.Desc);
                    rt.CreateCell(k++).SetCellValue(item.SeoTitle);
                    rt.CreateCell(k++).SetCellValue(item.SeoDesc);
                    rt.CreateCell(k++).SetCellValue(item.SeoKeyWord);
                    rt.CreateCell(k++).SetCellValue(item.ProductShelf);
                    rt.CreateCell(k++).SetCellValue(item.Logistic);
                    rt.CreateCell(k++).SetCellValue(item.ProductFee);
                    rt.CreateCell(k++).SetCellValue(item.VirtualNum);
                    rt.CreateCell(k++).SetCellValue(item.FollowStock);
                    rt.CreateCell(k++).SetCellValue(item.StockRule);
                    rt.CreateCell(k++).SetCellValue(item.EditionName);
                    rt.CreateCell(k++).SetCellValue(item.Label);
                    rt.CreateCell(k++).SetCellValue(item.SupName);
                    rt.CreateCell(k++).SetCellValue(item.SupUrl);
                    rt.CreateCell(k++).SetCellValue(item.Attr);
                    rt.CreateCell(k++).SetCellValue(item.Style);
                    rt.CreateCell(k++).SetCellValue(item.Size);
                    rt.CreateCell(k++).SetCellValue(item.Color);
                    rt.CreateCell(k++).SetCellValue(item.Price);
                    rt.CreateCell(k++).SetCellValue(item.OldPrice);
                    rt.CreateCell(k++).SetCellValue(item.Sku);
                    rt.CreateCell(k++).SetCellValue(item.Weight);
                    rt.CreateCell(k++).SetCellValue(item.Unit);
                    rt.CreateCell(k++).SetCellValue(item.BarCode);
                    rt.CreateCell(k++).SetCellValue(item.ProductStock);
                    rt.CreateCell(k++).SetCellValue(item.ImageUrl);
                    rt.CreateCell(k++).SetCellValue(item.FirstCategary);
                    rt.CreateCell(k++).SetCellValue(item.SecondCategary);
                    rt.CreateCell(k++).SetCellValue(item.ProductCategary);
                    rt.CreateCell(k++).SetCellValue(item.Spu);
                }

                using (Stream stream = new MemoryStream())
                {
                    book.Write(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    book.Close();
                    return File(Util.StreamToBytes(stream), "application/ms-excel", DateTime.Now.ToString("yyyy-MM-dd_HH_mm") + "导出.xls");
                }
                #endregion
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}
