﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopWeb.Models;

namespace ShopWeb.Controllers
{
    public class BuyGoodsController : Controller
    {
        // GET: BuyGoods
        [HttpGet]
        public ActionResult Index()
        {
            string now_goods_id = Request.QueryString["goods_id"];
            ShopBusinessLogic.LoginMember loginMember = new ShopBusinessLogic.LoginMember();
            ShopBusinessLogic.SellerSell sellerSell = new ShopBusinessLogic.SellerSell();
            if (now_goods_id== null) return Redirect("/Error");
            if (!sellerSell.isInGoodsList(Convert.ToInt32(now_goods_id))) return Redirect("/Error");
            else
            {
                int now_goods_id_int = Convert.ToInt32(now_goods_id);
                ShopBusinessLogic.MemberPurchase memberPurchase = new ShopBusinessLogic.MemberPurchase();
                var goods_list = memberPurchase.getGoodsList().Select(goods_info => new MemberPurchaseCarViewModel()
                {
                    goods_id = goods_info.goods_id,
                    goods_name = goods_info.goods_name,
                    goods_img_path = goods_info.goods_img_path,
                    unit_price = goods_info.goods_price,
                    sell_stock=goods_info.goods_stock,
                    sell_volume=goods_info.goods_volume,
                    seller_phone=goods_info.seller_phone,
                }).ToList();
                var now_img_list = memberPurchase.getGoodsImgs(now_goods_id_int).Select(img_info => new GoodsImgView()
                {
                    img_path = img_info.img_path,
                }).ToList();
                var now_goods = memberPurchase.getGoods(now_goods_id_int);
                var resView = new PurchaseHomeTotalInfo()
                {
                    now_goods_id = now_goods_id_int,
                    now_goods_name = now_goods.goods_name,
                    now_goods_unit_price = now_goods.goods_price,
                    now_goods_detail = now_goods.goods_details,
                    now_stock= now_goods.goods_stock,
                    now_volume=now_goods.goods_volume,
                    now_seller_phone=now_goods.seller_phone,
                    now_seller_name=loginMember.GetMemberByPhone(now_goods.seller_phone).mem_name,
                    now_goods_tag=now_goods.goods_tag,
                    total_goods_list = goods_list,
                    now_img_lists = now_img_list,
                };
                return View(resView);
            }
        }
    }
}