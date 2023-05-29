namespace Companies.API.Services
{
    public class ExtensionServiceDemo : IExtensionServiceDemo
    {
        private IConfiguration configuration { get; }
        public ExtensionServiceDemo(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string DemoMethod()
        {
            var value = configuration.GetValueFromExtension("SomeValue");
            //Do work

            return value.ToString();
        }

    }

    public interface IExtensionServiceDemo
    {
        string DemoMethod();
    }

    public static class ConfigExtensions
    {
        public static Func<IConfiguration, string, string> Implementation { private get; set; } = (config, key) => config.GetValue<string>(key)!;
        public static string GetValueFromExtension(this IConfiguration configuration, string key)
        {
            return Implementation(configuration, key);
        }
    } 
    
    //public class GetValueFromConfig : IGetValueFromConfig
    //{
    //    public  string GetValue(IConfiguration configuration, string key)
    //    {
    //        return configuration.GetValue<string>(key)!;
    //    }
    //}

    //public interface IGetValueFromConfig
    //{
    //    string GetValue(IConfiguration configuration, string key);
    //}




}
