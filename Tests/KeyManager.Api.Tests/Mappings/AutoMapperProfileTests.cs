namespace KeyManager.Api.Tests.Mappings;

public class AutoMapperProfileTests
{
    [Fact]
    public Task Given_ApiAutoMapper_When_ValidateMappings_Then_BeValid()
    {
        var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile(new AutoMapperProfile()); });
        IMapper mapper = new Mapper(mapperConfig);
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
        return Task.CompletedTask;
    }
}