using BSUIR.BL.Interfaces.Services;
using BSUIR.BL.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BSUIR.BL.ServicesConfiguration
{
    public static class BusinessLogicServicesConfigurations
    {
        public static void ConfigureBusinessLogicServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IItemService, ItemService>();
            services.AddTransient<IAddressService, AddressService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IDeliveryAddressService, DeliveryAddressService>();
            services.AddTransient<IDiscountsService, DiscountsService>();
            services.AddTransient<IPhotoService, PhotoService>();
            services.AddTransient<IOrderHasItemService, OrderHasItemService>();

        }
    }
}
