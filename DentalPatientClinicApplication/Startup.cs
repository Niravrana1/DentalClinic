using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DentalPatientClinicApplication.Startup))]
namespace DentalPatientClinicApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
