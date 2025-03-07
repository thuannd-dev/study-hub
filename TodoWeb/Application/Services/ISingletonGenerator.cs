namespace TodoWeb.Application.Services
{
    public interface ISingletonGenerator
    {
        Guid Generate();
    }

    public class SingltonGenerator : ISingletonGenerator
    {

        private readonly IServiceProvider _serviceProvider;


        public SingltonGenerator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Guid Generate()
        {
            var guidGenerator = _serviceProvider.GetService<IGuidGenerator>();
            return guidGenerator.Generate();
        } 
    } 
}
