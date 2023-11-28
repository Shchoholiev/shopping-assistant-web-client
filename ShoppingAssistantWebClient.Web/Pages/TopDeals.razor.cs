using GraphQL;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Components;
using ShoppingAssistantWebClient.Web.Models;
using ShoppingAssistantWebClient.Web.Network;
namespace ShoppingAssistantWebClient.Web.Pages;


public partial class TopDeals : ComponentBase
{

    [Inject]
    private ApiClient _apiClient { get; set; }

    public List<TopDealsProduct> TopDealsGift { get; set; } = new List<TopDealsProduct>();

    public List<TopDealsProduct> TopDealsGaming { get; set; } = new List<TopDealsProduct>();
    protected override async Task OnInitializedAsync()
    {

        TopDealsGift.Add(new TopDealsProduct
        {
            Id = "1",
            Name = "Earth Rated Dog Poop Bags",
            Price = 7.59,
            ImagesUrls = "https://m.media-amazon.com/images/I/71o3gZtswWL._AC_SL1500_.jpg",
            Url = "https://www.amazon.com/dp/B00EJXIZ60?th=1&linkCode=ll1&tag=cartaid-20&linkId=a3d56ac5cb63d2ec31711694f2146227&language=en_US&ref_=as_li_ss_tl"
        });

        TopDealsGift.Add(new TopDealsProduct
        {
            Id = "2",
            Name = "Playmobil A-Team Van",
            Price = 45.99,
            ImagesUrls = "https://m.media-amazon.com/images/I/81x-2k0wQPL._AC_SL1500_.jpg",
            Url = "https://www.amazon.com/dp/B095KP2T5V/ref=cm_gf_adjq_d_p0_e0_qd0_wDMrpoirWfFUdKIYB3rB"
        });


        TopDealsGift.Add(new TopDealsProduct
        {
            Id = "3",
            Name = "Amazfit GTS 3 Smart",
            Price = 70.21,
            ImagesUrls = "https://m.media-amazon.com/images/I/61v3RM+Y9-L._AC_SL1500_.jpg",
            Url = "https://www.amazon.com/Amazfit-Android-Fitness-Display-Tracking/dp/B09H5TWZQT?pf_rd_r=F00JZCSF46A9QDAJQ16B&pf_rd_t=Events&pf_rd_i=cybermonday&pf_rd_p=b5d7c894-3da1-49e6-8e23-1993d3fa6730&pf_rd_s=slot-6&ref=dlx_cyber_gd_dcl_img_4_d2b1e900_dt_sl6_30&th=1"
        });

        TopDealsGaming.Add(new TopDealsProduct
        {
            Id = "4",
            Name = "Biolage Color Last Conditioner",
            Price = 15.72,
            ImagesUrls = "https://m.media-amazon.com/images/I/51DwIN83TqL._SL1500_.jpg",
            Url = "https://www.amazon.com/BIOLAGE-Colorlast-Conditioner-Color-Treated-Hair/dp/B00ILBUG40?ref_=Oct_DLandingS_D_f9985aa4_0"
        });

        TopDealsGaming.Add(new TopDealsProduct
        {
            Id = "5",
            Name = "GAP Men's Logo Fleece Hoodie",
            Price = 23.29,
            ImagesUrls = "https://m.media-amazon.com/images/I/71cWXbl8PoL._AC_SY741_.jpg",
            Url = "https://www.amazon.com/dp/B09R2LRRLN/?pf_rd_r=PG3YVRHHJ9KBPM271AHQ&pf_rd_t=Events&pf_rd_i=cybermonday&pf_rd_p=b5d7c894-3da1-49e6-8e23-1993d3fa6730&pf_rd_s=slot-6&ref=dlx_cyber_gd_dcl_img_5_51d0c142_dt_sl6_30&th=1&psc=1"
        });

        TopDealsGaming.Add(new TopDealsProduct
        {
            Id = "6",
            Name = "Baseus Power Bank, 65W 20000mAh",
            Price = 45.99,
            ImagesUrls = "https://m.media-amazon.com/images/I/71yLzSKcWmL._AC_SL1500_.jpg",
            Url = "https://www.amazon.com/Portable-Charger-Baseus-20000mAh-Charging/dp/B08THCNNCS?pd_rd_w=dXZn8&content-id=amzn1.sym.adeb688d-35a7-4952-bbb3-fcbab0fec4f0&pf_rd_p=adeb688d-35a7-4952-bbb3-fcbab0fec4f0&pf_rd_r=EVGJXF4CV6XX9W7N5B0Z&pd_rd_wg=AAlho&pd_rd_r=379bd4e7-b04b-4752-873a-c3badfe748ff&pd_rd_i=B08THCNNCS&ref_=pd_bap_d_grid_rp_0_2_t&th=1"
        });

    }




}
