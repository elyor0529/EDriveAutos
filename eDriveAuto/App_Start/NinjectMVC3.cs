using Edrive.Logic;
using Edrive.Logic.Interfaces;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Edrive.App_Start.NinjectMVC3), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(Edrive.App_Start.NinjectMVC3), "Stop")]

namespace Edrive.App_Start
{
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Mvc;

    public static class NinjectMVC3 
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestModule));
            DynamicModuleUtility.RegisterModule(typeof(HttpApplicationInitializationModule));
            Bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
			kernel.Bind<IProductService>().To<ProductService>();
			kernel.Bind<IProductPictureService>().To<ProductPictureService>();
			kernel.Bind<IDealerService>().To<DealerService>();
			kernel.Bind<IInterestedCustomerService>().To<InterestedCustomerService>();
			kernel.Bind<IProductTypeService>().To<ProductTypeService>();
			kernel.Bind<IProductBodyService>().To<ProductBodyService>();
			kernel.Bind<IProductModelService>().To<ProductModelService>();
			kernel.Bind<IProductMakeService>().To<ProductMakeService>();
			kernel.Bind<ICustomerProfileService>().To<CustomerProfileService>();
			kernel.Bind<IStateProvinceService>().To<StateProvinceService>();
			kernel.Bind<ICountryService>().To<CountryService>();
			kernel.Bind<ISettingsService>().To<SettingsService>();
			kernel.Bind<IProductOptionService>().To<ProductOptionService>();
        }        
    }
}
