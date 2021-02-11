using BSUIR.DAL.Context;
using BSUIR.DAL.Interfaces.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BSUIR.DAL.ServicesConfiguration
{
    public static class DataAccessConfiguration
    {
        public static void ConfigureDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<InternetShopContext>(opts => opts.UseSqlServer(configuration.GetConnectionString(typeof(InternetShopContext)?.Name)));

            services.AddTransient<IAddressesContext, AddressesContext>();
            services.AddTransient<ICustomersContext, CustomersContext>();
            services.AddTransient<IDeliveryAddressesContext, DeliveryAddressesContext>();
            services.AddTransient<IDiscountsContext, DiscountsContext>();
            services.AddTransient<IItemsContext, ItemsContext>();
            services.AddTransient<IOrdersContext, OrdersContext>();
            services.AddTransient<IPhotoContext, PhotoContext>();
            services.AddTransient<ICategoryContext, CategoryContext>();
            services.AddTransient<IOrderHasItemContext, OrderHasItemContext>();
        }
    }
}
