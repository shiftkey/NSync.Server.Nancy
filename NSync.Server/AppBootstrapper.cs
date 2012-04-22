using Nancy.Hosting.Aspnet;

namespace NSync.Server
{
    public class AppBootstrapper : DefaultNancyAspNetBootstrapper
    {
        public AppBootstrapper()
        {
            
        }

        protected override void RequestStartup(TinyIoC.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines, Nancy.NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);
        }
        
    }
}